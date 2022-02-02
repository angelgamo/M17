using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    #region Singleton
    private static TooltipSystem current;

    private void Awake()
    {
        current = this;
        current.tooltip.gameObject.SetActive(false);
    }
    #endregion

    public Tooltip tooltip;

    public static void Show(string content, string header="")
    {
        current.tooltip.SetText(content, header);
        current.tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        if (current != null)
            current.tooltip.gameObject.SetActive(false);
    }
}
