namespace TeamApp.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string TypeOfEqp { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public int InUse { get; set; }
        public string UserId { get; set; }
    }
}
