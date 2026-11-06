using NightCity.Gameplay;
using UnityEngine;

namespace NightCity.UI
{
    /// <summary>
    /// Shows the character's remaining lives as a row of hearts.
    /// </summary>
    public sealed class LivesBar : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The character whose lives are shown.")]
        private Character _character;

        [SerializeField]
        [Tooltip("One heart per life, in order. Heart i is shown while the character has more than i lives.")]
        private GameObject[] _hearts;

        private void Awake()
        {
            if (_character == null || _hearts == null || _hearts.Length == 0)
            {
                Debug.LogError($"{nameof(LivesBar)} on '{name}' needs a character and its hearts.", this);
                enabled = false;
            }
        }

        private void OnEnable()
        {
            _character.LivesChanged += Draw;
        }

        private void OnDisable()
        {
            _character.LivesChanged -= Draw;
        }

        private void Start()
        {
            Draw(_character.Lives);
        }

        private void Draw(int lives)
        {
            for (int i = 0; i < _hearts.Length; i++)
            {
                _hearts[i].SetActive(i < lives);
            }
        }
    }
}
