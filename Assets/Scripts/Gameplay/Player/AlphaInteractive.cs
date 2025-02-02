using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AlphaInteractive : MonoBehaviour
{
    [SerializeField] private int _bulletsCapacity = 5;
    [SerializeField, Range(0, 5)] private int _bulletsCount = 5;
    [SerializeField] private ParticleSystem _gunParticles;
    private bool _isReloading = false;

    private AlphaInteractable _currentInteractable;

    private GameInput _input;

    private void Awake()
    {
        _input = new GameInput();
        _input.Enable();
    }

    private void FixedUpdate()
    {
        CheckHover();
    }

    private void CheckHover()
    {
        var cursorPosition = Camera.main.ScreenToWorldPoint(_input.Gameplay.Cursor.ReadValue<Vector2>());
        cursorPosition.z = 0;

        var hit = Physics2D.OverlapPoint(cursorPosition);

        if (hit != null)
        {
            var interactable = hit.transform.GetComponent<AlphaInteractable>();
            if (interactable != null)
            {
                _currentInteractable = interactable;
                SelectObject();
            }
        }
        else if (_currentInteractable != null)
        {
            DeselectObject();
            _currentInteractable = null;
        }
    }

    public void CatchScroll(InputAction.CallbackContext context)
    {
        if (context.started && _currentInteractable != null)
        {
            var scrollInput = context.ReadValue<float>();

            if (!_currentInteractable.GetHideState()) _currentInteractable.SetAlphaValue(scrollInput == -1 ? -0.097f : 0.097f);
        }
    }

    public void SelectObject()
    {
        Debug.Log("SelectObject");
        _currentInteractable.EnableInteraction();
    }
    public void DeselectObject()
    {
        Debug.Log("DeselectObject");
        _currentInteractable.DisableInteraction();
    }

    #region Alpha Gun
    public void Shot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_bulletsCount == 0 || Time.timeScale == 0f) return;

            _gunParticles.Play();

            Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(_input.Gameplay.Cursor.ReadValue<Vector2>());
            var radius = 1.75f;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(cursorPosition, radius);

            foreach (var collider in colliders)
            {
                var interactable = collider.GetComponent<AlphaInteractable>();
                if (interactable != null && interactable.GetHideState()) interactable.Unhide();
            }

            _bulletsCount--;

            UIManager.Instance.ShowBulletCount(_bulletsCount);
            if (_bulletsCount == _bulletsCapacity - 1) TryReload();
        }
    }

    private void TryReload()
    {
        if (!_isReloading)
        {
            StartCoroutine(Reloading());
        }
    }

    private IEnumerator Reloading()
    {
        _isReloading = true;

        float timer = 0f;
        float reloadTime = 5f;

        while (timer < reloadTime)
        {
            timer += Time.deltaTime;
            UIManager.Instance.ShowBulletFill(timer / reloadTime);
            yield return null;
        }

        _bulletsCount++;
        UIManager.Instance.ShowBulletCount(_bulletsCount);
        if (_bulletsCount != _bulletsCapacity) StartCoroutine(Reloading());

        _isReloading = false;
    }
    #endregion
}
