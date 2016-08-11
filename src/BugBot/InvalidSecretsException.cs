using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugBot
{
    public class InvalidSecretsException : Exception
    {
        public InvalidSecretsException(string resourceName) : base($"Invalid secrets provided for [{resourceName}]")
        {
            
        }
    }
}
