namespace ArivalBank2FATask.Models
{
    public class ActiveCode
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
        public DateTime Timestamp { get; set; }
    }

}
