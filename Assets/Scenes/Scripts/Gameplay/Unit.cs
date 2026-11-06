using UnityEngine;

namespace NightCity.Gameplay
{
    /// <summary>
    /// The side a unit fights on. Projectiles only damage units of the other team.
    /// </summary>
    public enum Team
    {
        /// <summary>The player's character.</summary>
        Player,

        /// <summary>Monsters.</summary>
        Enemy,
    }

    /// <summary>
    /// Anything that belongs to a team and can be damaged.
    /// </summary>
    public abstract class Unit : MonoBehaviour
    {
        /// <summary>
        /// The team this unit fights on.
        /// </summary>
        public abstract Team Team { get; }

        /// <summary>
        /// Applies one hit to this unit.
        /// </summary>
        public abstract void ReceiveDamage();
    }
}
