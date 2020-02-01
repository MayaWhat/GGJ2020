using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private Player _player;

    [SerializeField]
    private Slider _healthSlider;

    [SerializeField]
    private Text _healthText;

    [SerializeField]
    private Slider _blockSlider;
    
    [SerializeField]
    private Text _blockText;

    [SerializeField]
    private GameObject _blockPulseObject;
    private Image _blockPulseImage;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _healthSlider.maxValue = _player.StartingHealth;
        _blockSlider.maxValue = _player.StartingHealth;
        _blockSlider.value = 0;
        _blockPulseImage = _blockPulseObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        _healthSlider.value = _player.Health;
        _blockSlider.value = _player.Block;
        UpdateText();
    }

    public void UpdateText()
    {
        _healthText.text = $"{_player.Health.ToString()} I {_player.StartingHealth.ToString()}";
        _blockText.text = _player.Block.ToString();
    }

    private void FadeHitMarkerOut()
    {
        StartCoroutine(FadeHitMarker(0, .4f, null));
    }

    IEnumerator FadeHitMarker(float newAlphaValue, float aTime, Action onFinish)
    {
        float alpha = _blockPulseImage.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, newAlphaValue, t));
            _blockPulseImage.color = newColor;
            yield return null;
        }

        if (onFinish != null)
        {
            onFinish();
        }
    }

    private void BlockAnimation()
    {
        StartCoroutine(FadeHitMarker(1f, .1f, () => Invoke("FadeHitMarkerOut", 1f)));
    }

    void OnEnable()
    {
        Player.OnBlock += BlockAnimation;
    }

    void OnDisable()
    {
        Player.OnBlock -= BlockAnimation;
    }
}
