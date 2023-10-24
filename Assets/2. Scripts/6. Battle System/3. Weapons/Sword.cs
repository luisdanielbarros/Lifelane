using System.Collections.Generic;
using UnityEngine;
public class Sword : MonoBehaviour, IWeapon
{
    //List of Base Stats
    public List<baseStat> Stats { get; set; }
    //Weapon Type
    public enumWeapon weaponType { get; set; }

    public void performAttack()
    {
        Debug.Log("performAttack");
    }
}
