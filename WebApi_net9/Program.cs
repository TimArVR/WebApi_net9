using System.Reflection;
using WebApi_net9.BL;
namespace WebApi_net9
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            var database = new TestDatabase();

            //GET: Запрос данных с сервера
            app.MapGet("/tickets", () => 
            { 
                return Results.Ok(database.GetTickets()); //Results.Ok - по умолчанию, можно не писать. Это по сути HTTP код
            });
            //app.MapPost("/cart/", () => { return "Тест"; }); //Получить с большим количеством параметров

            //POST: Отправка данных на сервер
            //app.MapPost("/cart/{id}", () => { return "Тест"; });

            //PUT: Обновление/добавление всех данных на сервере
            app.MapPut("/ticket", (Ticket model) =>
            {
                int id = database.AddTicket(model);
                return new { id = id };
            });

            //PATCH: Пропатчить, частично обновить данные на сервере
            app.MapPatch("/ticket", (Ticket model) => 
            {
                try
                {
                    database.UpdateTicket(model);
                }
                catch (Exception) 
                {
                    return Results.NotFound();
                }
                return Results.Ok();
            }); //{id} не нужен, т.к. в модели есть Id

            //DELETE: Удаление данных на сервере
            app.MapDelete("/ticket", (int id) => 
            {
                database.DeleteTicket(id);
                return Results.Ok();
            });







            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    })
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast");


            app.Run();
        }
    }
}
