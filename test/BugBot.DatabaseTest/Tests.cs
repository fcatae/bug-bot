using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace BugBot.DatabaseTest
{
    public class Tests
    {
        public static readonly string CONNECTION_STRING;

        static Tests()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddEnvironmentVariables();
                

            if(Environment.GetEnvironmentVariable("ASPNETCORE_Environment") == "Development")
            {
                builder.AddUserSecrets();
            }

            var config = builder.Build();

            CONNECTION_STRING = config["Data:ConnectionString"];
        }

        public static void Main(string[] args)
        {
            var comments = new CommentsTests();

            comments.ListComments();            
        }
    }
}
