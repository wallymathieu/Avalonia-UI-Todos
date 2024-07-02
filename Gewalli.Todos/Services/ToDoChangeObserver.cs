using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using DynamicData.Binding;
using Gewalli.Todos.ViewModels;

namespace Gewalli.Todos.Services;

public static class ToDoChangeObserver
{
    /// <summary>
    /// Get an observable that is used to observe all changes on the collection and throttle the amount of changes
    /// to be able to limit changes. 
    /// </summary>
    public static IObservable<IList<object>> ObserveChangesInWindow(ObservableCollection<ToDoItemViewModel> toDoItems,
        TimeSpan changeWindow)
    {
        // You can see it as we want to get the following behavior:
        // |-----|-----|-----|-----|-----|-----|-----|
        // | o      o          ooo        oo         | 
        // |     o     o           o           o     |
        // |-----|-----|-----|-----|-----|-----|-----|
        // note that the updates are pushed to the end of the time window and throttled (in the picture).
        
        // Subscribe to changes in collection (invoke save items on task pool):
        var todoItemChanges =
            toDoItems.ObserveCollectionChanges()
                // https://stackoverflow.com/questions/43710988/looking-for-a-more-declarative-rx-way-to-observe-when-the-item-properties-change
                .Select(c =>
                    toDoItems
                        .Select(item => item.Changed)
                        .Merge())
                .Switch()
                .Select(c => c as object);
        var todoCollectionChanges =
            toDoItems.ObserveCollectionChanges()
                .Select(c => c as object);
        // we want to combine changes from when you have changed an item and when you have changed the collection
        return Observable.CombineLatest(todoItemChanges, todoCollectionChanges)
            // we want to avoid getting to many changes at once 
            // we throttle and then delay to capture a potential burst of changes
            .Throttle(changeWindow)
            .Delay(changeWindow);
    }
}