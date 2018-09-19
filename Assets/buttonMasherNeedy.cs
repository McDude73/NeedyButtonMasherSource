using UnityEngine;

public class buttonMasherNeedy : MonoBehaviour
{
    public KMAudio Audio;
    public KMSelectable Solvebutton;
    public TextMesh displayTxt;
    public KMNeedyModule Module;

    int rngTxt = 0;
    bool isActive = false;

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
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        Solvebutton.AddInteractionPunch(0.5f);
        if (isActive)
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
                    isActive = false;
                }
            }
        }
        return false;
    }

    protected void OnNeedyActivation()
    {
        isActive = true;
        rngTxt = Random.Range(25, 46);
        counter = rngTxt;
        displayTxt.text = counter.ToString();
        if (TwitchPlaysActive)
            Module.SetNeedyTimeRemaining(35);
    }

    protected void OnNeedyDeactivation()
    {
        rngTxt = 0;
        counter = rngTxt;
        displayTxt.text = counter.ToString();
        displayTxt.text = "__";
        isActive = false;
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
        isActive = false;
    }

#pragma warning disable 414
#pragma warning disable 649
#pragma warning disable IDE0044 // Add readonly modifier
    private readonly string TwitchHelpMessage = @"Press the button 20 times with “!{0} press 20”.";
    private bool TwitchPlaysActive;
#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning restore 414
#pragma warning restore 649

    KMSelectable[] ProcessTwitchCommand(string command)
    {
        command = command.ToLowerInvariant();
        if (!command.StartsWith("press ") &&
             !command.StartsWith("tap ") &&
             !command.StartsWith("push ") &&
             !command.StartsWith("mash "))
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