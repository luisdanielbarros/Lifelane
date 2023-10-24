using System;
using System.Collections.Generic;

public class battleStats
{
    //Experience
    private int experience;
    public int Experience { get { return experience; } set { experience += value; calcLevel(); } }
    //Level
    private int level;
    public int Level { get { return level; } }
    //List of Stats
    private List<baseStat> stats = new List<baseStat>();
    public List<baseStat> Stats
    {
        get
        {
            return stats;
        }
    }
    //Public Stats
    #region Public Stats
    //Max Health
    private int maxhealth;
    public int maxHealth { get { return maxhealth; } }
    //Health
    private int health;

    public int Health { get { return health; } }
    //Max Anima
    private int maxanima;

    public int maxAnima { get { return maxanima; } }
    //Anima

    private int anima;

    public int Anima { get { return anima; } }
    //Strength

    private int strength;

    public int Strength { get { return strength; } }
    //Toughness

    private int toughness;

    public int Toughness { get { return toughness; } }
    //Channelling

    private int channelling;

    public int Channelling { get { return channelling; } }
    //Sensitivity

    private int sensitivity;

    public int Sensitivity { get { return sensitivity; } }
    //Speed

    private int speed;

    public int Speed { get { return speed; } }
    //Intuition

    private int intuition;

    public int Intuition { get { return intuition; } }
    //Damage

    private int damage;

    //Exhaustion

    private int exhaustion;

    #endregion
    //Constructor
    public battleStats(int _Health, int _Anima, int _Strength, int _Toughness, int _Channelling, int _Sensitivity, int _Speed, int _Intuition, int _Experience)
    {
        stats.Add(new baseStat(statsList.Health, _Health));
        stats.Add(new baseStat(statsList.Anima, _Anima));
        stats.Add(new baseStat(statsList.Strength, _Strength));
        stats.Add(new baseStat(statsList.Toughness, _Toughness));
        stats.Add(new baseStat(statsList.Channelling, _Channelling));
        stats.Add(new baseStat(statsList.Sensitivity, _Sensitivity));
        stats.Add(new baseStat(statsList.Speed, _Speed));
        stats.Add(new baseStat(statsList.Intuition, _Intuition));
        experience = _Experience;
        calcLevel();
    }
    #region Health, Damage, Anima & Exhaustion

    public bool takeDamage(int _Damage)
    {
        if (_Damage <= 0) return false;
        damage += _Damage;
        return calcHealth();
    }
    public void takeExhaustion(int _Exhaustion)
    {
        if (_Exhaustion <= 0) return;
        else exhaustion += _Exhaustion;
        calcAnima();
    }
    //Calc Health
    private bool calcHealth()
    {
        health = maxhealth - damage;
        if (health <= 0) return Die();
        else return false;
    }
    //Calc Anime
    private void calcAnima()
    {
        anima = maxanima - exhaustion;
    }
    //Die
    private bool Die()
    {
        return true;
    }
    #endregion
    //CalcLevel
    private void calcLevel()
    {
        //Experience Function Variables
        float Exponent = 1.125f;
        int baseExperience = 1000;
        //Level Up Algorighm
        int currLevel = 1;
        int currExperience = experience;
        bool leveledUp = true;
        while (leveledUp)
        {
            int experienceNeeded = (int)Math.Round(baseExperience * Math.Pow(currLevel, Exponent));
            if (currExperience >= experienceNeeded)
            {
                currExperience -= experienceNeeded;
                currLevel++;
                leveledUp = true;
            }
            else leveledUp = false;
        }
        level = currLevel;
        foreach (baseStat _baseStat in stats)
        {
            //Level
            _baseStat.Level = level;
            //Health
            if (_baseStat.statName == statsList.Health)
            {
                maxhealth = _baseStat.twiceDerivedValue;
                calcHealth();
            }
            //Anima
            else if (_baseStat.statName == statsList.Anima)
            {
                maxanima = _baseStat.twiceDerivedValue;
                calcAnima();
            }
            //Strength
            else if (_baseStat.statName == statsList.Strength)
            {
                strength = _baseStat.twiceDerivedValue;
            }
            //Toughness
            else if (_baseStat.statName == statsList.Toughness)
            {
                toughness = _baseStat.twiceDerivedValue;
            }
            //Channelling
            else if (_baseStat.statName == statsList.Channelling)
            {
                channelling = _baseStat.twiceDerivedValue;
            }
            //Sensitivity
            else if (_baseStat.statName == statsList.Sensitivity)
            {
                sensitivity = _baseStat.twiceDerivedValue;
            }
            //Speed
            else if (_baseStat.statName == statsList.Speed)
            {
                speed = _baseStat.twiceDerivedValue;
            }
            //Intuition
            else if (_baseStat.statName == statsList.Intuition)
            {
                intuition = _baseStat.twiceDerivedValue;
            }
        }
    }
    #region Stat Bonus
    //Add Stat Bonus
    public void addStatBonus(List<baseStat> _baseStats)
    {
        foreach (baseStat _baseStat in _baseStats)
        {
            stats.Find(currentBaseStat => currentBaseStat.statName == _baseStat.statName).addStatBonus(new statBonus(_baseStat.baseValue));
        }
    }
    //Remove Stat Bonus
    public void removeStatBonus(List<baseStat> _baseStats)
    {
        foreach (baseStat _baseStat in _baseStats)
        {
            stats.Find(currentBaseStat => currentBaseStat.statName == _baseStat.statName).removeStatBonus(new statBonus(_baseStat.baseValue));
        }
    }
    #endregion
}
