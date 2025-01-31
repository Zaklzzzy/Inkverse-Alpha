using UnityEngine;

public class CursorFollower : MonoBehaviour
{
    private GameInput _input;

    private void Awake()
    {
        _input = new GameInput();
        _input.Enable();
    }

    private void Update()
    {
        var cursorPosition = Camera.main.ScreenToWorldPoint(_input.Gameplay.Cursor.ReadValue<Vector2>());

        transform.position = new Vector3(cursorPosition.x, cursorPosition.y, 1);
    }
}