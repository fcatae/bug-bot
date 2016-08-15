using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BugBot
{
    public interface IEventActivity
    {
        int Add(string user, string message);
    }

    public class EventActivity : IEventActivity
    {
        string _connectionString;

        public EventActivity(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int Add(string user, string message)
        {
            int messageId;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("prAddMessage", connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@from", user));
                command.Parameters.Add(new SqlParameter("@message", message));

                connection.Open();

                object result = command.ExecuteScalar();
                
                messageId = Convert.ToInt32(result);
            }

            return messageId;
        }
    }
}
