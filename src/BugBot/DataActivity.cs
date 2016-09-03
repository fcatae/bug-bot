using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BugBot
{
    public interface IDataActivity
    {
        int Add(string user, string message);
        IEnumerable<DataModel> List();
    }

    public class DataModel
    {
        public string User;
        public string Message;
    }

    public class DataActivity : IDataActivity
    {
        string _connectionString;

        public DataActivity(string connectionString)
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

        public IEnumerable<DataModel> List()
        {
            List<DataModel> data = new List<DataModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("select [from], message from tbMessages order by id", connection);

                connection.Open();

                var reader = command.ExecuteReader();

                while(reader.Read())
                {
                    data.Add(new DataModel() { User = (string)reader[0], Message = (string)reader[1] });
                }
            }

            return data;
        }
    }
}
