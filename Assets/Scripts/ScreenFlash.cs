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
        _image.enabled = true;
        yield return new WaitForSeconds(0.05f);
        _image.enabled = false;
    }
}
