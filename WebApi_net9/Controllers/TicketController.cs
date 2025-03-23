using Microsoft.AspNetCore.Mvc;
using WebApi_net9.BL;

namespace WebApi_net9.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class TicketController : Controller
    {
        private static TestDatabase database = new TestDatabase();

        [HttpGet]
        [Route("tickets")]
        public IActionResult Tickets()
        {
            return new JsonResult(database.GetTickets());
        }

        [HttpGet]
        [Route("ticket/{id}")]
        public IActionResult Ticket(int id)
        {
            return new JsonResult(database.GetTickets().FirstOrDefault(x => x.Id == id));
        }

        [HttpPut]
        [Route("ticket")]
        public IActionResult TicketPut(Ticket model)
        {
            int id = database.AddTicket(model);
            return new JsonResult(new { id = id});
        }

        [HttpPatch]
        [Route("ticket")]
        public IActionResult TicketPatch(Ticket model)
        {
            database.UpdateTicket(model);
            return this.Ok();
        }

        [HttpDelete]
        [Route("ticket/{id}")]
        public IActionResult TicketDelete(int id)
        {
            database.DeleteTicket(id);
            return this.Ok();
        }

    }
}
