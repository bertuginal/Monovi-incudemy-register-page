using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;


namespace Angle.Helpers
{
    public class DataHelper
    {
        public enum RunTypes
        {
            NonQuery,
            Scalar,
            Reader
        }
        public static string ConnectionString { get; set; }

        public static T Run<T>(string query, Dictionary<string, object> parameters = null, CommandType type = CommandType.StoredProcedure, RunTypes rtype = RunTypes.Reader)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand(query, conn))
            {
                conn.Open();
                command.CommandType = type;
                if (parameters != null)
                {
                    foreach (var p in parameters)
                    {
                        command.Parameters.AddWithValue("@" + p.Key, p.Value.ToString());
                    }
                }
                if (rtype == RunTypes.Reader)
                {
                    DataTable x = new DataTable();

                    x.Load(command.ExecuteReader());
                    return (T)Convert.ChangeType(x, typeof(T));
                } else if (rtype == RunTypes.NonQuery)
                {
                    var data = (T)Convert.ChangeType(command.ExecuteNonQuery(), typeof(T));
                    return data;
                } else
                {
                    return (T)Convert.ChangeType(command.ExecuteScalar() ?? default(T), typeof(T));
                }
            }
        }

        public static DataTable ListFromStoredProcedure(string spName, Dictionary<string, object> parameters = null)
        {
            DataTable x= Run<DataTable>(spName, parameters);
            return x;
        }

        public static int RunFromStoredProcedure(string spName, Dictionary<string, object> parameters = null)
        {
            int x = Run<int>(spName, parameters, CommandType.StoredProcedure, RunTypes.NonQuery);
            return x;
        }
        public static T GetFromStoredProcedure<T>(string spName, Dictionary<string, object> parameters = null)
        {
            T x = Run<T>(spName, parameters, CommandType.StoredProcedure, RunTypes.Scalar);
            return x;
        }

    }

}
