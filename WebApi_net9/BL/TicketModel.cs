namespace WebApi_net9.BL
{
    public enum TicketStatus { Draft, Published }
    public class Ticket
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TicketStatus TicketStatus { get; set; }
    }
}
