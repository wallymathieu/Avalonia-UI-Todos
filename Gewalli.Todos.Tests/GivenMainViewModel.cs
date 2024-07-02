using System.Reactive.Linq;
using Gewalli.Todos.ViewModels;

namespace Gewalli.Todos.Tests;

public class GivenMainViewModel
{
    private readonly MainViewModel _viewModel = new();

    const string InputText = "hello todo";

    [Fact]
    public async Task CanAddNewToDo()
    {
        _viewModel.NewItemContent = InputText;
        await _viewModel.AddTodoCommand.Execute();
        var todoItem = Assert.Single(_viewModel.ToDoItems);
        Assert.Equal(InputText, todoItem.Content);
    }
    
    [Fact]
    public async Task CanAddAndRemoveNewToDo()
    {
        _viewModel.NewItemContent = InputText;
        await _viewModel.AddTodoCommand.Execute();
        var todoItem = Assert.Single(_viewModel.ToDoItems);
        await _viewModel.RemoveTodoCommand.Execute(todoItem);
        Assert.Empty(_viewModel.ToDoItems);
    }
}