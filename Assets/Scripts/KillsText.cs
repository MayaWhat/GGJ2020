using UnityEngine;
using UnityEngine.UI;

public class KillsText : MonoBehaviour
{
    [SerializeField]
    private string _text;
    private int _count;
    private Text _textUI;

    // Start is called before the first frame update
    void Start()
    {
        _textUI = this.GetComponent<Text>();
        _text = "0  kills";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateText()
    {
        _count++;
        SetText(_count.ToString());
    }

    public void SetText(string kills) {
        _text = $"{kills}  kills";
        _textUI.text = _text;
    }

    private void OnEnable() {
        GameManager.EnemyDefeatedEvent += UpdateText;
    }

    private void OnDisable() {
        GameManager.EnemyDefeatedEvent -= UpdateText;    
    }
}
