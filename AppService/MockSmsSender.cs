namespace ArivalBank2FATask.AppService
{
    public class MockSmsSender : ISmsSender
    {
        public void SendSms(string phoneNumber, string message)
        {
            Console.WriteLine($"Sending SMS to {phoneNumber}: {message}");
        }
    }

}
