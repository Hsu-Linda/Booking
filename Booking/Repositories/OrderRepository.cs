using Booking.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Booking.Repositories
{
    public interface IOrderRepository
    {
        List<Order> GetAll(short memberID);
        short CheckRemainingQuantity(int ticketTypeID);
        void ReduceTicket(int ticketTypeID, short purchaseQuantity);
        short CreateOrder(short memberID, string items);
        void CreateTicket(int ticketTypeID, short memberID, short orderID);

        Order GetOrderDetail(int orderID, short memberID);
    }
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;
        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("BookingDatabase");
        }

        public short CheckRemainingQuantity(int ticketTypeID)
        {
            string query = @"select remaining from TicketTypeRemainingView
                            where @ticketTypeid = id ;";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.Add("@ticketTypeid", SqlDbType.Int).Value = ticketTypeID;
                    return (short)command.ExecuteScalar();
                }
            }
        }

        public short CreateOrder(short memberID, string items)
        {
            string query = @"Insert into [Order]
                            Values(@userID,GETDATE(),0,@items);
                            select CAST(IDENT_CURRENT('Order') AS smallint);";


            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.Add("@userID", SqlDbType.SmallInt).Value = memberID;
                    cmd.Parameters.Add("@items", SqlDbType.NVarChar).Value = items;
                    cmd.ExecuteNonQuery();

                    return (short)cmd.ExecuteScalar();
                }
            }
        }

        public void CreateTicket(int ticketTypeID, short memberID, short orderID)
        {
            string query = @"Insert into Ticket
                            Values((select Activity from TicketType where @ticketTypeid = ID), @ticketTypeid, @orderID, @userID, 0, GETDATE());";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.Add("@ticketTypeid", SqlDbType.Int).Value = ticketTypeID;
                    cmd.Parameters.Add("@userID", SqlDbType.SmallInt).Value = memberID;
                    cmd.Parameters.Add("@orderID", SqlDbType.SmallInt).Value = orderID;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Order> GetAll(short memberID)
        {
            string query = @"select * from [Order]
                            where @userID = Member;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.Add("@userID", SqlDbType.SmallInt).Value = memberID;
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Order> orders = new List<Order>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Order entity = new Order();
                            entity.Id = (int)reader["id"];
                            entity.Member = (short)reader["member"];
                            entity.Trading = (DateTime)reader["trading"];
                            entity.Status = (byte)reader["status"];
                            orders.Add(entity);
                        }
                    }
                    return orders;
                }
            }
        }

        public Order GetOrderDetail(int orderID, short memberID)
        {
            string query = @"SELECT * FROM [Order]
                            WHERE @orderID = ID AND @userID = Member;";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.Add("@orderID", SqlDbType.Int).Value = orderID;
                    cmd.Parameters.Add("@userID", SqlDbType.SmallInt).Value = memberID;
                    SqlDataReader reader = cmd.ExecuteReader();
                    Order result = new Order();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Member = (short)reader["member"];
                            result.Id = (int)reader["id"];
                            result.Trading = (DateTime)reader["Trading"];
                            result.Status = (byte)reader["status"];
                        }
                    }
                    return result;
                }
            }
            throw new NotImplementedException();
        }

        public void ReduceTicket(int ticketTypeID, short purchaseQuantity)
        {
            string query = @"update TicketType
                                set Sales = Sales + @purchaseQuantity, LastModified = GETDATE()
                                where @ticketTypeid = ID;";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.Add("@ticketTypeid", SqlDbType.Int).Value = ticketTypeID;
                    cmd.Parameters.Add("@purchaseQuantity", SqlDbType.SmallInt).Value = purchaseQuantity;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
