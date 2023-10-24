using TMPro;
using UnityEngine;
public class collectiblesItemViewerMenu : MonoBehaviour
{
    //UI
    [SerializeField]
    private TextMeshProUGUI Body;
    //Pagination
    private int pageCount;
    private int currentPage;
    public void resetPagination()
    {
        pageCount = Body.GetTextInfo(Body.text).pageCount;
        currentPage = 1;
        Body.pageToDisplay = currentPage;
    }
    public void previousPage()
    {
        if (currentPage > 1)
        {
            currentPage--;
            Body.pageToDisplay = currentPage;
        }
    }
    public void nextPage()
    {
        if (currentPage < pageCount)
        {
            currentPage++;
            Body.pageToDisplay = currentPage;
        }
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
