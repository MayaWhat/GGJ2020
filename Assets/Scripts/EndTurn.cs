using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurn : MonoBehaviour
{
    private GameManager _gameManager;

    private Hand _hand;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _hand = FindObjectOfType<Hand>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_hand.CanPlaySomething()) {
            GetComponent<Image>().color = Color.blue; 
        }
        else 
        {
            GetComponent<Image>().color = Color.white; 
        }
    }

    public void EndPlayerTurn() {
        _gameManager.PlayerEndedTurn();
    }
}
