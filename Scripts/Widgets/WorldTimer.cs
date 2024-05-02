using Godot;
using System;
using Enums;
using System.Collections.Generic;
using Ids;
using System.Linq;

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
            GameEvent worldTimerEvent = null;
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
                            battlefield.TowerDefenseArea.SpawnMonster(param.LineNumbers[i], param.MonstersPackedScenes[i]);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < param.LineNumbers.Count; i++)
                        {
                            battlefield.TowerDefenseArea.SpawnMonster(param.LineNumbers[i], param.MonstersId[i]);
                        }
                    }
                    break;
                case GameEventType.OpenLine:
                    OpenLineSide openLineSide = (worldTimerEvent.EventParam as OpenLineParam).Side;
                    battlefield.TowerDefenseArea.AddLine(openLineSide);

                    OpenLineSide nextOpenLineSide = openLineSide == OpenLineSide.North ? OpenLineSide.South : OpenLineSide.North;
                    ScheduleOpenLineEvent(openLineSide);
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
        //public void ScheduleSpawnMonsterEvent()
        //{
        //    GameEvent spawnMonsterTimerEvent = new GameEvent();
        //    spawnMonsterTimerEvent.EmitSecond = CurrentSecond + randomizer.Next(5, 11);
        //    spawnMonsterTimerEvent.EventType = GameEventType.SpawnMonster;
        //    SpawnMonsterParam spawnMonsterParam = new SpawnMonsterParam();
        //    spawnMonsterTimerEvent.EventParam = spawnMonsterParam;

        //    int lastNorthernLine = battlefield.TowerDefenseArea.LastNorthernLine;
        //    int lastSouthernLine = battlefield.TowerDefenseArea.LastSouthernLine;


        //    double rndNumber = randomizer.NextDouble();
        //    ///decide what monster to spawn
        //    if (randomizer.NextDouble() > 0.666)
        //    {
        //        spawnMonsterParam.MonstersId.Add(PawnId.Monsters.Ant);
        //    }
        //    else if(randomizer.NextDouble() > 0.333)
        //    {
        //        spawnMonsterParam.MonstersId.Add(PawnId.Monsters.AntDog);
        //    }
        //    else
        //    {
        //        spawnMonsterParam.MonstersId.Add(PawnId.Monsters.Wasp);
        //    }
        //    spawnMonsterParam.LineNumbers.Add(randomizer.Next(lastNorthernLine, lastSouthernLine + 1));

        //    nextEvents.Add(spawnMonsterTimerEvent);
        //}
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
    }

}
