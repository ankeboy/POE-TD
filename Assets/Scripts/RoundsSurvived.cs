using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RoundsSurvived : MonoBehaviour
{
    public Text roundsText;
    void OnEnable()    //similar to start, but called every time the game object is enabled
    {
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()  //basically a for loop
    {

        roundsText.text = "0";
        int round = 0;

        yield return new WaitForSeconds(.7f);

        while (round < PlayerStats.Rounds)
        {
            round++;
            roundsText.text = round.ToString();

            yield return new WaitForSeconds(.05f);  //wait 0.05s
        }
    }
}
