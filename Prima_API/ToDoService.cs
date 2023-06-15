using Microsoft.EntityFrameworkCore;

namespace Prima_API
{
    public interface IToDoService 
    {
        public Task<ToDo> AddItemAsync(ToDo NewTodo);

        public Task<ToDo> UpdateItemAsync(int id, ToDo UpdateTodo);

        public Task<ToDo> DeleteItemAsync(int id);

        public Task<ToDo> GetItemAsync(int id);

        public Task<List<ToDo>> GetAllItemAsync(bool? getBool = null);
    }

    public class ToDoService : IToDoService
    {
        private readonly ToDodb db;

        public ToDoService(ToDodb db) 
        {
            this.db = db;
        }

        public async Task<ToDo> AddItemAsync(ToDo NewTodo)
        {
            db.Todos.Add(NewTodo);

            await db.SaveChangesAsync();

            return NewTodo;
        }

        public async Task<ToDo> UpdateItemAsync(int id, ToDo UpdateTodo)
        {
            var todo = await db.Todos.FindAsync(id);

            if (todo is null) return null;

            todo.name = UpdateTodo.name;
            todo.IsComplete = UpdateTodo.IsComplete;

            await db.SaveChangesAsync();

            return todo;
        }

        public async Task<ToDo> DeleteItemAsync(int id)
        {
            if (await db.Todos.FindAsync(id) is ToDo todo)
            {
                db.Todos.Remove(todo);
                await db.SaveChangesAsync();
                return todo;
            }

            return null;
        }

        public async Task<ToDo> GetItemAsync(int id)
        {
            var todo = await db.Todos.FindAsync(id);
            return todo;

        }

        public Task<List<ToDo>> GetAllItemAsync(bool? getBool = null)
        {
            if (getBool.HasValue)
            {
                if (getBool.Value)
                    return db.Todos.Where(t => t.IsComplete).ToListAsync();
                else
                    return db.Todos.Where(t => !t.IsComplete).ToListAsync();
            }
            else
                return db.Todos.ToListAsync();
        }
    }
}
