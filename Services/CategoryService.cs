using megamart_backend.DTOs;
using megamart_backend.Models;
using megamart_backend.Repositories;
using megamart_backend.Services.Interfaces;

namespace megamart_backend.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<IReadOnlyList<CategoryResponseDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();

            return categories
                    .Select(c => new CategoryResponseDto(c.Id, c.Name))
                    .ToList();
        }


        public async Task<CategoryResponseDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return null;
            }
            return new CategoryResponseDto(category.Id, category.Name);
        }


        public async Task<CategoryResponseDto> CreateCategoryAsync(CategoryCreateDto dto)
        {
            var category = new Category
            {
                Name = dto.Name.Trim()
            };

            await _unitOfWork.Categories.AddAsync(category);

            await _unitOfWork.CompleteAsync();

            return new CategoryResponseDto(category.Id, category.Name);
        } 


        public async Task<bool> UpdateCategoryAsync(int id, CategoryUpdateDto dto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return false;
            }

            category.Name = dto.Name.Trim();
            _unitOfWork.Categories.Update(category);

            // ADD THIS LINE TO COMMIT CHANGES TO THE DATABASE
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
            {
                return false;
            }
            try
            {
                _unitOfWork.Categories.Delete(category);
                await _unitOfWork.CompleteAsync();
            }
            catch(Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                // Throw a custom exception or handle it so the controller knows it's a constraint issue
                throw new InvalidOperationException("Cannot delete this category because there are products associated with it.");
            }


            return true;
        }


    }
}
