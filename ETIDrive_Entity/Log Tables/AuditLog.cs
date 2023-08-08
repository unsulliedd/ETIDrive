namespace ETIDrive_Entity.Log_Tables
{
    public class AuditLog
    {
        public int LogId { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
        public DateTime ActionDate { get; set; }
        public string IPAddress { get; set; }
        public string AffectedEntity { get; set; }
        public int AffectedEntityId { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
