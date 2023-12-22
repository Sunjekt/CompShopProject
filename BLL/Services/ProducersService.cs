using DomainModel.Models;
using Interfaces.Repositories;
using Interfaces.Services;
using System;
using System.Collections.Generic;

namespace BLL.Services
{
    public class ProducersService : IProducersService
    {
        private IDbRepos db;
        public ProducersService(IDbRepos repos)
        {
            db = repos;
        }

        public void AddProducer(Producer newProducer)
        {
            bool isExist = (db.Producers.GetItemByName(newProducer.Name) != null) ? true : false;
            if (isExist)
                throw new Exception($"Производитель с именем {newProducer.Name} уже существует!");
            else
            {
                db.Producers.Create(newProducer);
                db.Save();
            }
        }

        public void DeleteProducerById(int producerId)
        {
            db.Producers.Delete(producerId);
            db.Save();
        }

        public List<Producer> GetAllProducers()
        {
            return db.Producers.GetList();
        }

        public Producer GetProducerByName(string producerName)
        {
            return db.Producers.GetItemByName(producerName);
        }

        public List<Producer> GetProducersByContaintsLetters(string phrase)
        {
            return db.Producers.GetListByContaintsLetters(phrase);
        }

        public Producer GetProducerById(int producerId)
        {
            return db.Producers.GetItem(producerId);
        }

        public int UpdateProducer(Producer changedProducer)
        {
            db.Producers.GetItem(changedProducer.Id).Name = changedProducer.Name;
            return db.Save();
        }
    }
}
