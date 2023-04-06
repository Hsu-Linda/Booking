using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata;

namespace Booking.Services
{

    public class OpenSqlService
    {
        private readonly IConfiguration _configuration;
        public OpenSqlService(IConfiguration configuration)
        {
            _configuration= configuration;
        }

        public bool ExecuteQuery(string query, Dictionary<string, object> parameters, out DataTable dt)
        {

            using (Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(_configuration.GetConnectionString("BookingDatabase")))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = query;
                    if (parameters != null)
                    {
                        foreach (string param in parameters.Keys)
                        {
                            cmd.Parameters.AddWithValue(param, parameters[param]);
                        }
                    }
                    SqlDataReader dr = cmd.ExecuteReader();
                    bool drhasrows = dr.HasRows;
                    
                    dt = new DataTable();
                    dt.Load(dr);
                    
                    dr.Close();
                    return drhasrows;
                }
            }
        }

        public void ExecureNonQuery(string query, Dictionary<string, Tuple<object, SqlDbType>> parameter)
        {
            using(SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("BookingDatabase")))
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    if(parameter != null)
                    {
                        foreach (string param in parameter.Keys)
                        {
                            cmd.Parameters.Add(param, parameter[param].Item2);
                            cmd.Parameters[param].Value = parameter[param].Item1;
                        }
                    }
                     cmd.ExecuteNonQuery();
                }
            }
        }

        public int ExecuteSQLScalar(string query, Dictionary<string, Tuple<object, SqlDbType>> parametres)
        {
            using(SqlConnection con  = new SqlConnection(_configuration.GetConnectionString("BookingDatabase")))
            {
                con.Open();
                using(SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = query;
                    if (parametres != null)
                    {
                        foreach(string param in parametres.Keys)
                        {
                            cmd.Parameters.Add(param, parametres[param].Item2);
                            cmd.Parameters[param].Value = parametres[param].Item1;
                        }
                    }
                    try {
                        object changeLine = cmd.ExecuteScalar();
                        return (int)changeLine;

                    }
                    catch (Exception ex){
                        Console.WriteLine(ex.ToString());
                        return -1;
                    }
                    
                }
            }
        }
    }
}
