using UnityEngine;

/// <summary>
/// PlayerMovement — Movimentação do personagem via teclado (WASD) e mouse.
/// Anexe este script ao GameObject do jogador (ex: XR Origin ou Camera Rig).
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [Tooltip("Velocidade de caminhada")]
    public float moveSpeed = 4f;

    [Tooltip("Velocidade de corrida (Shift)")]
    public float runSpeed = 8f;

    [Tooltip("Força do pulo")]
    public float jumpForce = 4f;

    [Tooltip("Intensidade da gravidade")]
    public float gravity = -9.81f;

    [Header("Configurações da Câmera / Mouse")]
    [Tooltip("Sensibilidade horizontal do mouse")]
    public float mouseSensitivityX = 2f;

    [Tooltip("Sensibilidade vertical do mouse")]
    public float mouseSensitivityY = 2f;

    [Tooltip("Limite de ângulo de visão vertical (graus)")]
    public float verticalLookLimit = 80f;

    [Header("Referências")]
    [Tooltip("Câmera principal (filho do Player)")]
    public Camera playerCamera;

    // Internos
    private CharacterController characterController;
    private Vector3 velocity;
    private float verticalRotation = 0f;
    private bool isGrounded;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (characterController == null)
        {
            Debug.LogError("PlayerMovement: CharacterController não encontrado! Adicione um ao GameObject.");
        }

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            Debug.LogWarning("PlayerMovement: Camera não atribuída, usando Camera.main.");
        }

        // Tranca e esconde o cursor durante o jogo
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleCursorToggle();
    }

    /// <summary>Rotaciona o personagem e a câmera com o mouse.</summary>
    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY;

        // Rotação horizontal → gira o corpo do Player
        transform.Rotate(Vector3.up * mouseX);

        // Rotação vertical → inclina apenas a câmera
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    /// <summary>Move o personagem com WASD/setas e permite pulo.</summary>
    private void HandleMovement()
    {
        isGrounded = characterController.isGrounded;

        // Zera velocidade vertical quando no chão
        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        float h = Input.GetAxis("Horizontal"); // A / D
        float v = Input.GetAxis("Vertical");   // W / S

        Vector3 move = transform.right * h + transform.forward * v;

        // Corrida com Shift
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;
        characterController.Move(move * currentSpeed * Time.deltaTime);

        // Pulo com Espaço
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // Aplica gravidade
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    /// <summary>Pressione Escape para liberar/travar o cursor.</summary>
    private void HandleCursorToggle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
