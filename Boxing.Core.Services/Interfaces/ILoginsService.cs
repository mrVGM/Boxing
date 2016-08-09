using Boxing.Contracts;
using Boxing.Contracts.Dto;
using System;
using System.Threading.Tasks;

namespace Boxing.Core.Services.Interfaces
{
    public interface ILoginsService
    {
        Task<Object> create(User request);
        Task<Unit> delete(int id);
    }
}
