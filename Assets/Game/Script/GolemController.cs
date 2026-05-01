using UnityEngine;

public class GolemController : MonoBehaviour
{
    [Header("Текущая форма")] public GolemForm currentForm = GolemForm.Titan;
    [SerializeField]private GameObject TitanTaran;

    [Header("Общие настройки движения")] [SerializeField]
    private float titanSpeed = 2.5f;

    [SerializeField] private float strangerSpeed = 4f;
    [SerializeField] private float strangerJumpForce = 8f;
    [SerializeField] private float chrysalisSpeed = 1.8f;
    [SerializeField] private float butterflySpeed = 5f;
    [SerializeField] private float butterflyFlapForce = 10f;

    [Header("Проверка земли (для прыжка)")] [SerializeField]
    private Transform groundCheck;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;


    private Rigidbody2D rb;
    [SerializeField]private bool isGrounded;
    private float horizontalInput;


    private bool facingRight = true;

    public enum GolemForm
    {
        Titan,
        Stranger,
        Chrysalis,
        Butterfly
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        TaranEnabler();
    }

    private void Update()
    {
        if (currentForm != GolemForm.Butterfly)
            horizontalInput = Input.GetAxisRaw("Horizontal");


        if (currentForm == GolemForm.Stranger && Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * strangerJumpForce, ForceMode2D.Impulse);
        }


        if (currentForm == GolemForm.Butterfly && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * butterflyFlapForce * Time.deltaTime, ForceMode2D.Force);
        }


        // Отражение персонажа (кроме бабочки – ей можно лететь в любую сторону)
        if (currentForm != GolemForm.Butterfly && horizontalInput != 0)
            Flip(horizontalInput > 0);
    }

    private void FixedUpdate()
    {
        if (currentForm != GolemForm.Butterfly)
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);


        switch (currentForm)
        {
            case GolemForm.Titan:
                TitanMovement();
                break;
            case GolemForm.Stranger:
                StrangerMovement();
                break;
            case GolemForm.Chrysalis:
                ChrysalisMovement();
                break;
            case GolemForm.Butterfly:
                ButterflyMovement();
                break;
        }
    }


    private void TitanMovement()
    {
        rb.linearVelocity = new Vector2(horizontalInput * titanSpeed, rb.linearVelocity.y);
    }

    private void StrangerMovement()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(horizontalInput * strangerSpeed, rb.linearVelocity.y);
        }
    }

    private void ChrysalisMovement()
    {
        rb.linearVelocity = new Vector2(horizontalInput * chrysalisSpeed, rb.linearVelocity.y);
    }

    private void ButterflyMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 targetVelocity = new Vector2(moveX, moveY) * butterflySpeed;
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, 0.1f);
    }
    private void TaranEnabler()
    {
        TitanTaran.SetActive(currentForm == GolemForm.Titan);
    }


    public void ChangeForm(GolemForm newForm)
    {
        currentForm = newForm;
        Debug.Log($"Голем превратился в {newForm}");
        TaranEnabler();
    }


    private void Flip(bool faceRight)
    {
        if (faceRight != facingRight)
        {
            facingRight = faceRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}