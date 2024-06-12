
CREATE VIEW PlayerStats AS
SELECT 
    J.id AS player_id,
    J.nombre AS player_name,
    J.juegos_jugados AS games_played,
    J.juegos_ganados AS games_won,
    (J.juegos_ganados / J.juegos_jugados) * 100 AS win_percentage
FROM 
    Jugador J;


CREATE VIEW GameDetails AS
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


CREATE VIEW CardsPlayed AS
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


CREATE VIEW CardDetails AS
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


CREATE VIEW TopPlayers AS
SELECT 
    J.id AS player_id,
    J.nombre AS player_name,
    J.juegos_ganados AS games_won
FROM 
    Jugador J
ORDER BY 
    J.juegos_ganados DESC
LIMIT 10;


CREATE VIEW MostPlayedCards AS
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


CREATE VIEW GameDurationStats AS
SELECT 
    AVG(TIME_TO_SEC(duracion)) / 60 AS avg_duration_minutes,
    MIN(TIME_TO_SEC(duracion)) / 60 AS shortest_duration_minutes,
    MAX(TIME_TO_SEC(duracion)) / 60 AS longest_duration_minutes
FROM 
    Partida;


CREATE VIEW DetallesCartas AS
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

CREATE VIEW EstadisticasJugadores AS
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

CREATE VIEW CartasJugadasEnPartidas AS
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
