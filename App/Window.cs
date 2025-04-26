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

namespace OpenTKExample.App;

public class Window : GameWindow 
{
    private Scene _scene;
    private CoordinateSystem _coordinateSystem;
    private bool _showCoordinates = true;
    private Camera _camera;
    private Object3D _selectedObject;
    private Part _selectedPart;
    private float _scaleFactor = 1.1f; // Factor de escalado (10% de aumento)
    private const float MIN_SCALE = 0.01f;
    private const float MAX_SCALE = 100.0f;
    private const float ROTATION_SPEED = 100.0f;
    private const float TRANSLATION_SPEED = 40.1f;
    private bool _lastZKeyState = false;
    private bool _lastXKeyState = false;
    private bool _lastQKeyState = false;
    private bool _lastWKeyState = false;
    private bool _lastEKeyState = false;
    private bool _lastAKeyState = false;
    private bool _lastSKeyState = false;
    private bool _lastDKeyState = false;
    private bool _lastUpKeyState = false;
    private bool _lastDownKeyState = false;
    private bool _lastLeftKeyState = false;
    private bool _lastRightKeyState = false;
    private bool _lastIKeyState = false;
    private bool _lastJKeyState = false;
    private bool _lastKKeyState = false;
    private bool _lastLKeyState = false;

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

        // Seleccionar el primer objeto y su primera parte por defecto
        if (_scene.Objects.Count > 0)
        {
            _selectedObject = _scene.Objects[0];
            if (_selectedObject.Parts.Count > 0)
            {
                _selectedPart = _selectedObject.Parts[0];
            }
        }
    }

    private Vector3 ClampScale(Vector3 scale)
    {
        return new Vector3(
            Math.Clamp(scale.X, MIN_SCALE, MAX_SCALE),
            Math.Clamp(scale.Y, MIN_SCALE, MAX_SCALE),
            Math.Clamp(scale.Z, MIN_SCALE, MAX_SCALE)
        );
    }

    private void RotateSelected(float angleX, float angleY, float angleZ)
    {
        if (_selectedPart != null)
        {
            // Rotar parte
            _selectedPart.RotateBy(new Vector3(angleX, angleY, angleZ));
        }
        else if (_selectedObject != null)
        {
            // Rotar objeto
            _selectedObject.RotateBy(new Vector3(angleX, angleY, angleZ));
        }
        else
        {
            // Rotar escena
            _scene.RotateBy(new Vector3(angleX, angleY, angleZ));
        }
    }

    private void TranslateSelected(float x, float y, float z)
    {
        if (_selectedPart != null)
        {
            // Mover parte
            _selectedPart.TranslateBy(new Vector3(x, y, z));
        }
        else if (_selectedObject != null)
        {
            // Mover objeto
            _selectedObject.TranslateBy(new Vector3(x, y, z));
        }
        else
        {
            // Mover escena
            _scene.TranslateBy(new Vector3(x, y, z));
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

        // Controles de selección
        if (input.IsKeyPressed(Keys.F1))
        {
            // Seleccionar escena
            _selectedObject = null;
            _selectedPart = null;
        }
        else if (input.IsKeyPressed(Keys.F2) && _scene.Objects.Count > 0)
        {
            // Seleccionar objeto 0
            _selectedObject = _scene.Objects[0];
            _selectedPart = null;
        }
        else if (input.IsKeyPressed(Keys.F3) && _scene.Objects.Count > 1)
        {
            // Seleccionar objeto 1
            _selectedObject = _scene.Objects[1];
            _selectedPart = null;
        }
        else if (input.IsKeyPressed(Keys.F5) && _selectedObject != null && _selectedObject.Parts.Count > 0)
        {
            // Seleccionar parte 0
            _selectedPart = _selectedObject.Parts[0];
        }
        else if (input.IsKeyPressed(Keys.F6) && _selectedObject != null && _selectedObject.Parts.Count > 1)
        {
            // Seleccionar parte 1
            _selectedPart = _selectedObject.Parts[1];
        }
        else if (input.IsKeyPressed(Keys.F12))
        {
            // Reset a todo
            _selectedObject = null;
            _selectedPart = null;
            _scene.Reset();
        }

        // Controles de rotación
        bool currentQKeyState = input.IsKeyDown(Keys.R);
        bool currentWKeyState = input.IsKeyDown(Keys.T);
        bool currentEKeyState = input.IsKeyDown(Keys.Y);
        bool currentAKeyState = input.IsKeyDown(Keys.F);
        bool currentSKeyState = input.IsKeyDown(Keys.G);
        bool currentDKeyState = input.IsKeyDown(Keys.H);

        float rotationStep = ROTATION_SPEED * (float)e.Time;

        if (currentQKeyState && !_lastQKeyState)
        {
            // Rotar en X positivo
            RotateSelected(rotationStep, 0, 0);
        }
        else if (currentWKeyState && !_lastWKeyState)
        {
            // Rotar en Y positivo
            RotateSelected(0, rotationStep, 0);
        }
        else if (currentEKeyState && !_lastEKeyState)
        {
            // Rotar en Z positivo
            RotateSelected(0, 0, rotationStep);
        }
        else if (currentAKeyState && !_lastAKeyState)
        {
            // Rotar en X negativo
            RotateSelected(-rotationStep, 0, 0);
        }
        else if (currentSKeyState && !_lastSKeyState)
        {
            // Rotar en Y negativo
            RotateSelected(0, -rotationStep, 0);
        }
        else if (currentDKeyState && !_lastDKeyState)
        {
            // Rotar en Z negativo
            RotateSelected(0, 0, -rotationStep);
        }

        _lastQKeyState = currentQKeyState;
        _lastWKeyState = currentWKeyState;
        _lastEKeyState = currentEKeyState;
        _lastAKeyState = currentAKeyState;
        _lastSKeyState = currentSKeyState;
        _lastDKeyState = currentDKeyState;

        // Controles de traslación
        bool currentUpKeyState = input.IsKeyDown(Keys.Up);
        bool currentDownKeyState = input.IsKeyDown(Keys.Down);
        bool currentLeftKeyState = input.IsKeyDown(Keys.Left);
        bool currentRightKeyState = input.IsKeyDown(Keys.Right);
        bool currentIKeyState = input.IsKeyDown(Keys.I);
        bool currentJKeyState = input.IsKeyDown(Keys.J);
        bool currentKKeyState = input.IsKeyDown(Keys.K);
        bool currentLKeyState = input.IsKeyDown(Keys.L);

        float translationStep = TRANSLATION_SPEED * (float)e.Time;

        if (currentUpKeyState && !_lastUpKeyState)
        {
            // Mover en Y positivo
            TranslateSelected(0, translationStep, 0);
        }
        else if (currentDownKeyState && !_lastDownKeyState)
        {
            // Mover en Y negativo
            TranslateSelected(0, -translationStep, 0);
        }
        else if (currentLeftKeyState && !_lastLeftKeyState)
        {
            // Mover en X negativo
            TranslateSelected(-translationStep, 0, 0);
        }
        else if (currentRightKeyState && !_lastRightKeyState)
        {
            // Mover en X positivo
            TranslateSelected(translationStep, 0, 0);
        }
        else if (currentIKeyState && !_lastIKeyState)
        {
            // Mover en Z positivo
            TranslateSelected(0, 0, translationStep);
        }
        else if (currentKKeyState && !_lastKKeyState)
        {
            // Mover en Z negativo
            TranslateSelected(0, 0, -translationStep);
        }

        _lastUpKeyState = currentUpKeyState;
        _lastDownKeyState = currentDownKeyState;
        _lastLeftKeyState = currentLeftKeyState;
        _lastRightKeyState = currentRightKeyState;
        _lastIKeyState = currentIKeyState;
        _lastKKeyState = currentKKeyState;

        // Controles de escalado
        bool currentZKeyState = input.IsKeyDown(Keys.Z);
        bool currentXKeyState = input.IsKeyDown(Keys.X);

        if (currentZKeyState && !_lastZKeyState)
        {
            // Escalar según la selección
            if (_selectedPart != null)
            {
                _selectedPart.ScaleBy(ClampScale(new Vector3(_scaleFactor)));
            }
            else if (_selectedObject != null)
            {
                _selectedObject.ScaleBy(ClampScale(new Vector3(_scaleFactor)));
            }
            else
            {
                _scene.ScaleBy(ClampScale(new Vector3(_scaleFactor)));
            }
        }
        else if (currentXKeyState && !_lastXKeyState)
        {
            // Escalar según la selección
            if (_selectedPart != null)
            {
                _selectedPart.ScaleBy(ClampScale(new Vector3(1.0f / _scaleFactor)));
            }
            else if (_selectedObject != null)
            {
                _selectedObject.ScaleBy(ClampScale(new Vector3(1.0f / _scaleFactor)));
            }
            else
            {
                _scene.ScaleBy(ClampScale(new Vector3(1.0f / _scaleFactor)));
            }
        }

        _lastZKeyState = currentZKeyState;
        _lastXKeyState = currentXKeyState;
        
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