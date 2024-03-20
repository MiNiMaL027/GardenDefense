using Godot;
using System;
using Enums;
using System.Collections.Generic;
using ItemsId;
using System.Linq;

namespace Widgets
{
    
    public partial class WorldTimer : Control
    {
        public Label LabelTime;
        public Timer Timer;
        public int CurrentSecond = 0;
        public int OpenLineDelay = 20;
        public Random randomizer = new Random();
        public WorldTimerMode worldTimerMode = WorldTimerMode.None;
        SortedSet<WorldTimerEvent> nextEvents = new SortedSet<WorldTimerEvent>(new WorldTimerEvent.ByEmitSecond());
        public override void _Ready()
        {
            LabelTime = GetNode<Label>("VBoxContainer/LabelTime");
            Timer = GetNode<Timer>("Timer");

        }

        public void Timer_DefaultTimeout()
        {
            LabelTime.Text = TimeSpan.FromSeconds(CurrentSecond).ToString("mm\\:ss");
            WorldTimerEvent worldTimerEvent = null;
            do
            {
                worldTimerEvent = nextEvents.FirstOrDefault();
                if (worldTimerEvent != null)
                {
                    if (CurrentSecond == worldTimerEvent.EmitSecond)
                    {
                        ApplyEvent(worldTimerEvent);
                        nextEvents.Remove(worldTimerEvent);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            } while (true);
            
            CurrentSecond++;
        }
        public void ApplyEvent(WorldTimerEvent worldTimerEvent)
        {
            switch (worldTimerEvent.EventType)
            {
                case WorldTimerEventType.SpawnMonster:
                    SpawnMonsterParam param = worldTimerEvent.EventParam as SpawnMonsterParam;
                    for(int i = 0; i < param.LineNumbers.Count; i++)
                    {
                        GameInstance.World.TowerDefenseArea.SpawnMonster(param.LineNumbers[i], param.MonstersId[i]);
                    }
                    ScheduleSpawnMonsterEvent();
                    break;
                case WorldTimerEventType.OpenLine:
                    OpenLineSide openLineSide = (worldTimerEvent.EventParam as OpenLineParam).Side;
                    GameInstance.World.TowerDefenseArea.AddLine(openLineSide);

                    OpenLineSide nextOpenLineSide = openLineSide == OpenLineSide.North ? OpenLineSide.South : OpenLineSide.North;
                    ScheduleOpenLineEvent(openLineSide);
                    break;
            }
            
        }
        public void ScheduleOpenLineEvent(OpenLineSide openLineSide)
        {
            WorldTimerEvent openLineTimerEvent = new WorldTimerEvent();
            openLineTimerEvent.EmitSecond = CurrentSecond + OpenLineDelay;
            openLineTimerEvent.EventType = WorldTimerEventType.OpenLine;
            openLineTimerEvent.EventParam = new OpenLineParam(openLineSide);
            nextEvents.Add(openLineTimerEvent);
        }
        public void ScheduleSpawnMonsterEvent()
        {
            WorldTimerEvent spawnMonsterTimerEvent = new WorldTimerEvent();
            spawnMonsterTimerEvent.EmitSecond = CurrentSecond + randomizer.Next(5, 11);
            spawnMonsterTimerEvent.EventType = WorldTimerEventType.SpawnMonster;
            SpawnMonsterParam spawnMonsterParam = new SpawnMonsterParam();
            spawnMonsterTimerEvent.EventParam = spawnMonsterParam;

            int lastNorthernLine = GameInstance.World.TowerDefenseArea.LastNorthernLine;
            int lastSouthernLine = GameInstance.World.TowerDefenseArea.LastSouthernLine;


            spawnMonsterParam.MonstersId.Add(PawnId.Monsters.Ant);

            /////decide what monster to spawn
            //if (randomizer.NextDouble() > 0.5)
            //{
            //    spawnMonsterParam.MonstersId.Add(PawnId.Monsters.Ant);
            //}
            //else
            //{
            //    spawnMonsterParam.MonstersId.Add(PawnId.Monsters.AntDog);

            //}
            spawnMonsterParam.LineNumbers.Add(randomizer.Next(lastNorthernLine, lastSouthernLine + 1));

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
                    nextEvents.Clear();
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

                    #region AddStartEvents
                    ScheduleOpenLineEvent(OpenLineSide.North);
                    ScheduleSpawnMonsterEvent();
                    #endregion

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
    }

}
