using System;
using System.Collections.Generic;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKExample.Models;
using OpenTKExample.Loaders;
using OpenTKExample.Core;
using OpenTKExample.Utils;
namespace OpenTKExample;

public class Window : GameWindow 
{
    private Scene _scene;
    private CoordinateSystem _coordinateSystem;
    private bool _showCoordinates = true;
    private Camera _camera;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
        CursorState = CursorState.Grabbed;
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        GL.Enable(EnableCap.DepthTest);

        // Inicializar la cámara
        _camera = new Camera(new Vector3(-0.4f, 0.2f, 5.0f), Size.X / (float)Size.Y);

        // Inicializar objetos
        _scene = new Scene();
        _coordinateSystem = new CoordinateSystem();
        InitObjects();
    }

    private void InitObjects()
    {
        // Cargar la escena desde el archivo JSON
        var objects = GeometryLoader.LoadSceneFromJson("Data/scene.json");
        
        // Agregar los objetos a la escena
        foreach (var obj in objects)
        {
            _scene.AddObject(obj);
        }
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        // Renderizar la escena
        _scene.Render(_camera.GetViewMatrix(), _camera.GetProjectionMatrix());

        // Renderizar el sistema de coordenadas si está activado
        if (_showCoordinates)
        {
            _coordinateSystem.Render(_camera.GetViewMatrix(), _camera.GetProjectionMatrix());
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

        // Procesar entrada de teclado para la cámara
        _camera.ProcessKeyboard(input, (float)e.Time);
    }

    protected override void OnMouseMove(MouseMoveEventArgs e)
    {
        _camera.ProcessMouseMove(e);
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        _camera.ProcessMouseWheel(e.OffsetY);
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Size.X, Size.Y);
        _camera.UpdateAspectRatio(Size.X / (float)Size.Y);
    }

    protected override void OnUnload()
    {
        _coordinateSystem.Dispose();
        _scene.Dispose();
        base.OnUnload();
    }
} 