Scripts Prontos (C#)
Crie dois arquivos em Assets/Scripts/:
📜 PlayerMovement.cs
(Movimentação para teste no PC, conforme exigido no item 1)

  using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidade = 5f;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("CharacterController não encontrado no jogador!");
        }
    }

    void Update()
    {
        // Só funciona no Editor (PC) para teste inicial
#if UNITY_EDITOR
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direcao = transform.right * horizontal + transform.forward * vertical;
        direcao.y = 0f; // Evita voar ou afundar

        controller.Move(direcao * velocidade * Time.deltaTime);
#endif
    }
}
