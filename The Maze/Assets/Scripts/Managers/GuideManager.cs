using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuideManager : MonoBehaviour
{
    [SerializeField]
    List<TMP_Text> page;
    int currentPage;

    public void SetUp()
    {
        page[0].enabled = true;
        if (page[0].GetComponentInChildren<UnityEngine.UI.Image>() != null) page[0].GetComponentInChildren<UnityEngine.UI.Image>().enabled = true;
        currentPage = 0;
        for (int i = 1; i < page.Count; i++)
        {
            page[i].enabled = false;
            if (page[i].GetComponentInChildren<UnityEngine.UI.Image>() != null) page[i].GetComponentInChildren<UnityEngine.UI.Image>().enabled = false;
        }
    }

    public void NextPage()
    {
        if (currentPage < page.Count - 1)
        {
            if (page[currentPage].GetComponentInChildren<UnityEngine.UI.Image>() != null) page[currentPage].GetComponentInChildren<UnityEngine.UI.Image>().enabled = false;
            page[currentPage++].enabled = false;
            page[currentPage].enabled = true;
            if (page[currentPage].GetComponentInChildren<UnityEngine.UI.Image>() != null) page[currentPage].GetComponentInChildren<UnityEngine.UI.Image>().enabled = true;
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            if (page[currentPage].GetComponentInChildren<UnityEngine.UI.Image>() != null) page[currentPage].GetComponentInChildren<UnityEngine.UI.Image>().enabled = false;
            page[currentPage--].enabled = false;
            page[currentPage].enabled = true;
            if (page[currentPage].GetComponentInChildren<UnityEngine.UI.Image>() != null) page[currentPage].GetComponentInChildren<UnityEngine.UI.Image>().enabled = true;
        }
    }
}
