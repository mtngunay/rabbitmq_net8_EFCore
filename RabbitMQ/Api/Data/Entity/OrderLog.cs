namespace Api.Data.Entity
{
    public class OrderLog
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
