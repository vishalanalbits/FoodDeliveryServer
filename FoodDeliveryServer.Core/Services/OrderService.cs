using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using FoodDeliveryServer.Common.Enums;
using FoodDeliveryServer.Common.Exceptions;
using FoodDeliveryServer.Data.Interfaces;
using FoodDeliveryServer.Core.Interfaces;
using FoodDeliveryServer.Data.Models;
using NetTopologySuite.Geometries;
using Stripe;
using Stripe.Checkout;
using Menu = FoodDeliveryServer.Data.Models.Menu;
using Microsoft.Extensions.Configuration;
using FoodDeliveryServer.Common.Dto.Request;
using FoodDeliveryServer.Common.Dto.Response;

namespace FoodDeliveryServer.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IValidator<Order> _validator;
        private readonly IMapper _mapper;
        private readonly IConfigurationSection _clientSettings;

        public OrderService(IOrderRepository orderRepository, IMenuRepository menuRepository, IRestaurantRepository restaurantRepository, IValidator<Order> validator, IMapper mapper, IConfiguration config)
        {
            _orderRepository = orderRepository;
            _menuRepository = menuRepository;
            _restaurantRepository = restaurantRepository;
            _validator = validator;
            _mapper = mapper;
            _clientSettings = config.GetSection("ClientSettings");
        }

        public async Task<List<OrderResponseDto>> GetOrders(long userId, UserType userType)
        {
            List<Order> orders = new List<Order>();

            switch (userType)
            {
                case UserType.Customer:
                    orders = await _orderRepository.GetOrdersByCustomer(userId);
                    break;
                case UserType.Partner:
                    orders = await _orderRepository.GetOrdersByPartner(userId);
                    break;
                case UserType.Delivery:
                    orders = await _orderRepository.GetOrdersByDelivery(userId);
                    break;
                case UserType.Admin:
                    orders = await _orderRepository.GetAllOrders();
                    break;
            }

            return _mapper.Map<List<OrderResponseDto>>(orders);
        }

        public async Task<CheckoutResponseDto> CreateCheckout(long customerId, OrderRequestDto requestDto)
        {
            Order order = _mapper.Map<Order>(requestDto);
            order.CustomerId = customerId;

            ValidationResult validationResult = _validator.Validate(order);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            Point deliveryLocationPoint = _mapper.Map<Point>(order.Coordinate);

            if (!deliveryLocationPoint.IsValid)
            {
                throw new InvalidTopologyException("Delivery location is not a valid location");
            }

            deliveryLocationPoint.SRID = 4326;
            order.DeliveryLocation = deliveryLocationPoint;

            Restaurant? restaurant = await _restaurantRepository.GetRestaurantById(order.RestaurantId);

            if (restaurant == null)
            {
                throw new ResourceNotFoundException("Restaurant with this id doesn't exist");
            }

            order.Restaurant = restaurant;

            if (!deliveryLocationPoint.Within(restaurant.DeliveryArea))
            {
                throw new AddressNotSupportedException("This restaurant doesn't deliver to your location.");
            }

            foreach (OrderItem orderItem in order.Items)
            {
                Menu? menu = await _menuRepository.GetMenuById(orderItem.MenuId);

                if (menu == null)
                {
                    // Should it throw exception and stop the order or just ignore this order item?
                    throw new ResourceNotFoundException($"Menu with this id ({orderItem.MenuId}) doesn't exist");
                }

                if (menu.RestaurantId != restaurant.Id)
                {
                    throw new IncompatibleItemsError("All items in one order must be from the same restaurant");
                }

                if (menu.Quantity < orderItem.Quantity)
                {
                    throw new InsufficientQuantityException($"Not enough menus available. Available quantity: {menu.Quantity}");
                }

                // Save menu's current information
                orderItem.MenuName = menu.Name;
                orderItem.MenuPrice = menu.Price;
                orderItem.MenuImage = menu.Image;
                orderItem.MenuDescription = menu.Description;

                orderItem.TotalPrice = orderItem.Quantity * menu.Price;
            }

            order.ItemsPrice = order.Items.Aggregate(0m, (total, item) => total + item.TotalPrice);
            order.DeliveryFee = restaurant.DeliveryFee;
            order.TotalPrice = order.ItemsPrice + order.DeliveryFee;
            order.CreatedAt = DateTime.UtcNow;
            order = _orderRepository.CreateOrder(order).Result;
            //List<SessionLineItemOptions> lineItems = order.Items.Select(item =>
            //{
            //    List<string> lineItemImages = new List<string>();

            //    if (item.MenuImage != null)
            //    {
            //        lineItemImages.Add(item.MenuImage);
            //    }

            //    return new SessionLineItemOptions()
            //    {
            //        PriceData = new SessionLineItemPriceDataOptions()
            //        {
            //            MenuData = new SessionLineItemPriceDataMenuDataOptions()
            //            {
            //                Name = item.MenuName,
            //                Description = item.MenuDescription,
            //                Images = lineItemImages,
            //                Metadata = new Dictionary<string, string>()
            //                {
            //                    { "MenuId", item.MenuId.ToString() },
            //                    { "Quantity", item.Quantity.ToString() }
            //                }
            //            },
            //            UnitAmountDecimal = item.TotalPrice * 100,
            //            Currency = "usd"
            //        },
            //        Quantity = 1
            //    };
            //}).ToList();

            //var clientDomain = _clientSettings.GetValue<string>("ClientDomain");

            //var options = new SessionCreateOptions()
            //{
            //    LineItems = lineItems,
            //    Metadata = new Dictionary<string, string>()
            //    {
            //        { "CustomerId", order.CustomerId.ToString() },
            //        { "RestaurantId", order.RestaurantId.ToString() },
            //        { "Address", order.Address },
            //        { "Coordinate", $"{order.Coordinate.X};{order.Coordinate.Y}" },
            //    },
            //    Mode = "payment",
            //    SuccessUrl = clientDomain + "/payment?status=success",
            //    CancelUrl = clientDomain + "/payment?status=cancel"
            //};

            //var service = new SessionService();
            //Session session = new();
            //try
            //{
            //    session = service.Create(options);
            //}
            //catch(Exception ex)
            //{

            //}

            return new CheckoutResponseDto()
            {
                Order = _mapper.Map<OrderResponseDto>(order),
                //SessionUrl = session.Url
            };
        }

        public async Task<OrderResponseDto> CreateOrder(long customerId, OrderRequestDto requestDto)
        {
            Order order = _mapper.Map<Order>(requestDto);
            order.CustomerId = customerId;

            ValidationResult validationResult = _validator.Validate(order);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            Point deliveryLocationPoint = _mapper.Map<Point>(order.Coordinate);

            if (!deliveryLocationPoint.IsValid)
            {
                throw new InvalidTopologyException("Delivery location is not a valid location");
            }

            deliveryLocationPoint.SRID = 4326;
            order.DeliveryLocation = deliveryLocationPoint;

            Restaurant? restaurant = await _restaurantRepository.GetRestaurantById(order.RestaurantId);

            if (restaurant == null)
            {
                throw new ResourceNotFoundException("Restaurant with this id doesn't exist");
            }

            if (!deliveryLocationPoint.Within(restaurant.DeliveryArea))
            {
                throw new AddressNotSupportedException("This restaurant doesn't deliver to your location.");
            }

            foreach (OrderItem orderItem in order.Items)
            {
                Menu? menu = await _menuRepository.GetMenuById(orderItem.MenuId);

                if (menu == null)
                {
                    // Should it throw exception and stop the order or just ignore this order item?
                    throw new ResourceNotFoundException($"Menu with this id ({orderItem.MenuId}) doesn't exist");
                }

                if (menu.RestaurantId != restaurant.Id)
                {
                    throw new IncompatibleItemsError("All items in one order must be from the same restaurant");
                }

                if (menu.Quantity < orderItem.Quantity)
                {
                    throw new InsufficientQuantityException($"Not enough menus available. Available quantity: {menu.Quantity}");
                }

                // Save menu's current information
                orderItem.MenuName = menu.Name;
                orderItem.MenuPrice = menu.Price;
                orderItem.MenuImage = menu.Image;

                orderItem.TotalPrice = orderItem.Quantity * menu.Price;
                menu.Quantity -= orderItem.Quantity;
            }

            order.ItemsPrice = order.Items.Aggregate(0m, (total, item) => total + item.TotalPrice);
            order.DeliveryFee = restaurant.DeliveryFee;
            order.TotalPrice = order.ItemsPrice + order.DeliveryFee;
            order.CreatedAt = DateTime.UtcNow;

            order = await _orderRepository.CreateOrder(order);

            return _mapper.Map<OrderResponseDto>(order);
        }

        public async Task<DeleteEntityResponseDto> RefundOrder(long orderId, long customerId)
        {
            Order? order = await _orderRepository.GetOrderById(orderId);

            if (order == null)
            {
                throw new ResourceNotFoundException("Order with this id doesn't exist");
            }

            if (order.CustomerId != customerId)
            {
                throw new ActionNotAllowedException("Unauthorized to cancel this order. Only the creator can perform this action.");
            }

            DateTime deliveryTime = order.CreatedAt.AddMinutes((int)order.Restaurant.DeliveryTimeInMinutes);

            if (DateTime.UtcNow > deliveryTime)
            {
                throw new OrderCancellationException("Cannot cancel the order because it has already been completed.");
            }

            var options = new RefundCreateOptions()
            {
                PaymentIntent = order.PaymentIntentId,
                Metadata = new Dictionary<string, string>()
                {
                    { "CustomerId", order.CustomerId.ToString() },
                    { "OrderId", order.Id.ToString() },
                }
            };

            var service = new RefundService();

            service.Create(options);

            return _mapper.Map<DeleteEntityResponseDto>(order);
        }

        public async Task<DeleteEntityResponseDto> CancelOrder(long orderId, long customerId)
        {
            Order? order = await _orderRepository.GetOrderById(orderId);

            if (order == null)
            {
                throw new ResourceNotFoundException("Order with this id doesn't exist");
            }

            if (order.CustomerId != customerId)
            {
                throw new ActionNotAllowedException("Unauthorized to cancel this order. Only the creator can perform this action.");
            }

            DateTime deliveryTime = order.CreatedAt.AddMinutes((int)order.Restaurant.DeliveryTimeInMinutes);

            if (DateTime.UtcNow > deliveryTime)
            {
                throw new OrderCancellationException("Cannot cancel the order because it has already been completed.");
            }

            order.IsCanceled = true;
            order.Items.ForEach(item =>
            {
                item.Menu.Quantity += item.Quantity;
            });

            await _orderRepository.UpdateOrder(order);

            return _mapper.Map<DeleteEntityResponseDto>(order);
        }

        public async Task<bool> UpdateOrderStatus(long orderId, OrderStatus orderStatus, long userId, UserType userType)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            if (order == null)
            {
                throw new ResourceNotFoundException("Order not exist with this Id.");
            }
            if (userType == UserType.Customer && order.CustomerId != userId) 
            {
                throw new ResourceNotFoundException("Order is not related to you.");
            }
            else if (userType == UserType.Partner)
            {
                var restaurant = await _restaurantRepository.GetRestaurantById(order.RestaurantId);
                if (restaurant == null || restaurant.PartnerId != userId)
                {
                    throw new ResourceNotFoundException("Order is not related to you.");
                }
            }
            else if (userType == UserType.Delivery && order.Delivery_ID != userId)
            {
                if (order.Delivery_ID != 0)
                {
                    throw new ResourceNotFoundException("Order is not assigned to you.");
                }
                else
                {
                    order.Delivery_ID = userId;  
                }
                
            }
            order.OrderStatus = orderStatus;
            var orderUpdate = await _orderRepository.UpdateOrder(order);
            return orderUpdate != null ? true : false;
        }
    }
}
