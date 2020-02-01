using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    private int _energy;
    private Image[] _energies;

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
         _energies = transform.GetComponentsInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i<3; i++)
        {
            if (_energy > i)
            {
                _energies[i].enabled = true;
            }
            else
            {
                _energies[i].enabled = false;
            }
        }
    }

    public void ResetEnergy()
    {
        _energy = _startingEnergy;
    }
}
