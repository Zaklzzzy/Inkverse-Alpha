using UnityEngine;
using UnityEngine.InputSystem;

public class AlphaInteractive : MonoBehaviour
{
    [SerializeField, Range(0, 10)] private int _bulletsCount = 5;
    
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
            if(interactable != null)
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

            if(!_currentInteractable.GetHideState()) _currentInteractable.SetAlphaValue(scrollInput == -1 ? -0.097f : 0.097f );
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
        if(context.started)
        {
            if (_bulletsCount == 0) return;

            Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(_input.Gameplay.Cursor.ReadValue<Vector2>());
            var radius = 1.75f;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(cursorPosition, radius);

            foreach (var collider in colliders)
            {
                var interactable = collider.GetComponent<AlphaInteractable>();
                if (interactable != null && interactable.GetHideState()) interactable.Unhide();
            }

            UIManager.Instance.ShowBulletCount(_bulletsCount);
            _bulletsCount--;
        }
    }
    #endregion
}
