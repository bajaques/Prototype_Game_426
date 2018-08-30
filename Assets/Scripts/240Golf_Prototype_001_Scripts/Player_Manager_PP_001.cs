using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class Player_Manager_PP_001 : MonoBehaviour
{
    public Camera playerCam;
    public bool startUp = false;
    public bool infiniteAmmo = false;
    public Canvas playerCanvas;
    private Transform[] children;
    private Dictionary<int, bool> HoleCompletion = new Dictionary<int, bool>();
    private struct PlayerProfile
    {
        public int playerID;
        public int Team;
        public string playerName;
    };

    private PlayerProfile playerProfile;
    private AudioSource[] audios;

    //this crap is for controlling the player canvas - it is currently a mess that needs to be readdressed eventually
    public GameObject bulletCounterImage;
    private TextMeshProUGUI bulletCounterText;
    private int singleBullets = 9;
    private int tensBullets = 0;
    //might need to have a hundreds and a thousands place or atleast a plan to not let someone break the system.

    public GameObject movementImage; // temp fix to get the buid working
    public GameObject textImage;
    public float textSpeed;
    private RectTransform playCanRectTrans; //playerCanvasRectTransform;
    private RectTransform textImageRectTrans;
    Vector3 textImagePosition;
    Vector3 textImageScale;
    private bool moveTextImage = false;

    void Start()
    {
        playerProfile = new PlayerProfile();
        playerProfile.playerID = 0;
        playerProfile.Team = 0;
        _GM.instance.RegisterPlayers(this.gameObject,0);//sending zero right now for all teams

        children = playerCanvas.GetComponentsInChildren<Transform>();
        for (int i = 1; i < children.Length; ++i)
        {
            children[i].gameObject.SetActive(false);
            HoleCompletion[i] = false;
        }

        playCanRectTrans = playerCanvas.GetComponent<RectTransform>();
        textImageRectTrans = textImage.GetComponent<RectTransform>();
        textImagePosition = textImage.transform.position;
        textImageScale = textImage.transform.localScale;
        textImage.transform.localScale = new Vector3(0, 0, 0);

        audios = GetComponents<AudioSource>();

        if (startUp)
        {
            GetComponent<Player_Input_Manager_PP_001>().enabled = false;
            GetComponent<Player_Death_PP_001>().enabled = false;
        }
        else
        {
            GetComponent<Player_TEMP_Input_Manager_PP_001>().TurnOffAllUIImages();
            GetComponent<Player_TEMP_Input_Manager_PP_001>().enabled = false;
            bulletCounterImage.SetActive(true);
            TurnOnPlayerAfterStartUp();//
        }
       
        bulletCounterText = bulletCounterImage.GetComponent<TextMeshProUGUI>();
        int temp = singleBullets + 1;
        bulletCounterText.SetText(temp.ToString());
    }

    void Update()
    {
        if (moveTextImage)
        {
            textImage.transform.localScale = Vector3.Slerp(textImage.transform.localScale, textImageScale, Time.deltaTime * 1.5f);
            if (textImage.transform.localScale == textImageScale)
            {
                moveTextImage = false;
                TurnOffACanvasChild(13);
            }
        }      
    }

    public void SetPlayerID(int id)
    {
        playerProfile.playerID = id;
        //print(playerProfile.playerID);
    }

    public int GetPlayerID()
    {
        return playerProfile.playerID;
    }

    public void SetPlayerName(string name)
    {
        playerProfile.playerName = name;
        print(playerProfile.playerName);
        TurnOnPlayerName(name);
        children[12].gameObject.GetComponent<TextMeshProUGUI>().SetText("Hello my name is " + name);
    }

    public void UpdateGolfHoles(int h, int p)//hole //player
    {
        if (p == 1)
        {
            int i = h;
            children[i].gameObject.SetActive(true);
            HoleCompletion[i] = true;
            StartCoroutine(CheckHoleStatus(p));
        }
        else if (p == 2)
        {
            int i = h + 5;
            children[i].gameObject.SetActive(true);
            HoleCompletion[i] = true;
            StartCoroutine(CheckHoleStatus(p));
        }
    }

    public void TurnOnPlayerAfterStartUp()
    {
        StartCoroutine(TurnOffUIImage(2.5f));
        GetComponent<Player_TEMP_Input_Manager_PP_001>().enabled = false;
        GetComponent<Player_Input_Manager_PP_001>().enabled = true;
        GetComponent<Player_Death_PP_001>().enabled = true;
    }

    public void TurnOffACanvasChild(int c)
    {
        children[c].gameObject.SetActive(false);
    }

    public void TurnOnPlayerName(string name)
    {
        children[12].gameObject.SetActive(true);
        bulletCounterImage.SetActive(true);
    }

    //FOR FUTURE REFERENCE, WHEN AN OBJECT'S IMAGE COMPONENT GOES TO ZERO, IT WILL NEED TO BE CROSS FADED UP AGAIN SO YOU CAN SEE IT, IF YOU TURN THAT OBJECT OFF.  
    //YOU ARE NOT DOING THIS RIGHT NOW, SO SOMETHING TO THINK ABOUT LATER ON IF THINGS ARE NOT GOING EXACTLY AS FORSEEN ...
    IEnumerator TurnOffUIImage(float time)
    {
        yield return new WaitForSeconds(time);
        Image im = movementImage.GetComponent<Image>();
        im.CrossFadeAlpha(0.0f, 1.5f, false);
        if(startUp)
        {
            StartCoroutine(TurnOnTextImage(time / 2));
        }
    }

    IEnumerator TurnOnTextImage(float time)
    {
        yield return new WaitForSeconds(time);
        textImage.SetActive(true);
        moveTextImage = true;
    }

    IEnumerator CheckHoleStatus(int p)
    {
        yield return new WaitForSeconds(.5f);
        bool tf = true;
        if (p == 1)
        {
            for (int i = 1; i < 5; ++i)
            {
                if (HoleCompletion[i] == false)
                    tf = false;
            }
            if (tf)
            {
                children[5].gameObject.SetActive(true);
            }
        }
        else if (p == 2)
        {
            for (int i = 6; i < 10; ++i)
            {
                if (HoleCompletion[i] == false)
                {
                    tf = false;
                }      
            }
            if (tf)
            {
                children[10].gameObject.SetActive(true);
            }
        }
    }

    public void PlaySound(int s)
    {
        audios[s].Play();
    }

    public void BulletAdder()
    {
        audios[1].Play();
        if (singleBullets == 9)
        {
            singleBullets = 0;
            tensBullets = tensBullets + 1;
        }
        else
        {
            singleBullets = singleBullets + 1;
        }
        int temp = singleBullets + 1;
        bulletCounterText.SetText(temp.ToString());
    }

    public void BulletSubtrac()
    {

        if ((tensBullets == 0) && (singleBullets == 0))
        {
            singleBullets = -1;
        }
        else if (singleBullets == 0)
        {
            singleBullets = 9;
            tensBullets = tensBullets - 1;
        }
        else
        {
            singleBullets = singleBullets - 1;
        }
        int temp = singleBullets + 1;
        bulletCounterText.SetText(temp.ToString());
    }

    public bool CheckBulletAvailability()
    {
        if (infiniteAmmo == true)
        {
            audios[2].Play();
            return true;
        }

        if (singleBullets >= 0) // and another check for the tens place right
        {
            BulletSubtrac();
            playerCam.GetComponent<ScreenShake>().GunFireShake();
            audios[2].Play();
            return true;
        }
        else
        {
            audios[3].Play();
            return false;
        }
    }
    public int GetTensBullets()
    {
        return tensBullets;
    }
    public int GetSinglesBullets()
    {
        return singleBullets;
    }
}

