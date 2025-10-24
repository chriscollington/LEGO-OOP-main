using Unity.LEGO.Behaviours.Actions;
using Unity.LEGO.UI;
using UnityEngine;
using Unity.LEGO.Behaviours;

public class SpeakActionExtension : SpeakAction
{
    [SerializeField]
    public AudioSource AudioReference;


    // Override the Start method to include audio setup.
    protected override void Start()
    {
        base.Start();

        // Additional setup for the audio clip.
        if (AudioReference)
        {


        }
    }

}


