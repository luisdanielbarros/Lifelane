using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class automaticScroll : MonoBehaviour
{
    //UI
    [SerializeField]
    private ScrollRect scrollBox;
    [SerializeField]
    private RectTransform scrollContainer;
    //Scroll Speed
    private float idleSpeed = 250f, pressingSpeed = 2000f;
    //Current Scroll
    private Vector2 currentScroll;
    //Scroll Ended
    private bool reachedEnd = false;
    void Start()
    {
        currentScroll = new Vector2(0, scrollContainer.rect.y + 540);
    }
    void Update()
    {
        if (reachedEnd) return;
        StartCoroutine("Scroll");
    }
    IEnumerator Scroll()
    {
        yield return new WaitForSeconds(1f);
        //Scroll Speed
        float scrollSpeed = idleSpeed;
        if (Input.GetKey(KeyCode.Escape)) scrollSpeed = pressingSpeed;
        var newScroll = new Vector2(0f, Time.deltaTime * scrollSpeed);
        //Current Scroll
        currentScroll += newScroll;
        scrollBox.content.localPosition = currentScroll;
        //Scroll Ended
        if (currentScroll.y >= scrollContainer.rect.height + scrollContainer.rect.y)
        {
            reachedEnd = true;
            scrollBox.scrollSensitivity = 50f;
            yield break;
        }
    }
}
