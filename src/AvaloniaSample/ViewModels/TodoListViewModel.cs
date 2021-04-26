using AvaloniaSample.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AvaloniaSample.ViewModels
{
    public class TodoListViewModel : ViewModelBase
    {
        // 构造函数
        public TodoListViewModel(IEnumerable<TodoItem> items)
        {
            Items = new ObservableCollection<TodoItem>(items);
        }

        // 通知属性 类集合
        public ObservableCollection<TodoItem> Items { get; set; }

    }
}
