using UnityEngine;
using UnityEngine.UI;

public class UIText : MonoBehaviour
{
    [SerializeField]
    private string _text;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Text>().text = _text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string text) {
        _text = text;
        this.GetComponent<Text>().text = _text;
    }
}
