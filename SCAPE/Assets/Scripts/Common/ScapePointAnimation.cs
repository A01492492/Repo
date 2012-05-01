using UnityEngine;
using System.Collections;

public class ScapePointAnimation : MonoBehaviour
{
	
	// Use this for initialization
	void Start () 
    {
		// Get some of the objects we need later.
		// This is often done in a script's Start function. That way, we've got all our initialization code in one place, 
		// And can simply count on the code being fine.
		
		mEmitterActive = transform.Find("RSParticlesActive").GetComponent<ParticleEmitter>();
		mEmitterInactive = transform.Find("RSParticlesInactive").GetComponent<ParticleEmitter>();
		mEmitterRespawn1 = transform.Find("RSParticlesRespawn1").GetComponent<ParticleEmitter>();
		mEmitterRespawn2 = transform.Find("RSParticlesRespawn2").GetComponent<ParticleEmitter>();
		mEmitterRespawn3 = transform.Find("RSParticlesRespawn3").GetComponent<ParticleEmitter>();
	
		mLight = transform.Find("RSSpotlight").GetComponent<Light>();	
		
		SetActive();
		
	}
	
	
	
	private void SetActive() 
	{
		mEmitterActive.emit = true;
		mEmitterInactive.emit = false;
	    mLight.intensity = 1.5f;	
	
		audio.Play();		// start playing the sound clip assigned in the inspector
	}

	private void SetInactive() 
	{
		mEmitterActive.emit = false;
		mEmitterInactive.emit = true;
		mLight.intensity = 1.5f;		
	
		audio.Stop();	// stop playing the active sound clip.			
	}
	
	

	void FireEffect () 
	{
		// Launch all 3 sets of particle systems.
		mEmitterRespawn1.Emit();
		mEmitterRespawn2.Emit();
		mEmitterRespawn3.Emit();
	
		mLight.intensity = 3.5f;
			
				
		//yield new WaitForSeconds(2);
		
		mLight.intensity = 2.0f;
	}
	
	
	private float SFXVolume;
	
	// references for the various particle emitters...
	private ParticleEmitter mEmitterActive;
	private ParticleEmitter mEmitterInactive;
	private ParticleEmitter mEmitterRespawn1;
	private ParticleEmitter mEmitterRespawn2;
	private ParticleEmitter mEmitterRespawn3;

	// ...and for the light:
	private Light mLight;
	
}