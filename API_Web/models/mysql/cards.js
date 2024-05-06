import mysql from 'mysql2/promise'

const config = {
    host: 'localhost',
    user: 'root',
    port: 3306,
    password: 'r922006',
    database: 'carddb'
}

const connection = await mysql.createConnection(config);

