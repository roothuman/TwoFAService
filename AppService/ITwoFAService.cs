namespace ArivalBank2FATask.AppService
{
    public interface ITwoFAService
    {
        bool SendConfirmationCode(string phone);
        public bool VerifyConfirmationCode(string phoneNumber, string code);

    }
}
