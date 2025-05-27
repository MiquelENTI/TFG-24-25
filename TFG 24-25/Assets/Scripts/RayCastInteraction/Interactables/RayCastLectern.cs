using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastLectern : RaycastInteractable
{
    [SerializeField] private bool isLeft;
    private LecternOnboarding lectern;

    private void Start()
    {
        lectern = transform.parent.GetComponent<LecternOnboarding>();
    }

    public override void Interact()
    {
        lectern.TurnPage(isLeft);
    }

    public override void Inspect()
    {
        lectern.DisplayPage(isLeft);
    }

    public override void ResetStatus()
    {
        lectern.CloseOnboardingImage();
    }
}
