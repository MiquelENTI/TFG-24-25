using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LecternOnboarding : MonoBehaviour
{
    int pagesNum = 0;
    int maxPages;

    GameObject onboardingImage;
    [SerializeField] Dictionary<int,Sprite> onboardingPages;

    private void Start()
    {
        onboardingImage = transform.GetChild(0).gameObject;
    }

    public void ChangeToNewPage(int index)
    {
        if (!onboardingPages.ContainsKey(index)) { Debug.LogError("KEY NOT FOUND"); return; }

        onboardingImage.GetComponent<Image>().sprite = onboardingPages[index];
    }

    public void DisplayPage()
    {
        onboardingImage.SetActive(!onboardingImage.activeSelf);
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
