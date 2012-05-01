using UnityEngine;
using System.Collections;

/// <summary>
/// This scripts plays the sound of water drop hitting a surface.
/// It plays this sound when water drop hits the surface of water collector
/// 
/// </summary>
public class PlayAudioOnCollision : MonoBehaviour 
{
    // Represents the audio file
    public AudioClip clip;

    void OnCollisionEnter(Collision collision)
    {
        // Play sound
        if (clip != null)
        {
            // Play the sound only once
            audio.PlayOneShot(clip);
        }
    }
	
}
