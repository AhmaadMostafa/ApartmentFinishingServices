using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Services.Contract
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        string Username { get; }
        string Role { get; }
        bool IsAuthenticated { get; }
    }
}
