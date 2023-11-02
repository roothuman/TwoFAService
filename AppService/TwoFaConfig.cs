namespace ArivalBank2FATask.AppService
{
    public class TwoFaConfig
    {
        public int CodeLifetimeMinutes { get; set; }
        public int MaxConcurrentCodesPerPhone { get; set; }
    }
}
