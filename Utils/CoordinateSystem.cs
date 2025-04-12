using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTKExample.Utils;

public class CoordinateSystem : IDisposable
{
    private int _vertexBufferObject;
    private int _vertexArrayObject;
    private Shader _shader;

    // Definimos los vértices para los ejes X (rojo), Y (verde) y Z (azul)
    private readonly float[] _vertices =
    {
        // Posiciones         // Colores
        // Eje X (rojo)
        0.0f,  0.0f,  0.0f, 1.0f, 0.0f, 0.0f, // Inicio del eje X
        1.0f,  0.0f,  0.0f, 1.0f, 0.0f, 0.0f, // Fin del eje X
        
        // Eje Y (verde)
        0.0f,  0.0f,  0.0f, 0.0f, 1.0f, 0.0f, // Inicio del eje Y
        0.0f,  1.0f,  0.0f, 0.0f, 1.0f, 0.0f, // Fin del eje Y
        
        // Eje Z (azul)
        0.0f,  0.0f,  0.0f, 0.0f, 0.0f, 1.0f, // Inicio del eje Z
        0.0f,  0.0f,  1.0f, 0.0f, 0.0f, 1.0f  // Fin del eje Z
    };

    public CoordinateSystem()
    {
        // Crear y configurar el VBO
        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        // Crear y configurar el VAO
        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        // Configurar el atributo de posición
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        // Configurar el atributo de color
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        // Crear shader específico para los ejes
        _shader = new Shader("./Core/Shaders/coordinate.vert", "./Core/Shaders/coordinate.frag");
    }

    public void Render(Matrix4 view, Matrix4 projection)
    {
        _shader.Use();
        
        // Configurar las matrices de transformación
        _shader.SetMatrix4("view", view);
        _shader.SetMatrix4("projection", projection);
        _shader.SetMatrix4("model", Matrix4.Identity);

        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawArrays(PrimitiveType.Lines, 0, 6);
    }

    public void Dispose()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
        GL.UseProgram(0);

        GL.DeleteBuffer(_vertexBufferObject);
        GL.DeleteVertexArray(_vertexArrayObject);
        _shader.Dispose();
    }
} 