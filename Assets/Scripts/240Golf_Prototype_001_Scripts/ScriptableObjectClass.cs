using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Inventory/List", order = 1)]
public class ScriptableObjectClass : ScriptableObject
{
    public string objectName = "Player_Prototype_001_002";
    public string objectName1 = "Player_Prototype_001_002(Clone)";
    public Player_Controller_Manager_PP_001 playerControllerManager;



}
