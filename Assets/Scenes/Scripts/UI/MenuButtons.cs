using UnityEngine;

namespace NightCity.UI
{
    /// <summary>
    /// Targets for the OnClick events of the menu screens' buttons.
    /// </summary>
    public sealed class MenuButtons : MonoBehaviour
    {
        /// <summary>
        /// Starts the level.
        /// </summary>
        public void Play()
        {
            SceneLoader.Load(GameScene.Level);
        }

        /// <summary>
        /// Returns to the main menu.
        /// </summary>
        public void OpenMainMenu()
        {
            SceneLoader.Load(GameScene.Menu);
        }

        /// <summary>
        /// Quits the game, or leaves play mode in the editor.
        /// </summary>
        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}
