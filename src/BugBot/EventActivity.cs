using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BugBot
{
    public interface IEventActivity
    {
        void Add(string eventName, string serviceUrl, string botAccount, string conversation, string messageTemplate);
    }

    public class EventModel
    {
        public string serviceUrl;
        public string botAccount;
        public string conversation;
        public string messageTemplate;
    }

    public class EventActivity : IEventActivity
    {
        string _connectionString;

        public EventActivity(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(string eventName, string serviceUrl, string botAccount, string conversation, string messageTemplate)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("prAddEvent", connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@eventName", eventName));
                command.Parameters.Add(new SqlParameter("@serviceUrl", serviceUrl));
                command.Parameters.Add(new SqlParameter("@botAccount", botAccount));
                command.Parameters.Add(new SqlParameter("@ConversationId", conversation));
                command.Parameters.Add(new SqlParameter("@MessageTemplate", messageTemplate));

                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        public EventModel Get(string eventName)
        {
            EventModel eventData;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("prGetEvent", connection);
                
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    reader.Read();

                    eventData = new EventModel()
                    {
                        serviceUrl = (string)reader["serviceUrl"],
                        botAccount = (string)reader["botAccount"],
                        conversation = (string)reader["conversation"],
                        messageTemplate = (string)reader["messageTemplate"]
                    };
                    
                }
            }

            return eventData;
        }
    }
}
