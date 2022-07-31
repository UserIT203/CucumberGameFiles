using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;

    private Vector2 lookDirection = new Vector2(0, -1);
    private Vector2 movementVelocity;

    public bool isAttacked = false;

    public Joystick joystick;


    //проверка нажатия 
    private void Update()
    { 
        if (!isAttacked && !DialogueManager.isActive)
        {
            movementVelocity.x = joystick.Horizontal;
            movementVelocity.y = joystick.Vertical;

            Vector2 moveInput = new Vector2(movementVelocity.x, movementVelocity.y);

            if (!Mathf.Approximately(moveInput.x, 0.0f) || !Mathf.Approximately(moveInput.y, 0.0f))
            {
                lookDirection.Set(moveInput.x, moveInput.y);
                lookDirection.Normalize();
            }

            anim.SetFloat("Horizontal", lookDirection.x);
            anim.SetFloat("Vertical", lookDirection.y);
            anim.SetFloat("Speed", movementVelocity.sqrMagnitude);

            movementVelocity = moveInput.normalized * moveSpeed;
        }else
            anim.SetFloat("Speed", 0);

    }

    //движение
    private void FixedUpdate()
    {
        if (!isAttacked && !DialogueManager.isActive)
            rb.MovePosition(rb.position + movementVelocity * Time.fixedDeltaTime);
    }
}
