using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class LivesUI : MonoBehaviour
{
    public static LivesUI instance;
    public GameObject liveLostFlickerUI;
    public Text livesText;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of LivesUI found!");
        }
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        livesText.text = PlayerStats.Lives.ToString() + " LIVES";
    }

    public void ScreenFlash()
    {
        StartCoroutine(SetFalseAferDelay(0.05f));   //it doesnt actually stop the code from running
    }

    IEnumerator SetFalseAferDelay(float seconds)
    {
        //you can replace 3 with the amount of seconds to wait
        //for a time like 1.2 seconds, use 1.2f (to show it's a float)
        liveLostFlickerUI.SetActive(true);
        yield return new WaitForSeconds(seconds);
        liveLostFlickerUI.SetActive(false);     //has to be in here otherwise the code runs without waiting
    }
}
