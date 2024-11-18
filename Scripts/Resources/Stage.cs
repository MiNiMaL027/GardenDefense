using Controllers;
using Enums;
using Godot;
using System.Collections.Generic;
using System.ComponentModel;

[GlobalClass]
public partial class Stage : Resource
{
    [Signal]
    public delegate void StageFinishEventHandler();

    [Export(PropertyHint.Range, "1,5")]
    [ExportGroup("Common")]
    public int LinesCount { get; set; }

    [Export]
    [ExportGroup("Common")]
    [Description("Duration of the stage in seconds")]
    public int StageDelay { get; set; }

    [Export]
    [ExportGroup("Common")]
    public StageType StageType { get; set; }



    [Export(PropertyHint.Range, "0,5")]
    [ExportGroup("Monser Settings")]
    public int Difficulty { get; set; }

    [Export]
    [ExportGroup("Monser Settings")]
    public int MinMonsterCount { get; set; }

    [Export]
    [ExportGroup("Monser Settings")]
    public int MaxMonsterCount { get; set; }

    [Export(PropertyHint.Range, "0,10,0.1")]
    [ExportGroup("Monser Settings")]
    public float SpawnRate { get; set; }


    private List<(int Line, AIController Cntroller)> _activeMonsters = new List<(int Line,AIController Cntroller)>();

    public List<(int Line, AIController Cntroller)> ActiveMonsters
    {
        get => _activeMonsters;
        set
        {
            _activeMonsters = value;

            if (_activeMonsters == null || _activeMonsters.Count == 0)
            {
                EmitSignal(SignalName.StageFinish);
                GameInstance.Hud.BattlefieldWidget.WorldTimer.RevokeNextSpawnMonsterEvent();
            }
        }
    }
    public void CheckIfFinished()
    {
        if (_activeMonsters == null || _activeMonsters.Count == 0)
        {
            EmitSignal(SignalName.StageFinish);
            GameInstance.Hud.BattlefieldWidget.WorldTimer.RevokeNextSpawnMonsterEvent();
        }
    }
}
