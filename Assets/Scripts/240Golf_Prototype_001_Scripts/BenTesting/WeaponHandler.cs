using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{

    Animator animator;

    [System.Serializable]
    public class UserSettings
    {
        public Transform rightHand;
        public Transform DriverUnequipSpot;
    }
    [SerializeField]
    public UserSettings userSettings;

    [System.Serializable]
    public class Animations
    {
        public string weaponTypeInt = "WeaponType";
        public string aimingBool = "Aiming";
    }
    [SerializeField]
    public Animations animations;

    public Weapon currentWeapon;
    public List<Weapon> weaponsList = new List<Weapon>();
    public int maxWeapons = 3;
    bool aim;
    int weaponType;
    bool settingWeapon;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
		if (currentWeapon) {
			currentWeapon.SetEquipped(true);
			currentWeapon.SetOwner(this);
			AddWeaponToList(currentWeapon);
		}

		if (weaponsList.Count > 0) {
			for ( int i = 0; i < weaponsList.Count; i++) {
				if (weaponsList[i] != currentWeapon) {
					weaponsList[i].SetEquipped(false);
					weaponsList[i].SetOwner(this);
				}
			}
		}

		Animate();

    }

    // Animates the character
    void Animate()
    {
        if (!animator)
        {
            return;
        }

        animator.SetBool(animations.aimingBool, aim);
        animator.SetInteger(animations.weaponTypeInt, weaponType);

        if (!currentWeapon)
        {
            weaponType = 0;
            return;
        }

        switch (currentWeapon.weaponType)
        {
            case Weapon.WeaponType.Driver:
                weaponType = 1;
                break;
        }
    }

    // Adds a weapon to the weaponsList
    void AddWeaponToList(Weapon weapon)
    {
        if (weaponsList.Contains(weapon))
        {
            return;
        }

        weaponsList.Add(weapon);
    }

    // Puts the finger on the trigger and asks if we pull it
    public void FingerOnTrigger(bool pulling)
    {
        if (!currentWeapon)
        {
            return;
        }

        currentWeapon.PullTrigger(pulling);

    }

    // Sets aim bool
    public void Aim(bool aiming)
    {
        aim = aiming;
    }

    // Drop current weapon
    public void DropCurrentWeapon()
    {
        if (!currentWeapon)
        {
            return;
        }

        currentWeapon.SetEquipped(false);
        currentWeapon.SetOwner(null);
        weaponsList.Remove(currentWeapon);
    }

    // Switches to the next weapon
    public void SwitchWeapons()
    {
        if (settingWeapon)
        {
            return;
        }

        if (currentWeapon)
        {
            int currentWEaponIndex = weaponsList.IndexOf(currentWeapon);
            int nextWeaponIndex = (currentWEaponIndex + 1) % weaponsList.Count;

            currentWeapon = weaponsList[nextWeaponIndex];
        }
        else
        {
            // if no current weapon, set the weapon as first weapon in list
            currentWeapon = weaponsList[0];
        }

        settingWeapon = true;
        StartCoroutine(StopSettingWeapon());
    }

    // Stops swapping weapons
    IEnumerator StopSettingWeapon()
    {
        yield return new WaitForSeconds(0.7f);
        settingWeapon = false;
    }

    void OnAnimatorIK()
    {

        if (!animator)
        {
            return;
		}


            if (currentWeapon)
            {
                
                if (currentWeapon.userSettings.leftHandIKTarget && weaponType == 1 && !settingWeapon)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                    Transform target = currentWeapon.userSettings.leftHandIKTarget;
                    Vector3 targetPos = target.position;
                    Quaternion targetRot = target.rotation;
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, targetPos);
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, targetRot);
                }
                else
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
                }
            }
    }


}
