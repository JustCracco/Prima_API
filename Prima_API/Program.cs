using Microsoft.EntityFrameworkCore;
using Prima_API;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ToDodb>(opt => opt.UseInMemoryDatabase("ToDoList"));
var app = builder.Build();

app.MapGet("/todoitems", async (ToDodb db) => await db.Todos.ToListAsync());

app.MapGet("/todoitems/complete", async (ToDodb db) => await db.Todos.Where(t => t.IsComplete).ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, ToDodb db) => await db.Todos.FindAsync(id) is ToDo todo ? Results.Ok(todo) : Results.NotFound());

app.MapPost("/todoitems", async (ToDo todo, ToDodb db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", todo);
});

app.MapPut("/todoitems/{id}", async (int id, ToDo inputTodo, ToDodb db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if(todo is null) return Results.NotFound();

    todo.name = inputTodo.name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, ToDodb db) =>
{
    if(await db.Todos.FindAsync(id) is ToDo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();
