using MaktabGram.Domain.Core.UserAgg.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaktabGram.Domain.Core.UserAgg.Contracts.Otp
{
    public interface IOtpRepository
    {
        public Task Create(string mobile, int code, OtpTypeEnum type,CancellationToken cancellationToken);
        public Task<bool> Verify(string mobile, int code, OtpTypeEnum otpType , CancellationToken cancellationToken);
        public Task SetUsed(string mobile, int code, OtpTypeEnum otpType , CancellationToken cancellationToken);
    }
}
