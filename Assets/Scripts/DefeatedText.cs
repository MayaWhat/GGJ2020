using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DefeatedText : MonoBehaviour
{
    private Image _image;
    private Button _button;

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponentInChildren<Image>();
        _button = GetComponentInChildren<Button>();

        _image.enabled = false;
        _button.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Defeated()
    {
        FadeIn();
        _button.gameObject.SetActive(true);
        _image.enabled = true;
        transform.SetAsLastSibling();
        _image.color = new Color(1f,1f,1f,0f);

        StartCoroutine(FadeIn());
    }

    public void Restart()
    {
        SceneManager.LoadScene("StartScene");
    }

    IEnumerator FadeIn()
    {
        float aTime = 1f;
        float newAlphaValue = 1f;
        float alpha = _image.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, newAlphaValue, t));
            _image.color = newColor;
            yield return null;
        }
    }
}
