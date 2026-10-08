using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitBtn : MonoBehaviour
{
    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();

    }
}
