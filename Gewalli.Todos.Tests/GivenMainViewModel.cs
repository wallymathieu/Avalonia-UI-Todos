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
        var todoItem = await CreateToDo();
        Assert.Equal(InputText, todoItem.Content);
    }

    [Fact]
    public async Task CanAddAndRemoveNewToDo()
    {
        var todoItem = await CreateToDo();
        await _viewModel.RemoveTodoCommand.Execute(todoItem);
        Assert.Empty(_viewModel.ToDoItems);
    }

    private async Task<ToDoItemViewModel> CreateToDo()
    {
        _viewModel.NewItemContent = InputText;
        await _viewModel.AddTodoCommand.Execute();
        var todoItem = Assert.Single(_viewModel.ToDoItems);
        return todoItem;
    }
}