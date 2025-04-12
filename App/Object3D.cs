using OpenTK.Mathematics;

namespace OpenTKExample;

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
    }

    public void SetPosition(Vector3 position)
    {
        Position = position;
    }

    public void AddPart(Part part)
    {
        Parts.Add(part);
    }

    public void Render(Matrix4 view, Matrix4 projection)
    {
        Matrix4 model = Matrix4.CreateScale(Scale) *
                       Matrix4.CreateRotationX(Rotation.X) *
                       Matrix4.CreateRotationY(Rotation.Y) *
                       Matrix4.CreateRotationZ(Rotation.Z) *
                       Matrix4.CreateTranslation(Position);

        foreach (var part in Parts)
        {
            part.Render(model, view, projection);
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