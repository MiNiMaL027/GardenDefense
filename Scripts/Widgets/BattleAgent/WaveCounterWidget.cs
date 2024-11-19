using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class WaveCounterWidget : Control
{
	HBoxContainer BlockContainer;
	List<WaveTimerBlock> timerBlocks = new List<WaveTimerBlock>();
	WaveTimerBlock currentStageBlock;

	public override void _Ready()
	{
		BlockContainer = GetNode<HBoxContainer>("PanelContainer/MarginContainer/VBoxContainer/BlockContainer");
	}

	public void Init(Stage[] stages)
	{
		BlockContainer.RemoveChildren();
		foreach (var stage in stages)
		{
			var timerBlock = Scenes.Widgets.BattleWidget.WaveTimerBlock();
			BlockContainer.AddChild(timerBlock);
			timerBlock.Init(stage);

			timerBlocks.Add(timerBlock);
        }
    }

	public void FinishCurrentBlock()
	{
		currentStageBlock.StartWave(true);

		StartTimer();
	}

    public void StartTimer()
    {
		currentStageBlock = timerBlocks.FirstOrDefault(s => s.isFinished == false);
		if(currentStageBlock != null)
		{
            currentStageBlock.StartTimer();
        }
    }
}
