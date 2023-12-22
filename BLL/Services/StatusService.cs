using DomainModel.Models;
using Interfaces.Repositories;
using Interfaces.Services;
using System.Collections.Generic;

namespace BLL.Services
{
    public class StatusService : IStatusService
    {
        private IDbRepos db;
        public StatusService(IDbRepos repos)
        {
            db = repos;
        }

        public List<Status> GetAllStatuses()
        {
            return db.Statuses.GetList();
        }
    }
}
