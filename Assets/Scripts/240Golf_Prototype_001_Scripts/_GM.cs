using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//using UnityEditor;
using TMPro;
using UnityEngine.Networking;//

//public class _GM : MonoBehaviour
public class _GM: NetworkBehaviour

{
    public static _GM instance = null;

    //CAMERAS
    public Camera mainCamera;
    public Camera fastTravelCamera;


    //STATIC POSITION
    public Transform respawnHeightPos;
    public Transform ballrespawnPoint;

    //DICTIONARIES
    public List<GameObject> Holes = new List<GameObject>();
    Dictionary<int, GameObject> HolesDictionary = new Dictionary<int, GameObject>();

    GameObject currentHole = null;

    //PLAYER DICTIONARY
    private Dictionary<int,GameObject> PlayerDictionary = new Dictionary<int, GameObject>();
    private Dictionary<int,List<int>> PlayerTeamAssignments = new Dictionary<int, List<int>>();
    private int playerID = 0;


    //CLOUD DICTIONARY ... PLAYER SELECT STUFF
    //public SyncList<GameObject> Clouds;
    public GameObject[] Clouds;
    public Material[] matColors;


    //NAME DICTIONARY STUFF
    private Dictionary<int, string> animalNames = new Dictionary<int, string>();
    private Dictionary<int, string> animalAdjectives = new Dictionary<int, string>();

    private int animalSize = 0;
    private int adjectiveSize = 0;
    private int animalHalf = 0;
    private int adjectiveHalf = 0;
    

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this) Destroy(gameObject);

        //if (isServer)
        //{
            //READING IN DATASETS - PRIMARILY LIST OF RANDOM NAMES
            int i = 0;
            //StreamReader inp_stm = new StreamReader(Application.dataPath + "/" + "Resources/240_animalNamesList");
            StreamReader inp_stm = new StreamReader(Application.streamingAssetsPath + "//" + "240_animalNamesList");

            while (!inp_stm.EndOfStream)
            {
                string inp_ln = inp_stm.ReadLine();
                animalNames[i] = inp_ln;
                i++;
            }
            inp_stm.Close();
            animalSize = animalNames.Count;
            animalHalf = animalSize / 2;
            i = 0;

            //inp_stm = new StreamReader(Application.dataPath + "/" + "Resources/240_adjectiveNames");
            inp_stm = new StreamReader(Application.streamingAssetsPath + "//" + "240_adjectiveNames");

            while (!inp_stm.EndOfStream)
            {
                string inp_ln = inp_stm.ReadLine();
                inp_ln = inp_ln.Replace(" ", string.Empty);
                // LastName = LastName.Replace(" ", String.Empty);
                animalAdjectives[i] = inp_ln;
                i++;
            }
            inp_stm.Close();
            adjectiveSize = animalAdjectives.Count;
            adjectiveHalf = adjectiveSize / 2;
        //}

        /*if (!isServer)
        {
            for (int i = 0; i < Clouds.Length; ++i)
            {
                //print(Clouds[i].transform.GetChild(2));
                int r = Random.Range(0, 6);
                Clouds[i].transform.GetChild(2).GetComponent<MeshRenderer>().material = matColors[r];
                string name = RandomNameGenerator();
                Clouds[i].transform.GetChild(3).GetComponent<TextMeshPro>().SetText(name);
            }
        }*/
    }

    void Start()
    {
        for (int i = 0; i < Holes.Count; ++i)
        {
            Holes[i].transform.GetChild(0).gameObject.SetActive(false);
            Holes[i].transform.GetChild(1).gameObject.SetActive(false);
            Holes[i].transform.GetChild(3).gameObject.SetActive(false);
            HolesDictionary[i + 1] = Holes[i]; // think about this for a sec before you go HAM, this could be lethal later.  
        }

        /*for (int i = 0; i < CloudsArray.Length; ++i)
        {
            Clouds.Add(CloudsArray[i]);
        }*/

        RegisterClouds(); 
    }

    //CAMERA FUNCTIONS
    public void TurnOffMainCamera()
    {
        mainCamera.enabled = false;
    }

    public void TurnOnFastTravelCamera()
    {
        fastTravelCamera.enabled = true; // this is being accessed alot by that horrible player camera controller, re do that script eventually
        ShowCaseHoles(true);
    }

    public void TurnOffFastTravelCamera()
    {
        fastTravelCamera.enabled = false;
        ShowCaseHoles(false);
    }

    public void CurrentHolePlayerTouched(GameObject h)
    {
        for (int i = 0; i < Holes.Count; ++i)
        {
            if (Holes[i] == h)
            {
                Holes[i].transform.GetChild(1).gameObject.SetActive(true);
                currentHole = h;
                return;
            }
        }
    }

    private void ShowCaseHoles(bool b)
    {
        for (int i = 0; i < Holes.Count; ++i)
        {
            Holes[i].transform.GetChild(0).gameObject.SetActive(b);
            Holes[i].transform.GetChild(3).gameObject.SetActive(b);
            if (b == false)
            {
                Holes[i].transform.GetChild(1).gameObject.SetActive(b);
            }
        }
    }


    //PLAYER FUNCTIONS
    public void RegisterPlayers(GameObject p, int team)
    {
        playerID = playerID + 1;
        PlayerDictionary.Add(playerID, p);
        if (PlayerTeamAssignments.ContainsKey(team) == true)
        {
            PlayerTeamAssignments[team].Add(playerID);
        }
        else
        {
            List<int> t = new List<int>();
            t.Add(playerID);
            PlayerTeamAssignments.Add(team, t);
        }
        p.GetComponent<Player_Manager_PP_001>().SetPlayerID(playerID);
    }

    private void RegisterClouds()
    {
        if (isServer)//
        {//
            //foreach (GameObject Cloud in Clouds)
            //{
                for (int i = 0; i < Clouds.Length; ++i)
                {
                //print(Clouds[i].transform.GetChild(2));
                int r = Random.Range(0, 6);
                Clouds[i].transform.GetChild(0).GetComponent<Light>().color = matColors[r].color;
                Clouds[i].transform.GetChild(2).GetComponent<MeshRenderer>().material = matColors[r];
                //Cloud.transform.GetChild(2).GetComponent<MeshRenderer>().material = matColors[r];
                string name = RandomNameGenerator();
                Clouds[i].transform.GetChild(3).GetComponent<TextMeshPro>().SetText(name);
                //Cloud.transform.GetChild(3).GetComponent<TextMeshPro>().SetText(name);
            }
        }//
    }

    public GameObject GetPlayer(int id)
    {
        //GameObject g = PlayerDictionary[id];
        return PlayerDictionary[id];
    }

    public Dictionary<int,GameObject> GetPlayers()
    {
        return PlayerDictionary;
    }

    public string RandomNameGenerator()
    {
        if (animalNames.Count == 0 || animalAdjectives.Count == 0) return "Man with No Name";

        //Animal Name
        int r = Random.Range(0, animalSize);
        bool containsCheck = false;
        while(containsCheck == false)
        {
            if (!animalNames.ContainsKey(r))
            {
                if (r == animalSize - 1) r = 0;
                else r = r + 1;
            }
            else containsCheck = true;
        }
        string name1 = animalNames[r];
        animalNames.Remove(r);
        if (animalNames.Count <= animalHalf)
        {
            Dictionary<int, string>.ValueCollection values = animalNames.Values;
            animalNames.Clear();
            int i = 0;
            foreach (string s in values)
            {
                animalNames[i] = s;
                i++;
            }
            animalSize = animalNames.Count;
            animalHalf = animalSize / 2;
        }
        //Adjective Name
        r = Random.Range(0, adjectiveSize);
        containsCheck = false;

        while (containsCheck == false)
        {
            if (!animalAdjectives.ContainsKey(r))
            {
                if (r == adjectiveSize - 1) r = 0;
                else r = r + 1;
            }
            else containsCheck = true;
        }

        string name2 = animalAdjectives[r];
        animalAdjectives.Remove(r);
        if (animalAdjectives.Count <= adjectiveHalf)
        {
            Dictionary<int, string>.ValueCollection values = animalAdjectives.Values;
            animalAdjectives.Clear();
            int i = 0;
            foreach (string s in values)
            {
                animalAdjectives[i] = s;
                i++;
            }
            adjectiveSize = animalAdjectives.Count;
            adjectiveHalf = adjectiveSize / 2;
        }

        name2 = name2 + " " + name1;
        return name2;
    }
}
