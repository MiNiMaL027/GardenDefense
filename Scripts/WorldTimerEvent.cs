using System;
using System.Collections;
using System.Collections.Generic;

public enum WorldTimerEventType
{
    /// <summary>
    /// Use SpawnMonsterParam
    /// </summary>
    SpawnMonster = 0,
    /// <summary>
    /// Use OpenLineParam
    /// </summary>
    OpenLine = 1,

}

public class WorldTimerEvent
{
    public class ByEmitSecond : IComparer<WorldTimerEvent>
    {
        public int Compare(WorldTimerEvent x, WorldTimerEvent y)
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
    public int EmitSecond;

    /// <summary>
    /// Marks what kind of event it is
    /// </summary>
    public WorldTimerEventType EventType;

    /// <summary>
    /// Type of param depends on EventType variable
    /// </summary>
    public object EventParam;
}
public class SpawnMonsterParam
{
    public List<int> LineNumbers = new List<int>();
    public List<int> MonstersId= new List<int>();
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
