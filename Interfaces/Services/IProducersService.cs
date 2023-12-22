using DomainModel.Models;
using System.Collections.Generic;

namespace Interfaces.Services
{
    public interface IProducersService
    {
        List<Producer> GetAllProducers();
        List<Producer> GetProducersByContaintsLetters(string phrase);
        Producer GetProducerById(int producerId);
        Producer GetProducerByName(string producerName);
        void DeleteProducerById(int producerId);
        void AddProducer(Producer newProducer);
        int UpdateProducer(Producer changedProducer);
    }
}
