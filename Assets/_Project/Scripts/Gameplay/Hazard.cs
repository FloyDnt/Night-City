using UnityEngine;

namespace NightCity.Gameplay
{
    /// <summary>
    /// A trigger that damages the character when it enters.
    /// </summary>
    public sealed class Hazard : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Character character))
            {
                character.ReceiveDamage();
            }
        }
    }
}
