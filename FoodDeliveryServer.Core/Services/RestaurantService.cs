using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using FoodDeliveryServer.Common.Exceptions;
using FoodDeliveryServer.Data.Interfaces;
using FoodDeliveryServer.Core.Interfaces;
using FoodDeliveryServer.Data.Models;
using NetTopologySuite.Geometries;
using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using CloudinaryDotNet.Actions;
using Point = NetTopologySuite.Geometries.Point;
using FoodDeliveryServer.Common.Dto.Request;
using FoodDeliveryServer.Common.Dto.Response;

namespace FoodDeliveryServer.Core.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IValidator<Restaurant> _validator;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;

        public RestaurantService(IConfiguration config, IRestaurantRepository restaurantRepository, IValidator<Restaurant> validator, IMapper mapper)
        {
            _restaurantRepository = restaurantRepository;
            _validator = validator;
            _mapper = mapper;

            IConfigurationSection cloudinarySettings = config.GetSection("CloudinarySettings");
            string cloudinaryUrl = cloudinarySettings.GetValue<string>("CloudinaryUrl");
            _cloudinary = new Cloudinary(cloudinaryUrl);
        }

        public async Task<List<RestaurantResponseDto>> GetRestaurants(long? partnerId, double? latitude, double? longitude)
        {
            List<Restaurant> allRestaurants = await _restaurantRepository.GetAllRestaurants();
            List<Restaurant> restaurants = new List<Restaurant>();

            if (partnerId.HasValue)
            {
                restaurants = allRestaurants.Where(x => x.PartnerId == partnerId.Value).ToList();
            }
            else if (latitude.HasValue && longitude.HasValue)
            {
                Point point = new Point(new Coordinate(longitude.Value, latitude.Value));
                restaurants = allRestaurants.Where(x => point.Within(x.DeliveryArea)).ToList();
            }
            else
            {
                restaurants = await _restaurantRepository.GetAllRestaurants();
            }

            return _mapper.Map<List<RestaurantResponseDto>>(restaurants);
        }

        public async Task<RestaurantResponseDto> GetRestaurant(long id)
        {
            Restaurant? restaurant = await _restaurantRepository.GetRestaurantById(id);

            if (restaurant == null)
            {
                throw new ResourceNotFoundException("Restaurant with this id doesn't exist");
            }

            return _mapper.Map<RestaurantResponseDto>(restaurant);
        }

        public async Task<RestaurantResponseDto> CreateRestaurant(long partnerId, RestaurantRequestDto requestDto)
        {
            Restaurant restaurant = _mapper.Map<Restaurant>(requestDto);
            restaurant.PartnerId = partnerId;

            ValidationResult validationResult = _validator.Validate(restaurant);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            Polygon deliveryAreaPolygon = _mapper.Map<Polygon>(restaurant.Coordinates);

            if (!deliveryAreaPolygon.IsValid)
            {
                throw new InvalidTopologyException("Delivery area is not a valid polygon");
            }

            deliveryAreaPolygon.SRID = 4326;
            restaurant.DeliveryArea = deliveryAreaPolygon;

            restaurant = await _restaurantRepository.CreateRestaurant(restaurant);

            return _mapper.Map<RestaurantResponseDto>(restaurant);
        }

        public async Task<RestaurantResponseDto> UpdateRestaurant(long id, long partnerId, RestaurantRequestDto requestDto)
        {
            Restaurant? restaurant = await _restaurantRepository.GetRestaurantById(id);

            if (restaurant == null)
            {
                throw new ResourceNotFoundException("Restaurant with this id doesn't exist");
            }

            if (restaurant.PartnerId != partnerId)
            {
                throw new ActionNotAllowedException("Unauthorized to update this restaurant. Only the creator can perform this action.");
            }

            Restaurant updatedRestaurant = _mapper.Map<Restaurant>(requestDto);

            ValidationResult validationResult = _validator.Validate(updatedRestaurant, options =>
            {
                options.IncludeProperties(x => x.Name);
                options.IncludeProperties(x => x.Description);
                options.IncludeProperties(x => x.Address);
                options.IncludeProperties(x => x.City);
                options.IncludeProperties(x => x.PostalCode);
                options.IncludeProperties(x => x.Phone);
                options.IncludeProperties(x => x.Coordinates);
            });

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            Polygon deliveryAreaPolygon = _mapper.Map<Polygon>(updatedRestaurant.Coordinates);

            if (!deliveryAreaPolygon.IsValid)
            {
                throw new InvalidTopologyException("Delivery area is not a valid polygon");
            }

            deliveryAreaPolygon.SRID = 4326;

            _mapper.Map(requestDto, restaurant);

            restaurant.DeliveryArea = deliveryAreaPolygon;

            restaurant = await _restaurantRepository.UpdateRestaurant(restaurant);

            return _mapper.Map<RestaurantResponseDto>(restaurant);
        }

        public async Task<DeleteEntityResponseDto> DeleteRestaurant(long id)
        {
            Restaurant? restaurant = await _restaurantRepository.GetRestaurantById(id);

            if (restaurant == null)
            {
                throw new ResourceNotFoundException("Restaurant with this id doesn't exist");
            }

            await _restaurantRepository.DeleteRestaurant(restaurant);

            return _mapper.Map<DeleteEntityResponseDto>(restaurant);
        }

        public async Task<ImageResponseDto> UploadImage(long restaurantId, long partnerId, Stream imageStream, string imageName)
        {
            if (imageStream == null || imageStream.Length == 0)
            {
                throw new InvalidImageException("Provided image is invalid. Please ensure that the image has valid content");
            }

            Restaurant? existingRestaurant = await _restaurantRepository.GetRestaurantById(restaurantId);

            if (existingRestaurant == null)
            {
                throw new ResourceNotFoundException("Restaurant with this id doesn't exist");
            }

            if (existingRestaurant.PartnerId != partnerId)
            {
                throw new ActionNotAllowedException("Unauthorized to update this restaurant. Only the creator can perform this action.");
            }

            if (existingRestaurant.ImagePublicId != null)
            {
                DeletionParams deletionParams = new DeletionParams(existingRestaurant.ImagePublicId)
                {
                    ResourceType = ResourceType.Image
                };

                await _cloudinary.DestroyAsync(deletionParams);
            }

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imageName, imageStream),
                Tags = "restaurants"
            };

            ImageUploadResult uploadResult = await _cloudinary.UploadAsync(uploadParams);

            existingRestaurant.ImagePublicId = uploadResult.PublicId;
            existingRestaurant.Image = uploadResult.Url.ToString();

            existingRestaurant = await _restaurantRepository.UpdateRestaurant(existingRestaurant);

            return _mapper.Map<ImageResponseDto>(existingRestaurant);
        }
    }
}
