using Booking.Models;
using Booking.Repositories;
using Microsoft.Data.SqlClient;
using System.ComponentModel.Design;

namespace Booking.Services
{
    public interface IActivityService
    {
        void Add(Activity activity, short companyID);
        void UpdateAsync(Activity activity);
        void Delete(int id);
    }
    public class ActivityService : IActivityService
    {
        private readonly ActivityRepository _activityRepository;
        public ActivityService(ActivityRepository activityRepository)
        {
            _activityRepository= activityRepository;
        }
        public void Add(Activity activity, short companyID)
        {
            //try
            //{
                _activityRepository.Add(activity, companyID);

            //}
            //catch (SqlException sqlex)
            //{
            //    if (sqlex.Number == 5001)
            //    {
            //        Console.WriteLine("-------------");
            //        Console.WriteLine(sqlex.Message);
            //        //throw new ArgumentException("name cannot same", nameof(Activity.Name));
            //    }
            //}
            //catch (Exception ex)
            //{
            //    res = new ResponseDto()
            //    {
            //        message = ex.Message
            //    };
            //    return false;
            //}
        }

        public void Delete(int id)
        {
            //try 
            //{
            //    if (_activityRepository.UpdateDeleteDataCheck(id, companyID))
            //    {
            //        return false;
            //    }
            //    _activityRepository.Delete(id);
            //    return true;
            //}
            //catch(Exception ex)
            //{
            //    return false;
            //}


            throw new NotImplementedException();
        }

        public void UpdateAsync(Activity activity)
        {
            throw new NotImplementedException();
        }
    }
}
