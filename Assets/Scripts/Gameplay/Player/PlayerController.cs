using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 10f;
    [Header("Jump Settings")]
    [SerializeField] private float _jumpForce = 7f;
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;
    [Header("Dash Settings")]
    [SerializeField] private float _dashSpeed = 30f;
    [SerializeField] private float _dashDuration = 0.2f;
    [SerializeField] private float _dashCooldown = 1.5f;
    [Header("Rotate")]
    [SerializeField] private GameObject _player;
    [Header("Animation")]
    [SerializeField] private Animator _playerAnim;

    private Rigidbody2D _rb;
    private Vector2 _moveInput;

    private bool _isGrounded = false;

    private bool _isDashReady = true;
    private bool _isDashing = false;

    private GameInput _input;

    private void Awake()
    {
        _input = new GameInput();
        _input.Enable();

        _rb = GetComponent<Rigidbody2D>();

        _input.Gameplay.Jump.performed += Jump;
        _input.Gameplay.Dash.performed += Dash;
    }

    private void FixedUpdate()
    {
        CheckGround();
        Move();
    }

    private void Move()
    {
        if (_isDashing) return;

        _moveInput = _input.Gameplay.Move.ReadValue<Vector2>();
        float moveDirection = _moveInput.x;

        _rb.velocity = new Vector2(moveDirection * _moveSpeed, _rb.velocity.y);

        Flip(moveDirection);
        Animate(moveDirection);
    }

    private void Flip(float moveDirection)
    {
        if (moveDirection < 0)
        {
            _player.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (moveDirection > 0)
        {
            _player.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void Animate(float moveDirection)
    {

        if (moveDirection == 0)
        {
            _playerAnim.SetBool("IsRunning", false);
        }
        else
        {
            _playerAnim.SetBool("IsRunning", true);
        }

        if (_isGrounded)
        {
            _playerAnim.SetBool("IsJump", false);
        }
        else
        {
            _playerAnim.SetBool("IsJump", true);
        }
    }

    #region Dash
    private void Dash(InputAction.CallbackContext context)
    {
        if (!_isDashReady) return;
        else if (!_isDashing) StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        _isDashReady = false;

        _isDashing = true;
        float originalGravity = _rb.gravityScale;
        _rb.gravityScale = originalGravity / 3;
        _playerAnim.SetTrigger("Dash");
        _rb.velocity = new Vector2(transform.localScale.x * _dashSpeed * (_player.transform.rotation.y == 0 ? 1 : -1), 0);

        yield return new WaitForSeconds(_dashDuration);

        _rb.gravityScale = originalGravity;
        _isDashing = false;

        StartCoroutine(DashCooldown());
        StartCoroutine(UIManager.Instance.CooldownIndicate(_dashCooldown));
    }

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(_dashCooldown);

        _isDashReady = true;
    }
    #endregion

    #region Jump
    private void Jump(InputAction.CallbackContext context)
    {
        if (_isGrounded)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
            _playerAnim.SetTrigger("TakeOf");
        }
    }
    private void CheckGround()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheckPoint.position, _groundCheckRadius, _groundLayer);
    }
    #endregion
}
