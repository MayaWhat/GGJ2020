using UnityEngine;
using UnityEngine.UI;

public class NoBlockUIImageEnemy : MonoBehaviour
{
    private Image _image;

    void Start()
    {
        _image = GetComponent<Image>();
    }

    void Update()
    {
        if (GameManager.Instance.Enemy.Block < 1)
        {
            _image.enabled = true;
        }
        else
        {
            _image.enabled = false;
        }
    }
}
