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
        host: 'database-1.c3coace2uz12.us-east-2.rds.amazonaws.com',
        user: 'admin',
        port: 3306,
        password: 'B1tD3str0y3r..',
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

app.post('/api/jugador/registro', async (req, res) => { /* Realmente es async ?*/
    const { nombre, clave } = req.body; /* Validar el body correcto */

    let connection = null;

    try {
        connection = await connectToDB();

        const clave_encriptada = bcrypt.hashSync(clave, saltRounds);

        const query = 'INSERT INTO Jugador (nombre, juegos_jugados, juegos_ganados, clave) VALUES (?, 0, 0, ?)';
        await connection.query(query, [nombre, clave_encriptada]);

        res.status(200).json({code: 'SUCCESS', message: 'Jugador registrado exitosamente'});

    } catch (error) {
         
        if (error.code === 'ER_DUP_ENTRY') {
            res.status(409).json({code: 'ERROR', message: 'El nombre de usuario del jugador ya existe'});
        } else {
            console.log('Error intentar registrar el jugador:', error);
            res.status(500).json({code: 'ERROR', message: 'Error al registrar jugador, intenta de nuevo'});
        }

    } finally {

        if (connection !== null) {
            connection.end();
            console.log('Conexión cerrada exitosamente');
        }
    }
});

app.get('/api/jugador/inicio_sesion/:nombre_usuario/:clave', async (req, res) => {

    const { nombre_usuario, clave } = req.params;

    let connection = null;

    try {
        connection = await connectToDB();

        const query = 'SELECT id, nombre, juegos_jugados, juegos_ganados, clave FROM Jugador WHERE nombre = ?';
        const [jugador] = await connection.query(query, [nombre_usuario]);

        if (jugador.length === 0) {
            res.status(404).json({code: 'ERROR', message: 'Jugador no encontrado'});
            return;
        }

        if (await bcrypt.compare(clave, jugador[0].clave)) {
            
            res.status(200).json({ code: 'SUCCESS', "jugadores" : jugador });
        
        } else {

            res.status(401).json({code: 'ERROR', message: 'Credenciales incorrectas'});
        }

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

