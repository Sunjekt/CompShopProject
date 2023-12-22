using DomainModel.Models;
using System.Collections.Generic;

namespace Interfaces.Services
{
    public interface ICategoriesService
    {
        List<Category> GetAllCategories();
        List<Category> GetCategoriesByContaintsLetters(string phrase);
        Category GetCategoryById(int categoryId);
        Category GetCategoryByName(string categoryName);
        int UpdateCategory(Category changedCategory);
        void DeleteCategory(int categoryId);
        void AddCategory(Category newCategory);
    }
}
