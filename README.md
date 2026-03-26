# Proyecto Final de Dispositivos Hápticos

Proyecto desarrollado en Unity.

## Requisitos

- Unity (versión del proyecto definida en `ProjectSettings/ProjectVersion.txt`)

## Cómo jugar

1. Abre el proyecto en Unity.
2. Ve a la escena `Assets/Scenes/Menu.unity`.
3. Ejecuta el juego desde la escena **Menu** (`Play`).

> Importante: para jugar correctamente, el inicio debe hacerse desde la escena `Menu`.

## Cambiar el tiempo del partido

Si quieres cambiar la duración del partido:

1. Abre la escena `Assets/Scenes/Game.unity`.
2. En la jerarquía, selecciona el objeto que tenga el componente `GameHUD`.
3. En el Inspector, busca la variable `matchDurationSeconds`.
4. Cambia su valor para definir el tiempo del partido en segundos.

Ejemplo:

- `60` = 1 minuto
- `120` = 2 minutos
- `180` = 3 minutos

## Estructura relevante

- `Assets/Scenes/Menu.unity`: escena de inicio del juego.
- `Assets/Scenes/Game.unity`: escena principal del partido.
- `Assets/UI/GameHUD/GameHUD.cs`: lógica del HUD y temporizador (`matchDurationSeconds`).
