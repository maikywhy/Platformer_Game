using System;
using UnityEngine;
using Unity.Cinemachine;

public class GameController : MonoBehaviour
{
    public GameObject player;
    public GameObject gameOverScreen;
    
    [Header("Cinemachine")]
    public CinemachineCamera vcam; 
    public PolygonCollider2D startingMapBoundary;

    public static event Action OnReset;

    private Vector3 startPosition;
    private Rigidbody2D rb;
    private CinemachineConfiner2D confiner;

    void Start()
    {
        PlayerHealth.OnPlayerDied += GameOverScreen;
        gameOverScreen.SetActive(false);

        if (player != null)
        {
            startPosition = player.transform.position;
            rb = player.GetComponent<Rigidbody2D>();
        }

        if (vcam != null)
        {
            confiner = vcam.GetComponent<CinemachineConfiner2D>();
        }
    }

    void GameOverScreen()
    {
        gameOverScreen.SetActive(true);
        MusicManager.PauseMusic();
        Time.timeScale = 0;
    }

    public void ResetGame()
    {
        gameOverScreen.SetActive(false);
        MusicManager.PlayBackgroundMusic(true);

        if (player != null && vcam != null)
        {
            Vector3 delta = startPosition - player.transform.position;

            player.transform.position = startPosition;

            vcam.OnTargetObjectWarped(player.transform, delta);

            if (confiner != null && startingMapBoundary != null)
            {
                confiner.BoundingShape2D = startingMapBoundary;
            }

            if (rb != null) rb.linearVelocity = Vector2.zero;
        }

        OnReset?.Invoke();
        Time.timeScale = 1;
    }
}