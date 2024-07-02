using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Gewalli.Todos.ViewModels;
using Gewalli.Todos.Views;

namespace Gewalli.Todos.Tests;

public class GivenMainView
{
    private readonly MainViewModel _viewModel;
    private readonly MainView _mainView;
    private readonly Window _window;

    public GivenMainView()
    {
        _viewModel = new MainViewModel();
        _mainView = new MainView { DataContext = _viewModel };
        _window = new Window { Content = _mainView };
    }

    [AvaloniaFact]
    public void When_typing_new_todo_and_hitting_enter_Should_add_new_todo()
    {
        _window.Show();

        var newTodo = _mainView.FindControl<TextBox>("NewTodo");

        Assert.NotNull(newTodo);
        newTodo.Focus();

        const string inputText = "hello todo";
        _window.KeyTextInput(inputText);
        Assert.Equal(inputText, newTodo.Text);

        _window.KeyPressQwerty(PhysicalKey.Enter, RawInputModifiers.None);

        var todoItem = Assert.Single(_viewModel.ToDoItems);
        Assert.Equal(inputText, todoItem.Content);
    }
}