DROP SCHEMA IF EXISTS cards_db;
CREATE SCHEMA cards_db;
USE cards_db;

-- Crear la tabla Jugador
DROP TABLE IF EXISTS Jugador;
CREATE TABLE Jugador (
    id INT NOT NULL AUTO_INCREMENT,
    nombre VARCHAR(255) NOT NULL,
    juegos_jugados INT NOT NULL DEFAULT 0,
    juegos_ganados INT NOT NULL DEFAULT 0,
    clave VARCHAR(255) NOT NULL,
    PRIMARY KEY (id)
);

-- Crear la tabla Partida
DROP TABLE IF EXISTS Partida;
CREATE TABLE Partida (
    id INT NOT NULL AUTO_INCREMENT,
    jugadorRojo INT NOT NULL,
    jugadorAzul INT NOT NULL,
    duracion TIME NOT NULL,
    ganador INT,
    PRIMARY KEY (id),
    FOREIGN KEY (jugadorRojo) REFERENCES Jugador(id),
    FOREIGN KEY (jugadorAzul) REFERENCES Jugador(id),
    FOREIGN KEY (ganador) REFERENCES Jugador(id)
);

-- Crear la tabla TipoCarta
DROP TABLE IF EXISTS TipoCarta;
CREATE TABLE TipoCarta (
    id INT NOT NULL AUTO_INCREMENT,
    tipo VARCHAR(50) NOT NULL,
    PRIMARY KEY (id)
);

-- Crear la tabla Efecto
DROP TABLE IF EXISTS Efecto;
CREATE TABLE Efecto (
    id INT NOT NULL AUTO_INCREMENT,
    tipo VARCHAR(50) NOT NULL,
    PRIMARY KEY (id)
);

-- Crear la tabla Carta
DROP TABLE IF EXISTS Carta;
CREATE TABLE Carta (
    id INT NOT NULL AUTO_INCREMENT,
    nombre VARCHAR(255) NOT NULL,
    descripcion VARCHAR(255) NOT NULL,
    tipoCarta INT NOT NULL,
    costoEnergia INT NOT NULL,
    efecto INT NOT NULL,
    valor INT NOT NULL,
    PRIMARY KEY (id),
    FOREIGN KEY (tipoCarta) REFERENCES TipoCarta(id),
    FOREIGN KEY (efecto) REFERENCES Efecto(id)
);

-- Crear la tabla CartaJugada
DROP TABLE IF EXISTS CartaJugada;
CREATE TABLE CartaJugada (
    id INT NOT NULL AUTO_INCREMENT,
    id_carta INT NOT NULL,
    id_partida INT NOT NULL,
    id_jugador INT NOT NULL,
    jugada_en TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id),
    FOREIGN KEY (id_carta) REFERENCES Carta(id),
    FOREIGN KEY (id_partida) REFERENCES Partida(id),
    FOREIGN KEY (id_jugador) REFERENCES Jugador(id)
);

-- Vistas
CREATE OR REPLACE VIEW PlayerStats AS
SELECT 
    J.id AS player_id,
    J.nombre AS player_name,
    J.juegos_jugados AS games_played,
    J.juegos_ganados AS games_won,
    (J.juegos_ganados / J.juegos_jugados) * 100 AS win_percentage
FROM 
    Jugador J;

CREATE OR REPLACE VIEW GameDetails AS
SELECT 
    P.id AS game_id,
    JRojo.nombre AS red_player,
    JAzul.nombre AS blue_player,
    P.duracion AS duration,
    JGanador.nombre AS winner
FROM 
    Partida P
JOIN 
    Jugador JRojo ON P.jugadorRojo = JRojo.id
JOIN 
    Jugador JAzul ON P.jugadorAzul = JAzul.id
LEFT JOIN 
    Jugador JGanador ON P.ganador = JGanador.id;

CREATE OR REPLACE VIEW CardsPlayed AS
SELECT 
    CJ.id_partida AS game_id,
    J.nombre AS player_name,
    C.nombre AS card_name,
    CJ.jugada_en AS played_at
FROM 
    CartaJugada CJ
JOIN 
    Jugador J ON CJ.id_jugador = J.id
JOIN 
    Carta C ON CJ.id_carta = C.id;

CREATE OR REPLACE VIEW CardDetails AS
SELECT 
    C.id AS card_id,
    C.nombre AS card_name,
    C.descripcion AS description,
    TC.tipo AS card_type,
    E.tipo AS effect,
    C.costoEnergia AS energy_cost,
    C.valor AS value
FROM 
    Carta C
JOIN 
    TipoCarta TC ON C.tipoCarta = TC.id
JOIN 
    Efecto E ON C.efecto = E.id;

CREATE OR REPLACE VIEW TopPlayers AS
SELECT 
    J.id AS player_id,
    J.nombre AS player_name,
    J.juegos_ganados AS games_won
FROM 
    Jugador J
ORDER BY 
    J.juegos_ganados DESC
LIMIT 10;

CREATE OR REPLACE VIEW MostPlayedCards AS
SELECT 
    C.id AS card_id,
    C.nombre AS card_name,
    COUNT(CJ.id) AS times_played
FROM 
    CartaJugada CJ
JOIN 
    Carta C ON CJ.id_carta = C.id
GROUP BY 
    C.id, C.nombre
ORDER BY 
    times_played DESC;

CREATE OR REPLACE VIEW GameDurationStats AS
SELECT 
    AVG(TIME_TO_SEC(duracion)) / 60 AS avg_duration_minutes,
    MIN(TIME_TO_SEC(duracion)) / 60 AS shortest_duration_minutes,
    MAX(TIME_TO_SEC(duracion)) / 60 AS longest_duration_minutes
FROM 
    Partida;

CREATE OR REPLACE VIEW DetallesCartas AS
SELECT
    Carta.id,
    Carta.nombre,
    Carta.descripcion,
    TipoCarta.tipo AS tipo_carta,
    Carta.costoEnergia,
    Efecto.tipo AS tipo_efecto,
    Carta.valor
FROM
    Carta
JOIN
    TipoCarta ON Carta.tipoCarta = TipoCarta.id
JOIN
    Efecto ON Carta.efecto = Efecto.id;

CREATE OR REPLACE VIEW EstadisticasJugadores AS
SELECT
    Jugador.id,
    Jugador.nombre,
    COUNT(Partida.id) AS partidas_jugadas,
    SUM(CASE WHEN Partida.ganador = Jugador.id THEN 1 ELSE 0 END) AS partidas_ganadas
FROM
    Jugador
LEFT JOIN
    Partida ON Jugador.id IN (Partida.jugadorRojo, Partida.jugadorAzul)
GROUP BY
    Jugador.id;

CREATE OR REPLACE VIEW CartasJugadasEnPartidas AS
SELECT
    Partida.id AS id_partida,
    Jugador.nombre AS nombre_jugador,
    Carta.nombre AS nombre_carta,
    CartaJugada.jugada_en
FROM
    CartaJugada
JOIN
    Partida ON CartaJugada.id_partida = Partida.id
JOIN
    Jugador ON CartaJugada.id_jugador = Jugador.id
JOIN
    Carta ON CartaJugada.id_carta = Carta.id;

-- Procedimientos almacenados
DELIMITER //

-- Procedimiento para registrar un nuevo jugador
CREATE PROCEDURE registrarJugador(
    IN p_nombre VARCHAR(255),
    IN p_clave VARCHAR(255)
)
BEGIN
    DECLARE jugadorExistente INT;

    -- Verificar si ya existe un jugador con el mismo nombre
    SELECT COUNT(*) INTO jugadorExistente
    FROM Jugador
    WHERE nombre = p_nombre;

    IF jugadorExistente > 0 THEN
        -- Si el jugador ya existe, devolver un mensaje de error
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'El jugador con este nombre ya existe.';
    ELSE
        -- Si el jugador no existe, insertarlo
        INSERT INTO Jugador (nombre, juegos_jugados, juegos_ganados, clave)
        VALUES (p_nombre, 0, 0, p_clave);
    END IF;
END //
DELIMITER ;

DELIMITER //

CREATE PROCEDURE registrarPartida(
    IN p_jugadorRojo INT,
    IN p_jugadorAzul INT,
    IN p_duracion TIME,
    IN p_ganador INT
)
BEGIN
    -- Verificar si los jugadores existen
    IF (SELECT COUNT(*) FROM Jugador WHERE id = p_jugadorRojo) = 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Jugador Rojo no existe';
    END IF;

    IF (SELECT COUNT(*) FROM Jugador WHERE id = p_jugadorAzul) = 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Jugador Azul no existe';
    END IF;

    -- Insertar la nueva partida en la tabla Partida
    INSERT INTO Partida (jugadorRojo, jugadorAzul, duracion, ganador)
    VALUES (p_jugadorRojo, p_jugadorAzul, p_duracion, p_ganador);

    -- Actualizar el conteo de juegos jugados para los jugadores participantes
    UPDATE Jugador
    SET juegos_jugados = juegos_jugados + 1
    WHERE id IN (p_jugadorRojo, p_jugadorAzul);

    -- Si hay un ganador, actualizar el conteo de juegos ganados para el jugador ganador
    IF p_ganador IS NOT NULL THEN
        UPDATE Jugador
        SET juegos_ganados = juegos_ganados + 1
        WHERE id = p_ganador;
    END IF;
END //

DELIMITER ;

DELIMITER //

CREATE PROCEDURE registrarCartaJugada(
    IN p_id_carta INT,
    IN p_id_partida INT,
    IN p_id_jugador INT
)
BEGIN
    -- Insertar la nueva carta jugada en la tabla CartaJugada
    INSERT INTO CartaJugada (id_carta, id_partida, id_jugador, jugada_en)
    VALUES (p_id_carta, p_id_partida, p_id_jugador, CURRENT_TIMESTAMP);
END //

DELIMITER ;

DELIMITER //

CREATE PROCEDURE actualizarJuegosJugados(
    IN p_jugadorRojo INT,
    IN p_jugadorAzul INT
)
BEGIN
    UPDATE Jugador
    SET juegos_jugados = juegos_jugados + 1
    WHERE id IN (p_jugadorRojo, p_jugadorAzul);
END //

DELIMITER ;

-- Eventos
DELIMITER //

-- Evento para actualizar el conteo de cartas jugadas cada hora
CREATE EVENT IF NOT EXISTS actualizarConteoCartasJugadas
ON SCHEDULE EVERY 1 HOUR
DO
BEGIN
    -- Actualizar la columna vecesJugada en la tabla Carta
    UPDATE Carta C
    JOIN (
        SELECT id_carta, COUNT(*) AS veces_jugada
        FROM CartaJugada
        GROUP BY id_carta
    ) AS CJ
    ON C.id = CJ.id_carta
    SET C.vecesJugada = CJ.veces_jugada;
END //
DELIMITER ;

-- actualizar los juegos jugados y ganados por jugador
DELIMITER //

CREATE EVENT IF NOT EXISTS actualizarEstadisticasJugadores
ON SCHEDULE EVERY 1 HOUR
DO
BEGIN
    -- Actualizar el conteo de juegos jugados para todos los jugadores
    UPDATE Jugador J
    SET J.juegos_jugados = (
        SELECT COUNT(*) 
        FROM Partida P
        WHERE P.jugadorRojo = J.id OR P.jugadorAzul = J.id
    );

    -- Actualizar el conteo de juegos ganados para todos los jugadores
    UPDATE Jugador J
    SET J.juegos_ganados = (
        SELECT COUNT(*) 
        FROM Partida P
        WHERE P.ganador = J.id
    );
END //

DELIMITER ;

-- TRIGGERS

DELIMITER //

-- Trigger 1: Actualizar juegos jugados después de insertar una partida
CREATE TRIGGER after_insert_partida
AFTER INSERT ON Partida
FOR EACH ROW
BEGIN
    UPDATE Jugador
    SET juegos_jugados = juegos_jugados + 1
    WHERE id IN (NEW.jugadorRojo, NEW.jugadorAzul);
END //

-- Trigger 2: Actualizar juegos ganados después de insertar una partida
CREATE TRIGGER after_insert_partida_update_ganador
AFTER INSERT ON Partida
FOR EACH ROW
BEGIN
    IF NEW.ganador IS NOT NULL THEN
        UPDATE Jugador
        SET juegos_ganados = juegos_ganados + 1
        WHERE id = NEW.ganador;
    END IF;
END //

-- Trigger 3: Evitar la eliminación de jugadores con partidas o cartas jugadas asociadas
CREATE TRIGGER before_delete_jugador
BEFORE DELETE ON Jugador
FOR EACH ROW
BEGIN
    DECLARE partidas_asociadas INT;
    DECLARE cartas_asociadas INT;

    SELECT COUNT(*) INTO partidas_asociadas
    FROM Partida
    WHERE jugadorRojo = OLD.id OR jugadorAzul = OLD.id;

    SELECT COUNT(*) INTO cartas_asociadas
    FROM CartaJugada
    WHERE id_jugador = OLD.id;

    IF partidas_asociadas > 0 OR cartas_asociadas > 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'No se puede eliminar el jugador porque tiene partidas o cartas jugadas asociadas.';
    END IF;
END //

DELIMITER ;