namespace ETIDrive_Entity.Log_Tables
{
    public class FolderLog
    {
        public int LogId { get; set; }
        public int FolderId { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
        public DateTime ActionDate { get; set; }
        public string IPAddress { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
