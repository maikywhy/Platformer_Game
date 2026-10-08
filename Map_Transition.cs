using UnityEngine;
using Unity.Cinemachine;
using System.Threading.Tasks;

public class MapTransition : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D mapBoundary;
    private CinemachineConfiner2D confiner;
    [SerializeField] private Direction direction;
    [SerializeField] private float additivePos = 2f;

    private enum Direction { Up, Down, Left, Right }

    private void Awake()
    {
        confiner = FindFirstObjectByType<CinemachineConfiner2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FadeTransition(collision.gameObject);
            Map_Controller_Manual.Instance?.HighlightArea(mapBoundary.name);
            MapController_Dynamic.Instance?.UpdateCurrentArea(mapBoundary.name);
        }
    }

    async void FadeTransition(GameObject player)
    {
        PauseController.SetPause(true);
    await ScreenFader.Instance.FadeOut();

    if (confiner != null)
    {
        confiner.BoundingShape2D = mapBoundary;
        confiner.InvalidateCache(); 
    }

    UpdatePlayerPosition(player);
    await Task.Yield(); 

    await ScreenFader.Instance.FadeIn();
    PauseController.SetPause(false);
    }

    private void UpdatePlayerPosition(GameObject player)
    {
        Vector3 newPos = player.transform.position;

        switch(direction)
        {
            case Direction.Up:
                newPos.y += additivePos;
                break;
            case Direction.Down:
                newPos.y -= additivePos;
                break;
            case Direction.Right:
                newPos.x += additivePos;
                break;
            case Direction.Left:
                newPos.x -= additivePos;
                break;
        }
        player.transform.position = newPos;
    }
}