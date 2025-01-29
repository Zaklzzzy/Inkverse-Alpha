using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AlphaInteractive : MonoBehaviour
{
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
            else
            {
                DeselectObject();
            }
        }
        else
        {
            Debug.Log("NONE");
        }
    }

/*    public void CatchScroll(InputAction.CallbackContext context)
    {
        if (_currentInteractable != null)
        {
            float alphaValue = 0;
            var scrollInput = _input.Gameplay.Scroll.ReadValue<float>();

            alphaValue += scrollInput * 0.1f;
            alphaValue = Mathf.Clamp01(alphaValue);

            _currentInteractable.SetAlphaValue(alphaValue);

            Debug.Log($"Scroll: {scrollInput}, Alpha: {alphaValue}");
        }
    }*/
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
}
