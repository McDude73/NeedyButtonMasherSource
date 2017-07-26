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

    public string TwitchManualCode = "Button Masher";
    public string TwitchHelpMessage = "!{0} press 13, !{0} tap 13, !{0} mash 13 (mash the button 13 times)";
    public KMSelectable[] ProcessTwitchCommand(string command)
    {
        command = command.ToLowerInvariant();
        if ( !command.StartsWith("press ") &&
            !command.StartsWith("tap ") && 
            !command.StartsWith("mash ") )
        {
            return null;
        }

        command = command.Substring(command.IndexOf(" ") + 1);
        int presses;
        if (!int.TryParse(command, out presses))
        {
            return null;
        }

        KMSelectable[] interacts = new KMSelectable[presses];
        for (int i = 0; i < presses; i++)
        {
            interacts[i] = Solvebutton;
        }
        return interacts;
    }
}
