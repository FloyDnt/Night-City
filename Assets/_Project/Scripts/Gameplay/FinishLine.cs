using UnityEngine;

namespace NightCity.Gameplay
{
    /// <summary>
    /// A trigger that ends the level when the character reaches it.
    /// </summary>
    public sealed class FinishLine : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Character _))
            {
                SceneLoader.Load(GameScene.FinishGame);
            }
        }
    }
}
