using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace basic_work_with_elements
{
     public class DB
    {
        public DB(string server, string DatabaseName, string UserName, string password)
        {
            this.Server = server;
            this.DatabaseName = DatabaseName;
            this.UserName = UserName;
            this.Password = password;
            IsConnect();
        }

        public List<Dictionary<string, object>> execute(string sql)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            var cmd = new MySqlCommand(sql, this.Connection);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();
                    bool start = true;
                    int number = 0;
                    while (start)
                    {
                        try
                        {
                            row.Add(reader.GetName(number),reader.GetValue(number));
                            number++;
                        }
                        catch (Exception e)
                        {
                            start = false;
                        }
                    }
                    result.Add(row);
                }
            }
            
            return result;
        }

        public string Server { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        private MySqlConnection Connection { get; set;}
        private bool IsConnect()
        {
            if (Connection == null)
            {
                if (String.IsNullOrEmpty(DatabaseName))
                    return false;
                string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}", Server, DatabaseName, UserName, Password);
                Connection = new MySqlConnection(connstring);
                Connection.Open();
            }
    
            return true;
        }
    
        private void Close()
        {
            Connection.Close();
        }        
    }
}