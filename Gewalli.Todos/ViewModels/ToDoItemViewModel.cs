using Gewalli.Todos.Models;
using ReactiveUI;

namespace Gewalli.Todos.ViewModels;

/// <summary>
/// This is a ViewModel which represents a <see cref="ToDoItem"/>
/// </summary>
public partial class ToDoItemViewModel : ViewModelBase
{
    /// <summary>
    /// Creates a new blank ToDoItemViewModel
    /// </summary>
    public ToDoItemViewModel()
    {
        // empty
    }
    
    /// <summary>
    /// Creates a new ToDoItemViewModel for the given <see cref="item"/>
    /// </summary>
    /// <param name="item">The item to load</param>
    public ToDoItemViewModel(ToDoItem item)
    {
        // Init the properties with the given values
        IsChecked = item.IsChecked;
        Content = item.Content;
    }
    
    /// <summary>
    /// Gets or sets the checked status of each item
    /// </summary>
    // NOTE: This property is made without source generator. Uncomment the line below to use the source generator
    private bool _isChecked;

    public bool IsChecked
    {
        get => _isChecked;
        set => this.RaiseAndSetIfChanged(ref _isChecked, value);
    }
    
    /// <summary>
    /// Gets or sets the content of the to-do item
    /// </summary>
    private string? _content;

    public string? Content
    {
        get => _content;
        set => this.RaiseAndSetIfChanged(ref _content, value);
    }

    /// <summary>
    /// Gets a ToDoItem of this ViewModel
    /// </summary>
    /// <returns>The ToDoItem</returns>
    public ToDoItem GetToDoItem()
    {
        return new ToDoItem
        {
            IsChecked = IsChecked,
            Content = Content
        };
    }
}