# PixelWallE - Web (C#)

## Descripción

PixelWallE es una aplicación web en C# para crear pixel art. Escribe código, Wall-E dibuja!

## Características

*   **Editor:** Escribe comandos PixelWallE.
*   **Canvas:** Visualiza tu arte en tiempo real.
*   **Carga/Guarda:** Comparte tus creaciones (`.pw`).

## El Lenguaje PixelWallE

Comandos sencillos para dibujar píxeles:

*   `Spawn(x, y)`: Inicia a Wall-E.
*   `Color(color)`: Cambia el color (Red, Blue, ...).
*   `Size(tamaño)`: Ajusta el tamaño del pincel.
*   `DrawLine(dirX, dirY, distancia)`: Traza una línea.
*   Y más!

## Arquitectura

*   **Backend:** API .NET (ejecuta el código, valida y gestiona el canvas).
*   **Frontend:** Blazor WebAssembly (interfaz de usuario).

## Estructura del proyecto

