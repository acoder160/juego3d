using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoScript : MonoBehaviour
{
    // --- Variables de Configuración ---

    // Distancia mínima para que el enemigo empiece a perseguir al jugador.
    public float distanciaDePersecucion = 15.0f;

    // Distancia máxima para que el enemigo abandone la persecución y regrese a casa.
    public float distanciaDeRetorno = 20.0f;

    // Velocidad del enemigo.
    public float velocidadDeMovimiento = 5.0f;

    // Referencia al componente de navegación (NavMeshAgent, etc.)
    // private AgenteDeNavegacion agenteDeNavegacion;

    // Referencia a la Transformada del Jugador.
    private Transform jugador;

    // Posición inicial/de "casa" del enemigo.
    private Vector3 posicionInicial;

    // --- Inicialización (Función 'Start') ---
    void Start()
    {
        // 1. Obtener el componente de navegación del enemigo.
        // agenteDeNavegacion = GetComponent<AgenteDeNavegacion>();

        // 2. Encontrar la transformada del jugador (asumiendo que tiene la etiqueta "Player").
        jugador = GameObject.FindGameObjectWithTag("Player").transform;

        // 3. Guardar la posición inicial del enemigo.
        posicionInicial = transform.position;
    }

    // --- Actualización por Frame (Función 'Update') ---
    void Update()
    {
        // 1. Calcular la distancia actual entre el enemigo y el jugador.
        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

        // 2. Aplicar la lógica basada en la distancia.

        // Si la distancia es menor a 15m, ¡PERSEGUIR!
        if (distanciaAlJugador < distanciaDePersecucion)
        {
            PerseguirJugador();
        }
        // Si la distancia es mayor a 20m Y no estamos ya en la posición inicial, ¡VOLVER A CASA!
        else if (distanciaAlJugador > distanciaDeRetorno)
        {
            VolverAPosicionInicial();
        }
        // Si la distancia está entre 15m y 20m, o si ya estamos en casa, ¡NO HACER NADA!
        else
        {
            // Detener el movimiento (o reproducir animación de 'idle').
            // agenteDeNavegacion.Stop();
        }
    }

    // --- Funciones Lógicas ---

    // Lógica de Persecución
    void PerseguirJugador()
    {
        // 1. Configurar la velocidad.
        // agenteDeNavegacion.velocidad = velocidadDeMovimiento;

        // 2. Establecer el destino al jugador.
        // agenteDeNavegacion.SetDestino(jugador.position);

        // NOTA: Puedes añadir aquí lógica de ataque si está muy cerca.
    }

    // Lógica de Retorno
    void VolverAPosicionInicial()
    {
        // 1. Configurar la velocidad.
        //agenteDeNavegacion.velocidad = velocidadDeMovimiento;

        // 2. Establecer el destino a la posición inicial guardada.
        //agenteDeNavegacion.SetDestino(posicionInicial);

        // Opcional: Si está muy cerca de la posición inicial, detenerse para evitar temblores.
        if (Vector3.Distance(transform.position, posicionInicial) < 1.0f)
        {
            //agenteDeNavegacion.Stop();
        }
    }
}
