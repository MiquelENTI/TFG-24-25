using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UpdatePositionPhoton : MonoBehaviour, IPunObservable
{
    private Vector3 networkedPosition;
    private Quaternion networkedRotation;

    private PhotonView photonView;

    private float speed = 20.0f;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView != null && !photonView.ObservedComponents.Contains(this))
        {
            photonView.ObservedComponents.Add(this);
            photonView.Synchronization = ViewSynchronization.UnreliableOnChange;
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            // Interpolar hacia la posición recibida por red
            transform.position = Vector3.Lerp(transform.position, networkedPosition, Time.deltaTime * speed);
            transform.rotation = Quaternion.Slerp(transform.rotation, networkedRotation, Time.deltaTime * speed);
            return;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Soy el dueño, mando mi posición y rotación
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // Recibo la posición y rotación
            networkedPosition = (Vector3)stream.ReceiveNext();
            networkedRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
