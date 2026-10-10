using UnityEngine;

// Meta del nivel (bandera, trofeo...). Necesita un Collider2D con "Is Trigger".
public class Goal : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D otro)
    {
        var jugador = otro.GetComponent<PlayerController>();
        if (jugador != null && GameManager.Instance != null)
            GameManager.Instance.LlegarMeta(jugador);
    }
}
