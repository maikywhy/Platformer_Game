using UnityEngine;
using Unity.Cinemachine;

public class CutsceneHandler: MonoBehaviour
{
    public Camera cam;
    public CinemachineCamera vcam;
    private CutsceneBase[] cutsceneBase;
    private int index = -1;

    public void Start()
    {
        cutsceneBase = GetComponents<CutsceneBase>();
    }

    private void ExecuteCurrentElement()
    {
        if( index >= 0 && index < cutsceneBase.Length)
            cutsceneBase[index].Execute();
    }

    public void PlayNextElement()
    {
        index++;
        ExecuteCurrentElement();
    }
}
