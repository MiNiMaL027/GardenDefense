using Godot;
using Pawns;

namespace Components
{
    public partial class StatsComponent: Node
    {
        [Signal]
        public delegate void HealthBelowZeroEventHandler();
        [Signal]
        public delegate void HealthUpdatedEventHandler(int currentHealth, int maxHealth);

        int currentHealth;
        int modifierMaxHealth;
        int maxHealth;
        int baseMaxHealth;


        int strength;
        int modifierStrength;
        int baseStrength;

        #region Health
        public int GetCurrentHealth()
        {
            return currentHealth;
        }
        public int GetMaxHealth()
        {
            return maxHealth;
        }
        public int GetModifierMaxHealth()
        {
            return modifierMaxHealth;
        }
        public int GetBaseMaxHealth()
        {
            return baseMaxHealth;
        }
        public void AddCurrentHealth(int amount)
        {
            SetCurrentHealth(GetCurrentHealth() + amount);
        }
        public void SetMaxHealth(int maxHealthToSet)
        {
            baseMaxHealth = maxHealthToSet;
            maxHealth = baseMaxHealth + modifierMaxHealth;
            EmitSignal(SignalName.HealthUpdated, currentHealth, maxHealth);
        }
        public void SetCurrentHealth(int healthToSet)
        {
            currentHealth = healthToSet;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
                EmitSignal(SignalName.HealthUpdated, currentHealth, maxHealth);
                return;
            }
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                EmitSignal(SignalName.HealthUpdated, currentHealth, maxHealth);
                EmitSignal(SignalName.HealthBelowZero);
            }
            else
            {
                EmitSignal(SignalName.HealthUpdated, currentHealth, maxHealth);
            }
        }
        #endregion

        #region Strength
        public int GetStrength()
        {
            return strength;
        }
        public int GetModifierStrength()
        {
            return modifierStrength;
        }
        public int GetBaseStrength()
        {
            return baseStrength;
        }
        public void SetStrength(int strengthToSet)
        {
            baseStrength = strengthToSet;
            strength = baseStrength + modifierStrength;
        }
        #endregion
    }
}
