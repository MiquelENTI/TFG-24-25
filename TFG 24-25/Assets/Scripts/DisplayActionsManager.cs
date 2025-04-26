using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class DisplayActionsManager : Singleton<DisplayActionsManager>
{
    [SerializeField] private GameObject textPrefab;
    PhotonView photonView;

    Vector3 positionOffset;

    Color damageColor;
    Color healColor;
    Color pointsColor;
    Color nameColor;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        positionOffset = new Vector3(0,0.1f,0);

        damageColor = new Color(245, 0, 76);
        pointsColor = new Color(255, 215, 0);
        healColor = new Color(50, 245, 99);
        nameColor = Color.white;
    }

    public void CreateDamageText(int text, Vector3 pos)
    {
        string text2 = text.ToString();
        if (text > 20) { text2 = "DELETED"; }

        GameObject damageText = PhotonNetwork.Instantiate(textPrefab.name, pos + positionOffset, Quaternion.identity);
        damageText.transform.parent = transform;
        damageText.GetComponent<TMP_Text>().text = text2;
        damageText.GetComponent<TMP_Text>().color = damageColor;
    }

    public void CreateHealText(int text, Vector3 pos) 
    {
        GameObject healText = PhotonNetwork.Instantiate(textPrefab.name, pos + positionOffset, Quaternion.identity);
        healText.transform.parent = transform;
        healText.GetComponent<TMP_Text>().text = text.ToString();
        healText.GetComponent<TMP_Text>().color = healColor;
    }
    public void CreateBuffText(int text, bool isDamage, Vector3 pos) 
    {
        GameObject pointsText = PhotonNetwork.Instantiate(textPrefab.name, pos + positionOffset, Quaternion.identity);
        pointsText.transform.parent = transform;
        pointsText.GetComponent<TMP_Text>().text = "+" + text.ToString();

        if (isDamage)
        {
            pointsText.GetComponent<TMP_Text>().color = damageColor;
        }
        else
        {
            pointsText.GetComponent<TMP_Text>().color = healColor;
        }
    }

    public void CreateDebuffText(int text, bool isDamage, Vector3 pos)
    {
        GameObject pointsText = PhotonNetwork.Instantiate(textPrefab.name, pos + positionOffset, Quaternion.identity);
        pointsText.transform.parent = transform;
        pointsText.GetComponent<TMP_Text>().text = "-" + text.ToString();

        if (isDamage)
        {
            pointsText.GetComponent<TMP_Text>().color = damageColor;
        }
        else
        {
            pointsText.GetComponent<TMP_Text>().color = healColor;
        }
    }
    public void CreatePointsText(int text, Vector3 pos)
    {
        GameObject pointsText = PhotonNetwork.Instantiate(textPrefab.name, pos + positionOffset, Quaternion.identity);
        pointsText.transform.parent = transform;
        pointsText.GetComponent<TMP_Text>().text = text.ToString();
        pointsText.GetComponent<TMP_Text>().color = pointsColor;
    }
    public void CreateNameText(string name, Vector3 pos)
    {
        GameObject pointsText = PhotonNetwork.Instantiate(textPrefab.name, pos + positionOffset, Quaternion.identity);
        pointsText.transform.parent = transform;
        pointsText.GetComponent<TMP_Text>().text = name;
        pointsText.GetComponent<TMP_Text>().color = nameColor;
        pointsText.GetComponent<TMP_Text>().fontSize = 4;
    }

    public void CreateCustomText(string text, Vector3 color, int fontSize, float duration, Vector3 pos, float offset = 0)
    {
        GameObject pointsText = PhotonNetwork.Instantiate(textPrefab.name, pos + positionOffset, Quaternion.identity);
        pointsText.transform.parent = transform;
        pointsText.GetComponent<TMP_Text>().text = text;
        pointsText.GetComponent<TMP_Text>().color = VectorToColor(color);
        pointsText.GetComponent<TMP_Text>().fontSize = fontSize;
        pointsText.GetComponent<ActionAnimation>().SetOffsetAndDuration(offset, duration);
    }

    private Color VectorToColor(Vector3 color)
    {
        return new Color(color.x, color.y, color.z);
    }
}
