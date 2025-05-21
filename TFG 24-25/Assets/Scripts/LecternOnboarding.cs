using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LecternOnboarding : MonoBehaviour
{
    int pagesNum = 0;
    int maxPages;

    GameObject book;
    GameObject onboardingImage;

    [SerializeField] List<Material> pagesMaterials;
    [SerializeField] List<Sprite> pagesSprites; 

    Dictionary<int,Material> onboardingPages;
    Dictionary<int, Sprite> onboardingPagesSprite;

    private void Start()
    {
        onboardingImage = transform.GetChild(0).GetChild(0).gameObject;
        book = transform.GetChild(1).gameObject;

        onboardingPages = new();
        onboardingPagesSprite = new();

        for (int i = 0; i < pagesMaterials.Count; i++)
        {
            onboardingPages.Add(i, pagesMaterials[i]);
        }
        for (int i = 0; i < pagesSprites.Count; i++)
        {
            onboardingPagesSprite.Add(i, pagesSprites[i]);
        }

        maxPages = onboardingPages.Count;
    }

    public void ChangeToNewPage(int index)
    {
        if (!onboardingPages.ContainsKey(index)) { Debug.LogError("KEY NOT FOUND"); return; }

        book.GetComponent<Renderer>().material = onboardingPages[index];
    }

    public void DisplayPage(bool isLeft)
    {
        if (!onboardingImage.activeSelf)
        {
            onboardingImage.SetActive(!onboardingImage.activeSelf);
        }
        onboardingImage.GetComponent<Image>().sprite = onboardingPagesSprite[isLeft ? pagesNum * 2 : pagesNum * 2 + 1];
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
