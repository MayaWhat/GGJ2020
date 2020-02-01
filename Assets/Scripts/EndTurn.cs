using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurn : MonoBehaviour
{
    private GameManager _gameManager;
    private Button _button;
    private Hand _hand;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _hand = FindObjectOfType<Hand>();
        _button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_hand.CanPlaySomething() && !Player.Instance.IsDead && !Enemy.Instance.IsDead) {
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

    private void PlayerDied()
    {
        _button.interactable = false;
    }

    private void EnemyDied()
    {
        _button.interactable = false;
    }

    void OnEnable()
	{
		Player.OnDeath += PlayerDied;
		Enemy.OnDeath += EnemyDied;
	}

	void OnDisable()
	{
		Player.OnDeath -= PlayerDied;
		Enemy.OnDeath += EnemyDied;
	}
}
