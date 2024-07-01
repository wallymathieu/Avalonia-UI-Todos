using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Linq;

namespace Gewalli.Todos.Infrastructure;

public static class RxHelper
{
    public static IObservable<NotifyCollectionChangedEventArgs> ObserveCollectionChanged<T>(this ObservableCollection<T> self) =>
        Observable
            .FromEvent<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                handler => (sender, args) => handler(args),
                handler => self.CollectionChanged += handler,
                handler => self.CollectionChanged -= handler
            );
}