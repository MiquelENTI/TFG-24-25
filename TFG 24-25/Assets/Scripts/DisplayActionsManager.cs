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
        photonView.RPC("CreateDamageText_RPC", RpcTarget.AllBuffered, text, pos);
    }

    [PunRPC]
    public void CreateDamageText_RPC(int text, Vector3 pos)
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
        GameObject damageText = PhotonNetwork.Instantiate(textPrefab.name, pos + positionOffset, Quaternion.identity);
        damageText.transform.parent = transform;
        damageText.GetComponent<TMP_Text>().text = text.ToString();
        damageText.GetComponent<TMP_Text>().color = healColor;
    }

    public void CreateBuffText(int text, bool isDamage, Vector3 pos) 
    {
        photonView.RPC("CreateBuffText_RPC", RpcTarget.AllBuffered, text, isDamage, pos);
    }

    [PunRPC]
    public void CreateBuffText_RPC(int text, bool isDamage, Vector3 pos)
    {
        GameObject pointsText = PhotonNetwork.Instantiate(textPrefab.name, pos + positionOffset, Quaternion.identity);
        pointsText.transform.parent = transform;
        pointsText.GetComponent<TMP_Text>().text = "+"+text.ToString();

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
        photonView.RPC("CreateDebuffText_RPC", RpcTarget.AllBuffered, text, isDamage, pos);
    }

    [PunRPC]
    public void CreateDebuffText_RPC(int text, bool isDamage, Vector3 pos)
    {
        GameObject pointsText = PhotonNetwork.Instantiate(textPrefab.name, pos + positionOffset, Quaternion.identity);
        pointsText.transform.parent = transform;
        pointsText.GetComponent<TMP_Text>().text = "-"+text.ToString();

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
        photonView.RPC("CreatePointsText_RPC", RpcTarget.AllBuffered, text, pos);
    }

    [PunRPC]
    public void CreatePointsText_RPC(int text, Vector3 pos)
    {
        GameObject pointsText = PhotonNetwork.Instantiate(textPrefab.name, pos + positionOffset, Quaternion.identity);
        pointsText.transform.parent = transform;
        pointsText.GetComponent<TMP_Text>().text = text.ToString();
        pointsText.GetComponent<TMP_Text>().color = pointsColor;
    }

    public void CreateNameText(string name, Vector3 pos)
    {
        photonView.RPC("CreateNameText_RPC", RpcTarget.AllBuffered, name, pos);
    }

    [PunRPC]
    public void CreateNameText_RPC(string name, Vector3 pos)
    {
        GameObject pointsText = PhotonNetwork.Instantiate(textPrefab.name, pos + positionOffset, Quaternion.identity);
        pointsText.transform.parent = transform;
        pointsText.GetComponent<TMP_Text>().text = name;
        pointsText.GetComponent<TMP_Text>().color = nameColor;
        pointsText.GetComponent<TMP_Text>().fontSize = 4;
    }

    public void CreateCustomText(string text, Vector3 color, int fontSize, float duration, Vector3 pos, float offset = 0)
    {
        photonView.RPC("CreateCustomText_RPC", RpcTarget.AllBuffered, text, color, fontSize, duration, pos, offset);
    }
    [PunRPC]
    public void CreateCustomText_RPC(string text, Vector3 color, int fontSize,  float duration, Vector3 pos, float offset)
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
