using UnityEngine;
public class battleMenu : MonoBehaviour
{
    public void Attack()
    {
        //battleSystem.Instance.Attack();
    }
    public void Inventory()
    {
        battleSystem.Instance.Inventory();
    }
    public void Team()
    {
        battleSystem.Instance.Team();
    }
    public void Run()
    {
        battleSystem.Instance.Run();
    }
    public void UIClick()
    {
        AudioManager.Instance.UIClick();
    }
    public void UIHover()
    {
        AudioManager.Instance.UIHover();
    }
}
