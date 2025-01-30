using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaInteractable : MonoBehaviour
{
    private bool _isInteractive = false;

    public void EnableInteraction()
    {
        _isInteractive = true;
    }
    public void DisableInteraction()
    {
        _isInteractive = false;
    }
    public void SetAlphaValue(float newAlpha)
    {
        if (_isInteractive)
        {   
            var color = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, Mathf.Clamp01(color.a + newAlpha));
            
            if (newAlpha == 0)
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
