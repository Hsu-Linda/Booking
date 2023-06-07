using Booking.Models;
using Booking.Repositories;
using Microsoft.Data.SqlClient;
using System.ComponentModel.Design;
using Booking.Exceptions;
using Booking.Dtos;
using AutoMapper;
using System.Transactions;

namespace Booking.Services
{
    public interface IActivityService
    {
        IEnumerable<Activity> GetAll(short companyID);
        Activity GetInfoFromCompany(short id, short companyID);
        bool Add(AddActivityRequestDto activity, short companyID, out byte errorCode);
        bool Update(UpdateActivityRequest activity, short companyID);
        bool Delete(short id, short companyID);
    }

    public class ActivityService : IActivityService
    {
        private readonly ActivityRepository _activityRepository;
        private readonly IMapper _iMapper;
        private static readonly object _key = new object();
        public ActivityService(
            ActivityRepository activityRepository,
            IMapper iMapper)
        {
            _activityRepository = activityRepository;
            _iMapper = iMapper;
        }

        public IEnumerable<Activity> GetAll(short companyID)
        {
            return _activityRepository.GetAllFromCompany(companyID);
        }
        public IEnumerable<ActivityInfoResponse> SearchActivities(SearchCondition condition)
        {
            return _activityRepository.SearchActivities(condition);
        }
        public Activity GetInfoFromCompany(short id, short companyID)
        {
            return _activityRepository.GetInfoFromCompany(id, companyID);
        }
        public ActivityInfoResponse? GetInfo(short id)
        {
            return _activityRepository.GetInfo(id);
        }
        public bool Add(AddActivityRequestDto activity, short companyID, out byte errorCode)
        {
            return _activityRepository.Add(activity, companyID, out errorCode);
        }

        public bool Delete(short id, short companyID)
        {
            lock (_key)
            {
                if (_activityRepository.CanUpdate(id, companyID))
                {
                    _activityRepository.Delete(id);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool Update(UpdateActivityRequest activity, short companyID)
        {
            lock (_key)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (_activityRepository.CanUpdate(activity.Id, companyID))
                    {
                        _activityRepository.Update(activity);
                        scope.Complete();
                    }
                    else
                    {
                        if (activity.limit)
                        {
                            UpdateLimitActivityRequest limitActivity = _iMapper.Map<UpdateLimitActivityRequest>(activity);
                            _activityRepository.UpdateLimit(limitActivity);
                            scope.Complete();
                        }
                        else return false;
                    }
                    return true;
                }


            }
        }
    }
}
