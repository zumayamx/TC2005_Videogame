/* Importar las librerias necesarias */
import mysql from 'mysql2/promise';
import express from 'express';
import fs from 'fs';
import  bcrypt from 'bcrypt';
const saltRounds = 10;
const app = express();
app.use(express.json());
app.use(express.static('web/public'))

async function connectToDB() {
    return await mysql.createConnection({
        host: 'database-1.c3coace2uz12.us-east-2.rds.amazonaws.com',
        user: 'admin',
        port: 3306,
        password: 'B1tD3str0y3r..',
        database: 'cards_db'
    });
}


app.get('/api/web', (req, res) => {
    const file = fs.readFileSync("/Users/leonblanga/Desktop/github/TC2005_Videojuego/API_Web/web/public/html/index.html", "utf-8"); 
    res.status(200).send(file);
}) 

app.get('/api/cards', async (req, res) => {

    let connection = null;

    try {
        connection = await connectToDB();

        const query = 'SELECT * FROM Carta';
        const [cards] = await connection.query(query);

        console.log(`${cards.length} rows returned`);
        console.log(cards);
        const result = {"cards":cards}
        res.status(200).json(result);

    } catch (error) {

        res.status(500);
        res.json(error);
        console.log(error);
        /* throw new Error ('Error al obtener las cartas') */
    } finally {

        if (connection !== null) {
            connection.end();
            console.log('Conexión cerrada exitosamente');
        }
    }
});

app.get('/api/estadisticas_jugadores', async (req, res) => {

    let connection = null;

    try {
        connection = await connectToDB();

        const query = 'SELECT * FROM EstadisticasJugadores';
        const [stats] = await connection.query(query);

        console.log(`${stats.length} rows returned`);
        console.log(stats);
        const result = {"Estadisticas jugadores":stats}
        res.status(200).json(result);

    } catch (error) {

        res.status(500);
        res.json(error);
        console.log(error);
        /* throw new Error ('Error al obtener las cartas') */
    } finally {

        if (connection !== null) {
            connection.end();
            console.log('Conexión cerrada exitosamente');
        }
    }
});

app.get('/api/info_cartas', async (req, res) => {

    let connection = null;

    try {
        connection = await connectToDB();

        const query = 'SELECT C.id AS "ID Carta", C.nombre AS "Nombre", C.descripcion AS "Descripción", TC.tipo AS "Tipo de carta", C.costoEnergia AS "Costo de energía", C.valor AS "Valor" FROM Carta C JOIN  TipoCarta TC ON C.tipoCarta = TC.id JOIN  Efecto E ON C.efecto = E.id ORDER BY C.id;';
        const [stats] = await connection.query(query);

        console.log(`${stats.length} rows returned`);
        console.log(stats);
        const result = {"Descripción cartas":stats}
        res.status(200).json(result);

    } catch (error) {

        res.status(500);
        res.json(error);
        console.log(error);
        /* throw new Error ('Error al obtener las cartas') */
    } finally {

        if (connection !== null) {
            connection.end();
            console.log('Conexión cerrada exitosamente');
        }
    }
});

app.post('/api/jugador', async (req, res) => { /* Realmente es async ?*/
    const { nombre, clave } = req.body; /* Validar el body correcto */

    let connection = null;

    try {
        connection = await connectToDB();

        const clave_encriptada = bcrypt.hashSync(clave, saltRounds);

        const query = 'INSERT INTO Jugador (nombre, juegos_jugados, juegos_ganados, clave) VALUES (?, 0, 0, ?)';
        connection.query(query, [nombre, clave_encriptada], (err, result, fields) => {
            if (err instanceof Error) {
                console.log('Error al registrar el jugador');
                return;
            }
        });

        res.status(200).send('Jugador registrado exitosamente');

    } catch (error) {

        console.log('Error intentar registrar el jugador');
        res.status(500).send('Error al registrar jugador, intenta de nuevo');

    } finally {

        if (connection !== null) {
            connection.end();
            console.log('Conexión cerrada exitosamente');
        }
    }
});

app.get('/api/jugador/:id', async (req, res) => {

    const {id} = req.params;

    let connection = null;

    try {
        connection = await connectToDB();

        const query = 'SELECT id, nombre, juegos_jugados, juegos_ganados, clave FROM Jugador WHERE id = ?';
        const [jugador] = await connection.query(query, [id]);
        res.status(200).json(jugador);
    } catch  (error) {
        console.log(error);
        res.status(500);
    } finally {
        if (connection !== null) {
            connection.end();
            console.log('Conexión cerrada exitosamente');
        }
    }

});

app.get('/api/jugadores', async (req, res) => {
    let connection = null;
    try {
    
        connection = await connectToDB();
        const query = 'SELECT * FROM Jugador';
        const [jugadores] = await connection.query(query);
        res.status(200).json(jugadores);

    } catch (error) {
        console.log(error);
        res.status(500);
    } finally {
        if (connection !== null) {
            connection.end();
            console.log('Conexión cerrada exitosamente');
        }
    }
});
/* CON UNA SOLA PETICION A PARTIR DEL NOMBRE DE USUARIO UNICO ? , PREGUNAR DEL WARNING SLQ, QUE VALE LA PENA QUE SEA VAR -> PROC*/
app.post('/api/partida', async (req, res) => {
    const {jugadorRojo, jugadorAzul} = req.body;

    let connection = null;

    try {
        connection = await connectToDB();

        const queryRojo = 'SELECT id FROM Jugador WHERE nombre = ?';
        // const idRojo = await connection.query(queryRojo, [jugadorRojo]);
        // console.log(idRojo);

        //const idRojo = - 1;
        
        const queryAzul  = 'SELECT id FROM Jugador WHERE nombre = ?';
        // const idAzul = await connection.query(queryAzul, [jugadorAzul]);
        // console.log(idAzul);

        //const idAzul = -1;

        const [resultsRojo] = await connection.query(queryRojo, [jugadorRojo]);

        console.log(resultsRojo);

        const idRojo = resultsRojo[0].id;

        const [resultsAzul] = await connection.query(queryAzul, [jugadorAzul], (err, results) => {
            if (err instanceof Error) {
                res.status(500).send('Error al obtener credenciales jugador azul');
                return;
            }
        })
        
        const idAzul = resultsAzul[0].id;

        // console.log(resultsAzul);
        // console.log(resultsRojo);
        // console.log(resultsAzul[0].id);
        // console.log(resultsRojo[0].id);
        // console.log(idAzul);
        // console.log(idRojo);

        const queryPartida = 'INSERT INTO Partida(jugadorRojo, jugadorAzul, duracion) VALUES (?, ?, "00:30:01")';
        connection.query(queryPartida, [idRojo, idAzul], (err, results) => {
            if (err instanceof Error) {
                res.status(500).send('Error al iniciar partida');
                return;
            }
        })

        res.status(200).send('Partida iniciada correctamente');

    } catch (error) {
        res.status(500).send('Error al conectarse a la base de datos');
    } finally {
        if (connection !== null) {
            connection.end();
            console.log('Conexión cerrada exitosamente');
        }
    }
});

/* VERIFICAR EL ULTIMO INSERTADO Y COMO OBTENER ESA ID, ADEMÁS SI UNA ID SE INSERTA JUSTO DESPUES */
/* COMO VERIFICAR QUE ESE JUGADOR ESTUVO EN ESA PARTIDA REALMENTE */

app.post('/api/partida/ganador', async (req, res) => {
    
    let connection = null;
    const {id_partida, id_jugador_ganador} = req.body;

    try {

        connection = await connectToDB();

        const query = 'UPDATE Partida SET ganador = ? WHERE id = ?';
        connection.query( query, [id_jugador_ganador, id_partida], (err, results) => {
            if (err instanceof Error) {
                res.status(500).send('Error al registrar el ganador');
                return;
            }
        });

        res.status(200).send('Ganador de la partida registrado correctamente'); /* Poder regresar el nombre del ganador */

    } catch (error) {
        console.log('Error al registrar el jugador ganador');
        res.status(500).send('Error al actualizar o encontrar una partida');

    } finally {
        if (connection !== null) {
            connection.end();
            console.log('Conexión cerrada exitosamente');
        }
    }
});

const PORT = process.env.port ?? 3000;
app.listen(PORT, () => {
    console.log(`Running on port ${PORT}`)
});

