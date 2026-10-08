using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CutsceneBase : MonoBehaviour
{
    public float duration; 
    public CutsceneHandler cutsceneHandler {get; private set; }

    void Start()
    {
        cutsceneHandler = GetComponent<CutsceneHandler>();
    }

    public virtual void Execute()
    {

    }

    protected IEnumerator WaitAndAdvance()
    {
        yield return new WaitForSeconds(duration);
        cutsceneHandler.PlayNextElement();
    }
}
