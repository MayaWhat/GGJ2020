using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentTurnText : MonoBehaviour
{    
    private Sprite _yourTurnText;
    private Sprite _enemyTurnText;
    private Image _imageRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _yourTurnText = Resources.Load<Sprite>("Sprites/UI/Turn Text/turn_text_player");
        _enemyTurnText = Resources.Load<Sprite>("Sprites/UI/Turn Text/turn_text_monster");
        _imageRenderer = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.Phase == GameplayPhase.PlayCards ||
            GameManager.Instance.Phase == GameplayPhase.DrawCards)
        {
            _imageRenderer.enabled = true;
            _imageRenderer.sprite = _yourTurnText;
        }
        else if (GameManager.Instance.Phase == GameplayPhase.EnemyTurn)
        {
            _imageRenderer.enabled = true;
            _imageRenderer.sprite = _enemyTurnText;
        }
        else 
        {
            _imageRenderer.enabled = false;
            _imageRenderer.sprite = null;
        }
    }
}
