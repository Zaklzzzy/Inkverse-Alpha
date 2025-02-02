using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Gameplay")]
    [SerializeField] private Image _dashCooldownImage;
    [SerializeField] private Image _bulletFiller;
    [SerializeField] private TextMeshProUGUI _bulletText;
    [SerializeField] private GameObject _shotEffect;
    [Header("UI")]
    [SerializeField] private GameObject _pauseContainer;
    [SerializeField] private GameObject _mainMenuContainer;
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

        GoToMenu();
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

    #region ShotSystem
    public void ShowBulletFill(float count)
    {
        _bulletFiller.fillAmount = Mathf.Clamp01(count);
    }
    public void ShowBulletCount(int count)
    {
        _bulletText.text = "x" + count;
    }

    public void ShotEffectAnimation(Vector2 position)
    {
        var duration = 1f;
        _shotEffect.gameObject.SetActive(true);
        _shotEffect.transform.position = position;
        _shotEffect.transform.DOShakeScale(duration, 0.8f, 4, 25f);
        StartCoroutine(ShotEffectHide(duration));
    }
    private IEnumerator ShotEffectHide(float duration)
    {
        float timer = 0f;

        var color = _shotEffect.GetComponent<SpriteRenderer>().color;
        _shotEffect.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 1f);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            _shotEffect.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 1f - timer / duration);
            yield return null;
        }

        _shotEffect.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 0f);
    }
    #endregion

    #endregion

    #region UI

    #region Pause
    public void ShowPause(bool enabledState)
    {
        _pauseContainer.SetActive(enabledState);
    }
    public void GoToMenu()
    {
        _mainMenuContainer.SetActive(true);
        _pauseContainer.SetActive(false);
        Time.timeScale = 0f;
        Cursor.visible = true;
    }
    #endregion

    #region Main Menu
    public void StartGame()
    {
        _mainMenuContainer.SetActive(false);
        _pauseContainer.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
    }
    public void Exit()
    {
        Application.Quit();
    }
    #endregion

    public void ChangeVolume()
    {
        AudioManager.Instance.SetVolume(_volumeSlider.value);
    }
    #endregion
}
