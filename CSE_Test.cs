using UnityEngine;

public class CSE_Test : CutsceneBase
{
    public override void Execute()
    {
        StartCoroutine(WaitAndAdvance());
        Debug.Log("Executing" + name);
    }
}
