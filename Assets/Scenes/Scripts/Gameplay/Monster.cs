using UnityEngine;

namespace NightCity.Gameplay
{
    /// <summary>
    /// An enemy that dies from a single hit and hurts the character on contact.
    /// </summary>
    public class Monster : Unit
    {
        /// <inheritdoc />
        public override Team Team => Team.Enemy;

        /// <inheritdoc />
        public override void ReceiveDamage()
        {
            Destroy(gameObject);
        }
    }
}
