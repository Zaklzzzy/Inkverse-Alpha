using UnityEngine;

public class AlphaInteractable : MonoBehaviour
{
    private bool _isHiden = true;
    private bool _isInteractive = false;

    public bool GetHideState() { return _isHiden; }
    public void Unhide()
    {
        _isHiden = false;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public void EnableInteraction()
    {
        if (!_isHiden) _isInteractive = true;
    }
    public void DisableInteraction()
    {
        if (!_isHiden) _isInteractive = false;
    }
    public void SetAlphaValue(float newAlpha)
    {
        if (_isInteractive && !_isHiden)
        {   
            var color = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, Mathf.Clamp((color.a + newAlpha), 0.03f, 1f));
            
            if (GetComponent<SpriteRenderer>().color.a == 0.03f)
            {
                GetComponent<BoxCollider2D>().isTrigger = true;
            }
            else
            {
                GetComponent<BoxCollider2D>().isTrigger = false;
            }
        }
    }
}
