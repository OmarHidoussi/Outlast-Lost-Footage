using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptiveMusic : MonoBehaviour
{

    #region Variables
    public EnemyAI agent;
    public EnemySight sight;

    public bool ChaseStart;
    public bool IsChasing;
    public bool Investigating;
    public bool InvestigationEnd;

    private bool PreviousSight;
    private bool currentSight;
    #endregion

    #region BuildInMethods
    // Start is called before the first frame update
    void Start()
    {
        ChaseStart = false;
        IsChasing = false;
        Investigating = false;
        InvestigationEnd = false;

        currentSight = sight.PlayerInSight;
        PreviousSight = currentSight;
    }

    // Update is called once per frame
    void Update()
    {
        currentSight = sight.PlayerInSight;

        if (currentSight != PreviousSight)
            ChaseStart = true;

        PreviousSight = currentSight;

        IsChasing = agent.IsChasing;
        Investigating = agent.IsInvestigating;

        if (Investigating)
            InvestigationEnd = agent.isSearching;
    }
    #endregion

    #region CustomMethods

    void GameState()
    {

    }

    #endregion

}
