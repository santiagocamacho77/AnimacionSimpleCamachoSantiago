using UnityEngine;

// Obstaculo (pinchos, sierra, etc.). Si el jugador lo toca, vuelve al inicio.
// Funciona con Collider2D normal o con "Is Trigger".
public class Hazard : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D otro) { Golpear(otro.gameObject); }
    void OnCollisionEnter2D(Collision2D col) { Golpear(col.gameObject); }

    void Golpear(GameObject obj)
    {
        var jugador = obj.GetComponent<PlayerController>();
        if (jugador != null) jugador.Morir();
    }
}
