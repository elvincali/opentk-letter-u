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

    // Vértices para la U en 3D usando rectángulos
    private readonly float[] _vertices =
    {
        // Posiciones de los vértices
        // Lado izquierdo
        -0.6f,  0.8f,  0.2f,  // 0: frente-arriba-izquierda
        -0.3f,  0.8f,  0.2f,  // 1: frente-arriba-derecha
        -0.3f, -0.8f,  0.2f,  // 2: frente-abajo-derecha
        -0.6f, -0.8f,  0.2f,  // 3: frente-abajo-izquierda
        -0.6f,  0.8f, -0.2f,  // 4: atrás-arriba-izquierda
        -0.3f,  0.8f, -0.2f,  // 5: atrás-arriba-derecha
        -0.3f, -0.8f, -0.2f,  // 6: atrás-abajo-derecha
        -0.6f, -0.8f, -0.2f,  // 7: atrás-abajo-izquierda

        // Lado derecho
        0.3f,  0.8f,  0.2f,   // 8: frente-arriba-izquierda
        0.6f,  0.8f,  0.2f,   // 9: frente-arriba-derecha
        0.6f, -0.8f,  0.2f,   // 10: frente-abajo-derecha
        0.3f, -0.8f,  0.2f,   // 11: frente-abajo-izquierda
        0.3f,  0.8f, -0.2f,   // 12: atrás-arriba-izquierda
        0.6f,  0.8f, -0.2f,   // 13: atrás-arriba-derecha
        0.6f, -0.8f, -0.2f,   // 14: atrás-abajo-derecha
        0.3f, -0.8f, -0.2f,   // 15: atrás-abajo-izquierda

        // Base
        -0.3f, -0.5f,  0.2f,  // 16: frente-arriba-izquierda
        0.3f, -0.5f,  0.2f,   // 17: frente-arriba-derecha
        0.3f, -0.8f,  0.2f,   // 18: frente-abajo-derecha
        -0.3f, -0.8f,  0.2f,  // 19: frente-abajo-izquierda
        -0.3f, -0.5f, -0.2f,  // 20: atrás-arriba-izquierda
        0.3f, -0.5f, -0.2f,   // 21: atrás-arriba-derecha
        0.3f, -0.8f, -0.2f,   // 22: atrás-abajo-derecha
        -0.3f, -0.8f, -0.2f   // 23: atrás-abajo-izquierda
    };

    // Índices para dibujar los rectángulos
    private readonly uint[] _indices =
    {
        // Lado izquierdo
        0, 1, 2, 0, 2, 3,     // cara frontal
        4, 5, 6, 4, 6, 7,     // cara trasera
        0, 4, 7, 0, 7, 3,     // cara izquierda
        1, 5, 6, 1, 6, 2,     // cara derecha
        0, 1, 5, 0, 5, 4,     // cara superior
        3, 2, 6, 3, 6, 7,     // cara inferior

        // Lado derecho
        8, 9, 10, 8, 10, 11,  // cara frontal
        12, 13, 14, 12, 14, 15, // cara trasera
        8, 12, 15, 8, 15, 11,   // cara izquierda
        9, 13, 14, 9, 14, 10,   // cara derecha
        8, 9, 13, 8, 13, 12,    // cara superior
        11, 10, 14, 11, 14, 15, // cara inferior

        // Base
        16, 17, 18, 16, 18, 19, // cara frontal
        20, 21, 22, 20, 22, 23, // cara trasera
        16, 20, 23, 16, 23, 19, // cara izquierda
        17, 21, 22, 17, 22, 18, // cara derecha
        16, 17, 21, 16, 21, 20, // cara superior
        19, 18, 22, 19, 22, 23  // cara inferior
    };

    public LetterU()
    {
        _position = Vector3.Zero;  // Posición inicial en (0,0,0)
        _scale = Vector3.One;      // Escala inicial 1:1:1
        _rotation = Vector3.Zero;  // Sin rotación inicial
        UpdateModelMatrix();
        
        // Crear y configurar el VBO
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
        // Color azul claro
        _shader.SetVector3("objectColor", new Vector3(1.0f, 1.0f, 1.0f));

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