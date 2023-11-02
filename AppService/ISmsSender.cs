namespace ArivalBank2FATask.AppService
{
    public interface ISmsSender
    {
        void SendSms(string phoneNumber, string message);
    }

}
