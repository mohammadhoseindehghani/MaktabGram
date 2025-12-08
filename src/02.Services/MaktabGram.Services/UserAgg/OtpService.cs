using MaktabGram.Domain.Core.UserAgg.Contracts.Otp;
using MaktabGram.Domain.Core.UserAgg.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaktabGram.Domain.Services.UserAgg
{
    public class OtpService(IOtpRepository otpRepository) : IOtpService
    {
        public async Task Create(string mobile, int code, OtpTypeEnum type, CancellationToken cancellationToken)
        {
           await otpRepository.Create(mobile, code, type, cancellationToken);
        }

        public async Task SetUsed(string mobile, int code, OtpTypeEnum otpType, CancellationToken cancellationToken)
        {
            await otpRepository.Create(mobile, code, otpType, cancellationToken);
        }

        public async Task<bool> Verify(string mobile, int code, OtpTypeEnum otpType, CancellationToken cancellationToken)
        {
            return await otpRepository.Verify(mobile, code, otpType, cancellationToken);
        }
    }
}
