using System;
using System.Collections;
using System.Collections.Generic;
using Character.Base;
using UnityEngine;

public enum CombatActionType
{
    Attack,
    Defense,
    Heal,
}

[Serializable]
public class CombatMove
{
    public string MoveName;

    public virtual void DoMove(BaseCharacter user, List<BaseCharacter> targets)
    {
        Targets = targets;

        GameLog.Debug(this, $"Called {MoveName}");
        GameLog.Debug(this, targets);
    }
}

[Serializable]
public class CombatAction
{
    public CombatMove Move;
    public CombatActionType ActionType;
    public List<BaseCharacter> Targets;

    public 
}
