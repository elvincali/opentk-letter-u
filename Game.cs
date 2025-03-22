using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace OpenTKExample;

public static class Game
{
    private const int WIDTH = 800;
    private const int HEIGHT = 600;
    private const string TITLE = "OpenTK en Fedora";

    public static void Main()
    {
        var nativeWindowSettings = new NativeWindowSettings()
        {
            ClientSize = new Vector2i(WIDTH, HEIGHT),
            Title = TITLE
        };

        var gameWindowSettings = GameWindowSettings.Default;

        using (var window = new Window(gameWindowSettings, nativeWindowSettings))
        {
            window.Run();
        }
    }
} 