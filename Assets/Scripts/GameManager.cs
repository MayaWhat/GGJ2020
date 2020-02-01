using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    [SerializeField]
    private Deck _startDeck;
    private DrawPile _drawPile;
    private Hand _hand;
    public Enemy Enemy
    {
        get;
        private set;
    }

    private Enemy[] _enemyPool;

    public GameplayPhase Phase
    {
        get;
        private set;
    }
    private Player _player;

    private PlayerEnergy _playerEnergy;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        Phase = GameplayPhase.Calm;

        _hand = FindObjectOfType<Hand>();        
        _drawPile = FindObjectOfType<DrawPile>();
        _enemyPool = GetComponentsInChildren<Enemy>();
        _player = FindObjectOfType<Player>();
        _playerEnergy = FindObjectOfType<PlayerEnergy>();

        _drawPile.Init(_startDeck.GetCards());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) && Phase == GameplayPhase.Calm)
        {
            IntroduceEnemy();
        }
    }

    private void IntroduceEnemy()
    {
        Phase = GameplayPhase.IntroduceEnemy;

        var random = Random.Range(0, _enemyPool.Length);
        var selectedEnemy = _enemyPool[random];

        if (Enemy != null)
        {
            Enemy.OnDeath -= EnemyDefeated;
        }

        Enemy = Instantiate(selectedEnemy).GetComponent<Enemy>();
        Enemy.Appear = true;
        Enemy.OnDeath += EnemyDefeated;

        Debug.Log("Spawned new enemy");

        DrawCards();
    }

    private void DrawCards()
    {
        Phase = GameplayPhase.DrawCards;

        _player.StartTurn();
        _hand.DrawHand();
        _playerEnergy.ResetEnergy();

        PlayCards();
    }

    private void PlayCards()
    {
        Phase = GameplayPhase.PlayCards;
    }

    public void PlayerEndedTurn()
    {
        _hand.DiscardHand();
        EnemyTurn();
    }

    private void EnemyTurn()
    {
        Phase = GameplayPhase.EnemyTurn;

        Enemy.DoTurn();

        if (_player.Health > 0)
        {
            Enemy.DrawHand();
            DrawCards();
        }
        else
        {
            // TODO: YOU LOSE
        }
    }

    private void EnemyDefeated()
    {
        _hand.DiscardHand();

        // TODO: pick a shiny new card

        Phase = GameplayPhase.Calm;
    }
}

public enum GameplayPhase
{
    Calm,
    IntroduceEnemy,
    DrawCards,
    PlayCards,
    EnemyTurn,
    EnemyDefeated,
    PlayerDefeated
}