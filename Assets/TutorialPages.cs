using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPages : MonoBehaviour
{
    public Sprite[] pages;
    public Button nextButton;
    public Button prevButton;
    public GameObject imagePanel;


    int counter = 1;

    public void NextPage()
    {
        counter += 1;
        imagePanel.GetComponent<Image>().sprite = pages[counter - 1];
        prevButton.interactable = true;
        if (counter == pages.Length)
        {
            nextButton.interactable = false;
        }
    }

    public void PrevPage()
    {
        counter -= 1;
        imagePanel.GetComponent<Image>().sprite = pages[counter - 1];
        nextButton.interactable = true;
        if (counter == 1)
        {
            prevButton.interactable = false;
        }
    }
}
