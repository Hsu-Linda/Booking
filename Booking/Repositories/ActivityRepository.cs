using Azure.Core;
using Booking.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System.ComponentModel.Design;
using System.Data;

namespace Booking.Repositories
{
    public interface IActivityReporitory
    {
        void Add(Activity activity, short companyID);
        Task<int> UpdateAsync(Activity activity, short companyID);
        void Delete(int id);
    }
    public class ActivityRepository : IActivityReporitory
    {
        private readonly string _connectionString;
        public ActivityRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("BookingDatabase");
        }
        public void Add(Activity activity, short companyID)
        {
            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("usp_create_activity",connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = activity.Name;
                    cmd.Parameters.Add("@user", SqlDbType.SmallInt).Value = companyID;
                    cmd.Parameters.Add("@photoURL", SqlDbType.NVarChar).Value = activity.PhotoUrl;
                    cmd.Parameters.Add("@city", SqlDbType.TinyInt).Value = activity.City;
                    cmd.Parameters.Add("@district", SqlDbType.SmallInt).Value = activity.District;
                    cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = activity.Address;
                    cmd.Parameters.Add("@showingStart", SqlDbType.DateTime).Value = activity.ShowingStart;
                    cmd.Parameters.Add("@showingEnd", SqlDbType.DateTime).Value = activity.ShowingEnd;
                    cmd.Parameters.Add("@salesStart", SqlDbType.DateTime).Value = activity.SalesStart;
                    cmd.Parameters.Add("@salesEnd", SqlDbType.DateTime).Value = activity.SalesEnd;
                    cmd.Parameters.Add("@errorNum", SqlDbType.Int).Value=DBNull.Value;
                    cmd.Parameters.Add("@errorSeverity", SqlDbType.Int).Value = DBNull.Value;
                    cmd.Parameters.Add("@errorState", SqlDbType.Int).Value = DBNull.Value;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            string query = @"update Activity set Active = 0 where @id = id;";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using(SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.Parameters.Add("@id", SqlDbType.SmallInt).Value = id;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool UpdateDeleteDataCheck(int id, short companyID)
        {
            string query = @"select * from ActivityView 
                            where @id = id and @user = Company and SALES>0;";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.Add("@id", SqlDbType.SmallInt).Value = id;
                    cmd.Parameters.Add("@user", SqlDbType.SmallInt).Value = companyID;

                    return cmd.ExecuteReader().HasRows;
                }
            }
        }

        public Task<int> UpdateAsync(Activity activity, short companyID)
        {
            throw new NotImplementedException();
        }
    }
}
