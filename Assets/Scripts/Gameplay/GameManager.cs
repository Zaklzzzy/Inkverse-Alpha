using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _cursor;

    private bool _pauseState = false;

    private void Awake()
    {
        ShowCursor(false);
    }

    public void EnablePause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Pause();
        }
    }
    public void Pause()
    {
        if (_pauseState)
        {
            UIManager.Instance.ShowPause(false);
            Time.timeScale = 1f;
            _pauseState = false;
        }
        else
        {
            UIManager.Instance.ShowPause(true);
            Time.timeScale = 0f;
            _pauseState = true;
        }
        ShowCursor(_pauseState);
        _cursor.SetActive(!_pauseState);
    }

    private void ShowCursor(bool state)
    {
        Cursor.visible = state;
    }
}
