using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OpenTKExample.Models
{
    public class SceneData
    {
        [JsonPropertyName("objects")]
        public List<Object3DData> Objects { get; set; }
    }

    public class Object3DData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("position")]
        public float[] Position { get; set; }

        [JsonPropertyName("parts")]
        public List<PartData> Parts { get; set; }
    }

    public class PartData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("vertices")]
        public List<VertexData> Vertices { get; set; }

        [JsonPropertyName("indices")]
        public uint[] Indices { get; set; }

        [JsonPropertyName("color")]
        public float[] Color { get; set; }
    }

    public class VertexData
    {
        [JsonPropertyName("x")]
        public float X { get; set; }

        [JsonPropertyName("y")]
        public float Y { get; set; }

        [JsonPropertyName("z")]
        public float Z { get; set; }
    }
} 