using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewWeapon : MonoBehaviour 
{
    private BaseWeapon newWeapon;

	// Use this for initialization
	void Start () 
    {
        CreateWeapon();
	}
	
    public void CreateWeapon()
    {
        newWeapon = new BaseWeapon();
        newWeapon.ItemName = "Gun" + Random.Range(1, 101);
        newWeapon.ItemDescription = newWeapon.ItemName + " is a moderate gun for beginners";
        ChooseWeaponType();
    }

    private void ChooseWeaponType()
    {
        var temp = Random.Range(1, 3);

        switch(temp)
        {
            case 1:
                newWeapon.weaponType = BaseWeapon.WeaponType.HANDGUN;
                break;
            case 2:
                newWeapon.weaponType = BaseWeapon.WeaponType.SHOTGUN;
                break;
            case 3:
                newWeapon.weaponType = BaseWeapon.WeaponType.RIFLE;
                break;
        }
    }
}
