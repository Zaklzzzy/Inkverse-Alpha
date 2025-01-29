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
            GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, newAlpha);
            
            if (newAlpha == 0)
            {
                GetComponent<BoxCollider2D>().enabled = false;
                //GetComponent<BoxCollider2D>().isTrigger = true;
            }
            else
            {
                GetComponent<BoxCollider2D>().enabled = true;
                //GetComponent<BoxCollider2D>().isTrigger = false;
            }
        }
    }
}
