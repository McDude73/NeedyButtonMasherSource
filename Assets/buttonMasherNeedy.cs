using UnityEngine;
using System.Collections;
using System;

public class buttonMasherNeedy : MonoBehaviour {

    public KMAudio KMAudio;
    public KMSelectable Solvebutton;
    public TextMesh displayTxt;
    int rngTxt = 0;
    int isActive = 0;

    protected int counter;

    void Awake()
    {
        GetComponent<KMNeedyModule>().OnNeedyActivation += OnNeedyActivation;
        GetComponent<KMNeedyModule>().OnNeedyDeactivation += OnNeedyDeactivation;
        Solvebutton.OnInteract += Solve;
        GetComponent<KMNeedyModule>().OnTimerExpired += OnTimerExpired;
    }

    protected bool Solve()
    {
        KMAudio.HandlePlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        Solvebutton.AddInteractionPunch(0.5f);
        if (isActive == 1)
        {
            if (counter > -1)
            {
                counter--;
                displayTxt.text = counter.ToString();
                if (counter == -1)
                {
                    GetComponent<KMNeedyModule>().OnStrike();
                    displayTxt.text = "__";
                    GetComponent<KMNeedyModule>().OnPass();
                    isActive = 0;
                    return false;
                }
                return false;
            }
            return false;
        }
        return false;
    }

    protected void OnNeedyActivation()
    {
        isActive = 1;
        System.Random rnd = new System.Random();
        rngTxt = rnd.Next(25, 46);
        counter = rngTxt;
        displayTxt.text = counter.ToString();
    }

    protected void OnNeedyDeactivation()
    {
        rngTxt = 0;
        counter = rngTxt;
        displayTxt.text = counter.ToString();
        displayTxt.text = "__";
        isActive = 0;
    }

    protected void OnTimerExpired()
    {
        if (counter == 0)
        {
            GetComponent<KMNeedyModule>().OnPass();
            displayTxt.text = "__";
        }
        else
        {
            GetComponent<KMNeedyModule>().OnStrike();
            displayTxt.text = "__";
        }
        isActive = 0;
    }
}