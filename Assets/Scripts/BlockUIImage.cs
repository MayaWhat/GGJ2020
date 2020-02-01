using UnityEngine;
using UnityEngine.UI;

public class BlockUIImage : MonoBehaviour
{
    private Image _image;

    void Start()
    {
        _image = GetComponent<Image>();
    }

    void Update()
    {
        if (Player.Instance.Block > 0)
        {
            _image.enabled = true;
        }
        else
        {
            _image.enabled = false;
        }
    }
}
