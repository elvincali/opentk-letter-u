using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKExample.Core
{
    public class Camera
    {
        private Vector3 _position;
        private Vector3 _front;
        private Vector3 _up;
        private float _aspectRatio;
        private float _speed;
        private float _sensitivity;
        private float _yaw;
        private float _pitch;
        private bool _firstMove;
        private Vector2 _lastPos;

        public Vector3 Position => _position;
        public Vector3 Front => _front;
        public Vector3 Up => _up;
        public float Speed { get => _speed; set => _speed = value; }

        public Camera(Vector3 position, float aspectRatio)
        {
            _position = position;
            _front = new Vector3(0.0f, 0.0f, -1.0f);
            _up = Vector3.UnitY;
            _aspectRatio = aspectRatio;
            _speed = 2.5f;
            _sensitivity = 0.1f;
            _yaw = -90.0f;
            _pitch = 0.0f;
            _firstMove = true;
        }

        public void ProcessKeyboard(KeyboardState input, float deltaTime)
        {
            if (input.IsKeyDown(Keys.W))
                _position += _front * _speed * deltaTime;
            if (input.IsKeyDown(Keys.S))
                _position -= _front * _speed * deltaTime;
            if (input.IsKeyDown(Keys.A))
                _position -= Vector3.Normalize(Vector3.Cross(_front, _up)) * _speed * deltaTime;
            if (input.IsKeyDown(Keys.D))
                _position += Vector3.Normalize(Vector3.Cross(_front, _up)) * _speed * deltaTime;
        }

        public void ProcessMouseMove(MouseMoveEventArgs e)
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

                _front.X = MathF.Cos(MathHelper.DegreesToRadians(_pitch)) * MathF.Cos(MathHelper.DegreesToRadians(_yaw));
                _front.Y = MathF.Sin(MathHelper.DegreesToRadians(_pitch));
                _front.Z = MathF.Cos(MathHelper.DegreesToRadians(_pitch)) * MathF.Sin(MathHelper.DegreesToRadians(_yaw));
                _front = Vector3.Normalize(_front);
            }
        }

        public void ProcessMouseWheel(float offsetY)
        {
            _position += _front * offsetY * 0.5f;
        }

        public void UpdateAspectRatio(float aspectRatio)
        {
            _aspectRatio = aspectRatio;
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(_position, _position + _front, _up);
        }

        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f),
                _aspectRatio,
                0.1f,
                100.0f
            );
        }
    }
} 