using BL.Api;
using BL.Models;
using Dal.Api;

namespace BL.Services
{
    public class BlServiceBucketService : IBlServiceBucket
    {
        private readonly IDalServiceBucket _dal;

        public BlServiceBucketService(IDalServiceBucket dal)
        {
            _dal = dal;
        }

        public BlServiceBucket CastingServiceBucketFromBlToDal(BlServiceBucket? e)
        {
            throw new NotImplementedException();
        }

        public Task<BlServiceBucket> Create(BlServiceBucket item)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BlServiceBucket>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BlServiceBucket> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BlServiceBucket> Update(BlServiceBucket item)
        {
            throw new NotImplementedException();
        }
    }
}
