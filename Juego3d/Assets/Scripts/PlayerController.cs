using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Requerimos automáticamente los componentes para evitar errores
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] Camera playerCamera; // Arrastra aquí tu cámara FreeLook

    [Header("Configuración de Movimiento")]
    [SerializeField] float walkSpeed = 3.0f;    // Velocidad de caminata
    [SerializeField] float runSpeed = 7.0f;     // Velocidad de carrera
    [SerializeField] float jumpForce = 8.0f;
    [SerializeField] float gravityScale = 2.0f; // Mayor gravedad = salto más "nítido"

    // Variables privadas
    private CharacterController playerController;
    private Animator animator;
    private Vector3 moveDirection;
    private float currentSpeed; // Velocidad actual (caminata o carrera)

    //Tiempo de espera
    private float lastActionTime;
    private float cooldownDuration = 1.0f;

    void Start()
    {
        playerController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        // Bloqueamos el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        lastActionTime = -cooldownDuration;
    }

    void Update()
    {
        // --- 1. Procesamiento de entrada (WASD) ---
        // Obtenemos la entrada no como -1 a 1, sino específicamente 0 o 1 (o -1)
        float verticalInput = 0f;
        if (Input.GetKey(KeyCode.W)) verticalInput = 1f;
        if (Input.GetKey(KeyCode.S)) verticalInput = -1f;

        float horizontalInput = 0f;
        if (Input.GetKey(KeyCode.D)) horizontalInput = 1f;
        if (Input.GetKey(KeyCode.A)) horizontalInput = -1f;

        // --- 2. Lógica de Correr (Shift) ---
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        currentSpeed = isSprinting ? runSpeed : walkSpeed; // Seleccionamos la velocidad

        // --- 3. Cálculo de la dirección del movimiento ---
        // Guardamos 'y' (gravedad/salto) para no perderlo
        float yStore = moveDirection.y;

        // Giramos al personaje para que mire hacia donde mira la cámara (solo en el eje Y)
        transform.rotation = Quaternion.Euler(0f, playerCamera.transform.rotation.eulerAngles.y, 0f);

        // Vector de entrada (local)
        Vector3 inputVector = new Vector3(horizontalInput, 0f, verticalInput);

        // Normalizamos el vector de entrada para que el movimiento diagonal no sea más rápido
        if (inputVector.magnitude > 1f)
        {
            inputVector.Normalize();
        }

        // Calculamos la dirección del movimiento en relación con la rotación del personaje
        moveDirection = (transform.forward * inputVector.z) + (transform.right * inputVector.x);

        // Aplicamos la velocidad actual (caminata o carrera)
        moveDirection = moveDirection * currentSpeed;

        // Devolvemos la gravedad/salto guardado
        moveDirection.y = yStore;

        // --- 4. Lógica de Salto (Espacio) ---
        if (playerController.isGrounded)
        {
            // "Pegamos" al suelo mientras estamos parados
            moveDirection.y = -0.1f;

            // Saltamos al presionar Espacio
            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveDirection.y = jumpForce;
                
                animator.SetTrigger("Jump");
            }
        }
        // Atackar
        if (Input.GetMouseButtonDown(0) && (Time.time - lastActionTime) >= cooldownDuration)
        {
            // Si entra aquí, el usuario acaba de hacer clic
            animator.SetTrigger("Ataque");
            lastActionTime = Time.time;

            // Aquí pones tu lógica (disparar, interactuar, etc.)
        }

        // --- 5. Aplicación de la Gravedad ---
        // La gravedad se aplica constantemente si el personaje no está en el suelo
        moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;

        // --- 6. Control del Animator ---

        // inputVector.magnitude es tu "Math.Abs" de la velocidad.
        // Será 0 si no se presionan botones, y 1 si se presiona cualquiera (W, A, S, D).
        float animationSpeed = inputVector.magnitude;

        // "speed" - es un parámetro Float en el Animator
        animator.SetFloat("Speed", animationSpeed);

        // "isSprinting" - es un parámetro Bool en el Animator (tu "trigger")
        animator.SetBool("isSprinting", isSprinting);

        // (Opcional) Se puede agregar un parámetro para la caída
        // animator.SetBool("isGrounded", playerController.isGrounded);

        // --- 7. Movimiento del Personaje ---
        playerController.Move(moveDirection * Time.deltaTime);
    }
}