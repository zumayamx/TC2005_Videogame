
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

        let stats = results["Estadisticas jugadores"];
        // Ordenar y limitar los datos para cada gráfico
        let topJugados = [...stats].sort((a, b) => b['Juegos jugados'] - a['Juegos jugados']).slice(0, 5);
        let topGanados = [...stats].sort((a, b) => b['Juegos ganados'] - a['Juegos ganados']).slice(0, 5);
        let topPorcentaje = [...stats].sort((a, b) => b['Porcentaje de victorias'] - a['Porcentaje de victorias']).slice(0, 5);

        // Preparar datos para el gráfico de partidas jugadas
        const nombre_jugados = topJugados.map(e => e['Nombre']);
        const jugador_jugados = topJugados.map(e => e['Juegos jugados']);

        // Preparar datos para el gráfico de partidas ganadas
        const nombre_ganados = topGanados.map(e => e['Nombre']);
        const jugador_ganados = topGanados.map(e => e['Juegos ganados']);

        // Preparar datos para el gráfico de porcentaje de victorias
        const nombre_porcentaje = topPorcentaje.map(e => e['Nombre']);
        const jugador_porcentaje = topPorcentaje.map(e => e['Porcentaje de victorias']);

        // Establecer colores y bordes para los gráficos
        const colors = results["Estadisticas jugadores"].map(e => random_color(0.8))
        const borders = results["Estadisticas jugadores"].map(e => 'rgba(0, 0, 0, 1.0)')

        const ctx_levels1 = document.getElementById('apiChart1').getContext('2d');
        const levelChart1 = new Chart(ctx_levels1, 
            {
                type: 'bar',
                data: {
                    labels: nombre_jugados,
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
                    labels: nombre_ganados,
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
                    labels: nombre_porcentaje,
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

async function fetchAndRenderPieChart() {
    try {
        const response = await fetch('http://localhost:3000/api/cartas_info', { method: 'GET' });

        if (response.ok) {
            const data = await response.json();
            const cartas = data.cartas;

            // Preparar los datos para el pie chart
            const nombres = cartas.map(carta => carta.Nombre);
            const vecesJugadas = cartas.map(carta => carta['Veces jugada']);

            // Crear el pie chart usando Chart.js
            const ctx = document.getElementById('pieChart').getContext('2d');
            new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: nombres,
                    datasets: [{
                        data: vecesJugadas,
                        backgroundColor: nombres.map(() => random_color(0.8)),
                        borderColor: nombres.map(() => 'rgba(255, 255, 255, 1)'),
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true,
                    plugins: {
                        legend: {
                            position: 'top',
                            labels: {
                                color: 'white' // Color de las etiquetas de la leyenda
                            }
                        },
                        tooltip: {
                            callbacks: {
                                label: function (context) {
                                    return `${context.label}: ${context.raw}`;
                                }
                            }
                        }
                    }
                }
            });
        }
    } catch (error) {
        console.log(error);
    }
}

fetchAndRenderPieChart();