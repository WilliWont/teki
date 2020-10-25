using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataObjects.IRepository
{
    public interface IStatusRepository : IGenericRepository<Status>
    {
        // There are some additional methods for Status Implemetation
        Status GetStatus(string name);
    }
}
