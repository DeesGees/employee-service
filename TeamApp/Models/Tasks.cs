namespace TeamApp.Models
{
    public class Tasks
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string  UserId { get; set; }
        public string TaskHeader { get; set; }
        public string TaskDescription { get; set; }
        public DateTime TaskAddedDate { get; set; }
        public DateTime? TaskStarted { get; set; }
        public DateTime? TaskEnded { get; set; }
        public int? TaskStatus { get; set; }
    }
}
