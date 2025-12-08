using MaktabGram.Domain.Core.UserAgg.Contracts.Otp;
using MaktabGram.Domain.Core.UserAgg.Entities;
using MaktabGram.Domain.Core.UserAgg.Enum;
using MaktabGram.Infrastructure.EfCore.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MaktabGram.Infrastructure.EfCore.Repositories.UserAgg
{
    public class OtpRepository(AppDbContext dbContext) : IOtpRepository
    {
        public async Task Create(string mobile, int code, OtpTypeEnum type, CancellationToken cancellationToken)
        {
            var entity = new Otp
            {
                Mobile = mobile,
                Code = code,
                Type = type,
                SendAt = DateTime.Now,
                IsUsed = false
            };

            await dbContext.Otps.AddAsync(entity);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task SetUsed(string mobile, int code, OtpTypeEnum otpType, CancellationToken cancellationToken)
        {
            await dbContext.Otps
                .Where(o => o.Code == code
                    && o.Mobile == mobile
                    && o.Type == otpType)
                .ExecuteUpdateAsync(setters => setters.SetProperty(u => u.IsUsed, true), cancellationToken);
        }


        public async Task<bool> Verify(string mobile, int code, OtpTypeEnum otpType, CancellationToken cancellationToken)
        {
            var record = await dbContext.Otps.FirstOrDefaultAsync(o => o.IsUsed == false
            && o.Code == code
            && o.Mobile == mobile
            && o.Type == otpType);

            if (record is null) return false;

            return record.SendAt.AddMinutes(2) > DateTime.Now;
        }
    }
}
