using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastLectern : RayCastInteractable
{
    [SerializeField] bool isLeft;
    [SerializeField] LecternOnboarding lectern;

    public override void Interact()
    {
        lectern.TurnPage(isLeft);
    }

    public override void Inspect()
    {
        lectern.DisplayPage();
    }
}
