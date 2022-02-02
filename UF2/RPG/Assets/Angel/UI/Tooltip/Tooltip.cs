using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public TMPro.TextMeshProUGUI headerField;
    public TMPro.TextMeshProUGUI contentField;

    public LayoutElement layoutElement;
    public int characterWrapLimit;

    public RectTransform rectTransform;
    public Animator animator;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(string content, string header = "")
    {
        if (string.IsNullOrEmpty(header))
            headerField.gameObject.SetActive(false);
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }
        contentField.text = content;

        CheckWrap();
    }

    void CheckWrap()
    {
        animator.SetTrigger("Show");

        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        if (headerLength > characterWrapLimit || contentLength > characterWrapLimit)
            layoutElement.enabled = true;
        else
            layoutElement.enabled = false;
    }

    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;

        float pivotX = mousePos.x / Screen.width;
        float pivotY = mousePos.y / Screen.height;

        if (pivotX > .5f)
            pivotX = 1;
        else
            pivotX = 0;

        if (pivotY > .5f)
            pivotY = 1;
        else
            pivotY = 0;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = mousePos;
    }
}
