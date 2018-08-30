using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class IGolfable : NetworkBehaviour
{
    //Player_State_Manager_PP_001 playerState_Script;
    Player_Input_Manager_PP_001 playerState;
    public ScriptableObjectClass myScriptableObject;
    public Rigidbody rb;
    private Shader normal;
    private Shader effect;

    [SerializeField] private Material materialNormal;
    [SerializeField] private Material materialEffect;
    Material tempMaterial;

    //we will not keep it like this, but for now it's fine.
    GameObject[] players;
    GameObject a;

    Renderer rend;
    MaterialPropertyBlock _originalpropBlock;
    MaterialPropertyBlock _propBlock;

    Color originalColor;

    bool foundPlayers = false;

    [SyncVar] private NetworkIdentity objNetId_object;
    private NetworkIdentity objNetId_player;

    private Vector3 playerForward;

    public int lastHitBy = 0; // dictates who last hit this ball

    private bool hit = false;
    private bool hitAndCanGuide = false;
    public bool isGolfBall = false;

    [SerializeField] private GameObject Shrapnel;
    [SerializeField] private GameObject Ammo;

    private bool withinGolfBubble = false;
    private bool chosenWithinGolfBubble = false;

    private Camera currentCamera;
    private GameObject currentPlayer;

    void Start()
    {
        //gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Custom/ImageEffectShader");
        rb = GetComponent<Rigidbody>();
        rb.mass = 1000;
        _propBlock = new MaterialPropertyBlock();

        players = GameObject.FindGameObjectsWithTag("PlayerCharacter");
        if (players.Length != 0)
        {
            playerState = players[0].GetComponent<Player_Input_Manager_PP_001>();
            a = players[0];
            foundPlayers = true;
        }

        rend = GetComponent<Renderer>();

        tempMaterial = materialEffect;

        //rend.GetPropertyBlock(_originalpropBlock);

        originalColor = GetComponent<Renderer>().material.GetColor("_Color");

        rend.GetPropertyBlock(_propBlock);
    }

    void Update()
    {
        if (foundPlayers == true)
        {
            if (playerState.playerCurrentState == Player_Input_Manager_PP_001.PlayerStatus.GolfFree)
            {
                //gameObject.GetComponent<Renderer>().material = materialEffect;
                //float distance = Vector3.Distance(transform.position, a.transform.position);
                rend.GetPropertyBlock(_propBlock);

                if (withinGolfBubble == false)
                {
                    //Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                    _propBlock.SetColor("_Color", Color.red);
                }
                else
                {
                    if (chosenWithinGolfBubble == true) _propBlock.SetColor("_Color", Color.green);
                    else _propBlock.SetColor("_Color", Color.yellow);
                }
                rend.SetPropertyBlock(_propBlock);
            }
            else
            {
                gameObject.GetComponent<Renderer>().material = materialNormal;
                _propBlock.SetColor("_Color", originalColor);
                rend.SetPropertyBlock(_propBlock);
            }
        }
        else FindPlayers();
        if (hitAndCanGuide)
        {
            float x = Input.GetAxis("Horizontal") * Time.deltaTime * 25.0f;
            float y = Input.GetAxis("Vertical") * Time.deltaTime * 25.0f;
            transform.Translate(0, 0, -y);
            transform.Translate(-x, 0, 0);
            //okay this is going to have to be from the perspective of the camera i believe and using those forward vectors???
            //THINK MORE OF HOW TO JUST GET THE VECTORS TO HELP SPIN THE BALL WITHOUT MOVING IT, SO YOU ARE JUST GUIDING IT INSTEAD OF CONTROLLING IT

            Vector3 cameraForward = currentCamera.transform.forward;
        }
    }

    private void FindPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("PlayerCharacter");
        if (players.Length != 0)
        {
            playerState = players[0].GetComponent<Player_Input_Manager_PP_001>();
            a = players[0];
            foundPlayers = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hit == true)
        {
            hit = false;
            if (isGolfBall == false)
            {
                hitAndCanGuide = false;
                ExplodeAndReleaseAmmo();
                StartCoroutine(ReturnCameraToPlayer());
            }
        }

        if (collision.gameObject.tag == "GolfHitBox") 
        {
            playerForward = collision.gameObject.transform.root.transform.forward;
            GameObject collisionParent = collision.transform.root.gameObject;
            collisionParent.gameObject.GetComponent<Player_Controller_Manager_PP_001>().HitAnIGolfableObject(GetComponent<NetworkIdentity>(), this.gameObject, collision.gameObject.name);
            lastHitBy = collisionParent.gameObject.GetComponent<Player_Manager_PP_001>().GetPlayerID();
            hit = true;

            if (playerState.playerCurrentState == Player_Input_Manager_PP_001.PlayerStatus.GolfCircle)
            {
                currentPlayer = collision.gameObject.transform.root.gameObject;
                currentPlayer.GetComponent<Player_Input_Manager_PP_001>().circleGolfModeBallInFlight = false;
                currentCamera = currentPlayer.GetComponent<Player_Input_Manager_PP_001>().myCamera;
                currentCamera.transform.parent = this.gameObject.transform;
                hitAndCanGuide = true;
            }
        }
        else
        {
            rb.mass = 50.0f;
        }
    }

    public void ExplodeAndReleaseAmmo()
    {
        if ((Shrapnel != null) && (Ammo != null))
        {
            GetComponent<AudioSource>().Play();
            for (int i = 0; i < 30; i++)
            {
                Instantiate(Shrapnel, transform.position, transform.rotation);
            }  
            for (int i = 0; i < 10; i++)
            {
                Instantiate(Ammo, transform.position, transform.rotation);
            }
        }
        //Destroy(this.gameObject);
    }

    public void GolfTheObject(string clubType)
    {
        //DRIVER
        if (clubType == "HitBoxForGolfClub_Driver")
        {
            GetComponent<ParticleSystem>().Play();
            rb.mass = 1.0f;
            playerForward = playerForward + new Vector3(0, 1, 0);
            playerForward = playerForward * 1000;
            rb.AddForce(playerForward);
            playerForward = Vector3.zero;    
        }
        //IRON
        else if (clubType == "HitBoxForGolfClub_Iron")
        {
            GetComponent<ParticleSystem>().Play();
            rb.mass = 1.0f;
            playerForward = playerForward + new Vector3(0, 1, 0);
            playerForward = playerForward * 500;
            rb.AddForce(playerForward);
            playerForward = Vector3.zero;
        }
        //PUTTER
        else if (clubType == "HitBoxForGolfClub_Putter")
        {
            GetComponent<ParticleSystem>().Play();
            rb.mass = 1.0f;
            playerForward = playerForward * 750;
            rb.AddForce(playerForward);
            playerForward = Vector3.zero;
        }
    } 
    
    public void WithinGolfBubble(bool b)
    {
        withinGolfBubble = b;
    }     
    
    public void ChosenWithinGolfBubble(bool b)
    {
        chosenWithinGolfBubble = b;
    }  

    IEnumerator ReturnCameraToPlayer()
    {
        yield return new WaitForSeconds(2.0f);
        GetComponent<ParticleSystem>().Stop();
        currentCamera.transform.parent = currentPlayer.transform;
        GameObject temp = currentCamera.GetComponent<PlayerCameraController>().camAlivePosition;
        currentCamera.transform.position = temp.transform.position;//this is going back to not the right space?
        currentCamera.transform.rotation = temp.transform.rotation;
        currentCamera.transform.Rotate(-9, 0, 0);
        currentPlayer.GetComponent<Player_Input_Manager_PP_001>().circleGolfModeBallInFlight = true;

    }
}