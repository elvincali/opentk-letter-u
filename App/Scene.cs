using OpenTK.Mathematics;

namespace OpenTKExample;

public class Scene : IDisposable
{
    public List<Object3D> Objects { get; private set; }
    public Vector3 Position { get; set; }

    public Scene()
    {
        Objects = new List<Object3D>();
        Position = new Vector3(Vector3.Zero);
    }

    public Scene(Vector3 position)
    {
        Objects = new List<Object3D>();
        Position = position;
    }

    public void AddObject(Object3D obj)
    {
        Objects.Add(obj);
    }

    public void Render(Matrix4 view, Matrix4 projection)
    {
        foreach (var obj in Objects)
        {
            obj.Render(view, projection);
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