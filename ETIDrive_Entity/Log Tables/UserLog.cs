namespace ETIDrive_Entity.Log
{
    public class UserLog
    {
        public int LogId { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
        public DateTime ActionDate { get; set; }
        public string IPAddress { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
