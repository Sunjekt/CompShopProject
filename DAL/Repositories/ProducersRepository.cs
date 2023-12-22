using DomainModel.Models;
using DAL.Manager;
using Interfaces.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DAL.Repositories
{
    public class ProducersRepository : IProducersRepository
    {
        private ModelsManager db;

        public ProducersRepository(ModelsManager dbcontext)
        {
            this.db = dbcontext;
        }

        public List<Producer> GetList()
        {
            return db.Producer.ToList();
        }

        public List<Producer> GetListByContaintsLetters(string phrase)
        {
            return db.Producer.Where(p => p.Name.Contains(phrase)).ToList();
        }

        public Producer GetItem(int id)
        {
            return db.Producer.Find(id);
        }

        public Producer GetItemByName(string name)
        {
            return db.Producer.Where(c => c.Name == name).FirstOrDefault();
        }

        public void Create(Producer item)
        {
            db.Producer.Add(item);
        }

        public void Update(Producer item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Producer item = db.Producer.Find(id);
            if (item != null)
                db.Producer.Remove(item);
        }

        public bool Save()
        {
            return db.SaveChanges() > 0;
        }

    }
}
