using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
    [SerializeField] private Image image;
    public Sprite pauseSprite, pauseSpriteToggled, playSprite, playSpriteToggled;

    private void Awake() {
        image.sprite = pauseSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // if (GetPaused())
        // {
        //     image.sprite = playSpriteToggled;
        // }
        // else 
        // {
        //     image.sprite = pauseSpriteToggled;
        // }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // if (GetPaused())
        // {
        //     image.sprite = playSprite;
        // }
        // else
        // {
        //     image.sprite = pauseSprite;
        // }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void Pause()
    {
        if (image.sprite == pauseSprite) image.sprite = playSprite;

        else image.sprite = pauseSprite;

        PauseManager.TogglePause();
    }

    private bool GetPaused()
    {
        return Time.timeScale == 0;
    }
}