using Controllers;
using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public enum GameEventType
{
    /// <summary>
    /// Use SpawnMonsterParam
    /// </summary>
    SpawnMonster = 0,
    /// <summary>
    /// Use OpenLineParam
    /// </summary>
    OpenLine = 1,
    /// <summary>
    /// Use OpenLineParam
    /// </summary>
    NextStage = 3,
}

public class GameEvent
{
    public class ByEmitSecond : IComparer<GameEvent>
    {
        public int Compare(GameEvent x, GameEvent y)
        {
            if (x.EmitSecond > y.EmitSecond)
            {
                return 1;
            }
            else if(x.EmitSecond < y.EmitSecond)
            {
                return -1;
            }
            return 0;
        }
    }
    /// <summary>
    /// Second at which event will be applied
    /// </summary>
    public float EmitSecond;

    /// <summary>
    /// Marks what kind of event it is
    /// </summary>
    public GameEventType EventType;

    /// <summary>
    /// Type of param depends on EventType variable
    /// </summary>
    public object EventParam;
}
public class SpawnMonsterParam
{
    public List<int> LineNumbers = new List<int>();
    public List<int> MonstersId = new List<int>();
    public List<AIController> MonsterAiControllers = new List<AIController>();
    public List<PackedScene> MonstersPackedScenes = new List<PackedScene>();
}
public enum OpenLineSide
{
    North=0,
    South=1
};
public class OpenLineParam
{
    public OpenLineParam(OpenLineSide sideToSet)
    {
        Side = sideToSet;
    }
    public OpenLineSide Side;
}
