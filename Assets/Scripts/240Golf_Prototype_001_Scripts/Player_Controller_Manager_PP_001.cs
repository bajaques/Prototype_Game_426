using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player_Controller_Manager_PP_001 : NetworkBehaviour
{
    [SerializeField] private GameObject NormalManny; //used now for guns and vanilla
    [SerializeField] private GameObject GolfingManny;
    [SerializeField] private GameObject MannyAngelWings;
    [SerializeField] private ClubWeaponManager_PP_001 clubWeaponManagerScript;
    [SerializeField] private GameObject RealGolfSphereCaster;

    private Vector3 golfEulerAngles;
    private bool golfFreeState = false;
    
    Animator anim;
    Animator animGolf;

    int walkBoolHash = Animator.StringToHash("WalkBool");
    int walkToRunBoolHash = Animator.StringToHash("WalkToRunBool");
    int crouchBoolHash = Animator.StringToHash("CrouchBool");
    int jumpBoolHash = Animator.StringToHash("JumpBool");
    int rifleIdleHash = Animator.StringToHash("RifleIdleBool");
    int rifleWalkHash = Animator.StringToHash("RifleWalkBool");
    int rifleCrouchHash = Animator.StringToHash("RifleCrouchBool");
    int golfReverseHash = Animator.StringToHash("reverseAnimation");
    int golfPullToSwingHash = Animator.StringToHash("GolfDrivePullToSwing");
    int meleeBoolHash = Animator.StringToHash("MeleeBool");
    int shotBoolHash = Animator.StringToHash("ShotBool");
    int deathRiseBoolHash = Animator.StringToHash("DeathRiseBool");
    int deathFallBoolHash = Animator.StringToHash("DeathFallBool");
    int gunWalkToWalkBoolHash = Animator.StringToHash("GunWalkToWalkBool");

    [SerializeField] private GameObject StockClub_Driver;//
    [SerializeField] private GameObject StockClub_Iron;//
    [SerializeField] private GameObject StockClub_Putter;//

    [SerializeField] private GameObject Rifle_Driver;//
    [SerializeField] private GameObject Rifle_Iron;//
    [SerializeField] private GameObject Rifle_Putter;//

    [SerializeField] private GameObject bulletPrefabDriver;//
    [SerializeField] private GameObject bulletPrefabIron;
    [SerializeField] private GameObject bulletPrefabPutter;//
    [SerializeField] private Transform bulletSpawn;//

    [SerializeField] private GameObject meleeSlash;

    private float preSwing;
    private bool swingThrough;

    private float height = 0;
    private bool crouching = false;
    private bool jumping = false;
    private bool buttonAOn = true;
    bool jump2 = false;

    public bool die = true;
    public bool dieRise = false;

    float x;
    float y;
    float z;

    private Vector3 moveDirection = Vector3.zero;
    public float speed = 10.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 200.0F;

    private float speedUpRotation = 100.0f;
    public GameObject currentTargetGolfable = null;

    void Awake()
    {
        anim = GetComponent<Animator>();
        animGolf = GolfingManny.GetComponent<Animator>();
    }

    void Start()
    {
        golfEulerAngles = GolfingManny.transform.rotation.eulerAngles;

        GolfingManny.SetActive(false);
        MannyAngelWings.SetActive(false);

        preSwing = 0.0f; //last Swing Float
        swingThrough = false;
    }

    public void RunTranslate()
    {
        z = Input.GetAxis("CameraHorizontal") * Time.deltaTime * 35.0f;
        transform.Rotate(0, z * 4.0f, 0);
    }

    public void RunWalkRun(bool run)

    {
        //WALKING
        /*x = Input.GetAxis("Horizontal") * Time.deltaTime * 10.0f;
        y = Input.GetAxis("Vertical") * Time.deltaTime * 10.0f;

        if (run == false)
        {
            transform.Translate(0, 0, y);
            transform.Translate(x, 0, 0);
        }
        else
        {
            transform.Translate(0, 0, y * 4);
            transform.Translate(x * 4, 0, 0);
        }*/

        if (run == false)
        {
            x = Input.GetAxis("Horizontal") * Time.deltaTime * 10.0f;
            y = Input.GetAxis("Vertical") * Time.deltaTime * 10.0f;
            CharacterController controller = GetComponent<CharacterController>();
            if (controller.isGrounded)
            {
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= speed;
                /*if (Input.GetButton("Jump")) moveDirection.y = jumpSpeed;*/

            }
            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
        }
        else
        {
            x = Input.GetAxis("Horizontal") * Time.deltaTime * 40.0f;
            y = Input.GetAxis("Vertical") * Time.deltaTime * 40.0f;
            CharacterController controller = GetComponent<CharacterController>();
            if (controller.isGrounded)
            {
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= speed * 10.0f;
                /*if (Input.GetButton("Jump")) moveDirection.y = jumpSpeed;*/

            }
            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
        }
        
    }

    public void RunVanillaState(bool run)
    {
        if ((x != 0.0f || y != 0.0f))
        {
            anim.SetBool(walkBoolHash, true);
            if (run)//RUNNING
            {
                anim.SetBool(walkToRunBoolHash, true);
            }
            else
            {
                anim.SetBool(walkToRunBoolHash, false);
            }
        }
        else
        {
            anim.SetBool(walkBoolHash, false);
        }

        //MELEE
        if (Input.GetKeyDown("joystick button 5"))
        {
            if (anim.GetBool(meleeBoolHash) == false)
            {
                anim.SetBool(meleeBoolHash, true);
                Vector3 MannyMiddleBody = NormalManny.transform.position;
                MannyMiddleBody.y = MannyMiddleBody.y + .5f;
                GameObject GO = Instantiate(meleeSlash, MannyMiddleBody, NormalManny.transform.rotation) as GameObject;
                Physics.IgnoreCollision(GO.GetComponent<Collider>(), this.GetComponent<Collider>());
                GO.GetComponent<Rigidbody>().velocity = NormalManny.transform.forward * 15.0f;
                Destroy(GO, 0.1f);
                StartCoroutine(allowMeleeAttackAgain());
            } 
        }


        if (Input.GetKeyDown("joystick button 9"))
        {
            if (anim.GetBool(crouchBoolHash) == false)
            {
                anim.SetBool(crouchBoolHash, true);
            }
            else
            {
                anim.SetBool(crouchBoolHash, false);
            }
        }

        if (Input.GetKey("joystick button 0"))
        {
            //height = height + 0.5f * Time.deltaTime;
            //height = 0.5f;
            if (!run) height = 0.5f;
            else height = 0.75f;
        }

        if (Input.GetKeyUp("joystick button 0"))
        {
            jumping = true;
        }

        if (jumping)
        {
            if (!run)
            {
                if (height > 2.0f) height = 2.0f;
            }
            //if (height > 2.0f) height = 2.0f;
            transform.Translate(0, height, 0);
            //anim.SetBool(jumpBoolHash, true);
            StartCoroutine(transitionAfterJump());
        }
    }

    IEnumerator transitionAfterJump()
    {
        yield return new WaitForSeconds(1.0f);
        //anim.SetBool(jumpBoolHash, false);
        jumping = false;
        height = 0.0f;
    }

    IEnumerator allowMeleeAttackAgain()
    {
        yield return new WaitForSeconds(.5f);
        anim.SetBool(meleeBoolHash, false);
    }

    public void RunGunState()
    {
        anim.SetBool(rifleIdleHash, true);

        if ((x != 0.0f || y != 0.0f))
        {
            anim.SetBool(rifleWalkHash, true);
        }
        else
        {
            anim.SetBool(rifleWalkHash, false);
        }

        //CROUCHING
        if (Input.GetKeyDown("joystick button 0"))
        {
            if (anim.GetBool(rifleCrouchHash) == false)
            {
                anim.SetBool(rifleCrouchHash, true);
            }
            else
            {
                anim.SetBool(rifleCrouchHash, false);
            }
        }
       
        //FIRING GUN
        if (Input.GetKeyDown("joystick button 5"))
        {
            if (GetComponent<Player_Manager_PP_001>().CheckBulletAvailability() == false)
            {
                return;
            }

            if (!isServer)
            {
                if (clubWeaponManagerScript.currentBulletState == ClubWeaponManager_PP_001.BulletStates.DriverBullet)
                {
                    CmdFireDriver();
                }
                else if (clubWeaponManagerScript.currentBulletState == ClubWeaponManager_PP_001.BulletStates.IronBullet)
                {
                    CmdFireIron();
                }
                else if (clubWeaponManagerScript.currentBulletState == ClubWeaponManager_PP_001.BulletStates.PutterBullet)
                {
                    CmdFirePutter();
                }

            }
            else
            {
                if (clubWeaponManagerScript.currentBulletState == ClubWeaponManager_PP_001.BulletStates.DriverBullet) // not sure if this is smart?
                {
                    GameObject GO = Instantiate(bulletPrefabDriver, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
                    Physics.IgnoreCollision(GO.GetComponent<Collider>(), this.GetComponent<Collider>());
                    GO.GetComponent<Rigidbody>().velocity = Rifle_Driver.transform.forward * 150.0f;
                    Destroy(GO, 10.0f);
                    StartCoroutine(TurnOnFriendlyFire(GO));
                    RpcShootDriver();
                }
                else if (clubWeaponManagerScript.currentBulletState == ClubWeaponManager_PP_001.BulletStates.IronBullet) // not sure if this is smart?
                {
                    GameObject GO = Instantiate(bulletPrefabIron, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
                    GO.GetComponent<Bullet_Iron_Spread_Individual>().col = this.GetComponent<Collider>();
                    Physics.IgnoreCollision(GO.GetComponent<Collider>(), this.GetComponent<Collider>());
                    GO.GetComponent<Rigidbody>().velocity = Rifle_Iron.transform.forward * 50.0f; //first place to check if bug
                    Destroy(GO, 0.2f);
                    StartCoroutine(TurnOnFriendlyFire(GO));
                    RpcShootIron();
                }
                else if (clubWeaponManagerScript.currentBulletState == ClubWeaponManager_PP_001.BulletStates.PutterBullet) // not sure if this is smart?
                {
                    GameObject GO = Instantiate(bulletPrefabPutter, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
                    Physics.IgnoreCollision(GO.GetComponent<Collider>(), this.GetComponent<Collider>());
                    GO.GetComponent<Rigidbody>().velocity = Rifle_Putter.transform.forward * 10.0f;
                    GO.GetComponent<Bullet_Putter_CirclePlayer>().SetPlayerToFollow(NormalManny);
                    //Destroy(GO, 0.2f);
                    //StartCoroutine(TurnOnFriendlyFire(GO));
                    RpcShootPutter();
                }
            }
        }
    }

    public void RunGolfFreeState()
    {
        if (golfFreeState == false)
        {
            GolfingManny.transform.forward = NormalManny.transform.forward;
            GolfingManny.transform.position = NormalManny.transform.parent.transform.position;
            golfFreeState = true;
        }

        NormalManny.SetActive(false);
        StockClub_Driver.SetActive(false);//
        StockClub_Iron.SetActive(false);//
        StockClub_Putter.SetActive(false);//
        GolfingManny.SetActive(true);

        animGolf.enabled = true;
        animGolf.SetFloat(golfReverseHash, 1.0f);

        //GOLFING
        float curSwing = Input.GetAxis("CameraVertical");
        if(curSwing == preSwing)
        {
            preSwing = 0.0f;
        }
        if ((curSwing < preSwing) && (curSwing >= 0.0f))
        {
            animGolf.SetFloat(golfReverseHash, -1.0f);
        }
        else if (curSwing < 0.0f)
        {
            clubWeaponManagerScript.TurnOnGolfClubColliders();
            swingThrough = true;
            animGolf.enabled = true;
            animGolf.SetFloat(golfReverseHash, 1.0f);
            animGolf.SetBool(golfPullToSwingHash, true);
        }
        preSwing = curSwing;
    }

    public void RunGolfCircleState(bool b)
    {
        if (b == true)
        {
            float rot = Input.GetAxis("Horizontal");

            if (rot != 0)
            {
                speedUpRotation = speedUpRotation + 0.5f;
                transform.RotateAround(currentTargetGolfable.transform.position, Vector3.up, -rot * Time.deltaTime * speedUpRotation);
            }
            else
            {
                speedUpRotation = 25.0f;
            }
        }
       
        //GOLFING
        float curSwing = Input.GetAxis("CameraVertical");
        if (curSwing == preSwing)
        {
            preSwing = 0.0f;
        }
        if ((curSwing < preSwing) && (curSwing >= 0.0f))
        {
            animGolf.SetFloat(golfReverseHash, -1.0f);
        }
        else if (curSwing < 0.0f)
        {
            clubWeaponManagerScript.TurnOnGolfClubColliders();
            swingThrough = true;
            animGolf.enabled = true;
            animGolf.SetFloat(golfReverseHash, 1.0f);
            animGolf.SetBool(golfPullToSwingHash, true);
        }
        preSwing = curSwing;
    }

    public void RunMapState()
    {

    }

    public void RunJustDiedState()
    {
        if (die == true)
        {
            die = false;
            anim.SetBool(shotBoolHash, true);
            StartCoroutine(DeathRise());
        }
    }

    public void RunDeadRiseState()
    {
        transform.position = Vector3.MoveTowards(transform.position, _GM.instance.respawnHeightPos.position, 20.0f * Time.deltaTime);
    }

    public void RunDeadFallState()
    {
        MannyAngelWings.SetActive(false);
        anim.SetBool(deathRiseBoolHash, false);
        anim.SetBool(deathFallBoolHash, true);
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<BoxCollider>().enabled = true;

        /*float x = Input.GetAxis("Horizontal") * Time.deltaTime * 100.0f;
        float y = Input.GetAxis("Vertical") * Time.deltaTime * 100.0f;
        transform.Translate(0, 0, y);
        transform.Translate(x, 0, 0);*/
    }

    public void RunComeBackAliveState()
    {
        anim.SetBool(deathFallBoolHash, false);

        /*float x = Input.GetAxis("Horizontal") * Time.deltaTime * 100.0f;
        float y = Input.GetAxis("Vertical") * Time.deltaTime * 100.0f;
        transform.Translate(0, 0, y);
        transform.Translate(x, 0, 0);*/
    }

    IEnumerator DeathRise()
    {
        yield return new WaitForSeconds(1.0f);
        dieRise = true;
        MannyAngelWings.SetActive(true);
        anim.SetBool(shotBoolHash, false);
        anim.SetBool(deathRiseBoolHash, true);
        GetComponent<Rigidbody>().useGravity = false;
        //GetComponent<BoxCollider>().enabled = false;
    }

    public void resetCharacterToVanilla()
    {

        if (golfFreeState == true)
        {
            RealGolfSphereCaster.GetComponent<RealGolfSphereCaster>().ReturnToScale();
            GetComponent<Player_Input_Manager_PP_001>().ChangeFromFreeGolfToCircle(true);
            golfFreeState = false;
        }

        anim.SetBool(rifleIdleHash, false);
        anim.SetBool(rifleCrouchHash, false);
        anim.SetBool(rifleWalkHash, false);
        anim.SetBool(gunWalkToWalkBoolHash, true);

        clubWeaponManagerScript.TurnOffGolfClubColliders();
        GolfingManny.transform.forward = NormalManny.transform.forward;
        print("Returning to Vanilla");
        transform.position = GolfingManny.transform.position;
        NormalManny.SetActive(true);
        GolfingManny.SetActive(false);
    }

    public void resetCharToVan_Start() //used only for starting tutorial
    {
        anim.SetBool(rifleIdleHash, false);
        anim.SetBool(rifleCrouchHash, false);
        anim.SetBool(rifleWalkHash, false);
        anim.SetBool(gunWalkToWalkBoolHash, true);

        clubWeaponManagerScript.TurnOffGolfClubColliders();
        GolfingManny.transform.forward = NormalManny.transform.forward;
        NormalManny.SetActive(true);
        GolfingManny.SetActive(false);
    }

    public void PlayWalkingAnimation(bool b)
    {
        anim.SetBool(walkBoolHash, b);
    }

    public void SetMannyColor(Color c)
    {
        //NormalManny.GetComponent<Renderer>().material.color = new Color(6.0f / 255.0f, 72.0f / 255.0f, 1.0f);
        //GolfingManny.GetComponentInChildren<Renderer>().material.color = new Color(6.0f / 255.0f, 72.0f / 255.0f, 1.0f);

        NormalManny.GetComponent<Renderer>().material.color = c;
        GolfingManny.GetComponentInChildren<Renderer>().material.color = c;
    }

    [Command]
    void CmdFireDriver()
    {
        GameObject GO = Instantiate(bulletPrefabDriver, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
        Physics.IgnoreCollision(GO.GetComponent<Collider>(), this.GetComponent<Collider>());
        GO.GetComponent<Rigidbody>().velocity = Rifle_Driver.transform.forward * 150.0f;
        Destroy(GO, 10.0f);
        StartCoroutine(TurnOnFriendlyFire(GO));
        RpcShootDriver();
    }

    [Command]
    void CmdFireIron()
    {
        GameObject GO = Instantiate(bulletPrefabIron, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
        GO.GetComponent<Bullet_Iron_Spread_Individual>().col = this.GetComponent<Collider>();
        Physics.IgnoreCollision(GO.GetComponent<Collider>(), this.GetComponent<Collider>());
        GO.GetComponent<Rigidbody>().velocity = Rifle_Iron.transform.forward * 50.0f; //first place to check if bug
        Destroy(GO, 0.2f);
        StartCoroutine(TurnOnFriendlyFire(GO));
        RpcShootIron();
    }

    [Command]
    void CmdFirePutter()
    {
        GameObject GO = Instantiate(bulletPrefabPutter, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
        Physics.IgnoreCollision(GO.GetComponent<Collider>(), this.GetComponent<Collider>());
        GO.GetComponent<Rigidbody>().velocity = Rifle_Putter.transform.forward * 10.0f;
        GO.GetComponent<Bullet_Putter_CirclePlayer>().SetPlayerToFollow(NormalManny);
        //Destroy(GO, 0.2f);
        //StartCoroutine(TurnOnFriendlyFire(GO));
        RpcShootPutter();
    }


    [ClientRpc]
    void RpcShootDriver()
    {
        if (!isServer)
        {
            GameObject GO = Instantiate(bulletPrefabDriver, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
            Physics.IgnoreCollision(GO.GetComponent<Collider>(), this.GetComponent<Collider>());
            GO.GetComponent<Rigidbody>().velocity = Rifle_Driver.transform.forward * 150.0f;
            Destroy(GO, 10.0f);
            StartCoroutine(TurnOnFriendlyFire(GO));
        }
    }

    [ClientRpc]
    void RpcShootIron()
    {
        if (!isServer)
        {
            GameObject GO = Instantiate(bulletPrefabIron, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
            GO.GetComponent<Bullet_Iron_Spread_Individual>().col = this.GetComponent<Collider>();
            Physics.IgnoreCollision(GO.GetComponent<Collider>(), this.GetComponent<Collider>());
            GO.GetComponent<Rigidbody>().velocity = Rifle_Iron.transform.forward * 50.0f; //first place to check if bug
            Destroy(GO, 0.2f);
            StartCoroutine(TurnOnFriendlyFire(GO));
        }
    }

    [ClientRpc]
    void RpcShootPutter()
    {
        if (!isServer)
        {
            GameObject GO = Instantiate(bulletPrefabPutter, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
            Physics.IgnoreCollision(GO.GetComponent<Collider>(), this.GetComponent<Collider>());
            GO.GetComponent<Rigidbody>().velocity = Rifle_Putter.transform.forward * 10.0f;
            GO.GetComponent<Bullet_Putter_CirclePlayer>().SetPlayerToFollow(NormalManny);
            //Destroy(GO, 0.2f);
            //StartCoroutine(TurnOnFriendlyFire(GO));
        }
    }

    public void HitAnIGolfableObject(NetworkIdentity golfableObject, GameObject golfableActual, string clubType)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        CmdServerAssignClient(golfableObject, golfableActual, clubType);
    }

    [Command]
    void CmdServerAssignClient(NetworkIdentity golfableObject, GameObject golfableActual, string clubType)
    {
        golfableObject.AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);
        RpcGolfTheObject(golfableObject, golfableActual, clubType);//
    }

    [ClientRpc]
    void RpcGolfTheObject(NetworkIdentity golfableObject,GameObject golfableActual, string clubType)
    {
        golfableActual.GetComponent<IGolfable>().GolfTheObject(clubType); 
    }

    IEnumerator TurnOnFriendlyFire(GameObject go)
    {
        yield return new WaitForSeconds(0.1f);
        Physics.IgnoreCollision(go.GetComponent<Collider>(), this.GetComponent<Collider>(), false);//
    }
}



