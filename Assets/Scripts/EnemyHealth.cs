using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private Enemy _enemy;

    [SerializeField]
    private Slider _healthSlider;

    // Start is called before the first frame update
    void Start()
    {
        _enemy = FindObjectOfType<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        _healthSlider.value = _enemy.Health;
    }
}
