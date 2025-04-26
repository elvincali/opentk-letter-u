using System.Collections.Generic;
using OpenTK.Mathematics;
using OpenTKExample.Models;

namespace OpenTKExample.App;

public class Scene : IDisposable
{
    public List<Object3D> Objects { get; private set; }
    public Vector3 Position { get; set; }
    public Vector3 Scale { get; set; }
    public Vector3 Rotation { get; set; }

    public Scene()
    {
        Objects = new List<Object3D>();
        Position = new Vector3(Vector3.Zero);
        Scale = new Vector3(Vector3.One);
        Rotation = new Vector3(Vector3.Zero);
    }

    public Scene(Vector3 position)
    {
        Objects = new List<Object3D>();
        Position = position;
        Scale = new Vector3(Vector3.One);
        Rotation = new Vector3(Vector3.Zero);
    }

    public void AddObject(Object3D obj)
    {
        Objects.Add(obj);
    }

    public void ScaleBy(Vector3 scale)
    {
        Scale *= scale;
        foreach (var obj in Objects)
        {
            obj.ScaleBy(scale);
        }
    }

    public void RotateBy(Vector3 rotation)
    {
        Rotation += rotation;
        foreach (var obj in Objects)
        {
            obj.RotateBy(rotation);
        }
    }

    public void TranslateBy(Vector3 translation)
    {
        Position += translation;
        foreach (var obj in Objects)
        {
            obj.TranslateBy(translation);
        }
    }

    public void Reset()
    {
        Scale = new Vector3(Vector3.One);
        Rotation = new Vector3(Vector3.Zero);
        Position = new Vector3(Vector3.Zero);
        foreach (var obj in Objects)
        {
            obj.Reset();
        }
    }

    public void Render(Matrix4 view, Matrix4 projection)
    {
        Matrix4 model = Matrix4.CreateScale(Scale) *
                       Matrix4.CreateRotationX(Rotation.X) *
                       Matrix4.CreateRotationY(Rotation.Y) *
                       Matrix4.CreateRotationZ(Rotation.Z) *
                       Matrix4.CreateTranslation(Position);

        foreach (var obj in Objects)
        {
            obj.Render(model, view, projection);
        }
    }

    public void Dispose()
    {
        foreach (var obj in Objects)
        {
            obj.Dispose();
        }
    }
}   