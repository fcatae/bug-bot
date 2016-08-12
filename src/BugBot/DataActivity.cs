using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BugBot
{
    public interface IDataActivity
    {
        void Add(string user, string message);
    }

    public class DataActivity : IDataActivity
    {
        string _connectionString;

        public DataActivity(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(string user, string message)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT tbMessages([from], [message]) VALUES (@from, @message)", connection);

                command.Parameters.Add(new SqlParameter("@from", user));
                command.Parameters.Add(new SqlParameter("@message", message));

                connection.Open();

                command.ExecuteNonQuery();
            }
        }
    }
}
