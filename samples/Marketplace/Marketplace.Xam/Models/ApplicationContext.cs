using Marketplace.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prolix.Core.Ioc;

namespace Marketplace.Xam.Models
{
    public sealed class ApplicationContext : IInstance
    {
        public string BaseUrl => "http://localhost:20000/api";
        public AccessModel Credentials { get; set; }
    }
}
