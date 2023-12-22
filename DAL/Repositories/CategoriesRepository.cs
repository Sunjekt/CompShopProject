using DomainModel.Models;
using DAL.Manager;
using Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private ModelsManager db;

        public CategoriesRepository(ModelsManager dbcontext)
        {
            this.db = dbcontext;
        }

        public List<Category> GetList()
        {
            return db.Category.ToList();
        }

        public List<Category> GetListByContaintsLetters(string phrase)
        {
            return db.Category.Where(c => c.Name.Contains(phrase)).ToList();
        }

        public Category GetItem(int id)
        {
            return db.Category.Find(id);
        }

        public Category GetItemByName(string name)
        {
            return db.Category.Where(c => c.Name == name).FirstOrDefault();
        }

        public void Create(Category item)
        {
            db.Category.Add(item);
        }

        public void Update(Category item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Category item = db.Category.Find(id);
            if (item != null)
                db.Category.Remove(item);
        }

        public bool Save()
        {
            return db.SaveChanges() > 0;
        }

    }
}
