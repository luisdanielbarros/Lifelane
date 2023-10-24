using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class battleUnit
{
    //Entity
    private battleEntity unitentity;
    //HUD
    [SerializeField]
    private TextMeshProUGUI unitName;
    [SerializeField]
    private Slider unitHealth;
    [SerializeField]
    private Slider unitAnima;
    [SerializeField]
    private Slider unitSpeed;
    [SerializeField]
    private GameObject unitattacks;
    public GameObject unitAttacks { get { return unitattacks; } }
    //Background Color
    [SerializeField]
    private Image backgroundImage;
    private Color bgNormal = new Color32(157, 213, 215, 128);
    private Color bgDead = new Color32(255, 128, 128, 192);
    private Color bgSelected = new Color32(157, 213, 215, 255);
    //Selection
    private bool isSelected;
    //Battle Simulator Input
    public battleEntity Entity { get { return unitentity; } }
    public battleStats Stats { get { return unitentity.Stats; } }
    public battleMove[] Movepool { get { return unitentity.Movepool; } }
    //Serialize
    public void Serialize(battleEntity _unitEntity)
    {
        unitentity = _unitEntity;
        updateHUD();
    }
    //Update UI
    public void updateHUD()
    {
        //Name & Level
        unitName.SetText("Lvl. " + unitentity.Stats.Level + " " + unitentity.Name);
        //Health
        unitHealth.maxValue = unitentity.Stats.maxHealth;
        unitHealth.value = unitentity.Stats.Health;
        //Background Image
        if (isSelected) backgroundImage.color = bgSelected;
        else if (unitentity.Stats.Health >= 0) backgroundImage.color = bgNormal;
        else backgroundImage.color = bgDead;
        //Anima
        unitAnima.value = unitentity.Stats.maxAnima;
        unitAnima.value = unitentity.Stats.Anima;
    }
    //Take Damage
    public int takeDamage(int _Damage)
    {
        bool Died = unitentity.Stats.takeDamage(_Damage);
        updateHUD();
        if (Died) return unitentity.OnDefeatExperience;
        else return 0;
    }
    //Select
    public void Select()
    {
        isSelected = true;
        updateHUD();
    }
    //Unselect
    public void Unselect()
    {
        isSelected = false;
        updateHUD();
    }
}
