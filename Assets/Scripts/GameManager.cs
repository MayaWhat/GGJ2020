using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    [SerializeField]
    private Deck _startDeck;
    private Canvas _canvas;
    private DrawPile _drawPile;
    private Hand _hand;
    public Enemy Enemy
    {
        get;
        private set;
    }

    private Enemy[] _enemyPool;

    // super hacky lame way to allow other events to block things from happening
    public int Busyness
    {
        get;
        set;
    }

    public SoundManager Sounds
    {
        get;
        private set;
    }

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

        Busyness = 0;

        Phase = GameplayPhase.Calm;

        Sounds = FindObjectOfType<SoundManager>();
        _canvas = FindObjectOfType<Canvas>();
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
        if(Busyness > 0)
        {
            return;
        }

        switch(Phase)
        {
            case GameplayPhase.Calm:
                IntroduceEnemy();
                break;
            case GameplayPhase.PlayerTurnEnded:
                EnemyTurn();
                break;
        }

        if (Phase == GameplayPhase.Calm)
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
        Enemy.transform.SetParent(_canvas.transform);
        Enemy.transform.localPosition = new Vector3(0f, 150f, 0f);
        Enemy.ShouldAppear = true;
        Enemy.OnDeath += EnemyDefeated;
        Enemy.OnAppear += DrawCards;
        Debug.Log("Spawned new enemy");
    }

    private void DrawCards()
    {
        Phase = GameplayPhase.DrawCards;

        _player.StartTurn();
        _playerEnergy.ResetEnergy();
        _hand.DrawHand(() =>
        {
            PlayCards();
        });
    }

    private void PlayCards()
    {
        Phase = GameplayPhase.PlayCards;
    }

    public void PlayerEndedTurn()
    {
        if(Phase != GameplayPhase.PlayCards)
        {
            return;
        }
        
        Player.Instance.DamageTaken = 0;
        
        _hand.DiscardHand();

        Phase = GameplayPhase.PlayerTurnEnded;
    }

    private void EnemyTurn()
    {
        Phase = GameplayPhase.EnemyTurn;

        Enemy.ClearBlock();
        Enemy.DoTurn(() =>
        {
            if (_player.Health > 0)
            {
                Enemy.DrawHand(DrawCards);
            }
            else
            {
                // TODO: YOU LOSE
            }
        });        
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
    PlayerTurnEnded,
    EnemyTurn,
    EnemyDefeated,
    PlayerDefeated
}