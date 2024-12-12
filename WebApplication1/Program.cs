using WebApplication1;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};


var todos = new List<Todo>();

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");


app.MapGet("/todoitems/{id}", (int id) =>
    {
       // var todo = todos.Where(x => x.Id == id).FirstOrDefault();

        foreach (var item in todos)
        {
            if (item.Id == id)
            {
                return item.Name;
            }
          
        }
       
        return "item not found";
    });
    //.WithName("asaapi");

app.MapPost("/todoitems", async (Todo todo) =>
{
    todos.Add(todo);
    return Results.Created($"/todoitems/{todo.Id}", todo);
});


app.MapDelete("/todoitems/{id}", async (int id) =>
{
    var todo = todos.Where(x => x.Id == id).FirstOrDefault();
    if (todo is not null)
    {
        todos.Remove(todo);
        return Results.NoContent();
    }
    return Results.NotFound();
});

app.MapPut("/todoitems/{id}", async (int id, Todo inputTodo) =>
{
    return Results.NoContent();
});



app.Run();


record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}