using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyHotels.WebApi.Domain;

namespace MyHotels.WebApi.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Country> Countries { get; }
        IGenericRepository<Hotel> Hotels { get; }
        Task Save();
    }
}
