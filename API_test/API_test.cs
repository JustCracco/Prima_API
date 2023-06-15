using Microsoft.EntityFrameworkCore;
using Moq;
using Prima_API;

namespace API_test
{
    public class API_test
    {
        DbContextOptions<ToDodb> GetOptions()
        {
            return new DbContextOptionsBuilder<ToDodb>()
                .UseInMemoryDatabase("UnitTest "+DateTime.UtcNow.Millisecond.ToString())
                .Options;
        }

        [Fact]
        public async Task GetAllItems_ReturnEmptyList()
        {
            await using var content = new ToDodb(GetOptions());
            ToDoService service = new ToDoService(content);

            Assert.Empty(await service.GetAllItemAsync());
        }

        [Fact]
        public async Task GetAllItems_ReturnAllItems_WhenConditionIsNull()
        {
            
            await using var content = new ToDodb(GetOptions());
            ToDoService service = new ToDoService(content);

            await service.AddItemAsync(new ToDo
            {
                name = "Test title 1",
                IsComplete = false
            });

           await service.AddItemAsync(new ToDo
            {
                name = "Test title 2",
                IsComplete = false
            });

            await service.AddItemAsync(new ToDo
            {
                name = "Test title 3",
                IsComplete = true
            });

            await service.AddItemAsync(new ToDo
            {
                name = "Test title 4",
                IsComplete = false
            });

            await content.SaveChangesAsync();
            var result = await service.GetAllItemAsync();
            Assert.Equal(4, result.Count);
        }

        [Fact]
        public async Task GetSingleItem_ReturnsObject_WhenIdMatches()
        {
            await using var content = new ToDodb(GetOptions());
            ToDoService service = new ToDoService(content);

            ToDo test = new ToDo
            {
                Id = 2,
                name = "Test title 2",
                IsComplete = false
            };

            await service.AddItemAsync(new ToDo
            {
                name = "Test title 1",
                IsComplete = false
            });

            await service.AddItemAsync(test);

            await content.SaveChangesAsync();

            var result = await service.GetItemAsync(2);

            Assert.Equal(test.name, result.name);
            Assert.Equal(test.Id, result.Id);
            Assert.Equal(test.IsComplete, result.IsComplete);
        }

        [Fact]
        public async Task GetItem_ReturnsNull_WhenObjectDoesNotExist()
        {
            await using var content = new ToDodb(GetOptions());
            ToDoService service = new ToDoService(content);

            await service.AddItemAsync(new ToDo
            {
                name = "Test title 1",
                IsComplete = false
            });

            await service.AddItemAsync(new ToDo
            {
                name = "Test title 2",
                IsComplete = false
            });

            await content.SaveChangesAsync();

            var result = await service.GetItemAsync(4);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllItem_ReturnsAllComplete_WhenParameterIsTrue()
        {
            await using var content = new ToDodb(GetOptions());
            ToDoService service = new ToDoService(content);

            await service.AddItemAsync(new ToDo
            {
                name = "Test title 1",
                IsComplete = false
            });

            await service.AddItemAsync(new ToDo
            {
                name = "Test title 2",
                IsComplete = false
            });

            await service.AddItemAsync(new ToDo
            {
                name = "Test title 3",
                IsComplete = true
            });

            await service.AddItemAsync(new ToDo
            {
                name = "Test title 4",
                IsComplete = false
            });

            await content.SaveChangesAsync();

            var result = await service.GetAllItemAsync(true);

            //Mi aspetto solo un elemento nella lista
            Assert.Single(result);
            // Mi aspetto siano tutti true
            Assert.True(result.All(a => a.IsComplete));
        }

        [Fact]
        public async Task GetAllItem_ReturnsAllNonComplete_WhenParameterIsFalse()
        {
            await using var content = new ToDodb(GetOptions());
            ToDoService service = new ToDoService(content);

            await service.AddItemAsync(new ToDo
            {
                name = "Test title 1",
                IsComplete = false
            });

            await service.AddItemAsync(new ToDo
            {
                name = "Test title 2",
                IsComplete = false
            });

            await service.AddItemAsync(new ToDo
            {
                name = "Test title 3",
                IsComplete = true
            });

            await service.AddItemAsync(new ToDo
            {
                name = "Test title 4",
                IsComplete = false
            });

            await content.SaveChangesAsync();

            var result = await service.GetAllItemAsync(false);

            // Mi aspetto 3 elementi perche ne ho inseriti 3 false
            Assert.Equal(3, result.Count);
            // Mi aspetto siano tutti false
            Assert.True(result.All(a => !a.IsComplete));


        }
    }
}