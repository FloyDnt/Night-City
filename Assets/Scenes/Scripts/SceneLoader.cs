using UnityEngine.SceneManagement;

namespace NightCity
{
    /// <summary>
    /// The scenes of the game. Each name equals the scene's file name.
    /// </summary>
    public enum GameScene
    {
        /// <summary>The main menu.</summary>
        Menu,

        /// <summary>The playable level.</summary>
        Level,

        /// <summary>The screen shown after the character loses its last life.</summary>
        GameOver,

        /// <summary>The screen shown after the character reaches the finish line.</summary>
        FinishGame,
    }

    /// <summary>
    /// Loads game scenes by <see cref="GameScene"/> instead of by string.
    /// </summary>
    public static class SceneLoader
    {
        /// <summary>
        /// Replaces the open scenes with <paramref name="scene"/>.
        /// </summary>
        /// <param name="scene">The scene to load.</param>
        public static void Load(GameScene scene)
        {
            SceneManager.LoadScene(scene.ToString());
        }
    }
}
