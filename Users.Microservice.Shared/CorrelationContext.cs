using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Microservice.Shared
{
    public static class CorrelationContext
    {
        private static readonly AsyncLocal<string> _current = new();

        public static string Current
        {
            get => _current.Value ??= Guid.NewGuid().ToString();
            set => _current.Value = value;
        }
    }

}
