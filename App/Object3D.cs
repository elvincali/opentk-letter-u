using OpenTK.Mathematics;

namespace OpenTKExample.App;

public class Object3D : IDisposable
{
    public string Name { get; set; }
    public List<Part> Parts { get; private set; }
    public Vector3 Position { get; set; }
    public Vector3 Rotation { get; set; }
    public Vector3 Scale { get; set; }

    public Object3D(string name)
    {
        Name = name;
        Parts = new List<Part>();
        Scale = Vector3.One;
        Rotation = Vector3.Zero;
        Position = Vector3.Zero;
    }

    public void SetPosition(Vector3 position)
    {
        Position = position;
    }

    public void AddPart(Part part)
    {
        Parts.Add(part);
    }

    public void ScaleBy(Vector3 scale)
    {
        Scale *= scale;
        foreach (var part in Parts)
        {
            part.ScaleBy(scale);
        }
    }

    public void RotateBy(Vector3 rotation)
    {
        Rotation += rotation;
        foreach (var part in Parts)
        {
            part.RotateBy(rotation);
        }
    }

    public void TranslateBy(Vector3 translation)
    {
        Position += translation;
        foreach (var part in Parts)
        {
            part.TranslateBy(translation);
        }
    }

    public void Reset()
    {
        Scale = Vector3.One;
        Rotation = Vector3.Zero;
        Position = Vector3.Zero;
        foreach (var part in Parts)
        {
            part.Reset();
        }
    }

    public void Render(Matrix4 model, Matrix4 view, Matrix4 projection)
    {
        Matrix4 objectModel = model *
                            Matrix4.CreateScale(Scale) *
                            Matrix4.CreateRotationX(Rotation.X) *
                            Matrix4.CreateRotationY(Rotation.Y) *
                            Matrix4.CreateRotationZ(Rotation.Z) *
                            Matrix4.CreateTranslation(Position);

        foreach (var part in Parts)
        {
            part.Render(objectModel, view, projection);
        }
    }

    public void Dispose()
    {
        foreach (var part in Parts)
        {
            part.Dispose();
        }
    }
}