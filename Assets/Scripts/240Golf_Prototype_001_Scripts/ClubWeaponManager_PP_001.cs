using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClubWeaponManager_PP_001 : MonoBehaviour
{

    [SerializeField] private GameObject StockClub_Driver;
    [SerializeField] private GameObject StockClub_Iron;
    [SerializeField] private GameObject StockClub_Putter;

    [SerializeField] private GameObject Rifle_Driver;
    [SerializeField] private GameObject Rifle_Iron;
    [SerializeField] private GameObject Rifle_Putter;

    [SerializeField] private GameObject GolfClub_Driver;
    [SerializeField] private GameObject GolfClub_Iron;
    [SerializeField] private GameObject GolfClub_Putter;


    GameObject currentClub;
    GameObject currentWeapon;
    GameObject currentGolfClub;

    List<GameObject> UnusedClubs = new List<GameObject>();
    List<GameObject> UnusedWeapons = new List<GameObject>();
    List<GameObject> UnusedGolfClubs = new List<GameObject>();

    public enum BulletStates { DriverBullet, IronBullet, PutterBullet };
    public BulletStates currentBulletState;

    // Use this for initialization
    void Start()
    {
        UnusedClubs.Add(StockClub_Iron);
        UnusedClubs.Add(StockClub_Putter);
        StockClub_Iron.SetActive(false);
        StockClub_Putter.SetActive(false);

        UnusedWeapons.Add(Rifle_Iron);
        UnusedWeapons.Add(Rifle_Putter);
        Rifle_Driver.SetActive(false);
        Rifle_Iron.SetActive(false);
        Rifle_Putter.SetActive(false);

        UnusedGolfClubs.Add(GolfClub_Iron);
        UnusedGolfClubs.Add(GolfClub_Putter);
        GolfClub_Driver.GetComponentInChildren<BoxCollider>().enabled = false;
        GolfClub_Iron.GetComponentInChildren<BoxCollider>().enabled = false;
        GolfClub_Putter.GetComponentInChildren<BoxCollider>().enabled = false;
        GolfClub_Driver.SetActive(false);
        GolfClub_Iron.SetActive(false);
        GolfClub_Putter.SetActive(false);

        currentClub = StockClub_Driver;
        currentWeapon = Rifle_Driver;
        currentGolfClub = GolfClub_Driver;

        currentBulletState = BulletStates.DriverBullet;
    }

    public void VanillaMode()
    {
        currentClub.SetActive(true);
        Rifle_Driver.SetActive(false);
        Rifle_Iron.SetActive(false);
        Rifle_Putter.SetActive(false);
        foreach (GameObject unusedClub in UnusedClubs)
        {
            unusedClub.SetActive(false);
        }
    }

    public void WeaponMode()
    {
        currentWeapon.SetActive(true);
        StockClub_Driver.SetActive(false);
        StockClub_Iron.SetActive(false);
        StockClub_Putter.SetActive(false);
        foreach (GameObject unusedWeapon in UnusedWeapons)
        {
            unusedWeapon.SetActive(false);
        }
    }

    public void GolfMode()
    {
        currentGolfClub.SetActive(true);
        /*StockClub_Driver.SetActive(false);
        StockClub_Iron.SetActive(false);
        StockClub_Putter.SetActive(false);
        Rifle_Driver.SetActive(false);
        Rifle_Iron.SetActive(false);
        Rifle_Putter.SetActive(false);*/
        foreach (GameObject unusedGolfClub in UnusedGolfClubs)
        {
            unusedGolfClub.SetActive(false);
        }
    }


    //DRIVER
    public void SwitchToDriver()
    {
        currentClub = StockClub_Driver;
        currentWeapon = Rifle_Driver;
        currentGolfClub = GolfClub_Driver;

        UnusedClubs.Clear();
        UnusedClubs.Add(StockClub_Iron);
        UnusedClubs.Add(StockClub_Putter);

        UnusedWeapons.Clear();
        UnusedWeapons.Add(Rifle_Iron);
        UnusedWeapons.Add(Rifle_Putter);

        UnusedGolfClubs.Clear();
        UnusedGolfClubs.Add(GolfClub_Iron);
        UnusedGolfClubs.Add(GolfClub_Putter);

        currentBulletState = BulletStates.DriverBullet;
        //playerManagerScript.CurrentClubIsDriver();
    }

    //Iron
    public void SwitchToIron()
    {
        currentClub = StockClub_Iron;
        currentWeapon = Rifle_Iron;
        currentGolfClub = GolfClub_Iron;

        UnusedClubs.Clear();
        UnusedClubs.Add(StockClub_Driver);
        UnusedClubs.Add(StockClub_Putter);

        UnusedWeapons.Clear();
        UnusedWeapons.Add(Rifle_Driver);
        UnusedWeapons.Add(Rifle_Putter);

        UnusedGolfClubs.Clear();
        UnusedGolfClubs.Add(GolfClub_Driver);
        UnusedGolfClubs.Add(GolfClub_Putter);

        currentBulletState = BulletStates.IronBullet;
        //playerManagerScript.CurrentClubIsIron();
    }

    //Putter
    public void SwitchToPutter()
    {
        currentClub = StockClub_Putter;
        currentWeapon = Rifle_Putter;
        currentGolfClub = GolfClub_Putter;

        UnusedClubs.Clear();
        UnusedClubs.Add(StockClub_Driver);
        UnusedClubs.Add(StockClub_Iron);

        UnusedWeapons.Clear();
        UnusedWeapons.Add(Rifle_Driver);
        UnusedWeapons.Add(Rifle_Iron);

        UnusedGolfClubs.Clear();
        UnusedGolfClubs.Add(GolfClub_Driver);
        UnusedGolfClubs.Add(GolfClub_Iron);

        currentBulletState = BulletStates.PutterBullet;
        //playerManagerScript.CurrentClubIsPutter();
    }

    public void TurnOnGolfClubColliders()
    {
        currentGolfClub.GetComponentInChildren<BoxCollider>().enabled = true;
    }

    public void TurnOffGolfClubColliders()
    {
        currentGolfClub.GetComponentInChildren<BoxCollider>().enabled = false;
    }
}
