using Microsoft.EntityFrameworkCore;
using Prima_API;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ToDodb>(opt => opt.UseInMemoryDatabase("ToDoList"));
builder.Services.AddScoped<IToDoService, ToDoService>();

var app = builder.Build();

app.MapGet("/todoitems", async (IToDoService toDoService) => await toDoService.GetAllItemAsync());

app.MapGet("/todoitems/complete", async (IToDoService toDoService) => await toDoService.GetAllItemAsync(true));

app.MapGet("/todoitems/{id}", async (int id, IToDoService toDoService) =>
{
    var todo = await toDoService.GetItemAsync(id);

    if (todo is null)
        return Results.NoContent();
    else
        return Results.Ok(todo);
});

app.MapPost("/todoitems", async (ToDo todo, IToDoService todoService) =>
{
    await todoService.AddItemAsync(todo);

    Results.Created($"/todoitems/{todo.Id}", todo);
});

app.MapPut("/todoitems/{id}", async (int id, ToDo inputTodo, IToDoService toDoService) =>
{
    var todo = await toDoService.UpdateItemAsync(id, inputTodo);

    if (todo is null) 
        return Results.NotFound();
    else 
        return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, IToDoService toDoService) =>
{
    var result = await toDoService.DeleteItemAsync(id);

    if (result is null)
        return Results.NotFound();
    else
        return Results.NoContent();
});

app.Run();
