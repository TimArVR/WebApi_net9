using System.Net.Sockets;

namespace WebApi_net9.BL
{
    public class TestDatabase
    {
        private static List<Ticket> tickets = new List<Ticket>();
        public IEnumerable<Ticket> GetTickets()
        {
            return tickets;
        }
        public int AddTicket(Ticket ticket)
        {
            ticket.Id = tickets.Count == 0 ? 1 : tickets[tickets.Count - 1].Id + 1;
            tickets.Add(ticket);
            return ticket.Id;
        }

        public void UpdateTicket(Ticket ticket)
        {
            //Полученный объект ищем в базе по Id
            Ticket? t = tickets.Where(x => x.Id == ticket.Id).FirstOrDefault();
            //Если объект не найден - выбросим исключение
            if (t == null)
            {
                throw new Exception("Такой тикет не найден");
            }
            //Если объект найден - обновим его
            t.Title = ticket.Title;
            t.Description = ticket.Description;
            t.TicketStatus = ticket.TicketStatus;
        }
        public void DeleteTicket(int id)
        {
            Ticket? t = tickets.Where(x => x.Id == id).FirstOrDefault();
            //Если объект не найден - выбросим исключение
            if (t == null)
            {
                throw new Exception("Такой тикет не найден");
            }
            tickets.Remove(t);
        }
    }
}
