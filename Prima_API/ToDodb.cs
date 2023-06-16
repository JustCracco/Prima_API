using Microsoft.EntityFrameworkCore;

namespace Prima_API
{
    public class ToDodb : DbContext
    {
        public ToDodb(DbContextOptions options) : base(options) { }

        public DbSet<ToDo> Todos => Set<ToDo>();
    }
}
