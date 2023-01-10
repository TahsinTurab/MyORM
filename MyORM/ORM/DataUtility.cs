using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment4
{
    public class DataUtility
    {
        private static string _connectionString = "Server = DESKTOP-1TR3TO7\\SQLEXPRESS; " +
        "Database = Assignment4; User Id = Assignment4; Password = Assignment4;";

        private static SqlCommand CreateCommand(string sql)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = sql;

            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            return command;
        }

        
        public static void ExecuteCommand(string sql)
        {

            using var command = CreateCommand(sql);
            command.ExecuteNonQuery();
        }

        public static List<Dictionary<string, string>> ExecuteQuery(string query)
        {
            using var command = CreateCommand(query);
            List<Dictionary<string, string>> values = new List<Dictionary<string, string>>();


            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Dictionary<string, string> row = new Dictionary<string, string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row.Add(reader.GetName(i), $"{reader.GetValue(i)}");
                }
                values.Add(row);
            }

            return values;
        }

        public static Dictionary<string, string> ExecuteGetIdQuery(string query)
        {
            using var command = CreateCommand(query);
            Dictionary<string, string> values = new Dictionary<string, string>();


            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                //Dictionary<string, string> row = new Dictionary<string, string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {

                    values.Add($"{reader.GetName(i)}", $"{reader.GetValue(i)}");
                    break;
                }
                //values.Add(row);
                if (values.Count > 0) break;
            }

            return values;
        }

        public static List<string> TableList()
        {
            var list = new List<string>();
            var query = "select TABLE_NAME from INFORMATION_SCHEMA.TABLES";
            using var command = CreateCommand(query);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    list.Add((string)reader.GetValue(i));
                }
            }
            return list;
        
        }


    }
}
