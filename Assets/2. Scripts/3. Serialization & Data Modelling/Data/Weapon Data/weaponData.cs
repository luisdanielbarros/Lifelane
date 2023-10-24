using System;
using UnityEngine;

[Serializable]
public class weaponData : itemData
{
    //Range
    private itemRange range;
    public itemRange Range { get { return range; } }
    //Attack
    private int attack;
    public int Attack { get { return attack; } }
    //Defense
    private int defense;
    public int Defense { get { return defense; } }
    //Speed
    private int speed;
    public int Speed { get { return speed; } }
    public weaponData(string _itemId, string _Name, string _Description, Sprite _itemIcon, itemDataType _Type, itemRarity _Rarity, int _slotSpace, itemRange _Range, int _Attack, int _Defense, int _Speed) : base(_itemId, _Name, _Description, _itemIcon, _Type, _Rarity, _slotSpace)
    {
        range = _Range;
        attack = _Attack;
        defense = _Defense;
        speed = _Speed;
    }
}
