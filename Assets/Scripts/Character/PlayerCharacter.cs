using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class PlayerCharacter : CharacterBase
{
    public List<ActionBase> actions;

    private bool actionSelected = false;
    private ActionBase selectedAction;
    public ActionBase lastAction;
    public ActionBase previousAction;


    public override IEnumerator TakeTurn()
    {
        yield return base.TakeTurn(); // checks for Stunned
        if (IsDead()) yield break;

        TurnJumpController jumper = GetComponent<TurnJumpController>();
        if (jumper != null)
            yield return jumper.JumpIn(this is PlayerCharacter);

        //Debug.Log("Take Turn");
        CombatUIManager.Instance.ShowActions(actions, this);
        //Debug.Log("show action");
        actionSelected = false;
        selectedAction = null;

        while (!actionSelected)
            yield return null;

        bool targetChosen = false;

        Debug.Log("choose target");
        TargetingManager.Instance.StartTargeting(this, selectedAction.targetingType, (CharacterBase target) =>
        {
            selectedAction.PerformAction(this, target);
            targetChosen = true;
        });

        while (!targetChosen)
            yield return null;
            
        if (jumper != null)
            yield return jumper.JumpBack();

        yield return new WaitForSeconds(1f);
    }

    public void SelectAction(ActionBase action)
    {
        selectedAction = action;
        actionSelected = true;

        previousAction = lastAction;
        lastAction = action;
    }
}
