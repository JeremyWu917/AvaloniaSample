using AvaloniaSample.Models;
using AvaloniaSample.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace AvaloniaSample.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        //public string Greeting => "Welcome to Avalonia!";

        ViewModelBase content;
        public ViewModelBase Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }

        //���캯�� ��ȡ���ݼ���
        public MainWindowViewModel(Database db)
        {
            content = List = new TodoListViewModel(db.GetItems());
        }

        //�Զ�ʵ��֪ͨ����
        public TodoListViewModel List { get; }

        public void AddItem()
        {
            var vm = new AddItemViewModel();

            Observable.Merge(
                vm.Ok,
                vm.Cancel.Select(_ => (TodoItem)null))
                .Take(1)
                .Subscribe(model =>
                {
                    if (model != null)
                    {
                        List.Items.Add(model);
                    }
                    Content = List;
                });

            Content = vm;
        }
    }
}
