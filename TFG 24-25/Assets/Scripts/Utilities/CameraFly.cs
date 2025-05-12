using System.Linq;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Camera))]
public class CameraFly : MonoBehaviour
{
    public bool isDisabled = true;

    public float acceleration = 50;
    public float accSprintMultiplier = 4;
    public float lookSensitivity = 1;
    public float dampingCoefficient = 5;
    public bool focusOnEnable = true;

    private Vector3 velocity;

    private PhotonView photonView;
    static bool Focused
    {
        get => Cursor.lockState == CursorLockMode.Locked;
        set
        {
            Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = value == false;
        }
    }

    void OnEnable()
    {
        if (focusOnEnable) Focused = true;
    }

    void OnDisable() => Focused = false;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!photonView.IsMine) { return; }

        // Es mi cámara, puedo moverla
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            isDisabled = !isDisabled;
            Focused = !isDisabled;
        }

        if (isDisabled)
            return;

        if (Focused)
            UpdateInput();
        else if (Input.GetMouseButtonDown(0))
            Focused = true;

        velocity = Vector3.Lerp(velocity, Vector3.zero, dampingCoefficient * Time.deltaTime);
        transform.position += velocity * Time.deltaTime;
    }

    void UpdateInput()
    {
        velocity += GetAccelerationVector() * Time.deltaTime;

        Vector2 mouseDelta = lookSensitivity * new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
        Quaternion rotation = transform.rotation;
        Quaternion horiz = Quaternion.AngleAxis(mouseDelta.x, Vector3.up);
        Quaternion vert = Quaternion.AngleAxis(mouseDelta.y, Vector3.right);
        transform.rotation = horiz * rotation * vert;

        if (Input.GetMouseButtonDown(1))
            Focused = false;
    }

    Vector3 GetAccelerationVector()
    {
        Vector3 moveInput = default;

        void AddMovement(KeyCode key, Vector3 dir)
        {
            if (Input.GetKey(key))
                moveInput += dir;
        }

        AddMovement(KeyCode.W, Vector3.forward);
        AddMovement(KeyCode.S, Vector3.back);
        AddMovement(KeyCode.D, Vector3.right);
        AddMovement(KeyCode.A, Vector3.left);
        AddMovement(KeyCode.E, Vector3.up);
        AddMovement(KeyCode.Q, Vector3.down);

        Vector3 direction = transform.TransformVector(moveInput.normalized);

        if (Input.GetKey(KeyCode.LeftShift))
            return direction * (acceleration * accSprintMultiplier);
        return direction * acceleration;
    }
}