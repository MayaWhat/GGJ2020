using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour
{
    private Canvas _canvas;
    private Image _image;

    // Start is called before the first frame update
    void Start()
    {
        _canvas = FindObjectOfType<Canvas>();  
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Flash()
    {
        StartCoroutine(AnimateFlash());
    }

    IEnumerator AnimateFlash()
    {
        transform.SetAsLastSibling();
        _image.color = new Color(1f, 1f, 1f, 0f);
        _image.enabled = true;

        var totalTime = 0.05f;
        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / totalTime)
        {
            _image.color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, 1f, t));

            yield return null;
        }

        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / totalTime)
        {
            _image.color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0f, t));

            yield return null;
        }

        _image.enabled = false;
    }
}
