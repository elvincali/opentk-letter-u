# Visualizador 3D de Letra U con OpenTK

Este proyecto es una aplicación 3D interactiva que muestra una letra "U" en un espacio tridimensional usando OpenTK (Open Toolkit) para C#.

## Requisitos Previos

1. **.NET 8.0 SDK**
   - Descarga e instala desde [.NET Downloads](https://dotnet.microsoft.com/download)
   - Verifica la instalación con: `dotnet --version`

2. **Dependencias de OpenGL**
   - Para Fedora/RHEL:
     ```bash
     sudo dnf install mesa-libGL-devel
     ```
   - Para Ubuntu/Debian:
     ```bash
     sudo apt-get install libgl1-mesa-dev
     ```
   - Para Windows:
     - Las dependencias vienen incluidas en el sistema operativo

## Instalación

1. Clona el repositorio:
   ```bash
   git clone [URL-del-repositorio]
   cd opentk-pg
   ```

2. Restaura las dependencias:
   ```bash
   dotnet restore
   ```

3. Compila el proyecto:
   ```bash
   dotnet build
   ```

4. Ejecuta la aplicación:
   ```bash
   dotnet run
   ```

## Controles

### Movimiento de Cámara
- **WASD**: Mover la cámara
- **Mouse**: Rotar la vista
- **Rueda del Mouse**: Zoom in/out

### Manipulación de la Letra
- **Flechas**: Mover en el plano XY
- **Page Up/Down**: Mover en el eje Z
- **Q/E**: Rotar alrededor del eje Y
- **Teclado Numérico 0**: Resetear posición y transformaciones

### Otros
- **C**: Mostrar/ocultar ejes de coordenadas
- **ESC**: Salir de la aplicación

## Estructura del Proyecto

- `Window.cs`: Clase principal que maneja la ventana y los controles
- `LetterU.cs`: Implementación de la geometría y transformaciones de la letra U
- `CoordinateSystem.cs`: Sistema de coordenadas visual
- `Shader.cs`: Manejo de shaders GLSL
- `Shaders/`: Directorio con los shaders GLSL
  - `letter.vert`: Vertex shader para la letra
  - `letter.frag`: Fragment shader para la letra
  - `coordinate.vert`: Vertex shader para los ejes
  - `coordinate.frag`: Fragment shader para los ejes

## Dependencias

El proyecto utiliza las siguientes dependencias de NuGet:
- OpenTK (v4.9.4)

## Licencia

© 2025 Elvin Callisaya.