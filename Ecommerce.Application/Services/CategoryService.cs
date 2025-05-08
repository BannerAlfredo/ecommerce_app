using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Interfaces;

namespace Ecommerce.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }

        public Task<Category> CreateAsync(Category category)
        {
            return _categoryRepository.AddAsync(category);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _categoryRepository.DeleteAsync(id);
        }

        public Task<IEnumerable<Category>> GetAllAsync()
        {
            return _categoryRepository.GetAllAsync();
        }

        public Task<Category> GetByIdAsync(int id)
        {
            return _categoryRepository.GetByIdAsync(id);
        }

        public Task<Category> UpdateAsync(Category category)
        {
            return _categoryRepository.UpdateAsync(category);
        }
    }
}
