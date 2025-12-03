using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaktabGram.Domain.Core.FollowerAgg.Contracts
{
    public interface IFollowerApplicationService
    {
        Task Follow(int userId, int followedId, CancellationToken cancellationToken);
        Task UnFollow(int userId, int followedId, CancellationToken cancellationToken);
    }

}
