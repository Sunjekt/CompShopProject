using DomainModel.Models;
using System.Collections.Generic;

namespace Interfaces.Services
{
    public interface IStatusService
    {
        List<Status> GetAllStatuses();
    }
}