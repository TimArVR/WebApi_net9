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

            //Добавляем детализацию Problem
            builder.Services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Extensions.TryAdd("dateTime", DateTime.Now.ToString());
                    //Можно добавить в Problem также информацию из HttpContext
                    //(там вся информация которая есть в запросе) как Request так и Response
                    context.ProblemDetails.Extensions.TryAdd("method", context.HttpContext.Request.Method);
                    context.ProblemDetails.Extensions.TryAdd("path", context.HttpContext.Request.Path);
                    context.ProblemDetails.Extensions.TryAdd("queryString", context.HttpContext.Request.QueryString);
                };
            });

            //Регистрируем IExceptionHandler
            builder.Services.AddExceptionHandler<WebapiExceptionHandler>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            //Регистрируем IExceptionHandler
            app.UseExceptionHandler();

            app.UseAuthorization();

            var database = new TestDatabase();

            //GET: Запрос данных с сервера
            app.MapGet("/tickets", () =>
            {
                return Results.Ok(database.GetTickets());
                //Results.Ok - по умолчанию, можно не писать. Это по сути HTTP код
                //Сейчас рекомендуется возвращать Problem
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
                database.UpdateTicket(model);
                return Results.Ok();

                //try
                //{
                //    database.UpdateTicket(model);
                //}
                //catch (Exception)
                //{
                //    return Results.Problem("Этот тикет не найден",
                //        statusCode: StatusCodes.Status404NotFound, title: "Искомое не нашли");
                //    //Сейчас рекомендуется возвращать Problem, причем детализацию Проблемы можно увеличить (см. выше строку 19)
                //}
                //return Results.Ok();
            }); //{id} не нужен, т.к. в модели есть Id

            //DELETE: Удаление данных на сервере
            app.MapDelete("/ticket/{id}", (int id) =>
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
