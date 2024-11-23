using Godot;
using System.Collections.Generic;

public partial class StatsComponent : Node
{
    [Signal]
    public delegate void HealthBelowZeroEventHandler();
    [Signal]
    public delegate void HealthUpdatedEventHandler(int currentHealth, int maxHealth);
    [Signal]
    public delegate void StrengthUpdatedEventHandler(int newStrength);
    [Signal]
    public delegate void CustomStatUpdatedEventHandler(string statName, int statValue);
    [Signal]
    public delegate void RangeUpdateEventHandler(int newAttackRange);

    public Dictionary<string, int> Stats { get; set; } = new Dictionary<string, int>();

    private void EnsureStatExists(string statKey, int defaultValue = 0)
    {
        if (!Stats.ContainsKey(statKey))
        {
            Stats[statKey] = defaultValue;
        }
    }

    public int GetCustomStat(string statName)
    {
        EnsureStatExists(statName);
        return Stats[statName];
    }

    public void SetCustomStat(string statName, int statValue)
    {
        switch (statName)
        {
            case "modifierMaxHealth":
                SetModifierMaxHealth(statValue);
                return;
            case "modifierStrength":
                SetModifierStrength(statValue);
                return;
            case "modifierAttackRange":
                SetModifierAttackRange(statValue);
                return;
        }

        EnsureStatExists(statName);
    
        Stats[statName] = statValue;      
        EmitSignal(SignalName.CustomStatUpdated, statName, statValue);
    }

    #region Health
    public int GetCurrentHealth()
    {
        EnsureStatExists("currentHealth");
        return Stats["currentHealth"];
    }

    public int GetMaxHealth()
    {
        EnsureStatExists("maxHealth");
        return Stats["maxHealth"];
    }

    public int GetModifierMaxHealth()
    {
        EnsureStatExists("modifierMaxHealth");
        return Stats["modifierMaxHealth"];
    }

    public int GetBaseMaxHealth()
    {
        EnsureStatExists("baseMaxHealth");
        return Stats["baseMaxHealth"];
    }

    public void AddCurrentHealth(int amount)
    {
        SetCurrentHealth(GetCurrentHealth() + amount);
    }

    public void SetMaxHealth(int maxHealthToSet)
    {
        EnsureStatExists("baseMaxHealth");
        EnsureStatExists("modifierMaxHealth");
        EnsureStatExists("currentHealth");

        Stats["baseMaxHealth"] = maxHealthToSet;
        Stats["maxHealth"] = Stats["baseMaxHealth"] + Stats["modifierMaxHealth"];
        EmitSignal(SignalName.HealthUpdated, Stats["currentHealth"], Stats["maxHealth"]);
    }

    public void SetModifierMaxHealth(int maxHealthToSet)
    {
        EnsureStatExists("modifierMaxHealth");
        EnsureStatExists("baseMaxHealth");
        EnsureStatExists("currentHealth");
        var oldModifierMAxHealth = Stats["modifierMaxHealth"];      

        Stats["modifierMaxHealth"] = maxHealthToSet;
        Stats["maxHealth"] = Stats["baseMaxHealth"] + Stats["modifierMaxHealth"];

        SetCurrentHealth(Stats["currentHealth"] + maxHealthToSet - oldModifierMAxHealth);

        EmitSignal(SignalName.HealthUpdated, Stats["currentHealth"], Stats["maxHealth"]);
    }

    public void SetCurrentHealth(int healthToSet)
    {
        EnsureStatExists("currentHealth");
        EnsureStatExists("maxHealth");

        Stats["currentHealth"] = healthToSet;
        if (Stats["currentHealth"] > Stats["maxHealth"])
        {
            Stats["currentHealth"] = Stats["maxHealth"];
        }
        else if (Stats["currentHealth"] <= 0)
        {
            Stats["currentHealth"] = 0;
            EmitSignal(SignalName.HealthBelowZero);
        }

        EmitSignal(SignalName.HealthUpdated, Stats["currentHealth"], Stats["maxHealth"]);
    }
    #endregion

    #region Strength
    public int GetStrength()
    {
        EnsureStatExists("strength");
        return Stats["strength"];
    }

    public int GetModifierStrength()
    {
        EnsureStatExists("modifierStrength");
        return Stats["modifierStrength"];
    }

    public int GetBaseStrength()
    {
        EnsureStatExists("baseStrength");
        return Stats["baseStrength"];
    }

    public void SetStrength(int strengthToSet)
    {
        EnsureStatExists("baseStrength");
        EnsureStatExists("modifierStrength");

        Stats["baseStrength"] = strengthToSet;
        Stats["strength"] = Stats["baseStrength"] + Stats["modifierStrength"];
        EmitSignal(SignalName.StrengthUpdated, Stats["strength"]);
    }

    public void SetModifierStrength(int strengthToSet)
    {
        EnsureStatExists("modifierStrength");
        EnsureStatExists("baseStrength");

        Stats["modifierStrength"] = strengthToSet;
        Stats["strength"] = Stats["baseStrength"] + Stats["modifierStrength"];
        EmitSignal(SignalName.StrengthUpdated, Stats["strength"]);
    }
    #endregion

    #region AttackRange

    public int GetAttackRange()
    {
        EnsureStatExists("attackRange");
        return Stats["attackRange"];
    }

    public int GetModifierAttackRange()
    {
        EnsureStatExists("modifierAttackRange");
        return Stats["modifierAttackRange"];
    }

    public int GetBaseAttackRange()
    {
        EnsureStatExists("baseAttackRange");
        return Stats["baseAttackRange"];
    }

    public void SetAttackRange(int strengthToSet)
    {
        EnsureStatExists("baseAttackRange");
        EnsureStatExists("modifierAttackRange");

        Stats["baseAttackRange"] = strengthToSet;
        Stats["attackRange"] = Stats["baseAttackRange"] + Stats["modifierAttackRange"];
        EmitSignal(SignalName.RangeUpdate, Stats["attackRange"]);
    }

    public void SetModifierAttackRange(int strengthToSet)
    {
        EnsureStatExists("modifierAttackRange");
        EnsureStatExists("baseAttackRange");

        Stats["modifierAttackRange"] = strengthToSet;
        Stats["attackRange"] = Stats["baseAttackRange"] + Stats["modifierAttackRange"];
        EmitSignal(SignalName.RangeUpdate, Stats["attackRange"]);
    }

    #endregion
}
