using System.Collections.Generic;
public interface IWeapon
{
    //List of Base Stats
    List<baseStat> Stats { get; set; }
    //Weapon Type
    enumWeapon weaponType { get; set; }
    void performAttack();
}
