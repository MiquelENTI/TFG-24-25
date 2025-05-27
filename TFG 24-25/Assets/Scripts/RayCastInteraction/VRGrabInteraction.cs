using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRGrabInteraction : MonoBehaviour
{
    private TurnManagerScript turnManagerScript;

    private XRGrabInteractable interactable;

    private bool isBlue;

    void Start()
    {
        turnManagerScript = TurnManagerScript.Instance;
        interactable = GetComponent<XRGrabInteractable>();
        isBlue = GetComponent<DragableGameObject>().GetIsBlue();

        if (turnManagerScript.isPCVersion) 
        { 
            interactable.enabled = false;
            this.enabled = false; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isBlue != turnManagerScript.GetIsTurnBlue())
        {
            interactable.enabled = false;
        }
        else
        {
            interactable.enabled = true;
        }
    }
}
