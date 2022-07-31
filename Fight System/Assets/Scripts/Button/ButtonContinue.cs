using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonContinue : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private RectTransform fingerRect;
    [SerializeField] private AudioClip clickSound;
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        SoundManager.instance.PlaySound(clickSound);

        if (fingerRect != null)
            fingerRect.position = new Vector3(transform.position.x - 175f, transform.position.y, transform.position.z);
    }
}
