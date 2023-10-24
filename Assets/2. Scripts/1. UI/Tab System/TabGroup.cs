using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    //List of Tab Buttons
    [SerializeField]
    private List<UITabButton> tabButtons;
    //List of Objects to Swap
    [SerializeField]
    private List<GameObject> objectsToSwap;
    //Tab Sprites
    [SerializeField]
    private Color tabIdle, tabHover, tabSelected;
    [SerializeField]
    private bool colorChanges;
    //Selected Tab
    private UITabButton selectedTab;
    public void Subscribe(UITabButton Btn)
    {
        if (tabButtons == null) tabButtons = new List<UITabButton>();
        tabButtons.Add(Btn);
        if (selectedTab == null) OnTabSelected(tabButtons[0]);
        resetTabs();
    }
    public void OnTabEnter(UITabButton Btn)
    {
        resetTabs();
        //Color
        if (selectedTab != null && selectedTab == Btn) return;
        if (colorChanges) Btn.background.color = tabHover;
    }
    public void OnTabExit(UITabButton Btn)
    {
        resetTabs();
    }
    public void OnTabSelected(UITabButton Btn)
    {
        selectedTab = Btn;
        resetTabs();
        if (Btn.background != null && colorChanges) Btn.background.color = tabSelected;
        int i = 0;
        foreach (UITabButton _Btn in tabButtons)
        {
            //Visibility
            if (i >= objectsToSwap.Count || objectsToSwap[i] == null) continue;
            if (Btn == _Btn) objectsToSwap[i].SetActive(true);
            else objectsToSwap[i].SetActive(false);
            i++;
        }
    }
    public void resetTabs()
    {
        foreach (UITabButton _Btn in tabButtons)
        {
            //Color
            if (selectedTab != null && selectedTab == _Btn) continue;
            if (_Btn.background != null && colorChanges) _Btn.background.color = tabIdle;
        }
    }
}
