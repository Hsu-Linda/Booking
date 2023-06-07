using Azure.Core;
using Booking.Dtos;
using Booking.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System.ComponentModel.Design;
using System.Data;
using Booking.Exceptions;
using System.Reflection.PortableExecutable;
using Microsoft.IdentityModel.Tokens;
using Dapper;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;

namespace Booking.Repositories
{
    public interface IActivityReporitory
    {
        /// <summary>
        /// 取得公司的所有活動
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        IEnumerable<Activity> GetAllFromCompany(short companyID);
        /// <summary>
        /// 取得活動詳情
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        ActivityInfoResponse GetInfo(short ID);
        Activity GetInfoFromCompany(short ID, short companyID);
        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="companyID"></param>
        bool Add(AddActivityRequestDto activity, short companyID, out byte errorCode);
        /// <summary>
        /// 刪除資料 active = 0 和 lastModified修改
        /// </summary>
        /// <param name="delete"></param>
        bool Delete(short id);
        /// <summary>
        /// 修改資料 全部修改
        /// </summary>
        /// <param name="activity"></param>
        bool Update(UpdateActivityRequest activity);
        /// <summary>
        /// 修改資料 限制修改
        /// </summary>
        /// <param name="activity"></param>
        bool UpdateLimit(UpdateLimitActivityRequest activity);

        /// <summary>
        /// 是否賣出過票券(當要刪除 或 修改時需確認)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="activityID"></param>
        /// <returns></returns>
        bool CanUpdate(short id, short companyID);
        /// <summary>
        /// 確認是否有company一樣 名字一樣
        /// </summary>
        /// <param name="id"></param>
        /// <param name="activityID"></param>
        /// <returns></returns>
       
    }
    public class ActivityRepository : IActivityReporitory
    {
        private readonly string _connectionString;
        private readonly IMemoryCache _iMemory;


        public ActivityRepository(
            IConfiguration configuration,
            IMemoryCache iMemory
            )
        {
            _connectionString = configuration.GetConnectionString("BookingDatabase");
            _iMemory = iMemory;
        }

        public IEnumerable<ActivityInfoResponse> SearchActivities(SearchCondition condition)
        {
            string query = @"SELECT [Name]
                                ,[Company]
                                ,[PhotoURL]
                                ,[City]
                                ,[District]
                                ,[Address]
                                ,[ShowingStart]
                                ,[ShowingEnd]
                                ,[SalesStart]
                                ,[SalesEnd]
                                FROM Activity
                                WHERE 1 = ACTIVE";

            var parameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(condition.keyWord))
            {
                query += "AND (Name LIKE '%' + @Keyword + '%')";
                parameters.Add("@Keyword", condition.keyWord, DbType.String, size: (15));
            }

            if (condition.companyID != null)
            {
                query += "AND @company = Company";
                parameters.Add("@company", condition.companyID, DbType.Int16);
            }

            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                return connection.Query<ActivityInfoResponse>(query, parameters);
            }
        }
        public IEnumerable<Activity> GetAllFromCompany(short companyID)
        {
            string query = @"SELECT *
                            FROM Activity 
                            WHERE @user = Company
                            AND 1 = ACTIVE;";
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {   
                var parameters = new DynamicParameters();
                parameters.Add("@user", companyID, DbType.Int16);

                return connection.Query<Activity>(query, parameters);
            }
        }
      
        public ActivityInfoResponse? GetInfo(short ID)
        {
            string query = @"SELECT [Name]
                                  ,[Company]
                                  ,[PhotoURL]
                                  ,[City]
                                  ,[District]
                                  ,[Address]
                                  ,[ShowingStart]
                                  ,[ShowingEnd]
                                  ,[SalesStart]
                                  ,[SalesEnd]
                              FROM [Activity]
                              WHERE 1= [Active]
                              AND @activityID = [ID];";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@activityID", ID, DbType.Int16);
                return connection.QuerySingleOrDefault<ActivityInfoResponse>(query, parameters);
            }
        }

        public Activity GetInfoFromCompany(short ID, short companyID)
        {
            string query = @"SELECT *
                             FROM Activity
                             WHERE @user = Company
                             AND @id = ID";
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", ID, DbType.Int16);
                parameters.Add("@user", companyID, DbType.Int16);
                
                return connection.QuerySingleOrDefault<Activity>(query, parameters);
            }
        }

        public bool Add(AddActivityRequestDto activity, short companyID, out byte errorCode )
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@name", activity.Name, DbType.String, size: 15);
                parameters.Add("@user", companyID, DbType.Int16);
                parameters.Add("@photoURL", activity.PhotoUrl, DbType.String, size: 2083);
                parameters.Add("@city", activity.City, DbType.Byte);
                parameters.Add("@district", activity.District, DbType.Int16);
                parameters.Add("@address", activity.Address, DbType.String,size: 100);
                parameters.Add("@showingStart", activity.ShowingStart, DbType.DateTime);
                parameters.Add("@showingEnd", activity.ShowingEnd, DbType.DateTime);
                parameters.Add("@salesStart", activity.SalesStart, DbType.DateTime);
                parameters.Add("@salesEnd", activity.SalesEnd, DbType.DateTime);
                parameters.Add("@errorCode", 0, DbType.Byte, direction: ParameterDirection.Output);
                parameters.Add("@result", null, DbType.Byte, direction: ParameterDirection.Output);

                connection.Execute("AddActivity", parameters, commandType: CommandType.StoredProcedure);
                errorCode = parameters.Get<byte>("@errorCode");
                return 1 == parameters.Get<byte>("@result");
            }
        }

        public bool Delete(short id)
        {
            string query = @"UPDATE Activity
                            SET Active = 0
                               ,LastModified = GETDATE()
                            WHERE @id = ID;";
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", id, DbType.Int16);

                return 1 == connection.Execute(query, parameters);
            }
        }


        public bool Update(UpdateActivityRequest activity)
        {
            string query = @"UPDATE Activity
                            SET Name = @name,
	                            PhotoURL = @PhotoUrl,
	                            City = @CITY,
	                            District = @district,
	                            Address = @address,
	                            SalesStart = @salesStart, 
	                            SalesEnd = @salesEnd, 
	                            ShowingStart = @showingStart,
	                            ShowingEnd = @showingEnd,
	                            LastModified = GETDATE()
                            WHERE @ID =ID;";

            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ID", activity.Id, DbType.Int16);
                parameters.Add("@name", activity.Name, DbType.String);
                parameters.Add("@photoURL", activity.PhotoUrl, DbType.String);
                parameters.Add("@city", activity.City, DbType.Byte);
                parameters.Add("@district", activity.District, DbType.Int16);
                parameters.Add("@address", activity.Address, DbType.String);
                parameters.Add("@showingStart", activity.ShowingStart, DbType.DateTime);
                parameters.Add("@showingEnd", activity.ShowingEnd, DbType.DateTime);
                parameters.Add("@salesStart", activity.SalesStart, DbType.DateTime);
                parameters.Add("@salesEnd", activity.SalesEnd, DbType.DateTime);

                return 1 == connection.Execute(query, parameters);
            }
        }

        public bool UpdateLimit(UpdateLimitActivityRequest activity)
        {
            string query = @"UPDATE Activity
                            SET Name = @name,
	                            PhotoURL = @PhotoUrl,
	                            City = @CITY,
	                            District = @district,
	                            Address = @address,
	                            SalesStart = @salesStart, 
	                            SalesEnd = @salesEnd, 
	                            ShowingStart = @showingStart,
	                            ShowingEnd = @showingEnd,
	                            LastModified = GETDATE()
                            WHERE @ID =ID;";
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ID", activity.Id, DbType.Int16);
                parameters.Add("@name", activity.Name, DbType.String);
                parameters.Add("@photoURL", activity.PhotoUrl, DbType.String);
                parameters.Add("@city", activity.City, DbType.Byte);
                parameters.Add("@district", activity.District, DbType.Int16);
                parameters.Add("@address", activity.Address, DbType.String);
                parameters.Add("@showingStart", activity.ShowingStart, DbType.DateTime);
                parameters.Add("@showingEnd", activity.ShowingEnd, DbType.DateTime);
                parameters.Add("@salesStart", activity.SalesStart, DbType.DateTime);
                parameters.Add("@salesEnd", activity.SalesEnd, DbType.DateTime);

                return 1 == connection.Execute(query, parameters);
            }
        }

        public bool CanUpdate(short id, short companyID)
        {
            if(_iMemory.TryGetValue(id, out bool result))
            {
                return result;
            }

            string query = @"SELECT SALES
                            FROM ActivityView 
                            WHERE @id = ID
                            AND @user = Company";

            using (var connection = new SqlConnection(_connectionString))
            {

                var parameters = new DynamicParameters();
                parameters.Add("@id", id, DbType.Int16);
                parameters.Add("@user", companyID, DbType.Int16);

                if(Int32.TryParse(connection.ExecuteScalar(query, parameters) + "", out int salesValue))
                {
                    bool canUpdate = (0 == (int)connection.ExecuteScalar(query, parameters));
                    _iMemory.Set(id, canUpdate);
                    return canUpdate;
                }
                return false;
            }
        }
    }
}
