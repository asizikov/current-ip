using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CurrentIp.DataModel;

namespace CurrentIp.Storage.Local
{
    public class InMemoryRecordsRepository : IRecordsRepository
    {
        public Task<IpRecord> GetLatestAsync(CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<IpRecord>> GetHistoryAsync(int maxItems, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        public Task<IpRecord> CreateAsync(Report record, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }
    }
}