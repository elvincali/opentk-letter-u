using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using OpenTK.Mathematics;
using OpenTKExample.Models;

namespace OpenTKExample.Loaders;

public static class GeometryLoader
{
    public static List<Object3D> LoadSceneFromJson(string filePath)
    {
        try
        {
            string jsonContent = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            var sceneData = JsonSerializer.Deserialize<SceneData>(jsonContent, options);
            var objects = new List<Object3D>();

            foreach (var objData in sceneData.Objects)
            {
                var object3D = CreateObject3D(objData);

                objects.Add(object3D);
            }

            return objects;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar la escena desde JSON: {ex.Message}");
            return new List<Object3D>();
        }
    }

    private static Object3D CreateObject3D(Object3DData objectData)
    {
        // Crear el objeto 3D
        var object3D = new Object3D(objectData.Name);
        
        // Establecer la posición
        if (objectData.Position != null && objectData.Position.Length == 3)
        {
            object3D.SetPosition(new Vector3(
                objectData.Position[0],
                objectData.Position[1],
                objectData.Position[2]
            ));
        }
        
        // Crear cada parte
        foreach (var partData in objectData.Parts)
        {
            var color = new Vector3(partData.Color[0], partData.Color[1], partData.Color[2]);
            var part = new Part(partData.Name, color);
            
            // Convertir vértices
            var vertices = partData.Vertices.Select(v => new Vertex(new Vector3(v.X, v.Y, v.Z))).ToList();
            
            // Crear las caras usando los índices directamente
            part.CreateFaces(vertices, new List<uint[]> { partData.Indices });
            
            // Agregar la parte al objeto
            object3D.AddPart(part);
        }
        
        return object3D;
    }
} 