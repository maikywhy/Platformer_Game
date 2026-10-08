using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CutSceneinitiator : MonoBehaviour
{
    private CutsceneHandler cutsceneHandler;

    public void Start()
    {
        cutsceneHandler = GetComponent<CutsceneHandler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
            cutsceneHandler.PlayNextElement();
    }
}
