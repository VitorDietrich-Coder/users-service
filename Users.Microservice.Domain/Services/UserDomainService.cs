using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Microservice.Domain.Services
{
    public class UserDomainService
    {
        public bool CanCreateUser(string email)
        {
            return !string.IsNullOrEmpty(email);
        }
    }
}
