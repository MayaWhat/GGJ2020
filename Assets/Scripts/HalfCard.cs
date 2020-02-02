using UnityEngine;
using UnityEngine.UI;

public abstract class HalfCard : Card 
{
    [SerializeField]
    protected Image _highlightImage;

    [SerializeField]
    protected Image _activeImage;

    public bool IsLeftHalf;
    public bool Clickable = true;

    public bool Affordable
    {
        get
        {
            return _playerEnergy.Energy >= _cost;
        }
    }

    public override void AttemptToPlay() {
        if (!Clickable) {
            return;
        }

        if (!Affordable) {
            CantPlayFlash();
            return;
        }
        _playerHand.HalfCardSelected(this);
    }

    public void ChangeCardBack(bool active, bool matches) {
        _activeImage.enabled = active;
        _highlightImage.enabled = !active && matches;
    }
}