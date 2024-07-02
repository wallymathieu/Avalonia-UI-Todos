using Avalonia;
using Avalonia.Headless;

namespace Gewalli.Todos.Tests;


public class TestAppBuilder
{
    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>()
        .UseHeadless(new AvaloniaHeadlessPlatformOptions());
}