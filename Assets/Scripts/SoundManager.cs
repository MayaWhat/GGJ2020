using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    public FMODUnity.StudioEventEmitter PlayerDrawCard;

    [SerializeField]
    public FMODUnity.StudioEventEmitter EnemyDrawCard;

    [SerializeField]
    public FMODUnity.StudioEventEmitter CombatImpact;

    [SerializeField]
    public FMODUnity.StudioEventEmitter CombatBlock;

    [SerializeField]
    public FMODUnity.StudioEventEmitter EnemyAppear;

    [SerializeField]
    public FMODUnity.StudioEventEmitter CastSpell;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
