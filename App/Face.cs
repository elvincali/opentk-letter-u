using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTKExample.App;

public class Face : IDisposable
{
    private int _vertexBufferObject;
    private int _elementBufferObject;
    private int _vertexArrayObject;
    private Shader _shader;
    private List<Vertex> _vertices;
    private uint[] _indices;
    private Vector3 _color;
    private Vector3 _scale;
    private Vector3 _rotation;
    private Vector3 _position;

    public Face(List<Vertex> vertices, uint[] indices, Vector3 color)
    {
        _vertices = vertices;
        _indices = indices;
        _color = color;
        _scale = Vector3.One;
        _rotation = Vector3.Zero;
        _position = Vector3.Zero;

        // Configurar buffers
        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Count * 3 * sizeof(float), _vertices.ToArray(), BufferUsageHint.StaticDraw);

        // Crear y configurar el EBO
        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

        // Crear y configurar el VAO
        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        // Configurar atributos (solo posición)
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        // Vincular el EBO al VAO
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);

        // Crear shader
        _shader = new Shader("./Core/Shaders/letter.vert", "./Core/Shaders/letter.frag");
    }

    public void ScaleBy(Vector3 scale)
    {
        _scale *= scale;
    }

    public void RotateBy(Vector3 rotation)
    {
        _rotation += rotation;
    }

    public void TranslateBy(Vector3 translation)
    {
        _position += translation;
    }

    public void Reset()
    {
        _scale = Vector3.One;
        _rotation = Vector3.Zero;
        _position = Vector3.Zero;
    }

    public void Render(Matrix4 model, Matrix4 view, Matrix4 projection)
    {
        _shader.Use();

        // Configurar uniforms
        Matrix4 finalModel = model * 
                           Matrix4.CreateScale(_scale) *
                           Matrix4.CreateRotationX(_rotation.X) *
                           Matrix4.CreateRotationY(_rotation.Y) *
                           Matrix4.CreateRotationZ(_rotation.Z) *
                           Matrix4.CreateTranslation(_position);
        
        _shader.SetMatrix4("model", finalModel);
        _shader.SetMatrix4("view", view);
        _shader.SetMatrix4("projection", projection);
        _shader.SetVector3("objectColor", _color);

        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
    }

    public void Dispose()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        GL.BindVertexArray(0);
        GL.UseProgram(0);

        GL.DeleteBuffer(_vertexBufferObject);
        GL.DeleteBuffer(_elementBufferObject);
        GL.DeleteVertexArray(_vertexArrayObject);
        _shader.Dispose();
    }
}