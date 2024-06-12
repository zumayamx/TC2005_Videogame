# Esquema de Base de Datos `cards_db`

## Descripción
Este archivo SQL crea un esquema de base de datos llamado `cards_db` que incluye las siguientes tablas: `Jugador`, `Partida`, `TipoCarta`, `Efecto`, `Carta`, y `CartaJugada`. Además, inserta datos de prueba en las tablas `TipoCarta`, `Efecto`, y `Carta`.

## Tablas y Atributos
### Tabla `Jugador`
- **id** (INT, NOT NULL, AUTO_INCREMENT): Identificador único del jugador.
- **nombre** (VARCHAR(255), NOT NULL): Nombre del jugador.
- **juegos_jugados** (INT, NOT NULL, DEFAULT 0): Número de juegos jugados por el jugador.
- **juegos_ganados** (INT, NOT NULL, DEFAULT 0): Número de juegos ganados por el jugador.
- **clave** (VARCHAR(255), NOT NULL): Clave del jugador.

### Tabla `Partida`
- **id** (INT, NOT NULL, AUTO_INCREMENT): Identificador único de la partida.
- **jugadorRojo** (INT, NOT NULL): Identificador del jugador rojo (referencia a `Jugador(id)`).
- **jugadorAzul** (INT, NOT NULL): Identificador del jugador azul (referencia a `Jugador(id)`).
- **duracion** (TIME, NOT NULL): Duración de la partida.
- **ganador** (INT): Identificador del jugador ganador (referencia a `Jugador(id)`).

### Tabla `TipoCarta`
- **id** (INT, NOT NULL, AUTO_INCREMENT): Identificador único del tipo de carta.
- **tipo** (VARCHAR(50), NOT NULL): Tipo de carta.

### Tabla `Efecto`
- **id** (INT, NOT NULL, AUTO_INCREMENT): Identificador único del efecto.
- **tipo** (VARCHAR(50), NOT NULL): Tipo de efecto.

### Tabla `Carta`
- **id** (INT, NOT NULL, AUTO_INCREMENT): Identificador único de la carta.
- **nombre** (VARCHAR(255), NOT NULL): Nombre de la carta.
- **descripcion** (VARCHAR(255), NOT NULL): Descripción de la carta.
- **tipoCarta** (INT, NOT NULL): Tipo de carta (referencia a `TipoCarta(id)`).
- **costoEnergia** (INT, NOT NULL): Costo de energía de la carta.
- **efecto** (INT, NOT NULL): Efecto de la carta (referencia a `Efecto(id)`).
- **valor** (INT, NOT NULL): Valor de la carta.

### Tabla `CartaJugada`
- **id** (INT, NOT NULL, AUTO_INCREMENT): Identificador único de la carta jugada.
- **id_carta** (INT, NOT NULL): Identificador de la carta (referencia a `Carta(id)`).
- **id_partida** (INT, NOT NULL): Identificador de la partida (referencia a `Partida(id)`).
- **id_jugador** (INT, NOT NULL): Identificador del jugador (referencia a `Jugador(id)`).
- **jugada_en** (TIMESTAMP, NOT NULL, DEFAULT CURRENT_TIMESTAMP): Momento en que se jugó la carta.

## Inserción de Datos de Prueba
### Tabla `TipoCarta`
Se insertan tres tipos de carta:
- Ataque
- Defensa
- Bootcamp

### Tabla `Efecto`
Se insertan cuatro efectos:
- Ataque
- Defensa
- Beneficio
- Ninguno

## Justificación del diseño siguiendo las formas normales
### Tabla Jugador

• Está en 1FN porque todos los valores son atómicos, las columnas tienen nombres únicos, y los tipos de datos son consistentes.

• Está en 2FN porque no tiene claves primarias compuestas.

• Está en 3FN porque todos los atributos no clave (nombre, juegos_jugados, juegos_ganados, clave) dependen únicamente de la clave primaria id.

#### Tabla Partida

• Está en 1FN porque todos los valores son atómicos, las columnas tienen nombres únicos, y los tipos de datos son consistentes.

• Está en 2FN porque no tiene claves primarias compuestas.

• Está en 3FN porque todos los atributos no clave (jugadorRojo, jugadorAzul, duracion, ganador) dependen únicamente de la clave primaria id.

#### Tabla TipoCarta

• Está en 1FN porque todos los valores son atómicos, las columnas tienen nombres únicos, y los tipos de datos son consistentes.

• Está en 2FN porque no tiene claves primarias compuestas.

• Está en 3FN porque todos los atributos no clave (tipo) dependen únicamente de la clave primaria id.

#### Tabla Efecto

• Está en 1FN porque todos los valores son atómicos, las columnas tienen nombres únicos, y los tipos de datos son consistentes.

• Está en 2FN porque no tiene claves primarias compuestas.

• Está en 3FN porque todos los atributos no clave (tipo) dependen únicamente de la clave primaria id.

#### Tabla Carta

• Está en 1FN porque todos los valores son atómicos, las columnas tienen nombres únicos, y los tipos de datos son consistentes.

• Está en 2FN porque no tiene claves primarias compuestas.

• Está en 3FN porque todos los atributos no clave (nombre, descripcion, tipoCarta, costoEnergia, efecto, valor) dependen únicamente de la clave primaria id.

#### Tabla CartaJugada

• Está en 1FN porque todos los valores son atómicos, las columnas tienen nombres únicos, y los tipos de datos son consistentes.

• Está en 2FN porque no tiene claves primarias compuestas.

• Está en 3FN porque todos los atributos no clave (id_carta, id_partida, id_jugador, jugada_en) dependen únicamente de la clave primaria id.
