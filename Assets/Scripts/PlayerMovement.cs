using UnityEngine;

/// <summary>
/// Controla el movimiento horizontal del jugador en un juego 2D.
/// Compatible con Unity 6 (2023.3+), usa linearVelocity en lugar de velocity.
/// </summary>

//esto agrega un RigidBody al GameObject si no lo tiene
//esto evita errores en tiempo de ejecución
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Configuración de Movimiento")]

    [Tooltip("Velocidad de desplazamiento del jugador en unidades por segundo.")]
    [SerializeField] // editable en el Inspector, pero no desde otros scripts
    private float speed = 5f;

    [Header("Salto")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    //almacenará el valor de entrada horizontal del juegador
    private float moveInput;
    private bool isGrounded;



    /// <summary>
    /// Velocidad actual (sólo lectura externa).
    /// </summary>
    public float Speed => speed;

    /// <summary>
    /// Obtiene el componente Rigidbody2D al iniciar.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Lee el input del jugador en cada frame.
    /// </summary>
    private void Update()
    {
        // Leer input horizontal (A/D o ← →)
        //GetAxisRaw devuelve valores enteros (-1, 0, 1),
        //ideal para controles nítidos.
        moveInput = Input.GetAxisRaw("Horizontal");

        // Chequear si está tocando el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Saltar si presiona espacio y está en el suelo
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    /// <summary>
    /// Aplica el movimiento horizontal en el motor de físicas.
    /// </summary>
    // el comportamiento de físicas siempre se hace aquí no en Update
    private void FixedUpdate()
    {
        // Aplicar movimiento horizontal sin alterar la velocidad vertical
        // con API linearVelocity (Unity 6)
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
        //rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}

