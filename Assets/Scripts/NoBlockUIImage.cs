using UnityEngine;
using UnityEngine.UI;

public class NoBlockUIImage : MonoBehaviour
{
    private Image _image;

    void Start()
    {
        _image = GetComponent<Image>();
    }

    void Update()
    {
        if (Player.Instance.Block < 1)
        {
            _image.enabled = true;
        }
        else
        {
            _image.enabled = false;
        }
    }
}
