using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField]
    private Slider _healthSlider;
    
    [SerializeField]
    private Text _healthText;
    
    [SerializeField]
    private Text _blockText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.Enemy == null)
        {
            _healthSlider.enabled = false;
            _healthText.enabled = false;
            _blockText.enabled = false;

            return;
        }

        // it's not super efficient to do this every Update but ez to do now instead of having to write something
        // that detects when a new enemy is added and updating then
        _healthSlider.maxValue = GameManager.Instance.Enemy.StartingHealth;
        _healthSlider.enabled = true;
        _healthText.enabled = true;
        _blockText.enabled = true;
        _healthSlider.value = GameManager.Instance.Enemy?.Health ?? 0;

        UpdateText();
    }

    public void UpdateText()
    {
        _healthText.text = $"{GameManager.Instance.Enemy.Health.ToString()}  {GameManager.Instance.Enemy.StartingHealth.ToString()}";
        _blockText.text = $"{GameManager.Instance.Enemy.Block.ToString()}";
    }
}
