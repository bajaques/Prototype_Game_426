using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{

    /*not sure if we should make this a public interfase or class?

    Interfaces are like class templates that defines required methods and functions. 
    If class implements specific interface, it has to implement all methods and functions included in this interface. 
    There are also other things you need to remember:

    You can not create an instance of interface.
    Class can inherit from only one base class and can implement multiple interfaces.
    Interface does not contain implementations of its methods.
    */

    /*public enum DamageType
    {
        //NotInitialized,
        Bullet,
        Missile,
        ArmorStealing,
        GrapplingHook,
        AoE,
        Disruption,
        Rock,
        EnergyBarrier
    }
    public interface IShootable
    {
        void Hit(GameObject source, float damageAmount, DamageType damageType, Vector3 worldPos);

        bool ShouldHighlight();
    }*/

    public State()
    {

    }

    public virtual void RunState()
    {

    }
}
