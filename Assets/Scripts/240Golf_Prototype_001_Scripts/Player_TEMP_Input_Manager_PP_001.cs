using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player_TEMP_Input_Manager_PP_001 : NetworkBehaviour
{
    [SerializeField] private Player_Controller_Manager_PP_001 playerControll_Script;
    [SerializeField] private ClubWeaponManager_PP_001 clubWeapon_Script;
    public Camera myCamera;
    public float speed;

    private enum StartStates { bootUp_1, bootUp_1_1, golfUp_2, golfUp_3, golfHit_4, gunUp_5, gunUp_6, gunUp_7, gunUp_8, begin_9, begin_10 };
    private StartStates startingStates;
    private Transform startTransform;

    public Canvas playerCanvas;
    private Transform[] children;
    private Image im;
    private bool Switch = false;
    private  float ControllerTrigger;

    public GameObject CameraAlivePosition;

    void Start ()
    {  
        int temp = GetComponent<Player_Manager_PP_001>().GetPlayerID();
        string temp2 = "StartPos" + temp.ToString();
        startTransform = GameObject.Find(temp2).transform;
        if (startTransform != null)
        {
            startingStates = StartStates.bootUp_1;
            playerControll_Script.PlayWalkingAnimation(true);
            myCamera.fieldOfView = 90.0f;
            myCamera.transform.parent = startTransform.transform;
            myCamera.transform.position = startTransform.GetChild(0).transform.position;
        }
        else
        {
            print("Could Not find this characters starting pos");
            Destroy(this.gameObject);
        }

        children = playerCanvas.GetComponentsInChildren<Transform>();
        for (int i = 1; i < children.Length; ++i)
        {
            children[i].gameObject.SetActive(false);
        }
    }

    public void TurnOffAllUIImages()
    {
        children = playerCanvas.GetComponentsInChildren<Transform>();
        for (int i = 1; i < children.Length; ++i)
        {
            children[i].gameObject.SetActive(false);
        }
    }
	
	void Update ()
    {
        if (!isLocalPlayer)
        {
            myCamera.enabled = false;
            return;
        }

        ControllerTrigger = Input.GetAxis("PullUpWeapon");

        if (startingStates == StartStates.bootUp_1) StartState1_WalkingUp();
        else if (startingStates == StartStates.bootUp_1_1) StartState1_1_Camera();
        else if (startingStates == StartStates.golfUp_2) StartState2_GolfMode();
        else if (startingStates == StartStates.golfUp_3) StartState3_GolfSwing();
        else if (startingStates == StartStates.golfHit_4) StartState4_GolfHitBall();
        else if (startingStates == StartStates.gunUp_5) StartState5_CharacterSelected();
        else if (startingStates == StartStates.gunUp_6) StartState6_GunMode_1();
        else if (startingStates == StartStates.gunUp_7) StartState7_GunMode_2();
        else if (startingStates == StartStates.gunUp_8) StartState8_GunMode_3();
        else if (startingStates == StartStates.begin_9) StartState9_BeginGame_1();
        else if (startingStates == StartStates.begin_10) StartState10_BeginGame_2();
    }

    private void StartState1_WalkingUp()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, startTransform.position, step);
        myCamera.transform.position = Vector3.MoveTowards(myCamera.transform.position, startTransform.GetChild(1).transform.position, 5.0f * Time.deltaTime);
        if(myCamera.fieldOfView > 55.0f)
        {
            myCamera.fieldOfView = myCamera.fieldOfView - 5.0f * Time.deltaTime;
        }
        

        if (transform.position == startTransform.position)
        {
            startingStates = StartStates.bootUp_1_1;
            myCamera.transform.parent = transform;
        }
    }

    private void StartState1_1_Camera()
    {
        myCamera.transform.parent = transform;
        myCamera.transform.position = Vector3.MoveTowards(myCamera.transform.position, CameraAlivePosition.transform.position, 2.0f * Time.deltaTime);
        if (myCamera.transform.position == CameraAlivePosition.transform.position)
        {
            startingStates = StartStates.golfUp_2;
            playerControll_Script.PlayWalkingAnimation(false);
            TurnOnACanvasChild(1, 2);
        }
    }

    private void StartState2_GolfMode()
    {
        if (ControllerTrigger > 0.0f)
        {
            startingStates = StartStates.golfUp_3;
            InvokeRepeating("GolfUpSwitch", .1f, 0.5f);
        }
    }

    private void StartState3_GolfSwing()
    {
        if (ControllerTrigger > 0.0f)
        {
            playerControll_Script.RunTranslate();
            //payerCurrentState = PlayerStatus.GolfFree;
            //myCamera.GetComponent<PlayerCameraController>().CameraGolfFree();
            myCamera.GetComponent<PlayerCameraController>().CameraGolfFree_Start();
            playerControll_Script.RunGolfFreeState();
            clubWeapon_Script.GolfMode();
            float curSwing = Input.GetAxis("CameraVertical");
            if (curSwing < 0.0f)
            {
                TurnOffACanvasChild();
                startingStates = StartStates.golfHit_4;
            }
        }
        else StartVanillaMode();
    }

    private void StartState4_GolfHitBall()
    {
        if (ControllerTrigger > 0.0f)
        {
            playerControll_Script.RunTranslate();
            myCamera.GetComponent<PlayerCameraController>().CameraGolfFree();
            playerControll_Script.RunGolfFreeState();
            clubWeapon_Script.GolfMode();
        }
        else StartVanillaMode();
    }

    private void StartState5_CharacterSelected()
    {
        myCamera.GetComponent<PlayerCameraController>().CameraGolfFree();
    }

    private void StartState6_GunMode_1()
    {
        startingStates = StartStates.gunUp_7;
        StartVanillaMode2();
        TurnOnACanvasChild2(5, 5);
    }

    private void StartState7_GunMode_2()
    {
        myCamera.GetComponent<PlayerCameraController>().CameraStartView(2.5f);
        if (ControllerTrigger < 0.0f)
        {
            startingStates = StartStates.gunUp_8;
            InvokeRepeating("GunUpSwitch", .1f, 0.5f);
        }
    }

    private void StartState8_GunMode_3()
    {
        if (ControllerTrigger < 0.0f)
        {
            playerControll_Script.RunTranslate();
            //payerCurrentState = PlayerStatus.GolfFree;
            //myCamera.GetComponent<PlayerCameraController>().CameraGolfFree();
            myCamera.GetComponent<PlayerCameraController>().CameraGolfFree_Start();
            playerControll_Script.RunGunState();
            clubWeapon_Script.WeaponMode();
        }
        else StartVanillaMode();
    }

    private void StartState9_BeginGame_1()
    {
        myCamera.GetComponent<PlayerCameraController>().CameraStartView(2.5f);
        StartVanillaMode2();
        CancelInvoke();
        for (int i = 1; i < children.Length - 1; i++)
        {
            children[i].gameObject.SetActive(false);
        }
        TurnOnACanvasChild2(7, 7);
        startingStates = StartStates.begin_10;
    }

    private void StartState10_BeginGame_2()
    {
        //TurnOnACanvasChild2(7, 7);

        //this is where the color swap goes for that specific player.  
        //NormalManny.GetComponent<Renderer>().material.color = new Color(6.0f / 255.0f, 72.0f / 255.0f, 1.0f);
        //GolfingManny.GetComponentInChildren<Renderer>().material.color = new Color(6.0f / 255.0f, 72.0f / 255.0f, 1.0f);

        myCamera.GetComponent<PlayerCameraController>().CameraStartView(2.5f);
        float one = Input.GetAxis("Horizontal");
        float two = Input.GetAxis("Vertical");
        float three = Input.GetAxis("CameraHorizontal");
        float four = Input.GetAxis("CameraVertical");

        if ((one != 0.0f) || (two != 0.0f) || (three != 0.0f) || (four != 0.0f))
        {
            this.gameObject.GetComponent<Player_Manager_PP_001>().TurnOnPlayerAfterStartUp();
        }
    }

    private void StartVanillaMode()
    {
        playerControll_Script.resetCharToVan_Start();
        clubWeapon_Script.VanillaMode();
        playerControll_Script.RunTranslate();
        myCamera.GetComponent<PlayerCameraController>().CameraStartView(5);
        playerControll_Script.RunVanillaState(false);
    }

    private void StartVanillaMode2()
    {
        playerControll_Script.resetCharToVan_Start();
        clubWeapon_Script.VanillaMode();
        playerControll_Script.RunTranslate();
        //myCamera.GetComponent<PlayerCameraController>().CameraStartView();
        playerControll_Script.RunVanillaState(false);
    }

    public void TurnOnACanvasChild(int c, int i)
    {
        im = children[c].GetComponent<Image>();
        children[c].gameObject.SetActive(true);
        im.GetComponent<CanvasRenderer>().SetAlpha(0.01f);
        im.CrossFadeAlpha(1f, 1.5f, false);
        StartCoroutine(SwitchUIImage(1.0f,i));
    }

    public void TurnOnACanvasChild2(int c, int i)
    {
        children[c].gameObject.SetActive(true);
        im = children[c].GetComponent<Image>();
        im.GetComponent<CanvasRenderer>().SetAlpha(1.0f);
    }

    public void TurnOffACanvasChild()
    {
        CancelInvoke();
        Switch = false;
        im.CrossFadeAlpha(0.0f, 1.0f, false);
    }

    IEnumerator SwitchUIImage(float time, int i)
    {
        yield return new WaitForSeconds(time);
        im.sprite = children[i].GetComponent<Image>().sprite;
    }

    IEnumerator TurnOnUIImage(float time, int i)
    {
        yield return new WaitForSeconds(time);
        im = children[i].GetComponent<Image>();
        children[i].gameObject.SetActive(true);
        im.GetComponent<CanvasRenderer>().SetAlpha(0.01f);
        im.CrossFadeAlpha(1f, 1.5f, false);
    }

    IEnumerator TurnOffUIImage(float time, int i)
    {
        yield return new WaitForSeconds(time);
        im = children[i].GetComponent<Image>();
        children[i].gameObject.SetActive(true);
        //im.GetComponent<CanvasRenderer>().SetAlpha(0.01f);
        im.CrossFadeAlpha(0.0f, 1.5f, false);
    }

    private void GolfUpSwitch()
    {
        if (Switch == false)
        {
            im.sprite = children[3].GetComponent<Image>().sprite;
            Switch = true;
        }
        else
        {
            im.sprite = children[4].GetComponent<Image>().sprite;
            Switch = false;
        }
    }

    private void GunUpSwitch()
    {
        if (Switch == false)
        {
            im.sprite = children[5].GetComponent<Image>().sprite;
            Switch = true;
        }
        else
        {
            im.sprite = children[6].GetComponent<Image>().sprite;
            Switch = false;
        }
    }

    public void UpdateState()
    {
        startingStates = StartStates.gunUp_5;
        myCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("PointLights"));
    }

    public void UpdateState2()
    {
        startingStates = StartStates.gunUp_6;
    }

    public void UpdateState3(string name,Color c)
    {
        print("This is the name " + name);
        GetComponent<Player_Manager_PP_001>().SetPlayerName(name);
        playerControll_Script.SetMannyColor(c);
        startingStates = StartStates.begin_9;
    }

    public Vector3 GetStartTransform()
    {
        return startTransform.position;
    }
}
