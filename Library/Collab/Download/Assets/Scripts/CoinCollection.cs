using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    public static int NumCoinsRemaining = 0;

    private void Start()
    {
        NumCoinsRemaining += 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        NumCoinsRemaining -= 1;
        gameObject.SetActive(false);

        if (NumCoinsRemaining <= 0)
        {
            //Win Condition
            SceneManager.LoadScene("scene_completed");
        }
    }
}