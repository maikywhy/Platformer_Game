using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class CSE_CameraPan: CutsceneBase
{
    private CinemachineCamera vcam;
    [SerializeField] private Vector2 distanceToMove;
    private Transform originalFollow;

    public override void Execute()
    {
        vcam = cutsceneHandler.vcam;
        originalFollow = vcam.Follow;
        vcam.Follow = null;
        StartCoroutine(PanCoroutine());
    }

    private IEnumerator PanCoroutine()
    {
        Vector3 originalPosition =vcam.transform.position;
        Vector3 targetPosition = originalPosition + new Vector3(distanceToMove.x, distanceToMove.y, 0);

        float startTime = Time.time;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            vcam.transform.position = Vector3.Lerp(originalPosition, targetPosition, t);

            elapsedTime = Time.time - startTime;
            yield return null;
        }

        vcam.transform.position = targetPosition;

        vcam.Follow = originalFollow;

        cutsceneHandler.PlayNextElement();
    }

    private void ObDestroy()
    {
        StopAllCoroutines();
    }
}
