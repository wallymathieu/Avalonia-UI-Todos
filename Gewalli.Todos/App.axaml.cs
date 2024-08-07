using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DynamicData;
using DynamicData.Binding;
using Gewalli.Todos.Services;
using Gewalli.Todos.ViewModels;
using Gewalli.Todos.Views;

namespace Gewalli.Todos;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    // This is a reference to our MainViewModel which we use to save the list on shutdown. You can also use Dependency Injection 
    // in your App.
    private readonly MainViewModel _mainViewModel = new MainViewModel();

    public override async void OnFrameworkInitializationCompleted()
    {
        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                desktop.MainWindow = new MainWindow
                {
                    DataContext = _mainViewModel
                };

                // Listen to the ShutdownRequested-event
                desktop.ShutdownRequested += DesktopOnShutdownRequested;
                break;
            case ISingleViewApplicationLifetime singleView:
                singleView.MainView = new MainView
                {
                    DataContext = _mainViewModel
                };
                break;
        }

        base.OnFrameworkInitializationCompleted();
        // Another way could potentially be to use something inspired by : https://www.reactiveui.net/docs/handbook/data-persistence.html
        // Subscribe to changes in collection (invoke save items on task pool):
        ToDoChangeObserver.ObserveChangesInWindow(_mainViewModel.ToDoItems, TimeSpan.FromMilliseconds(200))
            .SubscribeOn(TaskPoolScheduler.Default)
            .Subscribe((x) => { SaveItems().ConfigureAwait(false).GetAwaiter().GetResult(); });

        // Init the MainViewModel 
        await InitMainViewModelAsync();
    }


    // We want to save our ToDoList before we actually shutdown the App. As File I/O is async, we need to wait until file is closed 
    // before we can actually close this window

    private bool _canClose; // This flag is used to check if window is allowed to close

    private async void DesktopOnShutdownRequested(object? sender, ShutdownRequestedEventArgs e)
    {
        e.Cancel = !_canClose; // cancel closing event first time

        if (_canClose) return;
        await SaveItems();

        // Set _canClose to true and Close this Window again
        _canClose = true;
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }

    private async Task SaveItems()
    {
        // To save the items, we map them to the ToDoItem-Model which is better suited for I/O operations
        var itemsToSave = _mainViewModel.ToDoItems.Select(item => item.GetToDoItem());

        await ToDoListFileService.SaveToFileAsync(itemsToSave);
    }

    // Optional: Load data from disc
    private async Task InitMainViewModelAsync()
    {
        // get the items to load
        var itemsLoaded = await ToDoListFileService.LoadFromFileAsync();

        if (itemsLoaded is not null)
        {
            var toDoItemViewModels = from i in itemsLoaded select new ToDoItemViewModel(i);
            _mainViewModel.ToDoItems.AddRange(toDoItemViewModels);
        }
    }
}