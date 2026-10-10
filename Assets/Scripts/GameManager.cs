using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Lleva la cuenta de monedas, muestra el HUD y decide cuando se gana.
// Ponlo en un GameObject vacio llamado "GameManager".
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Sonidos (opcional)")]
    public AudioClip sonidoMoneda;
    public AudioClip sonidoCaida;
    public AudioClip sonidoVictoria;

    int monedas;
    int totalMonedas;
    int caidas;
    bool gano;

    string mensaje = "";
    float mensajeHasta;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        // R para reiniciar el nivel
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Cada moneda se registra sola al empezar el juego
    public void RegistrarMoneda() { totalMonedas++; }

    public void RecogerMoneda()
    {
        monedas++;
        Sonar(sonidoMoneda);
    }

    public void RegistrarCaida()
    {
        caidas++;
        Sonar(sonidoCaida);
        MostrarMensaje("¡Cuidado!");
    }

    public void LlegarMeta(PlayerController jugador)
    {
        if (gano) return;
        if (monedas >= totalMonedas)
        {
            gano = true;
            jugador.Celebrar();
            Sonar(sonidoVictoria);
        }
        else
        {
            MostrarMensaje("Te faltan " + (totalMonedas - monedas) + " monedas");
        }
    }

    void MostrarMensaje(string texto)
    {
        mensaje = texto;
        mensajeHasta = Time.time + 2f;
    }

    void Sonar(AudioClip clip)
    {
        if (clip != null) AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }

    // HUD sencillo, no necesita Canvas
    void OnGUI()
    {
        var estilo = new GUIStyle(GUI.skin.label) { fontSize = 28, fontStyle = FontStyle.Bold };
        estilo.normal.textColor = Color.white;
        GUI.Label(new Rect(20, 15, 500, 40), "Monedas: " + monedas + " / " + totalMonedas, estilo);
        GUI.Label(new Rect(20, 50, 500, 40), "Caidas: " + caidas, estilo);

        var centro = new GUIStyle(estilo) { alignment = TextAnchor.MiddleCenter, fontSize = 40 };
        if (gano)
            GUI.Label(new Rect(0, Screen.height / 2 - 60, Screen.width, 120),
                "¡GANASTE!\nPresiona R para reiniciar", centro);
        else if (Time.time < mensajeHasta)
            GUI.Label(new Rect(0, Screen.height / 2 - 30, Screen.width, 60), mensaje, centro);
    }
}
