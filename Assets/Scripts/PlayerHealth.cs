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

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _healthSlider.maxValue = _player.StartingHealth;
        _blockSlider.maxValue = _player.StartingHealth;
        _blockSlider.value = 0;
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
        _healthText.text = $"{_player.Health.ToString()}     {_player.StartingHealth.ToString()}";
        _blockText.text = _player.Block.ToString();
    }
}
