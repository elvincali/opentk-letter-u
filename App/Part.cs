using OpenTK.Mathematics;

namespace OpenTKExample.App;

public class Part : IDisposable
{
    private string _name;
    private List<Face> _faces;
    private Vector3 _color;
    private Vector3 _scale;
    private Vector3 _rotation;
    private Vector3 _position;

    public Part(string name, Vector3 color)
    {
        _name = name;
        _faces = new List<Face>();
        _color = color;
        _scale = Vector3.One;
        _rotation = Vector3.Zero;
        _position = Vector3.Zero;
    }

    public void AddFace(Face face)
    {
        _faces.Add(face);
    }

    public void CreateFaces(List<Vertex> vertices, List<uint[]> indicesList)
    {
        // Crear una cara por cada conjunto de índices
        foreach (var indices in indicesList)
        {
            var face = new Face(vertices, indices, _color);
            _faces.Add(face);
        }
    }

    public void ScaleBy(Vector3 scale)
    {
        _scale *= scale;
        foreach (var face in _faces)
        {
            face.ScaleBy(scale);
        }
    }

    public void RotateBy(Vector3 rotation)
    {
        _rotation += rotation;
        foreach (var face in _faces)
        {
            face.RotateBy(rotation);
        }
    }

    public void TranslateBy(Vector3 translation)
    {
        _position += translation;
        foreach (var face in _faces)
        {
            face.TranslateBy(translation);
        }
    }

    public void Reset()
    {
        _scale = Vector3.One;
        _rotation = Vector3.Zero;
        _position = Vector3.Zero;
        foreach (var face in _faces)
        {
            face.Reset();
        }
    }

    public void Render(Matrix4 model, Matrix4 view, Matrix4 projection)
    {
        Matrix4 partModel = model * 
                          Matrix4.CreateScale(_scale) *
                          Matrix4.CreateRotationX(_rotation.X) *
                          Matrix4.CreateRotationY(_rotation.Y) *
                          Matrix4.CreateRotationZ(_rotation.Z) *
                          Matrix4.CreateTranslation(_position);
        
        foreach (var face in _faces)
        {
            face.Render(partModel, view, projection);
        }
    }

    public void Dispose()
    {
        foreach (var face in _faces)
        {
            face.Dispose();
        }
    }
}