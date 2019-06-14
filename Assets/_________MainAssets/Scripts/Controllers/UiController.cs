using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UiController : MonoBehaviour {

    [SerializeField] TMP_Text win_TMP;
    [SerializeField] TMP_Text gameOver_TMP;

    private Dictionary<string, Action<float>> eventDictionary = new Dictionary<string, Action<float>>();


    public void StartListening(string eventName, Action<float> listener)
    {

        Action<float> thisEvent;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
        }
        else
        {
            thisEvent += listener;
            eventDictionary.Add(eventName, thisEvent);
        }
    }

    public void StopListening(string eventName, Action<float> listener)
    {
        Action<float> thisEvent;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
        }
    }

    public void TriggerEvent(string eventName, float h)
    {
        Action<float> thisEvent = null;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(h);
        }
    }


    public void WinFade(float ammount)
    {
        StartCoroutine(DoFadeWin(ammount));
    }

    public void GameOverFade(float ammount)
    {
        StartCoroutine(DoFadeGameOver(ammount));
    }

    private IEnumerator DoFadeWin(float ammount)
    {
        if (ammount > 0)
        {
            while (win_TMP.alpha < 1)
            {
                win_TMP.alpha += Time.deltaTime * 2;
                yield return null;
            }
        }
        else
        {
            while (win_TMP.alpha > 0)
            {
                win_TMP.alpha -= Time.deltaTime * 2;
                yield return null;
            }
        }

    }

    private IEnumerator DoFadeGameOver(float ammount)
    {
        if (ammount > 0)
        {
            while (gameOver_TMP.alpha < 1)
            {
                gameOver_TMP.alpha += Time.deltaTime * 2;
                yield return null;
            }
        }
        else
        {
            while (gameOver_TMP.alpha > 0)
            {
                gameOver_TMP.alpha -= Time.deltaTime * 2;
                yield return null;
            }
        }

    }

}
