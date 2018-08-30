using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_State_Manager_PP_001 : MonoBehaviour {

    [SerializeField] private Player_Controller_Manager_PP_001 playerControll_Script;

    public enum PlayerStatus { Vanilla, Gun, GolfFree, GolfCircle, Map };
    public PlayerStatus playerCurrentState;

    // Use this for initialization
    void Start ()
    {
        playerCurrentState = PlayerStatus.Vanilla;
    }
	
    public void UpdateState_Vanilla()
    {
        playerCurrentState = PlayerStatus.Vanilla;
        playerControll_Script.resetCharacterToVanilla();
        //playerControll_Script.VanillaState();
    }

    public void UpdateState_Gun()
    {
        playerCurrentState = PlayerStatus.Gun;
        //playerControll_Script.GunState();
    }

    public void UpdateState_GolfFree()
    {
        playerCurrentState = PlayerStatus.GolfFree;
        //playerControll_Script.GolfFreeState();
    }

    public void UpdateState_GolfCircle()
    {
        playerCurrentState = PlayerStatus.GolfFree;
    }

    public void UpdateState_Map()
    {
        playerCurrentState = PlayerStatus.Map;
    }

}
