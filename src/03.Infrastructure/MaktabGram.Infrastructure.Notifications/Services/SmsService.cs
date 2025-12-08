namespace MaktabGram.Infrastructure.Notifications.Services
{
    public class SmsService : ISmsService
    {
        public async Task Send(string number, string message)
        {
            var sender = "2000660110";
            var api = new Kavenegar.KavenegarApi("6D4A41526A632B304B4668436D4B59617953787654764738792B4A6A5A512F4D375A5471433354663262383D");
            var result = await api.Send(sender, number, message);
        }

        public async Task<int> SendOTP(string number)
        {
            var otp = GenerateOTP();
            var message = $"کد ورود شما : {otp}";


            var sender = "2000660110";
            var api = new Kavenegar.KavenegarApi("6D4A41526A632B304B4668436D4B59617953787654764738792B4A6A5A512F4D375A5471433354663262383D");
            var result = await api.Send(sender, number, message);

            return otp;

        }

        private int GenerateOTP()
        {
            Random rnd = new Random();
            return rnd.Next(10000, 100000);
        }
    }
}
