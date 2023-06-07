using Booking.Dtos;
using Booking.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Booking.Repositories
{
    public interface ITicketTypeRepository
    {
        bool IsRepeated(string name, short activityID);
        bool IsSold(int ticketTypeID);
        bool ActivityPermission(short activityID, short companyID);
        bool TicketTypePermission(int tickeTypeID, short companyID);
        void Add(AddTicketTypeRequestDto ticketType);
        void Update(UpdateTicketTypeRequestDto ticketType);
        void Delete(int ticketTypeID);
        List<TicketType> GetAllByActivityID(short activityID);
    }
    public class TicketTypeRepository : ITicketTypeRepository
    {
        private readonly string _connectionString;
        public TicketTypeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("BookingDatabase");
        }
        public void Add(AddTicketTypeRequestDto ticketType)
        {
            string query = @"INSERT INTO TicketType
                            VALUES(@name,@activity,@price,@description,@total,0,1,GETDATE())";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = ticketType.Name;
                    cmd.Parameters.Add("@activity", SqlDbType.SmallInt).Value = ticketType.Activity;
                    cmd.Parameters.Add("@price", SqlDbType.SmallInt).Value = ticketType.Price;
                    cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(ticketType.Description) ? "" : ticketType.Description;
                    cmd.Parameters.Add("@total", SqlDbType.SmallInt).Value = ticketType.Total;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            string query = @"UPDATE TicketType
                            SET Active = 0 , LastModified = GETDATE()
                            WHERE @id = ID;";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.Add("id", SqlDbType.Int).Value = id;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(UpdateTicketTypeRequestDto ticketType)
        {
            string query = @"update TicketType
                            set name = @name, Price = @price, Description = @description, Total = @total
                            where ID = @id;";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = ticketType.Id;
                    cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = ticketType.Name;
                    cmd.Parameters.Add("@price", SqlDbType.SmallInt).Value = ticketType.Price;
                    cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = ticketType.Description;
                    cmd.Parameters.Add("@total", SqlDbType.SmallInt).Value = ticketType.Total;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool IsRepeated(string name, short activityID)
        {
            string query = @"SELECT * FROM TicketType
                            WHERE @name =Name and @activity = Activity;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = name;
                    cmd.Parameters.Add("@activity", SqlDbType.SmallInt).Value = activityID;
                    return cmd.ExecuteReader().HasRows;
                }
            }
        }

        /// <summary>
        /// 不可為已販售 且 是自己的活動
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsSold(int ticketTypeID)
        {
            string query = @"select * from TicketType
                            where Sales>0 and ID = @id;";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = ticketTypeID;
                    return cmd.ExecuteReader().HasRows;
                }
            }
        }
        /// <summary>
        /// 只能將票種新增在有權限的活動。判斷用戶對該活動是否有權限
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public bool ActivityPermission(short activityID, short companyID)
        {
            string query = @"select * from Activity
                            WHERE ID = @activityID AND Company = @userID;";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.Add("@activityID", SqlDbType.SmallInt).Value = activityID;
                    cmd.Parameters.Add("@userID", SqlDbType.SmallInt).Value = companyID;
                    return cmd.ExecuteReader().HasRows;
                }
            }
        }

        public bool TicketTypePermission(int tickeTypeID, short companyID)
        {
            string query = @"select * from TicketType
                            join Activity
                            on TicketType.Activity = Activity.ID
                            where @id = TicketType.ID and @user = Activity.Company;";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = tickeTypeID;
                    cmd.Parameters.Add("@user", SqlDbType.SmallInt).Value = companyID;
                    return cmd.ExecuteReader().HasRows;
                }
            }
        }

        public List<TicketType> GetAllByActivityID(short activityID)
        {
            string query = @"SELECT * FROM TicketType
                            WHERE @activity = ACTIVITY;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.Add("@activity", SqlDbType.SmallInt).Value = activityID;
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<TicketType> result = new List<TicketType>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            TicketType entity = new TicketType();
                            entity.Id = (int)reader["id"];
                            entity.Name = (string)reader["name"];
                            entity.Activity = (short)reader["activity"];
                            entity.Price = (short)reader["price"];
                            entity.Description = reader["description"] == DBNull.Value ? "" : (string)reader["description"];
                            entity.Total = (short)reader["total"];
                            entity.Sales = (short)reader["sales"];
                            entity.LastModified = (DateTime)reader["LastModified"];
                            result.Add(entity);
                        }
                    }
                    return result;
                }
            }
        }
    }
}
