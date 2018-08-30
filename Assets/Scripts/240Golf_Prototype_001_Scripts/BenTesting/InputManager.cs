using System;
using System.Collections;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    PlayerMovement playerMove;
    WeaponHandler weaponHandler;

    [System.Serializable]
    public class InputSettings
    {
        public string verticalAxis = "Vertical";
        public string horizontalAxis = "Horizontal";
        public string jumpButton = "Jump";
        public string aimButton = "Aiming";
        public string fireButton = "Fire1";
        public string switchWeaponButton = "SwitchWeapon";
    }
    [SerializeField]
    InputSettings input;

    [System.Serializable]
    public class OtherSettings
    {
        public float lookSpeed = 5.0f;
        public float lookDistance = 10.0f;
        public bool requireInputForTurn = true;
        public LayerMask aimDetectionLayers;
    }
    [SerializeField]
    public OtherSettings other;

    public bool debugAim;
    public Transform spine;
    bool aiming;

    Camera cam;

    void Start()
    {
        playerMove = GetComponent<PlayerMovement>();
        cam = GameObject.FindWithTag("PlayerCamera").GetComponent<Camera>();
        weaponHandler = GetComponent<WeaponHandler>();
    }

    void Update()
    {
        CharacterLogic();
        CameraLookLogic();
        WeaponLogic();
    }

    void LateUpdate()
    {
        if (weaponHandler)
        {
            if (weaponHandler.currentWeapon)
            {
                if (aiming)
                {
                    PositionSpine();
                }
            }
        }
    }

    // Handles character move logic
    void CharacterLogic()
    {

        if (!playerMove)
        {
            return;
        }

        playerMove.Animate(Input.GetAxis(input.verticalAxis), Input.GetAxis(input.horizontalAxis));

        if (Input.GetButtonDown(input.jumpButton))
        {
            playerMove.Jump();
        }

    }

    // Handles camera logic
    void CameraLookLogic()
    {
        if (!cam)
        {
            return;
        }

        if (other.requireInputForTurn)
        {
            if (Input.GetAxis(input.horizontalAxis) != 0 || Input.GetAxis(input.verticalAxis) != 0)
            {
                CharacterLook();
            }
        }
        else
        {
            CharacterLook();
        }

    }

    // Handles weapon logic 
    void WeaponLogic()
    {
        if (!weaponHandler)
        {
            return;
        }

        if (Input.GetAxis("Aiming") <= -0.25f || debugAim) {
            aiming = true;
        } else {
            aiming = false;
        }

        print("Input: " + Input.GetAxis("Aiming"));
        print("Aiming: " + aiming);

        if (weaponHandler.currentWeapon)
        {
            weaponHandler.Aim(aiming);
            other.requireInputForTurn = !aiming;
        }

        weaponHandler.FingerOnTrigger(Input.GetButton(input.fireButton));

        /**
        if (Input.GetButtonDown(input.dropWeaponButton)) {
            weaponHandler.DropCurrentWeapon();
        } */

        if (Input.GetButtonDown(input.switchWeaponButton))
        {
            weaponHandler.SwitchWeapons();
        }
    }

    // Make character look away from camera
    void CharacterLook()
    {
        Transform camT = cam.transform;
        Vector3 camPos = camT.position;
        Vector3 lookTarget = camPos + (camT.forward * other.lookDistance);
        Vector3 thisPos = transform.position;
        Vector3 lookDir = lookTarget - thisPos;
        Quaternion lookRot = Quaternion.LookRotation(lookDir);
        lookRot.x = 0;
        lookRot.z = 0;

        Quaternion newRotation = Quaternion.Lerp(transform.rotation, lookRot, Time.deltaTime * other.lookSpeed);
        transform.rotation = newRotation;
    }

    // Positions Spine when aiming
    void PositionSpine()
    {
        if (!spine || !weaponHandler.currentWeapon || !cam)
        {
            return;
        }

        Transform camT = cam.transform;
        Vector3 camPos = camT.position;
        Vector3 dir = camT.forward;
        Ray ray = new Ray(camPos, dir);
        spine.LookAt(ray.GetPoint(50));

        Vector3 eulerAngleOffset = weaponHandler.currentWeapon.userSettings.spineRotation;
        spine.Rotate(eulerAngleOffset);
    }

}
