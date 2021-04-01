using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Money;    //static makes it accessible with only the PlayerStats type. Without requiring an instance or reference.
    public int startMoney = 400;
    public static int Lives;    //static means something which cannot be instantiated. You cannot create an object of a static class and cannot access static members using an object
    public int startLives = 20;
    public static int Rounds;
    void Start()
    {
        Money = startMoney + (PlayerPrefs.GetInt("Money Round Bonus", 0) * 100); //the money is set to the startmoney once, when the script is called
        Lives = startLives + (PlayerPrefs.GetInt("Life Bonus", 0) * 5);
        Rounds = 0;
    }
}
