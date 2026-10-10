using UnityEngine;

// Ponlo en la Main Camera y arrastra al jugador en "Objetivo".
public class CameraFollow : MonoBehaviour
{
    public Transform objetivo;
    public Vector3 desfase = new Vector3(0f, 1f, -10f);
    public float suavizado = 5f;

    void LateUpdate()
    {
        if (objetivo == null) return;
        transform.position = Vector3.Lerp(transform.position, objetivo.position + desfase, suavizado * Time.deltaTime);
    }
}
