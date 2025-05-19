using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LecternOnboarding : MonoBehaviour
{
    int pagesNum = 0;
    int maxPages;

    public void ChangeToNewPage(int index)
    {

    }

    public void DisplayPage()
    {

    }

    public void TurnPage(bool isLeft)
    {
        if (pagesNum == 0 || pagesNum == maxPages) { return; }

        if (isLeft)
        { pagesNum--; }
        else
        { pagesNum++; }

        ChangeToNewPage(pagesNum);
    }
}
