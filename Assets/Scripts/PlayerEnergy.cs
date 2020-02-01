using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    private int _energy;

    [SerializeField]
    private int _startingEnergy = 3;
    private Text _textComponent;

    public int Energy
    {
        get
        {
            return _energy;
        }
        set
        {
            _energy = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
         _textComponent = GetComponent<Text>();
         _energy = _startingEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        _textComponent.text = _energy.ToString();
    }

    public void ResetEnergy()
    {
        _energy = _startingEnergy;
    }
}
