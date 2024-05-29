
function random_color(alpha=1.0)
{
    const r_c = () => Math.round(Math.random() * 255)
    return `rgba(${r_c()}, ${r_c()}, ${r_c()}, ${alpha}`
}
Chart.defaults.font.size = 16;

try
{
    const levels_response = await fetch('http://localhost:3000/api/estadisticas_jugadores',{method: 'GET'})

    if(levels_response.ok)
    {
        console.log('Response is ok. Converting to JSON.')

        let results = await levels_response.json()

        console.log(results)
        console.log('Data converted correctly. Plotting chart.')

        // In this case, we just separate the data into different arrays using the map method of the values array. This creates new arrays that hold only the data that we need.
        const jugador_id = results["Estadisticas jugadores"].map(e => e['ID jugador'])
        const jugador_nombre = results["Estadisticas jugadores"].map(e => e['Nombre'])
        const jugador_jugados = results["Estadisticas jugadores"].map(e => e['Juegos jugados'])
        const jugador_ganados = results["Estadisticas jugadores"].map(e => e['Juegos ganados'])
        const jugador_porcentaje = results["Estadisticas jugadores"].map(e => e['Porcentaje de victorias'])
        const colors = results["Estadisticas jugadores"].map(e => random_color(0.8))
        const borders = results["Estadisticas jugadores"].map(e => 'rgba(0, 0, 0, 1.0)')

        const ctx_levels1 = document.getElementById('apiChart1').getContext('2d');
        const levelChart1 = new Chart(ctx_levels1, 
            {
                type: 'bar',
                data: {
                    labels: jugador_nombre,
                    datasets: [
                        {
                            label: 'Partidas jugadas',
                            backgroundColor: 'rgba(255, 49, 49, 1)',
                            borderColor: 'rgba(255, 255, 255, 1)',
                            borderWidth: 1,
                            data: jugador_jugados
                        }
                    ]
                },
                options: {
                    scales: {
                        x: {
                            ticks: {
                            color: 'white' // Color de las etiquetas del eje X
                            },
                            grid: {
                            color: null // Color de la línea del eje X
                            }
                        },
                        y: {
                            ticks: {
                            color: 'white' // Color de las etiquetas del eje Y
                            },
                            grid: {
                            color: null // Color de la línea del eje Y
                            }
                        }
                    }
                }
            })

        const ctx_levels2 = document.getElementById('apiChart2').getContext('2d');
        const levelChart2 = new Chart(ctx_levels2, 
            {
                type: 'bar',
                data: {
                    labels: jugador_nombre,
                    datasets: [
                        {
                            label: 'Partidas ganadas',
                            backgroundColor: 'rgba(127, 217, 89, 1)',
                            borderColor: 'rgba(255, 255, 255, 1)',
                            borderWidth: 1,
                            data: jugador_ganados
                        }
                    ]
                }
            })
        
        const ctx_levels3 = document.getElementById('apiChart3').getContext('2d');
        const levelChart3 = new Chart(ctx_levels3, 
            {
                type: 'bar',
                data: {
                    labels: jugador_nombre,
                    datasets: [
                        {
                            label: 'Porcentaje de Victorias',
                            backgroundColor: 'rgba(255, 3, 255, 1)',
                            borderColor: 'rgba(255, 255, 255, 1)',
                            borderWidth: 1,
                            data: jugador_porcentaje
                        }
                    ]
                }
            })
    }
}
catch(error)
{
    console.log(error)
}