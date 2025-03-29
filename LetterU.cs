using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTKExample;

public class LetterU : IDisposable
{
    private int _vertexBufferObject;
    private int _elementBufferObject;
    private int _vertexArrayObject;
    private Shader _shader;
    private Matrix4 _model;
    private Vector3 _position;
    private Vector3 _scale;
    private Vector3 _rotation;

    private Vector3 _color;

    private float[] _vertices;
    private uint[] _indices;

    public LetterU(Vector3 position, Vector3 color)
    {
        _position = position;
        _scale = Vector3.One;
        _rotation = Vector3.Zero;
        UpdateModelMatrix();

        _color = color;

        createRectangles();

        // Configurar buffers
        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        // Crear y configurar el EBO
        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

        // Crear y configurar el VAO
        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);
        
        // Configurar el atributo de posición
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        // Vincular el EBO al VAO
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);

        // Crear shader
        _shader = new Shader("Shaders/letter.vert", "Shaders/letter.frag");
    }

    private void createRectangles()
    {
        // Definir los rectángulos que forman la U
        var rectangles = new[]
        {
            // Lado izquierdo
            new Rectangle(
                new Vector3(-0.6f, 0.8f, 0.0f),
                new Vector3(-0.3f, 0.8f, 0.0f),
                new Vector3(-0.3f, -0.8f, 0.0f),
                new Vector3(-0.6f, -0.8f, 0.0f),
                0.2f
            ),
            // Lado derecho
            new Rectangle(
                new Vector3(0.3f, 0.8f, 0.0f),
                new Vector3(0.6f, 0.8f, 0.0f),
                new Vector3(0.6f, -0.8f, 0.0f),
                new Vector3(0.3f, -0.8f, 0.0f),
                0.2f
            ),
            // Base
            new Rectangle(
                new Vector3(-0.3f, -0.5f, 0.0f),
                new Vector3(0.3f, -0.5f, 0.0f),
                new Vector3(0.3f, -0.8f, 0.0f),
                new Vector3(-0.3f, -0.8f, 0.0f),
                0.2f
            )
        };

        // Generar vértices e índices
        var vertexList = new List<float>();
        var indexList = new List<uint>();
        uint currentIndex = 0;

        foreach (var rect in rectangles)
        {
            vertexList.AddRange(rect.ToVertices());
            indexList.AddRange(rect.ToIndices(currentIndex));
            currentIndex += 8; // Cada rectángulo usa 8 vértices
        }

        _vertices = vertexList.ToArray();
        _indices = indexList.ToArray();
    }

    private void UpdateModelMatrix()
    {
        // Crear matriz de transformación en este orden: Escala -> Rotación -> Traslación
        _model = Matrix4.CreateScale(_scale) *
                Matrix4.CreateRotationX(_rotation.X) *
                Matrix4.CreateRotationY(_rotation.Y) *
                Matrix4.CreateRotationZ(_rotation.Z) *
                Matrix4.CreateTranslation(_position);
    }

    // Métodos para modificar la posición
    public void SetPosition(Vector3 newPosition)
    {
        _position = newPosition;
        UpdateModelMatrix();
    }

    public void Move(Vector3 offset)
    {
        _position += offset;
        UpdateModelMatrix();
    }

    // Métodos para modificar la escala
    public void SetScale(Vector3 newScale)
    {
        _scale = newScale;
        UpdateModelMatrix();
    }

    public void Scale(float factor)
    {
        _scale *= factor;
        UpdateModelMatrix();
    }

    // Métodos para modificar la rotación
    public void SetRotation(Vector3 newRotation)
    {
        _rotation = newRotation;
        UpdateModelMatrix();
    }

    public void Rotate(Vector3 angles)
    {
        _rotation += angles;
        UpdateModelMatrix();
    }

    public void Render(Matrix4 view, Matrix4 projection)
    {
        _shader.Use();

        // Configurar uniforms
        _shader.SetMatrix4("model", _model);
        _shader.SetMatrix4("view", view);
        _shader.SetMatrix4("projection", projection);

        // Configurar color
        _shader.SetVector3("objectColor", _color);

        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
    }

    public void Dispose()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
        GL.UseProgram(0);

        GL.DeleteBuffer(_vertexBufferObject);
        GL.DeleteBuffer(_elementBufferObject);
        GL.DeleteVertexArray(_vertexArrayObject);
        _shader.Dispose();
    }
} 