using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Gameplay")]
    [SerializeField] private Image _dashCooldownImage;
    [SerializeField] private Image _bulletFiller;
    [SerializeField] private TextMeshProUGUI _bulletText;
    [Header("UI")]
    [SerializeField] private GameObject _pauseContainer;
    [SerializeField] private Slider _volumeSlider;

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

    private void OnEnable()
    {
        _volumeSlider.onValueChanged.AddListener(delegate { ChangeVolume(); });
        ChangeVolume();
    }

    private void OnDisable()
    {
        _volumeSlider.onValueChanged.RemoveAllListeners();
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

    public void ShowBulletFill(float count)
    {
        _bulletFiller.fillAmount = Mathf.Clamp01(count);
    }
    public void ShowBulletCount(int count)
    {
        _bulletText.text = "x" + count;
    }
    #endregion

    #region UI
    public void ShowPause(bool enabledState)
    {
        _pauseContainer.SetActive(enabledState);
    }

    public void ChangeVolume()
    {
        AudioManager.Instance.SetVolume(_volumeSlider.value);
    }
    #endregion
}
