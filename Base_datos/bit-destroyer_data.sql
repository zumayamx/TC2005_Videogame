use cards_db;

-- Insertar los tres tipos de carta
INSERT INTO TipoCarta (tipo)
VALUES
    ('ataque'),
    ('defensa'),
    ('bootcamp');
    
-- Insertar los cuatro efectos
INSERT INTO Efecto (tipo)
VALUES
    ('ataque'),
    ('defensa'),
    ('beneficio'),
    ('ninguno');

-- Restablecer la tabla Carta
SET SQL_SAFE_UPDATES = 0;
DELETE FROM Carta;
ALTER TABLE Carta AUTO_INCREMENT = 0;

-- Inserción de Cartas
INSERT INTO Carta (nombre, descripcion, tipoCarta, costoEnergia, efecto, valor) VALUES
('Cámaras de Seguridad', 'Monitorea tus instalaciones para evitar brechas de seguridad', 2, 1, 4, 3),
('Bat Dorado', 'Bat de béisbol capaz de destruir todo tipo de hardware.', 1, 3, 4, 6),
('Botnet', 'Aumenta 1 punto de ataque o defensa a una carta seleccionada.', 3, 2, 1, 0),
('Cerrojos Muertos', 'Mecanismo de bloqueo físico que protegen dispositivos críticos', 2, 1, 4, 4),
('Clonación de Llaves', 'Captura y replica las señales de las llaves inalámbricas dándote acceso a lugares restringidos', 1, 3, 4, 1),
('Coquita', 'Agrega 1 punto de vida al jugador que lo juega', 3, 2, 3, 0),
('Deauth Airplay', 'Envío de paquetes de desautenticación falsos para desconectar la red.', 1, 3, 4, 3),
('Descifrado de Handshake', 'Captura el proceso de autenticación del Wi-Fi y descifra su contraseña.', 1, 3, 4, 1),
('Encriptación WPA3', 'Protege tus comunicaciones mediante cifrado robusto y autenticación mejorada.', 2, 1, 4, 3),
('Firewall', 'Sistema que filtra el tráfico de red para proteger contra ciberataques.', 2, 1, 4, 6),
('Evil Twin', 'Crea e imita una red Wi-Fi y captura información sensible de tu rival.', 1, 3, 4, 6),
('Encriptación', 'Codifica todos tus datos inalámbricos para protegerlos de accesos no autorizados.', 2, 1, 4, 3),
('Ingeniería Social', 'Manipulación humana para obtener información confidencial.', 1, 3, 4, 1),
('Herramienta UDT', 'Manipula cerraduras por el espacio debajo de una puerta y accede a áreas restringidas.', 1, 3, 4, 3),
('Guardia de Seguridad', 'Supervisa y protege los sistemas físicos contra accesos no autorizados y manipulaciones.', 2, 1, 4, 6),
('MFOC', 'Tarjetas Mifare Classic para clonar datos de acceso almacenados.', 1, 3, 4, 2),
('Intercepción de Información', 'Captura las transmisiones de datos inalámbricos accediendo a información confidencial.', 1, 3, 4, 6),
('Inhibidor de Radio', 'Emite señales de interferencia bloqueando la comunicación de cualquier dispositivo.', 1, 3, 4, 3),
('Phishing', 'Engaña a los usuarios para que revelen información confidencial mediante correos electrónicos falsos.', 1, 3, 4, 4),
('Ataque DDoS', 'Sobrecarga un servidor con tráfico para hacerlo inaccesible.', 1, 3, 4, 5),
('Trojan', 'Oculta un software malicioso en una aplicación aparentemente legítima.', 1, 3, 4, 6),
('NMAP', 'Ve las cartas de la mano de tu oponente. Si tu oponente tiene Tor, anulará este efecto.', 3, 2, 1, 0),
('Palabras Clave', 'Frases codificadas para autenticar y asegurar las comunicaciones inalámbricas.', 2, 1, 4, 4),
('Pixie Dust', 'Descifra la contraseña de la red Wi-Fi enemiga.', 1, 3, 4, 2),
('Puerta Abierta?', 'Gira la ruleta y el color que caiga pierde 3 puntos de vida.', 3, 2, 1, 0),
('Señal Digital', 'Transmite datos codificados binariamente.', 2, 1, 4, 6),
('SHIM', 'Forza la cerradura para acceder al cuarto de servidores.', 1, 3, 4, 2),
('SSID Oculto', 'Oculta el nombre de la red Wi-Fi para evitar su detección por usuarios no autorizados.', 2, 1, 4, 4),
('TOR', 'Oculta el valor de tus cartas de defensa en el tablero por 2 turnos.', 3, 2, 3, 0),
('VPN', 'Cambia cartas de canal para aprovechar su máximo potencial en donde sea.', 3, 2, 3, 0),
('Wannacry', 'El jugador opuesto no pueda jugar ni recoger cartas en el siguiente turno.', 3, 2, 2, 0);

-- Insertar datos dummy en la tabla Jugador con partidas jugadas y ganadas
INSERT INTO Jugador (nombre, juegos_jugados, juegos_ganados, clave) VALUES
('Jugador1', 10, 5, 'clave1'),
('Jugador2', 8, 3, 'clave2'),
('Jugador3', 15, 7, 'clave3'),
('Jugador4', 12, 6, 'clave4'),
('Jugador5', 9, 4, 'clave5'),
('Jugador6', 14, 8, 'clave6'),
('Jugador7', 11, 5, 'clave7'),
('Jugador8', 13, 7, 'clave8'),
('Jugador9', 10, 6, 'clave9'),
('Jugador10', 8, 2, 'clave10');

-- Insertar datos dummy en la tabla CartaJugada
INSERT INTO CartaJugada (id_carta, id_partida, id_jugador, jugada_en) VALUES
(1, 1, 1, '2024-01-01 10:00:00'),
(2, 1, 2, '2024-01-01 10:01:00'),
(3, 2, 3, '2024-01-01 10:05:00'),
(4, 2, 4, '2024-01-01 10:10:00'),
(5, 3, 5, '2024-01-01 10:15:00'),
(6, 3, 6, '2024-01-01 10:20:00'),
(7, 4, 7, '2024-01-01 10:25:00'),
(8, 4, 8, '2024-01-01 10:30:00'),
(9, 5, 9, '2024-01-01 10:35:00'),
(10, 5, 10, '2024-01-01 10:40:00'),
(1, 6, 1, '2024-01-01 10:45:00'),
(2, 6, 3, '2024-01-01 10:50:00'),
(3, 7, 2, '2024-01-01 10:55:00'),
(4, 7, 4, '2024-01-01 11:00:00'),
(5, 8, 5, '2024-01-01 11:05:00'),
(6, 8, 7, '2024-01-01 11:10:00'),
(7, 4, 6, '2024-01-01 11:15:00'),
(8, 4, 8, '2024-01-01 11:20:00'),
(9, 5, 9, '2024-01-01 11:25:00'),
(10, 1, 1, '2024-01-01 11:30:00'),
(1, 1, 1, '2024-01-02 10:00:00'),
(2, 1, 2, '2024-01-02 10:01:00'),
(3, 2, 3, '2024-01-02 10:05:00'),
(4, 2, 4, '2024-01-02 10:10:00'),
(5, 3, 5, '2024-01-02 10:15:00'),
(6, 3, 6, '2024-01-02 10:20:00'),
(7, 4, 7, '2024-01-02 10:25:00'),
(8, 4, 8, '2024-01-02 10:30:00'),
(9, 5, 9, '2024-01-02 10:35:00'),
(10, 5, 10, '2024-01-02 10:40:00'),
(1, 6, 1, '2024-01-02 10:45:00'),
(2, 6, 3, '2024-01-02 10:50:00'),
(3, 7, 2, '2024-01-02 10:55:00'),
(4, 7, 4, '2024-01-02 11:00:00'),
(5, 8, 5, '2024-01-02 11:05:00'),
(6, 8, 7, '2024-01-02 11:10:00'),
(7, 5, 6, '2024-01-02 11:15:00'),
(8, 5, 8, '2024-01-02 11:20:00'),
(9, 2, 9, '2024-01-02 11:25:00'),
(10, 2, 1, '2024-01-02 11:30:00'),
(1, 1, 1, '2024-01-03 10:00:00'),
(2, 1, 2, '2024-01-03 10:01:00'),
(3, 2, 3, '2024-01-03 10:05:00'),
(4, 2, 4, '2024-01-03 10:10:00'),
(5, 3, 5, '2024-01-03 10:15:00'),
(6, 3, 6, '2024-01-03 10:20:00'),
(7, 4, 7, '2024-01-03 10:25:00'),
(8, 4, 8, '2024-01-03 10:30:00'),
(9, 5, 9, '2024-01-03 10:35:00'),
(10, 5, 10, '2024-01-03 10:40:00'),
(1, 6, 1, '2024-01-03 10:45:00'),
(2, 6, 3, '2024-01-03 10:50:00'),
(3, 7, 2, '2024-01-03 10:55:00'),
(4, 7, 4, '2024-01-03 11:00:00'),
(5, 8, 5, '2024-01-03 11:05:00'),
(6, 8, 7, '2024-01-03 11:10:00'),
(7, 3, 6, '2024-01-03 11:15:00'),
(8, 3, 8, '2024-01-03 11:20:00'),
(9, 7, 9, '2024-01-03 11:25:00'),
(10, 7, 1, '2024-01-03 11:30:00');