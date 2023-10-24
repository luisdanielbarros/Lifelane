using System;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class baseStat
{
    //Name
    public statsList statName;
    //Base Value
    [SerializeField]
    public int baseValue;
    //List of Bonuses
    public List<statBonus> statBonuses { get; set; }
    //Derived Value
    private int derivedvalue;
    public int derivedValue { get { return derivedvalue; } }
    //Twice Derived Value
    private int twicederivedvalue;
    public int twiceDerivedValue { get { return twicederivedvalue; } }
    //Stat Function Variables
    private int statFixedBase = 8;
    private int statBaseMultiplier = 4;
    //Level
    private int level;
    public int Level { set { level = value; calcDerivedValue(); } }
    //Constructor
    public baseStat(statsList _statName, int _baseValue)
    {
        statName = _statName;
        baseValue = _baseValue;
        statBonuses = new List<statBonus>();
        level = 1;
        calcDerivedValue();
    }
    //Add Bonus
    public void addStatBonus(statBonus _statBonus)
    {
        statBonuses.Add(_statBonus);
        calcDerivedValue();
    }
    //Remove Bonus
    public void removeStatBonus(statBonus _statBonus)
    {
        statBonuses.Remove(statBonuses.Find(currentStatBonus => currentStatBonus.bonusValue == _statBonus.bonusValue));
        calcDerivedValue();
    }
    //Calc Derived Value
    private void calcDerivedValue()
    {
        float convertedBaseValue = (float)Math.Round(baseValue * 0.01, 2);
        derivedvalue = (int)((level * (statBaseMultiplier * convertedBaseValue)) + statFixedBase);
        twicederivedvalue = derivedvalue;
        calcTwiceDerivedValue();
    }
    //Calc Twice Derived Value
    private void calcTwiceDerivedValue()
    {
        statBonuses.ForEach(currentStatBonus => twicederivedvalue += currentStatBonus.bonusValue);
    }
}
