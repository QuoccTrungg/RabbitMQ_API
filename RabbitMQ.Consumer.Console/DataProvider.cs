using System.Data;
using System.Data.SqlClient;

namespace RabbitMQ.Client
{
    public class DataProvider
    {
        private static DataProvider? instance;

        public static DataProvider Instance 
        {
            get
            {
                if (instance == null)
                    instance = new DataProvider();
                return instance;
            }
            private set => instance = value;
        }

        private DataProvider() { }

        private string connectionStr = "data Source=LAPTOP-UEL9U0VQ;initial catalog=RabbitMQ;user id=sa;password=123";

        public DataTable ExecuteQuery(string query, object[]? parameter = null)
        {
            DataTable data = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                SqlCommand command = new SqlCommand(query, connection);
                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string str in listPara)
                    {
                        if (str.Contains('@'))
                        {
                            command.Parameters.AddWithValue(str, parameter[i]);
                            i++;
                        }
                    }
                }
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);
                connection.Close();
            }
            return data;
        }

        public int ExecuteNonQuery(string query, object[]? parameter = null)
        {
            int data = 0;
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                SqlCommand command = new SqlCommand(query, connection);
                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string str in listPara)
                    {
                        if (str.Contains('@'))
                        {
                            command.Parameters.AddWithValue(str, parameter[i]);
                            i++;
                        }
                    }
                }
                connection.Open();
                data = command.ExecuteNonQuery();
                connection.Close();
            }
            return data;
        }

        public object ExecuteScalar(string query, object[]? parameter = null)
        {
            object data = 0;
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                SqlCommand command = new SqlCommand(query, connection);
                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string str in listPara)
                    {
                        if (str.Contains('@'))
                        {
                            command.Parameters.AddWithValue(str, parameter[i]);
                            i++;
                        }
                    }
                }
                connection.Open();
                data = command.ExecuteScalar();
                connection.Close();
            }
            return data;
        }
    }
}
