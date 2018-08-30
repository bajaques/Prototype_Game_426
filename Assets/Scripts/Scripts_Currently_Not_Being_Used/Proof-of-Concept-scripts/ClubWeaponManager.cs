using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClubWeaponManager : MonoBehaviour {

    GameObject PlayerManager;
    Player_Manager playerManagerScript;

    GameObject Club_Driver;
    GameObject Club_Iron;
    GameObject Club_Putter;

    GameObject Weapon_Driver;
    GameObject Weapon_Iron;
    GameObject Weapon_Putter;

    GameObject currentClub;
    GameObject currentWeapon;

    List<GameObject> UnusedClubs = new List<GameObject>();
    List<GameObject> UnusedWeapons = new List<GameObject>();

    public GameObject bulletPrefabDriver;
    public GameObject bulletPrefabIron;
    public GameObject bulletPrefabPutter;
    public Transform bulletSpawn;

    enum BulletStates {DriverBullet, IronBullet, PutterBullet };
    BulletStates currentBulletState;

    bool swingClub = false;


    // Use this for initialization
    void Start ()
    {
        PlayerManager = GameObject.Find("SmallHuman_001");
        playerManagerScript = PlayerManager.GetComponent<Player_Manager>();

        Club_Driver = GameObject.Find("GolfClub_Driver_elbow");
        Club_Iron = GameObject.Find("GolfClub_Iron_elbow");
        Club_Putter = GameObject.Find("GolfClub_Putter_elbow");

        Weapon_Driver = GameObject.Find("Weapon_Driver_elbow");
        Weapon_Iron = GameObject.Find("Weapon_Iron_elbow");
        Weapon_Putter = GameObject.Find("Weapon_Putter_elbow");

        UnusedClubs.Add(Club_Iron);
        UnusedClubs.Add(Club_Putter);
        Club_Iron.SetActive(false);
        Club_Putter.SetActive(false);

        UnusedWeapons.Add(Weapon_Iron);
        UnusedWeapons.Add(Weapon_Putter);
        Weapon_Driver.SetActive(false);
        Weapon_Iron.SetActive(false);
        Weapon_Putter.SetActive(false);

        currentClub = Club_Driver;
        currentWeapon = Weapon_Driver;

        currentBulletState = BulletStates.DriverBullet;
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (playerManagerScript.ShowCurrentPlayerState() == Player_Manager.PlayerStatus.Weapon)
        {
            currentWeapon.SetActive(true);
            Club_Driver.SetActive(false);
            Club_Iron.SetActive(false);
            Club_Putter.SetActive(false);
            foreach (GameObject unusedWeapon in UnusedWeapons)
            {
                unusedWeapon.SetActive(false);
            }

            if (Input.GetKeyDown("joystick button 5"))
            {
                Fire();
            }
        }

        if (playerManagerScript.ShowCurrentPlayerState() == Player_Manager.PlayerStatus.Vanilla)
        {
            currentClub.SetActive(true);
            Weapon_Driver.SetActive(false);
            Weapon_Iron.SetActive(false);
            Weapon_Putter.SetActive(false);
            foreach (GameObject unusedClub in UnusedClubs)
            {
                 unusedClub.SetActive(false);
            }
        }

        //Driver
        if (Input.GetKeyDown("joystick button 2"))
        {
            currentClub = Club_Driver;
            currentWeapon = Weapon_Driver;
            UnusedClubs.Clear();
            UnusedClubs.Add(Club_Iron);
            UnusedClubs.Add(Club_Putter);
            UnusedWeapons.Clear();
            UnusedWeapons.Add(Weapon_Iron);
            UnusedWeapons.Add(Weapon_Putter);
            currentBulletState = BulletStates.DriverBullet;
            playerManagerScript.CurrentClubIsDriver();
        }

        //Iron
        if (Input.GetKeyDown("joystick button 3"))
        {
            currentClub = Club_Iron;
            currentWeapon = Weapon_Iron;
            UnusedClubs.Clear();
            UnusedClubs.Add(Club_Driver);
            UnusedClubs.Add(Club_Putter);
            UnusedWeapons.Clear();
            UnusedWeapons.Add(Weapon_Driver);
            UnusedWeapons.Add(Weapon_Putter);
            currentBulletState = BulletStates.IronBullet;
            playerManagerScript.CurrentClubIsIron();
        }

        //Putter
        if (Input.GetKeyDown("joystick button 1"))
        {
            currentClub = Club_Putter;
            currentWeapon = Weapon_Putter;
            UnusedClubs.Clear();
            UnusedClubs.Add(Club_Driver);
            UnusedClubs.Add(Club_Iron);
            UnusedWeapons.Clear();
            UnusedWeapons.Add(Weapon_Driver);
            UnusedWeapons.Add(Weapon_Iron);
            currentBulletState = BulletStates.PutterBullet;
            playerManagerScript.CurrentClubIsPutter();
        }

    }

    void Fire()
    {
    // Create the Bullet from the Bullet Prefab

    if (currentBulletState == BulletStates.DriverBullet)
        {
            var bullet = (GameObject)Instantiate(bulletPrefabDriver, bulletSpawn.position, bulletSpawn.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * 150.0f;
            Destroy(bullet, 10.0f);
        }
    else if (currentBulletState == BulletStates.IronBullet)
        {
            var bullet = (GameObject)Instantiate(bulletPrefabIron, bulletSpawn.position, bulletSpawn.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * 100.0f;
            Destroy(bullet, 10.0f);
        }
    else if (currentBulletState == BulletStates.PutterBullet)
        {
            var bullet = (GameObject)Instantiate(bulletPrefabPutter, bulletSpawn.position, bulletSpawn.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * 50.0f;
            Destroy(bullet, 10.0f);
        }
    }
}
