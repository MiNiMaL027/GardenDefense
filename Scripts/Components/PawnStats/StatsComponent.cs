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
    [Signal]
    public delegate void HealthRegenUpdateEventHandler(int newHealthRegen);
    [Signal]
    public delegate void HealthRegenRateUpdateEventHandler(int newHealthRegenRate);
    [Signal]
    public delegate void ArmorUpdatedEventHandler(int currentArmor, int maxArmor);
    [Signal]
    public delegate void ArmorRegenRateUpdatedEventHandler(int newArmorRegenRate);
    [Signal]
    public delegate void ArmorRegenDelayUpdatedEventHandler(int newArmorRegenDelay);

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
            case "modifierHealthRegen":
                SetModifierHealthRegen(statValue);
                return;
            case "healthRegenRate":
                SetHealthRegenRate(statValue);
                return;
            case "modifierMaxArmor":
                SetModifierMaxArmor(statValue);
                return;
            case "armorRegenRate":
                SetArmorRegenRate(statValue);
                return;
            case "armorRegenDelay":
                SetArmorRegenDelay(statValue);
                return;
        }

        EnsureStatExists(statName);
    
        Stats[statName] = statValue;      
        EmitSignal(SignalName.CustomStatUpdated, statName, statValue);
    }

    #region Health
    public int GetHealthRegenRate()
    {
        EnsureStatExists("healthRegenRate");
        return Stats["healthRegenRate"];
    }
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
    public int GetHealthRegen()
    {
        return GetBaseHealthRegen() + GetModifierHealthRegen();
    }

    public int GetBaseHealthRegen()
    {
        EnsureStatExists("baseHealthRegen");
        return Stats["baseHealthRegen"];
    }

    public int GetModifierHealthRegen()
    {
        EnsureStatExists("modifierHealthRegen");
        return Stats["modifierHealthRegen"];
    }
    public void SetHealthRegenRate(int rate) // the number of regen activations per 5 second
    {
        EnsureStatExists("healthRegenRate");
        Stats["healthRegenRate"] = rate;

        EmitSignal(SignalName.HealthRegenRateUpdate, rate);
    }
    public void SetModifierHealthRegen(int amount)
    {
        EnsureStatExists("modifierHealthRegen");

        Stats["modifierHealthRegen"] = amount;
        EmitSignal(SignalName.HealthRegenUpdate, GetHealthRegen());
    }

    public void SetBaseHealthRegen(int amount)
    {
        EnsureStatExists("baseHealthRegen");

        Stats["baseHealthRegen"] = amount;
        EmitSignal(SignalName.HealthRegenUpdate, GetHealthRegen());
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
    #region Armor
    public int GetArmorRegenRate()
    {
        return Stats["armorRegenRate"];
    }
    public int GetArmorRegenDelay()
    {
        return Stats["armorRegenDelay"];
    }
    public int GetMaxArmor()
    {
        return GetBaseMaxArmor() + GetModifierMaxArmor();
    }
    public int GetBaseMaxArmor()
    {
        EnsureStatExists("baseMaxArmor");
        return Stats["baseMaxArmor"];
    }
    public int GetModifierMaxArmor()
    {
        EnsureStatExists("modifierMaxArmor");
        return Stats["modifierMaxArmor"];
    }
    public int GetCurrentArmor()
    {
        EnsureStatExists("currentArmor");
        return Stats["currentArmor"];
    }

    public void AddCurrentArmor(int amount)
    {
        SetCurrentArmor(GetCurrentArmor() + amount);
    }
    public void SetArmorRegenRate(int amount)
    {
        EnsureStatExists("armorRegenRate");

        Stats["armorRegenRate"] = amount;
        EmitSignal(SignalName.ArmorRegenRateUpdated, amount);
    }
    public void SetArmorRegenDelay(int amount)
    {
        EnsureStatExists("armorRegenDelay");

        Stats["armorRegenDelay"] = amount;
        EmitSignal(SignalName.ArmorRegenDelayUpdated, amount);
    }
    public void SetBaseMaxArmor(int maxArmorToSet)
    {
        EnsureStatExists("baseMaxArmor");
        EnsureStatExists("currentArmor");

        Stats["baseMaxArmor"] = maxArmorToSet;

        EmitSignal(SignalName.ArmorUpdated, Stats["currentArmor"], GetMaxArmor());
    }

    public void SetModifierMaxArmor(int maxArmorToSet)
    {
        EnsureStatExists("modifierMaxArmor");
        EnsureStatExists("baseMaxArmor");
        EnsureStatExists("currentArmor");
        var oldModifierMaxArmor = Stats["modifierMaxArmor"];

        Stats["modifierMaxArmor"] = maxArmorToSet;

        SetCurrentArmor(Stats["currentArmor"] + maxArmorToSet - oldModifierMaxArmor);

        EmitSignal(SignalName.ArmorUpdated, Stats["currentArmor"], GetMaxArmor());
    }

    public void SetCurrentArmor(int armorToSet)
    {
        EnsureStatExists("currentArmor");
        var maxArmor = GetMaxArmor();

        Stats["currentArmor"] = armorToSet;
        if (Stats["currentArmor"] > maxArmor)
        {
            Stats["currentArmor"] = maxArmor;
        }
        else if (Stats["currentArmor"] <= 0)
        {
            Stats["currentArmor"] = 0;

        }

        EmitSignal(SignalName.ArmorUpdated, Stats["currentArmor"], maxArmor);
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
