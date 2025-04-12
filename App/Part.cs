using OpenTK.Mathematics;

namespace OpenTKExample;

public class Part
{
    private string _name;
    private List<Face> _faces;
    private Vector3 _color;

    public Part(string name, Vector3 color)
    {
        _name = name;
        _faces = new List<Face>();
        _color = color;
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

    public void Render(Matrix4 model, Matrix4 view, Matrix4 projection)
    {
        foreach (var face in _faces)
        {
            face.Render(model, view, projection);
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