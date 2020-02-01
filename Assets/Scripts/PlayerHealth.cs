using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private Player _player;

    [SerializeField]
    private Slider _healthSlider;

    [SerializeField]
    private Text _healthText;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _healthSlider.maxValue = _player.StartingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        _healthSlider.value = _player.Health;
        UpdateText();
    }

    public void UpdateText()
    {
        _healthText.text = $"{_player.Health.ToString()} / {_player.StartingHealth.ToString()}";
    }
}
