using UnityEngine;
using TMPro;

public class CutScence_Text : CutsceneBase
{
    [SerializeField] private TMP_Text popUptext;
    [TextArea] [SerializeField] private string dialogue;
    [SerializeField] private TextPosition textPosition;
    [SerializeField] private Animator anim;
    private bool isTextActive;
    
    private bool hasTriggered = false; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            Execute();
            hasTriggered = true;
        }
    }

    public override void Execute()
    {
        SetTextPosition();
        anim.Play("FadeIn");
        isTextActive = true;
        popUptext.text = dialogue;
    }

    private void Update()
    {
        if(Input.GetButtonDown("Interact") && isTextActive)
        {
            anim.Play("FadeOut");
            isTextActive = false; 
        }
    }

    public void SetTextPosition()
    {
        RectTransform rectTransform = popUptext.rectTransform;

        switch (textPosition)
        {
            case TextPosition.Top:
                rectTransform.anchoredPosition = new Vector2(0, 360);
                break;
            case TextPosition.Middle:
                rectTransform.anchoredPosition = new Vector2(0, 0);
                break;
            case TextPosition.Bottom:
                rectTransform.anchoredPosition = new Vector2(0, -360);
                break;
        }
    }
}

public enum TextPosition
{
    Top,
    Middle,
    Bottom
}