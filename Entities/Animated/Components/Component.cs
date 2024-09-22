using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deltadust.Entities.Animated.Components
{
    public abstract class Component
    {
        protected Component(AnimatedEntity owner) {
            Owner = owner;
        }
        public AnimatedEntity Owner { get; set; }

        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }

    // public class HealthComponent : Component
    // {
    //     public float MaxHealth { get; private set; }
    //     public float CurrentHealth { get; private set; }
    //     public bool IsDead => CurrentHealth <= 0;

    //     // public event Action OnDeath;
    //     // public event Action<float> OnHealthChanged;

    //     public HealthComponent(float maxHealth)
    //     {
    //         MaxHealth = maxHealth;
    //         CurrentHealth = maxHealth;
    //     }

    //     // public void TakeDamage(float damage)
    //     // {
    //     //     CurrentHealth -= damage;
    //     //     CurrentHealth = MathHelper.Clamp(CurrentHealth, 0, MaxHealth);

    //     //     OnHealthChanged?.Invoke(CurrentHealth);

    //     //     if (IsDead)
    //     //     {
    //     //         Die();
    //     //     }
    //     // }

    //     // public void Heal(float amount)
    //     // {
    //     //     if (!IsDead)
    //     //     {
    //     //         CurrentHealth += amount;
    //     //         CurrentHealth = MathHelper.Clamp(CurrentHealth, 0, MaxHealth);

    //     //         OnHealthChanged?.Invoke(CurrentHealth);
    //     //     }
    //     // }

    //     // private void Die()
    //     // {
    //     //     OnDeath?.Invoke();
    //     // }

    //     public override void Update(GameTime gameTime)
    //     {
    //         // TODO: Health regen
    //     }

    //     public override void Draw(SpriteBatch spriteBatch)
    //     {
    //         // TODO: Health bar
    //     }
    // }

}