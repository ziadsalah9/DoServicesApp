namespace DoServicesApp.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string ServiceId { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
