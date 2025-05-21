using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Sprite normalSprite;
    public Sprite hoverSprite;

    [SerializeField] public Image buttonImage;

    public void OnHover()
    {
        Debug.Log("hacemos hover");
        buttonImage.sprite = hoverSprite;
    }
    public void OnExit()
    {
        buttonImage.sprite = normalSprite;
    }
    

}