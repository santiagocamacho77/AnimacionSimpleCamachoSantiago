using UnityEngine;
using UnityEngine.InputSystem;

// Controla al personaje: movimiento con flechas, salto, accion especial y animaciones.
// Parametros que debe tener el Animator (respetar mayusculas):
//   Speed (Float), IsGrounded (Bool), Accion (Trigger), Celebrar (Trigger)
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;
    public float fuerzaSalto = 10f;
    public float limiteCaida = -10f;   // si cae por debajo de esta altura, reaparece

    [Header("Sonidos (opcional)")]
    public AudioClip sonidoSalto;
    public AudioClip sonidoAccion;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sprite;

    float inputX;
    bool enSuelo;
    bool puedeMoverse = true;
    Vector3 posicionInicial;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb.freezeRotation = true;          // para que el personaje no se caiga de lado
    }

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        var teclado = Keyboard.current;
        if (teclado == null) return;

        inputX = 0f;
        if (puedeMoverse)
        {
            // Flechas (o A/D) para moverse
            if (teclado.leftArrowKey.isPressed || teclado.aKey.isPressed) inputX -= 1f;
            if (teclado.rightArrowKey.isPressed || teclado.dKey.isPressed) inputX += 1f;

            // Flecha arriba o Espacio para saltar (animacion de salto)
            if ((teclado.upArrowKey.wasPressedThisFrame || teclado.spaceKey.wasPressedThisFrame) && enSuelo)
                Saltar();

            // Tecla X: animacion especial
            if (teclado.xKey.wasPressedThisFrame)
            {
                if (anim != null) anim.SetTrigger("Accion");
                Sonar(sonidoAccion);
            }
        }

        // Voltear el sprite segun la direccion
        if (inputX != 0f && sprite != null) sprite.flipX = inputX < 0f;

        // Mandar datos al Animator
        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(inputX));
            anim.SetBool("IsGrounded", enSuelo);
        }

        if (transform.position.y < limiteCaida) Morir();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(inputX * velocidad, rb.linearVelocity.y);
    }

    void Saltar()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
        enSuelo = false;
        Sonar(sonidoSalto);
    }

    // Detecta el suelo: si algo nos toca por debajo, estamos en el suelo
    void OnCollisionStay2D(Collision2D col)
    {
        for (int i = 0; i < col.contactCount; i++)
            if (col.GetContact(i).normal.y > 0.5f) { enSuelo = true; return; }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        enSuelo = false;
    }

    // Lo llama la meta cuando el jugador gana
    public void Celebrar()
    {
        puedeMoverse = false;
        inputX = 0f;
        rb.linearVelocity = Vector2.zero;
        if (anim != null) anim.SetTrigger("Celebrar");
    }

    // Lo llaman los obstaculos: el jugador vuelve al inicio
    public void Morir()
    {
        transform.position = posicionInicial;
        rb.linearVelocity = Vector2.zero;
        if (GameManager.Instance != null) GameManager.Instance.RegistrarCaida();
    }

    void Sonar(AudioClip clip)
    {
        if (clip != null) AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }
}
