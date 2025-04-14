using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _runSpeed = 8f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _doubleJumpForce = 8f;

    private Rigidbody2D _body;
    private BoxCollider2D _boxCollider;
    private bool _grounded;
    private bool _canDoubleJump = false;
    private bool _touchingWall = false; 

    private bool _facingRight = true;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _body.freezeRotation = true;
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed = isRunning ? _runSpeed : _walkSpeed;

        _body.linearVelocity = new Vector2(horizontal * speed, _body.linearVelocity.y);

        if (horizontal > 0 && !_facingRight)
            Flip();
        else if (horizontal < 0 && _facingRight)
            Flip();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_grounded)
            {
                Jump();
                _canDoubleJump = true;
            }
            else if (_canDoubleJump && !_touchingWall) 
            {
                DoubleJump();
                _canDoubleJump = false;
            }
        }
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void Jump()
    {
        _body.linearVelocity = new Vector2(_body.linearVelocity.x, 0);
        _body.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        _grounded = false;
    }

    private void DoubleJump()
    {
        _body.linearVelocity = new Vector2(_body.linearVelocity.x, 0);
        _body.AddForce(Vector2.up * _doubleJumpForce, ForceMode2D.Impulse);
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _grounded = true;
            _canDoubleJump = false;
        }
    }
    
}
