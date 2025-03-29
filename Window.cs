using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Data.Common;

namespace OpenTKExample;

public class Window : GameWindow 
{
    private CoordinateSystem _coordinateSystem;
    private List<LetterU> _letters;
    private bool _showCoordinates = true;
    private Matrix4 _projection;

    // Variables para la cámara
    private Vector3 _cameraPos = new Vector3(0.0f, 0.0f, 3.0f);
    private Vector3 _cameraFront = new Vector3(0.0f, 0.0f, -1.0f);
    private Vector3 _cameraUp = Vector3.UnitY;
    private float _cameraSpeed = 2.5f;

    // Variables para el control del mouse
    private bool _firstMove = true;
    private Vector2 _lastPos;
    private float _sensitivity = 0.1f;
    private float _yaw = -90.0f;
    private float _pitch;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
        CursorState = CursorState.Grabbed;
        _letters = new List<LetterU>();
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        GL.Enable(EnableCap.DepthTest);

        // Configurar la matriz de proyección
        _projection = Matrix4.CreatePerspectiveFieldOfView(
            MathHelper.DegreesToRadians(45.0f),
            Size.X / (float)Size.Y,
            0.1f,
            100.0f
        );

        // Inicializar objetos
        InitObjects();
    }

    private void InitObjects()
    {
        // Ejes de coordenadas
        _coordinateSystem = new CoordinateSystem();

        // Letra U1
        var position1 = new Vector3(0.0f, 0.0f, 0.0f);
        var color1 = new Vector3(1.0f, 1.0f, 1.0f);
        _letters.Add(new LetterU(position1, color1));

        // Letra U2
        var position2 = new Vector3(2.0f, 1.0f, 0.0f);
        var color2 = new Vector3(0.5f, 1.0f, 0.5f);
        _letters.Add(new LetterU(position2, color2));
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        // Calcular la matriz de vista
        Matrix4 view = Matrix4.LookAt(_cameraPos, _cameraPos + _cameraFront, _cameraUp);

        // Renderizar la letra U
        foreach (var letter in _letters)
        {
            letter.Render(view, _projection);
        }

        // Renderizar el sistema de coordenadas si está activado
        if (_showCoordinates)
        {
            _coordinateSystem.Render(view, _projection);
        }

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        if (!IsFocused)
            return;

        var input = KeyboardState;

        if (input.IsKeyDown(Keys.Escape))
        {
            Close();
        }

        // Alternar sistema de coordenadas con la tecla C
        if (input.IsKeyPressed(Keys.C))
        {
            _showCoordinates = !_showCoordinates;
        }

        // Movimiento de la cámara con WASD
        if (input.IsKeyDown(Keys.W))
            _cameraPos += _cameraFront * _cameraSpeed * (float)e.Time;
        if (input.IsKeyDown(Keys.S))
            _cameraPos -= _cameraFront * _cameraSpeed * (float)e.Time;
        if (input.IsKeyDown(Keys.A))
            _cameraPos -= Vector3.Normalize(Vector3.Cross(_cameraFront, _cameraUp)) * _cameraSpeed * (float)e.Time;
        if (input.IsKeyDown(Keys.D))
            _cameraPos += Vector3.Normalize(Vector3.Cross(_cameraFront, _cameraUp)) * _cameraSpeed * (float)e.Time;

        // Mover la letra U con las flechas
        if (input.IsKeyDown(Keys.Left))
            _letters[0].Move(new Vector3(-1.0f * (float)e.Time, 0.0f, 0.0f));
        if (input.IsKeyDown(Keys.Right))
            _letters[0].Move(new Vector3(1.0f * (float)e.Time, 0.0f, 0.0f));
        if (input.IsKeyDown(Keys.Up))
            _letters[0].Move(new Vector3(0.0f, 1.0f * (float)e.Time, 0.0f));
        if (input.IsKeyDown(Keys.Down))
            _letters[0].Move(new Vector3(0.0f, -1.0f * (float)e.Time, 0.0f));

        // Mover en el eje Z con PageUp/PageDown
        if (input.IsKeyDown(Keys.PageUp))
            _letters[0].Move(new Vector3(0.0f, 0.0f, 1.0f * (float)e.Time));
        if (input.IsKeyDown(Keys.PageDown))
            _letters[0].Move(new Vector3(0.0f, 0.0f, -1.0f * (float)e.Time));

        // Escalar con + y -
        if (input.IsKeyDown(Keys.KeyPadAdd))
            _letters[0].Scale(1.0f + 0.5f * (float)e.Time);
        if (input.IsKeyDown(Keys.KeyPadSubtract))
            _letters[0].Scale(1.0f - 0.5f * (float)e.Time);

        // Rotar con Q/E (eje Y), R/F (eje X) y Z/X (eje Z)
        if (input.IsKeyDown(Keys.Q))
            _letters[0].Rotate(new Vector3(0.0f, MathHelper.DegreesToRadians(90.0f) * (float)e.Time, 0.0f));
        if (input.IsKeyDown(Keys.E))
            _letters[0].Rotate(new Vector3(0.0f, -MathHelper.DegreesToRadians(90.0f) * (float)e.Time, 0.0f));
        if (input.IsKeyDown(Keys.R))
            _letters[0].Rotate(new Vector3(MathHelper.DegreesToRadians(90.0f) * (float)e.Time, 0.0f, 0.0f));
        if (input.IsKeyDown(Keys.F))
            _letters[0].Rotate(new Vector3(-MathHelper.DegreesToRadians(90.0f) * (float)e.Time, 0.0f, 0.0f));
        if (input.IsKeyDown(Keys.Z))
            _letters[0].Rotate(new Vector3(0.0f, 0.0f, MathHelper.DegreesToRadians(90.0f) * (float)e.Time));
        if (input.IsKeyDown(Keys.X))
            _letters[0].Rotate(new Vector3(0.0f, 0.0f, -MathHelper.DegreesToRadians(90.0f) * (float)e.Time));

        // Resetear posición con la tecla 0 del teclado numérico
        if (input.IsKeyPressed(Keys.KeyPad0))
        {
            _letters[0].SetPosition(Vector3.Zero);
            _letters[0].SetRotation(Vector3.Zero);
            _letters[0].SetScale(Vector3.One);
        }
    }

    protected override void OnMouseMove(MouseMoveEventArgs e)
    {
        if (_firstMove)
        {
            _lastPos = new Vector2(e.X, e.Y);
            _firstMove = false;
        }
        else
        {
            var deltaX = e.X - _lastPos.X;
            var deltaY = e.Y - _lastPos.Y;
            _lastPos = new Vector2(e.X, e.Y);

            _yaw += deltaX * _sensitivity;
            _pitch -= deltaY * _sensitivity;

            // Limitar el pitch para evitar que la cámara se voltee
            _pitch = Math.Clamp(_pitch, -89.0f, 89.0f);

            _cameraFront.X = MathF.Cos(MathHelper.DegreesToRadians(_pitch)) * MathF.Cos(MathHelper.DegreesToRadians(_yaw));
            _cameraFront.Y = MathF.Sin(MathHelper.DegreesToRadians(_pitch));
            _cameraFront.Z = MathF.Cos(MathHelper.DegreesToRadians(_pitch)) * MathF.Sin(MathHelper.DegreesToRadians(_yaw));
            _cameraFront = Vector3.Normalize(_cameraFront);
        }
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        _cameraPos += _cameraFront * e.OffsetY * 0.5f;
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Size.X, Size.Y);
        _projection = Matrix4.CreatePerspectiveFieldOfView(
            MathHelper.DegreesToRadians(45.0f),
            Size.X / (float)Size.Y,
            0.1f,
            100.0f
        );
    }

    protected override void OnUnload()
    {
        _coordinateSystem.Dispose();
        foreach (var letter in _letters)
        {
            letter.Dispose();
        }

        base.OnUnload();
    }
} 