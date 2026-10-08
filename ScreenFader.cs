using UnityEngine;
using System.Threading.Tasks;
using Unity.Cinemachine;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;
    
    [Header("UI Settings")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("Cinemachine Settings")]
    [SerializeField] private CinemachineCamera vcam;

    private CinemachinePositionComposer transposer;
    private Vector3 originalDamping;

    private async void Start()
{
    canvasGroup.alpha = 1f; 
    
    await Task.Delay(500); 
    await FadeIn();
}

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (vcam != null)
        {
            transposer = vcam.GetComponent<CinemachinePositionComposer>();
            if (transposer != null)
            {
                originalDamping = transposer.Damping;
            }
        }
    }

    private async Task Fade(float targetTransparency)
    {
        if (canvasGroup == null) return;

        float start = canvasGroup.alpha;
        float elapsed = 0;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, targetTransparency, elapsed / fadeDuration);
            await Task.Yield();
        }

        canvasGroup.alpha = targetTransparency;
    }

    public async Task FadeOut()
    {
        await Fade(1);
        SetDamping(Vector3.zero);
    }

    public async Task FadeIn()
    {
        SetDamping(originalDamping);
        await Fade(0);
    }

    private void SetDamping(Vector3 d)
    {
        if (transposer != null)
        {
            transposer.Damping = d;
        }
    }
}