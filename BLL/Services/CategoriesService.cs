using DomainModel.Models;
using Interfaces.Repositories;
using System.Collections.Generic;
using System;
using Interfaces.Services;

namespace BLL.Services
{
    public class CategoriesService : ICategoriesService
    {
        private IDbRepos db;
        public CategoriesService(IDbRepos repos)
        {
            db = repos;
        }

        public void AddCategory(Category newCategory)
        {
            bool isExist = (db.Categories.GetItemByName(newCategory.Name) != null) ? true : false;
            if (isExist)
                throw new Exception($"Категория с именем {newCategory.Name} уже существует!");
            else
            {
                db.Categories.Create(newCategory);
                db.Save();
            }
        }

        public void DeleteCategory(int categoryId)
        {
            db.Categories.Delete(categoryId);
            db.Save();
        }

        public List<Category> GetAllCategories()
        {
            return db.Categories.GetList();
        }

        public List<Category> GetCategoriesByContaintsLetters(string phrase)
        {
            return db.Categories.GetListByContaintsLetters(phrase);
        }

        public Category GetCategoryById(int categoryId)
        {
            return db.Categories.GetItem(categoryId);
        }

        public Category GetCategoryByName(string categoryName)
        {
            return db.Categories.GetItemByName(categoryName);
        }

        public int UpdateCategory(Category changedCategory)
        {
            db.Categories.GetItem(changedCategory.Id).Name = changedCategory.Name;
            return db.Save();
        }
    }
}
