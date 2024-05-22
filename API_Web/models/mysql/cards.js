/* Importar las librerias necesarias */
import mysql from 'mysql2/promise';
import express from 'express';
import fs from 'fs';
import  bcrypt from 'bcrypt';
const saltRounds = 10;
const app = express();
app.use(express.json());

async function connectToDB() {
    return await mysql.createConnection({
        host: 'localhost',
        user: 'root',
        port: 3306,
        password: 'r922006',
        database: 'cards_db'
    });
}


app.get('/api/web', (req, res) => {
    const file = fs.readFileSync("index.html", "utf-8"); 
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
        res.status(200).json(cards);

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
                console.log('Erro al registrar el jugador');
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

        const [resultsRojo] = await connection.query(queryRojo, [jugadorRojo], (err, results) => {
            if (err instanceof Error) {
                res.status(500).send('Error al obtener credenciales jugador rojo');
                return;
            }
        })

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
