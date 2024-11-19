using Godot;
using System;
using Enums;
using System.Collections.Generic;
using Ids;
using System.Linq;
using Controllers;

namespace Widgets
{
    
    public partial class WorldTimer : Control
    {
        public Battlefield battlefield { get; set; }
        public Label LabelTime;
        public Timer Timer;
        public int CurrentSecond = 0;
        public int OpenLineDelay = 20;
        public Random randomizer = new Random();
        public WorldTimerMode worldTimerMode = WorldTimerMode.None;
        SortedSet<GameEvent> nextEvents = new SortedSet<GameEvent>(new GameEvent.ByEmitSecond());
        public override void _Ready()
        {
            LabelTime = GetNode<Label>("VBoxContainer/LabelTime");
            Timer = GetNode<Timer>("Timer");
            battlefield=GameInstance.World as Battlefield;
        }

        public void Timer_DefaultTimeout()
        {
            LabelTime.Text = TimeSpan.FromSeconds(CurrentSecond).ToString("mm\\:ss");
            GameEvent worldTimerEvent = nextEvents.FirstOrDefault();
            while(worldTimerEvent != null && worldTimerEvent.EmitSecond == CurrentSecond)
            {
                ApplyEvent(worldTimerEvent);
                nextEvents.Remove(worldTimerEvent);
                worldTimerEvent = nextEvents.FirstOrDefault();
            }     
            CurrentSecond++;
        }
        public void ApplyEvent(GameEvent worldTimerEvent)
        {
            switch (worldTimerEvent.EventType)
            {
                case GameEventType.SpawnMonster:
                    SpawnMonsterParam param = worldTimerEvent.EventParam as SpawnMonsterParam;
                    if (param.MonstersPackedScenes.Count > 0) //decide spawn scenes directly or spawn based on id
                    {
                        for (int i = 0; i < param.LineNumbers.Count; i++)
                        {
                            battlefield.TowerDefenseArea.SpawnMonster(param.LineNumbers[i], param.MonstersPackedScenes[i].Instantiate<AIController>());
                        }
                    }
                    else if(param.MonstersId.Count > 0)
                    {
                        for (int i = 0; i < param.LineNumbers.Count; i++)
                        {
                            battlefield.TowerDefenseArea.SpawnMonster(param.LineNumbers[i], param.MonstersId[i]);
                        }
                    }
                    else if(param.MonsterAiControllers.Count > 0)
                    {
                        for (int i = 0; i < param.LineNumbers.Count; i++)
                        {
                            battlefield.TowerDefenseArea.SpawnMonster(param.LineNumbers[i], param.MonsterAiControllers[i]);
                        }
                    }
                    break;
                case GameEventType.OpenLine:
                    OpenLineSide openLineSide = (worldTimerEvent.EventParam as OpenLineParam).Side;
                    battlefield.TowerDefenseArea.AddLine(openLineSide);

                    OpenLineSide nextOpenLineSide = openLineSide == OpenLineSide.North ? OpenLineSide.South : OpenLineSide.North;
                    ScheduleOpenLineEvent(openLineSide);
                    break;
                case GameEventType.NextStage:
                    battlefield.ScheduleNextStageTimeout();
                    break;
            }
            
        }
        public void ScheduleOpenLineEvent(OpenLineSide openLineSide)
        {
            GameEvent openLineTimerEvent = new GameEvent();
            openLineTimerEvent.EmitSecond = CurrentSecond + OpenLineDelay;
            openLineTimerEvent.EventType = GameEventType.OpenLine;
            openLineTimerEvent.EventParam = new OpenLineParam(openLineSide);
            nextEvents.Add(openLineTimerEvent);
        }
        public void ScheduleSpawnMonsterEvent(int timerSecond, List<int> lineNumbers, List<int> monsterIds)
        {
            GameEvent spawnMonsterTimerEvent = new GameEvent();
            spawnMonsterTimerEvent.EmitSecond = timerSecond;
            spawnMonsterTimerEvent.EventType = GameEventType.SpawnMonster;
            SpawnMonsterParam spawnMonsterParam = new SpawnMonsterParam();
            spawnMonsterTimerEvent.EventParam = spawnMonsterParam;
            spawnMonsterParam.LineNumbers = new List<int>(lineNumbers); //create copy of list
            spawnMonsterParam.MonstersId = new List<int>(monsterIds); //create copy of list

            nextEvents.Add(spawnMonsterTimerEvent);
        }
        public void ScheduleSpawnMonsterEvent(float timerSecond, int line, AIController monsterController)
        {
            GameEvent spawnMonsterTimerEvent = new GameEvent();
            spawnMonsterTimerEvent.EmitSecond = timerSecond;
            spawnMonsterTimerEvent.EventType = GameEventType.SpawnMonster;
            SpawnMonsterParam spawnMonsterParam = new SpawnMonsterParam();
            spawnMonsterTimerEvent.EventParam = spawnMonsterParam;
            spawnMonsterParam.LineNumbers = new List<int>() { line };
            spawnMonsterParam.MonsterAiControllers = new List<AIController>() { monsterController };

            nextEvents.Add(spawnMonsterTimerEvent);
        }

        public void ScheduleSpawnMonsterEvent(int timerSecond, List<int> lineNumbers, List<PackedScene> monsterScenes)
        {
            GameEvent spawnMonsterTimerEvent = new GameEvent();
            spawnMonsterTimerEvent.EmitSecond = timerSecond;
            spawnMonsterTimerEvent.EventType = GameEventType.SpawnMonster;
            SpawnMonsterParam spawnMonsterParam = new SpawnMonsterParam();
            spawnMonsterTimerEvent.EventParam = spawnMonsterParam;
            spawnMonsterParam.LineNumbers = new List<int>(lineNumbers); //create copy of list
            spawnMonsterParam.MonstersPackedScenes = new List<PackedScene>(monsterScenes); //create copy of list

            nextEvents.Add(spawnMonsterTimerEvent);
        }
        public void ScheduleSpawnMonsterEvent(int timerSecond, List<int> lineNumbers, List<AIController> monsterControllers)
        {
            GameEvent spawnMonsterTimerEvent = new GameEvent();
            spawnMonsterTimerEvent.EmitSecond = timerSecond;
            spawnMonsterTimerEvent.EventType = GameEventType.SpawnMonster;
            SpawnMonsterParam spawnMonsterParam = new SpawnMonsterParam();
            spawnMonsterTimerEvent.EventParam = spawnMonsterParam;
            spawnMonsterParam.LineNumbers = new List<int>(lineNumbers); //create copy of list
            spawnMonsterParam.MonsterAiControllers = new List<AIController>(monsterControllers); //create copy of list

            nextEvents.Add(spawnMonsterTimerEvent);
        }
        public void ScheduleNextStageEvent(int timerSecond)
        {
            GameEvent spawnMonsterTimerEvent = new GameEvent();
            spawnMonsterTimerEvent.EmitSecond = timerSecond;
            spawnMonsterTimerEvent.EventType = GameEventType.NextStage;
            nextEvents.Add(spawnMonsterTimerEvent);
        }
        public void Timer_CountDownTimeout()
        {
            LabelTime.Text = TimeSpan.FromSeconds(CurrentSecond).ToString("mm\\:ss");
            if(CurrentSecond == 0)
            {
                SetWorldTimerMode(WorldTimerMode.Default, 0);
            }
            else
            {
                CurrentSecond--;
            }
        }
        public void SetWorldTimerMode(WorldTimerMode worldTimerModeToSet, int? currentSecond = null)
        {
            ///leave previous state
            switch (worldTimerMode)
            {
                case WorldTimerMode.Default:
                    nextEvents.Clear();
                    Timer.Stop();
                    Timer.Timeout -= Timer_DefaultTimeout;
                    LabelTime.TooltipText = "";
                    break;
                case WorldTimerMode.CountDown:
                    Timer.Stop();
                    Timer.Timeout -= Timer_CountDownTimeout;
                    LabelTime.TooltipText = "";
                    break;
            }

            if(currentSecond != null)
            {
                CurrentSecond=currentSecond.Value;
            }
            worldTimerMode = worldTimerModeToSet;
            
            ///apply new state
            switch (worldTimerModeToSet)
            {
                case WorldTimerMode.Default:
                    LabelTime.TooltipText = "Survival time. Enemies are attacking.";
                    Timer.Timeout += Timer_DefaultTimeout;
                    Timer_DefaultTimeout();
                    Timer.Start();
                    break;
                case WorldTimerMode.CountDown:
                    LabelTime.TooltipText = "Timer countdown before attack";
                    Timer.Timeout += Timer_CountDownTimeout;
                    Timer_CountDownTimeout();
                    Timer.Start();
                    break;
            }

        }
        public void Init(WorldTimerMode worldTimerModeToSet, int? currentSecond = null)
        {
            SetWorldTimerMode(worldTimerModeToSet,currentSecond);
            
        }

        public void RevokeNextSpawnMonsterEvent()
        {
            GameEvent nextStageEvent = nextEvents.FirstOrDefault(e => e.EventType == GameEventType.NextStage);
            if (nextStageEvent != null)
            {
                nextEvents.Remove(nextStageEvent);
            }
        }
    }

}
