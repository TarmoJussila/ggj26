using System;
using Logbound.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Logbound
{
    public class GameLoseController : Singleton<GameLoseController>
    {
        public static event Action<float> OnGameEnded;

        public float GameTime;

        public bool GameEnded;

        public bool StopLosing;

        private void Update()
        {
            if (GameEnded)
            {
                if (Keyboard.current.rKey.wasPressedThisFrame)
                {
                    SceneManager.LoadScene(0);
                }
                else if (Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    Application.Quit();
                }

                return;
            }

            GameTime += Time.deltaTime;
        }

        public void LoseGame()
        {
#if UNITY_EDITOR
            if (StopLosing)
            {
                return;
            }
#endif

            GameEnded = true;

            OnGameEnded?.Invoke(GameTime);


            Cursor.lockState = CursorLockMode.None;
        }
    }
}
