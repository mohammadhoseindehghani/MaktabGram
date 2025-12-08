using MaktabGram.Domain.Core.UserAgg.Contracts.Otp;
using MaktabGram.Domain.Core.UserAgg.Enum;

namespace MaktabGram.Domain.ApplicationServices.UserAgg
{
    public class OtpApplicationService (IOtpService otpService) : IOtpApplicationService
    {
        public async Task Create(string mobile, int code, OtpTypeEnum type, CancellationToken cancellationToken)
        {
            await otpService.Create(mobile,code,type,cancellationToken);    
        }

        public async Task<bool> Verify(string mobile, int code, OtpTypeEnum otpType, CancellationToken cancellationToken)
        {
            return await otpService.Verify(mobile,code,otpType,cancellationToken);
        }
    }
}
