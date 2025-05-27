using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LecternOnboarding : MonoBehaviour
{
    private int pagesNum = 0;
    private int maxPages;

    private GameObject book;
    private GameObject onboardingImage;

    [SerializeField] private List<Material> pagesMaterials;
    [SerializeField] private List<Sprite> pagesSprites;

    private void Start()
    {
        onboardingImage = transform.GetChild(0).GetChild(0).gameObject;
        book = transform.GetChild(1).gameObject;

        maxPages = pagesMaterials.Count;
    }

    public void ChangeToNewPage(int index)
    {
        book.GetComponent<Renderer>().material = pagesMaterials[index];
    }

    public void DisplayPage(bool isLeft)
    {
        if (!onboardingImage.activeSelf)
        {
            onboardingImage.SetActive(!onboardingImage.activeSelf);
        }
        onboardingImage.GetComponent<Image>().sprite = pagesSprites[isLeft ? pagesNum * 2 : pagesNum * 2 + 1];
    }

    public void TurnPage(bool isLeft)
    {
        if (isLeft)
        { pagesNum--; }
        else
        { pagesNum++; }

        if (pagesNum < 0) { pagesNum = 0; return; }
        else if (pagesNum > maxPages-1) { pagesNum = maxPages-1; return; }

        ChangeToNewPage(pagesNum);
    }
    public void CloseOnboardingImage()
    {
        onboardingImage.SetActive(false);
    }
}
