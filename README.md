# Nombre del Juego
BitDestroyer

## Descripción del Juego
El objetivo de Bit Destroyer es dejar sin puntos de vida al enemigo mientras se ponen en práctica conocimientos básicos de hacking, con un énfasis en los conceptos generales. Los jugadores deben utilizar y combinar estratégicamente diferentes cartas para proteger sus propios canales de comunicación (radio, cable y wifi) y atacar los del enemigo a través de cartas defensivas y ofensivas.

## Temática del Juego
Una partida consta de dos jugadores, uno del equipo rojo y otro del equipo azul, que compiten utilizando cartas de ataque, defensa y bootcamp. Cada jugador tiene tres canales de comunicación (radio, cable y wifi) con tres ranuras cada uno, sumando nueve ranuras disponibles. Los jugadores pueden tener hasta seis cartas en la mano y cuentan con un temporizador de veinte segundos por turno para decidir si obtienen o juegan una carta, utilizando tres puntos de energía disponibles para obtener cartas. Las cartas tienen diferentes atributos como tipo (ataque, defensa o bootcamp), canal preferido (cable, radio, wifi), costo de obtención, y valor de defensa o ataque, con penalizaciones por uso en canales no preferidos. Las cartas bootcamp pueden combinarse con otras para obtener efectos superiores o jugarse solas. Los jugadores comienzan la partida con una ranura protegida por tres puntos de defensa y el primer turno se decide lanzando una moneda. El primer jugador debe obtener una carta del lote disponible, considerando el costo de cada una.

## Instrucciones para Correr el Juego

### Requisitos Previos
- Tener instalado Unity versión [2022.3.23f1]

### Pasos para Ejecutar el Juego
1. Clona este repositorio:
    ```sh
    git clone git@github.com:zumayamx/TC2005_Videojuego.git
    ```
2. Una vez clonado, abre Unity Hub con la versión mencionada y sigue estos pasos:
    - Selecciona "New Project".
    - Dirígete a la carpeta donde clonaste el repositorio.
    - Ve a la carpeta `Videojuego`.
    - Selecciona la carpeta `01_Prototipo_BitDestroyer` y haz clic en "Open".

3. Asegúrate de que la escena inicial esté configurada como la escena predeterminada:
    - Antes de iniciar, ve a `File` > `Build Settings` > `Player Settings` y marca la opción `Allow downloads over HTTP` como `Always allowed` en la sección de configuraciones para permitir conexión con nuestra API.
    - **Escena Inicial:** [EleccionModo]
    - **Credenciales por defecto**: 
        - Jugador_1: zumaya90
        - Clave_1: zum1234
        - Jugador_2: zumaya80
        - Clave_2: zum123
    - Corre el videojuego con el botón play.

### Controles del Juego
- **Obtener una carta:** Presiona sobre alguno de los tres lotes de cartas que se enceintran en la parte superior del tablero, esto te proporcionara una carta directo a tu mano.
- **Cambio de turno:** Presiona espacio para cambiar de turno, nota que tu energía disminuye cuando tomas una carta.
- **Arrastrar una carta a tu parte del tablero:** Selecciona una carta de tu mano de cartas, manten presionado y colocala sobre el slot deseado. Si esto tiene exito la carta se quedara sobre el slot seleccionado.
- **Ataque:** Nota que hay diferentes cartas, por lo cual puedes colocar una carta de ataque (roja) sobre el slot del enemigo. Haciendo daño a su respectivo slot, en caso de que este no tenga ninguna defensa sobre ese canal el daño pasara directo a la vida del enemigo.
- **Ver una carta:** A simple vista es difcil saber de que tipo y para que sirve una carta, por lo cual al presionar click derecho y mantenerlo podemos ver más de cerca una carta.

## Funcionalidades del Juego

### Funcionalidades Completadas
- [Arrastre de cartas]: Las cartas pueden arrastrarse por el tablero y colocarse solo en los slots designados segun las reglas del juego.
- [Ataque y defensa de cartas]: Las cartas de ataque restan correctamente puntos de defensa a cartas de defensa o hacen daño directo al servidor. Minetras que las cartasd de defensa contrarestan este daño correctamente.
- [Inicio de sesión]: Los jugadores puden inciar sesiónn o registrarse, estos datos se mantienen durante toda la partida del juego. 
- [Zoom a las carta]: Podemos ver una carta más de cerca con esta funcionalidad y saber mejor su objetivo.
- [Ocultar las cartas del enemigo]: en el turno actual, se ocultan las cartas del enemigo.

### Funcionalidades en Desarrollo
- [Elección de cartas]: Al presionar sobre alguno de los tres tipos de lotes de cartas debe saltar una ventana con 3 opciones de esa carta y elegir solo una.
- [Combinación de cartas]: Al tener una carta de tipo bootcamp, el poder seleccionarla y elegir con que carta de nuestra mano actual podemos combinarla para aumentar sus beneficios.

## Créditos
- [José Manuel García Zumaya]
- [Omar Emiliano Sánchez Villegas]
- [León Blanga Hasbani]

