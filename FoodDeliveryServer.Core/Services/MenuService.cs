using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using FoodDeliveryServer.Common.Exceptions;
using FoodDeliveryServer.Data.Interfaces;
using FoodDeliveryServer.Core.Interfaces;
using FoodDeliveryServer.Data.Models;
using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using CloudinaryDotNet.Actions;
using FoodDeliveryServer.Common.Dto.Request;
using FoodDeliveryServer.Common.Dto.Response;
using FoodDeliveryServer.Common.Enums;

namespace FoodDeliveryServer.Core.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IValidator<Menu> _validator;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;

        public MenuService(IConfiguration config, IMenuRepository menuRepository, IRestaurantRepository restaurantRepository, IValidator<Menu> validator, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _restaurantRepository = restaurantRepository;
            _validator = validator;
            _mapper = mapper;

            IConfigurationSection cloudinarySettings = config.GetSection("CloudinarySettings");
            string cloudinaryUrl = cloudinarySettings.GetValue<string>("CloudinaryUrl");
            _cloudinary = new Cloudinary(cloudinaryUrl);
        }

        public async Task<List<MenuResponseDto>> GetMenus(long? restaurantId, ItemCategory? Category, string? search)
        {
            List<Menu> menus;

            if (restaurantId == null && Category == null && search == null)
            {
                menus = await _menuRepository.GetAllMenus();
            }
            else
            {
                restaurantId = restaurantId ?? 0;
                menus = await _menuRepository.GetMenusByRestaurant(restaurantId.Value, Category, search);
            }

            return _mapper.Map<List<MenuResponseDto>>(menus);
        }

        public async Task<MenuResponseDto> GetMenu(long id)
        {
            Menu? menu = await _menuRepository.GetMenuById(id);

            if (menu == null)
            {
                throw new ResourceNotFoundException("Menu with this id doesn't exist");
            }

            return _mapper.Map<MenuResponseDto>(menu);
        }

        public async Task<MenuResponseDto> CreateMenu(long partnerId, CreateMenuRequestDto requestDto)
        {
            Menu menu = _mapper.Map<Menu>(requestDto);

            ValidationResult validationResult = _validator.Validate(menu);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            Restaurant? restaurant = await _restaurantRepository.GetRestaurantById(menu.RestaurantId);

            if (restaurant == null)
            {
                throw new ResourceNotFoundException("Restaurant with this id doesn't exist");
            }

            if (restaurant.PartnerId != partnerId)
            {
                throw new ActionNotAllowedException("Unauthorized to add menus to this restaurant. Only the creator can perform this action.");
            }

            menu = await _menuRepository.CreateMenu(menu);

            return _mapper.Map<MenuResponseDto>(menu);
        }

        public async Task<MenuResponseDto> UpdateMenu(long id, long partnerId, UpdateMenuRequestDto requestDto)
        {
            Menu? menu = await _menuRepository.GetMenuById(id);

            if (menu == null)
            {
                throw new ResourceNotFoundException("Menu with this id doesn't exist");
            }

            if (menu.Restaurant.PartnerId != partnerId)
            {
                throw new ActionNotAllowedException("Unauthorized to update this menu. Only the creator can perform this action.");
            }

            Menu updatedMenu = _mapper.Map<Menu>(requestDto);

            ValidationResult validationResult = _validator.Validate(updatedMenu, options =>
            {
                options.IncludeProperties(x => x.Name);
                options.IncludeProperties(x => x.Description);
                options.IncludeProperties(x => x.Price);
                options.IncludeProperties(x => x.Quantity);
            });

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            _mapper.Map(requestDto, menu);

            menu = await _menuRepository.UpdateMenu(menu);

            return _mapper.Map<MenuResponseDto>(menu);
        }

        public async Task<DeleteEntityResponseDto> DeleteMenu(long id, long partnerId)
        {
            Menu? menu = await _menuRepository.GetMenuById(id);

            if (menu == null)
            {
                throw new ResourceNotFoundException("Menu with this id doesn't exist");
            }

            if (menu.Restaurant.PartnerId != partnerId)
            {
                throw new ActionNotAllowedException("Unauthorized to delete this menu. Only the creator can perform this action.");
            }

            menu.IsDeleted = true;

            await _menuRepository.UpdateMenu(menu);

            return _mapper.Map<DeleteEntityResponseDto>(menu);
        }

        public async Task<ImageResponseDto> UploadImage(long menuId, long partnerId, Stream imageStream, string imageName)
        {
            if (imageStream == null || imageStream.Length == 0)
            {
                throw new InvalidImageException("Provided image is invalid. Please ensure that the image has valid content");
            }

            Menu? existingMenu = await _menuRepository.GetMenuById(menuId);

            if (existingMenu == null)
            {
                throw new ResourceNotFoundException("Restaurant with this id doesn't exist");
            }

            if (existingMenu.Restaurant.PartnerId != partnerId)
            {
                throw new ActionNotAllowedException("Unauthorized to update this restaurant. Only the creator can perform this action.");
            }

            if (existingMenu.ImagePublicId != null)
            {
                DeletionParams deletionParams = new DeletionParams(existingMenu.ImagePublicId)
                {
                    ResourceType = ResourceType.Image
                };

                await _cloudinary.DestroyAsync(deletionParams);
            }

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imageName, imageStream),
                Tags = "menus"
            };

            ImageUploadResult uploadResult = await _cloudinary.UploadAsync(uploadParams);

            existingMenu.ImagePublicId = uploadResult.PublicId;
            existingMenu.Image = uploadResult.Url.ToString();

            existingMenu = await _menuRepository.UpdateMenu(existingMenu);

            return _mapper.Map<ImageResponseDto>(existingMenu);
        }
    }
}
