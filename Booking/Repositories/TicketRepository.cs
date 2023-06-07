using Booking.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Booking.Repositories
{
    public interface ITicketRepository
    {
        List<Ticket> GetTicketsByOrder(int orderId, short memberID);
        List<Ticket> GetTicketsByMemberID(short memberID);
    }
    public class TicketRepository : ITicketRepository
    {
        private readonly string _connectionString;
        public TicketRepository
        (
            IConfiguration configuration
        )
        {
            _connectionString = configuration.GetConnectionString("BookingDatabase");
        }

        public List<Ticket> GetTicketsByMemberID(short memberID)
        {
            string query = @"SELECT * FROM Ticket
                            WHERE  @userID = Member;";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.Add("@userID", SqlDbType.SmallInt).Value = memberID;
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Ticket> result = new List<Ticket>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Ticket entity = new Ticket();
                            entity.Id = (int)reader["id"];
                            entity.Activity = (short)reader["activity"];
                            entity.TicketType = (int)reader["ticketType"];
                            entity.Status = (byte)reader["status"];
                            entity.LastModified = (DateTime)reader["lastModified"];
                            result.Add(entity);
                        }
                    }
                    return result;
                }
            }
            throw new NotImplementedException();
        }

        public List<Ticket> GetTicketsByOrder(int orderId, short memberID)
        {
            string query = @"SELECT * FROM Ticket
                            WHERE @orderID = OrderID AND @userID = Member;";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.Add("@orderID", SqlDbType.Int).Value = orderId;
                    cmd.Parameters.Add("@userID", SqlDbType.SmallInt).Value = memberID;
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Ticket> result = new List<Ticket>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Ticket entity = new Ticket();
                            entity.Id = (int)reader["id"];
                            entity.Activity = (short)reader["activity"];
                            entity.TicketType = (int)reader["ticketType"];
                            entity.Status = (byte)reader["status"];
                            entity.LastModified = (DateTime)reader["lastModified"];
                            result.Add(entity);
                        }
                    }
                    return result;
                }
            }
        }
    }
}
