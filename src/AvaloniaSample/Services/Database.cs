using AvaloniaSample.Models;
using System.Collections.Generic;

namespace AvaloniaSample.Services
{
    //fake database
    public class Database
    {
        public IEnumerable<TodoItem> GetItems() => new[]
        {
            new TodoItem { Description = "Walk the dog" },
            new TodoItem { Description = "Buy some milk" },
            new TodoItem { Description = "Learn WPF", IsChecked = true },
        };
    }
}
