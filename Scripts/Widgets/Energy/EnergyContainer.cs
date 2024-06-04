using Controllers;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class EnergyContainer : PanelContainer
{
    HBoxContainer EnergyBox{ get; set; }
    List<Energy> Energies { get; set; } = new List<Energy>();
    Label EnergyCountLabel { get; set; }

    PlayerController playerController { get; set; }

    public override void _Ready()
    {
        base._Ready();

        EnergyBox = GetNode<HBoxContainer>("MarginContainer/HBoxContainer/EnergyBox");
        EnergyCountLabel = GetNode<Label>("MarginContainer/HBoxContainer/Label");

        playerController = this.GetPlayerController();

        Init();
    }

    public void Refresh(int newEnergy)
    {
        var filledEnergyCount = Energies.Where(e => e.filled).Count();
        if (filledEnergyCount > newEnergy)
        {
            var different = filledEnergyCount - newEnergy;
            var energiesToRemove = Energies.Where(e => e.filled).Take(different).ToList();

            foreach (var energy in energiesToRemove)
            {
                Energies.Remove(energy);
                energy.QueueFree();
                AddEnergy(0);
            }
        }
        else
        {
            var firstUnfiledEnergy = Energies.Where(e => !e.filled).FirstOrDefault();
            if (firstUnfiledEnergy != null)
            {
                firstUnfiledEnergy.StartFilling();
            }
        }

        EnergyCountLabel.Text = newEnergy.ToString();
    }

    private void Init()
    {
        EnergyBox.RemoveChildren();
        Energies.Clear();

        for (int i = 0; i < playerController.MaxEnergy; i++)
        {
            AddEnergy(i, false);
        }

        if(playerController.BattlefieldEnergy < playerController.MaxEnergy)
        {
            var firstUnfiledEnergy = Energies.Where(e => !e.filled).FirstOrDefault();
            if(firstUnfiledEnergy != null)
            {
                firstUnfiledEnergy.StartFilling();
            }

        }

        EnergyCountLabel.Text = playerController.BattlefieldEnergy.ToString();
    }

    private void AddEnergy(int currentEnergy, bool newEnergy = true)
    {
        var energy = Scenes.Widgets.EnergyItems.Energy();
        EnergyBox.AddChild(energy);

        if(newEnergy)
        {
            energy.Init();
        }
        else
        {
            if (currentEnergy < playerController.BattlefieldEnergy)
                energy.Init(true);
            else
                energy.Init();
        }
       
        Energies.Add(energy);
    }
}
