using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Controls;
using Gewalli.Todos.Models;
using ReactiveUI;

namespace Gewalli.Todos.ViewModels;

/// <summary>
/// This is our MainViewModel in which we define the ViewModel logic to interact between our View and the TodoItems
/// </summary>
public partial class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
        // We can use this to add some items for the designer. 
        // You can also use a DesignTime-ViewModel
        if (Design.IsDesignMode)
        {
            ToDoItems = new ObservableCollection<ToDoItemViewModel>(new[]
            {
                new ToDoItemViewModel() { Content = "Hello" },
                new ToDoItemViewModel() { Content = "Avalonia", IsChecked = true }
            });
        }
        var canAddItem = this.ObservableForProperty(vm => vm.NewItemContent)
            .Select(change => !string.IsNullOrWhiteSpace(change.Value)).AsObservable();
        AddTodoCommand = ReactiveCommand.Create(AddItem, canAddItem);
        RemoveTodoCommand = ReactiveCommand.Create<ToDoItemViewModel>(RemoveItem);
    }


    /// <summary>
    /// Gets a collection of <see cref="ToDoItem"/> which allows adding and removing items
    /// </summary>
    public ObservableCollection<ToDoItemViewModel> ToDoItems { get; } = new ObservableCollection<ToDoItemViewModel>();


    // -- Adding new Items --
    /// <summary>
    /// This command is used to add a new Item to the List
    /// </summary>
    public ReactiveCommand<Unit, Unit> AddTodoCommand { get; }

    private void AddItem()
    {
        // Add a new item to the list
        ToDoItems.Add(new ToDoItemViewModel { Content = NewItemContent });

        // reset the NewItemContent
        NewItemContent = null;
    }

    /// <summary>
    /// Gets or set the content for new Items to add. If this string is not empty, the AddItemCommand will be enabled automatically
    /// </summary>
    private string? _newItemContent;

    public string? NewItemContent
    {
        get => _newItemContent;
        set => this.RaiseAndSetIfChanged(ref _newItemContent, value);
    }


    // -- Removing Items --
    /// <summary>
    /// Removes the given Item from the list
    /// </summary>
    public ReactiveCommand<ToDoItemViewModel, Unit> RemoveTodoCommand { get; }

    private void RemoveItem(ToDoItemViewModel item)
    {
        // Remove the given item from the list
        ToDoItems.Remove(item);
    }
}