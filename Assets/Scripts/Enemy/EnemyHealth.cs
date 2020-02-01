using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private Enemy _enemy;

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
        _enemy = FindObjectOfType<Enemy>();
        _healthSlider.maxValue = _enemy.Health;
        _blockSlider.maxValue = _enemy.StartingHealth;
        _blockSlider.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _healthSlider.value = _enemy.Health;
        _blockSlider.value = _enemy.Block;
        UpdateText();
    }

    public void UpdateText()
    {
        _healthText.text = $"{_enemy.Health.ToString()} / {_enemy.StartingHealth.ToString()}";
        _blockText.text = $"{_enemy.Block.ToString()}";
    }
}
