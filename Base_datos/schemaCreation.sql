CREATE SCHEMA cards_db;

USE cards_db;

-- Verificar si la tabla ya existe y eliminarla si es necesario
DROP TABLE IF EXISTS Jugador;

-- Crear la tabla Jugador con los atributos especificados
CREATE TABLE Jugador (
    id INT NOT NULL AUTO_INCREMENT,
    nombre VARCHAR(255) NOT NULL,
    juegos_jugados INT NOT NULL DEFAULT 0,
    juegos_ganados INT NOT NULL DEFAULT 0,
    clave VARCHAR(255) NOT NULL,
    PRIMARY KEY (id)
);


-- Verificar si la tabla ya existe y eliminarla si es necesario
DROP TABLE IF EXISTS Partida;

-- Crear la tabla Partida con los atributos especificados
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

-- Verificar si la tabla ya existe y eliminarla si es necesario
DROP TABLE IF EXISTS TipoCarta;

-- Crear la tabla tipoCarta con los atributos especificados
CREATE TABLE TipoCarta (
    id INT NOT NULL AUTO_INCREMENT,
    tipo VARCHAR(50) NOT NULL,
    PRIMARY KEY (id)
);

-- Insertar los tres tipos de carta
INSERT INTO TipoCarta (tipo)
VALUES
    ('ataque'),
    ('defensa'),
    ('bootcamp');

-- Verificar si la tabla ya existe y eliminarla si es necesario
DROP TABLE IF EXISTS Efecto;

-- Crear la tabla Efecto con los atributos especificados
CREATE TABLE Efecto (
    id INT NOT NULL AUTO_INCREMENT,
    tipo VARCHAR(50) NOT NULL,
    PRIMARY KEY (id)
);

-- Insertar los cuatro efectos
INSERT INTO Efecto (tipo)
VALUES
    ('ataque'),
    ('defensa'),
    ('beneficio'),
    ('ninguno');

-- Verificar si la tabla ya existe y eliminarla si es necesario
DROP TABLE IF EXISTS Carta;

-- Crear la tabla Carta con los atributos especificados
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

-- Verificar si la tabla ya existe y eliminarla si es necesario
DROP TABLE IF EXISTS CartaJugada;

-- Crear la tabla CartaJugada con los atributos especificados
CREATE TABLE CartaJugada (
    id INT NOT NULL AUTO_INCREMENT, -- Probablemente sea necesario
    id_carta INT NOT NULL,
    id_partida INT NOT NULL,
    id_jugador INT NOT NULL,
    jugada_en TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id),
    FOREIGN KEY (id_carta) REFERENCES Carta(id),
    FOREIGN KEY (id_partida) REFERENCES Partida(id),
    FOREIGN KEY (id_jugador) REFERENCES Jugador(id)
);

-- Inserción de datos de prueba en tabla Carta
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

