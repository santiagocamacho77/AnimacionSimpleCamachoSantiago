using UnityEngine;

// Objeto recolectable. Necesita un Collider2D con "Is Trigger" activado.
public class Coin : MonoBehaviour
{
    void Start()
    {
        if (GameManager.Instance != null) GameManager.Instance.RegistrarMoneda();
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (!otro.CompareTag("Player")) return;
        if (GameManager.Instance != null) GameManager.Instance.RecogerMoneda();
        Destroy(gameObject);
    }
}
