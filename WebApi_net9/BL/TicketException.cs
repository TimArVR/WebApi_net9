using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApi_net9.BL
{
    public class TicketException : Exception
    {
        public string Title { get; set; } = null!;
        public TicketException(string title)
        {
            this.Title = title;
        }
    }

    public class TicketNotFoundException : Exception
    {
        public string Title { get; set; } = null!;
        public TicketNotFoundException(string title)
        {
            this.Title = title;
        }
    }
}
