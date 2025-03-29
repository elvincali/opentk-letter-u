using OpenTK.Mathematics;

namespace OpenTKExample;

public struct Rectangle
{
    public Vector3 TopLeft;
    public Vector3 TopRight;
    public Vector3 BottomRight;
    public Vector3 BottomLeft;
    public float Depth;

    public Rectangle(Vector3 topLeft, Vector3 topRight, Vector3 bottomRight, Vector3 bottomLeft, float depth)
    {
        TopLeft = topLeft;
        TopRight = topRight;
        BottomRight = bottomRight;
        BottomLeft = bottomLeft;
        Depth = depth;
    }

    // Convierte un rectángulo en 6 vértices (2 triángulos)
    public float[] ToVertices()
    {
        return
        [
            // Cara frontal
            TopLeft.X, TopLeft.Y, Depth,
            TopRight.X, TopRight.Y, Depth,
            BottomRight.X, BottomRight.Y, Depth,
            BottomLeft.X, BottomLeft.Y, Depth,
            // Cara trasera
            TopLeft.X, TopLeft.Y, -Depth,
            TopRight.X, TopRight.Y, -Depth,
            BottomRight.X, BottomRight.Y, -Depth,
            BottomLeft.X, BottomLeft.Y, -Depth
        ];
    }

    // Convierte un rectángulo en 12 índices (2 triángulos por cara)
    public uint[] ToIndices(uint baseIndex)
    {
        return
        [
            // Cara frontal
            baseIndex, baseIndex + 1, baseIndex + 2,     // Primer triángulo
            baseIndex, baseIndex + 2, baseIndex + 3,     // Segundo triángulo
            // Cara trasera
            baseIndex + 4, baseIndex + 5, baseIndex + 6, // Primer triángulo
            baseIndex + 4, baseIndex + 6, baseIndex + 7, // Segundo triángulo
            // Caras laterales
            baseIndex, baseIndex + 4, baseIndex + 7,     // Izquierda
            baseIndex, baseIndex + 7, baseIndex + 3,
            baseIndex + 1, baseIndex + 5, baseIndex + 6, // Derecha
            baseIndex + 1, baseIndex + 6, baseIndex + 2,
            // Caras superior e inferior
            baseIndex, baseIndex + 1, baseIndex + 5,     // Superior
            baseIndex, baseIndex + 5, baseIndex + 4,
            baseIndex + 3, baseIndex + 2, baseIndex + 6, // Inferior
            baseIndex + 3, baseIndex + 6, baseIndex + 7
        ];
    }
} 