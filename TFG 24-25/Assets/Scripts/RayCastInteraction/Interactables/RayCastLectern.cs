using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastLectern : RayCastInteractable
{
    [SerializeField] bool isLeft;
    LecternOnboarding lectern;

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
        lectern.DisplayPage();
    }
}
