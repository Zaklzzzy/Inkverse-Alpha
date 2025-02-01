using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Gameplay")]
    [SerializeField] private Image _dashCooldownImage;
    [SerializeField] private Image _bulletFiller;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #region Gameplay
    public void ActivateCursor(bool activeState)
    {
        Cursor.visible = activeState;
    }

    public IEnumerator CooldownIndicate(float cooldownDuration)
    {
        float timer = 0f;
        _dashCooldownImage.fillAmount = 1f;

        while (timer < cooldownDuration)
        {
            timer += Time.deltaTime;
            _dashCooldownImage.fillAmount = Mathf.Clamp01(1f - timer / cooldownDuration);
            yield return null;
        }

        _dashCooldownImage.fillAmount = 0f;
    }

    public void ShowBulletCount(float count)
    {
        _bulletFiller.fillAmount = Mathf.Clamp01(count);
    }
    #endregion

    #region UI
    #endregion
}
