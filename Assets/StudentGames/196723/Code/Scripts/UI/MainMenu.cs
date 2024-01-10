using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _196723
{
    public class MainMenu : MonoBehaviour
    {
        public void OnLevel1Pressed()
        {
            SceneManager.LoadSceneAsync("196723");
        }

        public void OnExitPressed()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}
