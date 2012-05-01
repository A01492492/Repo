using UnityEngine;
using System.Collections.Generic;
using BaxaTech.ReviewSystem;
using System;

/// <summary>
/// Instantiate a sphere if the button's (or cube) state 'true'
/// </summary>
public class HotSpotBucket : MonoBehaviour
{
	
	/// <summary> Toggle monitor whose state indicates whether there is water in the pipe to the fountain </summary>
	public ToggleMonitor HotPipe;
	public ToggleMonitor ColdPipe;
	public ToggleMonitor HoodPipe;
	public MoleculeInstantiator moleculeInstantiator;
	
	#region Internal Methods	
		
	internal void Reset() 
    {
		Debug.Log("Resetting Level");
		HotPipe.ToggleState = false;
		ColdPipe.ToggleState = false;
        moleculeInstantiator.CleanSpace();
		mPullyScript.Reset();
        
    }	
	
	#endregion
	
	#region Private methods
	
	private void Awake()
	{
		// Get the water container object(water bucket)
        mWaterContainer = GameObject.Find("WaterContainer");
		
		// Get the reflective water script
        mRefWater = mWaterContainer.transform.Find("ReflectiveWater").gameObject;
		
		// get the water material. This is used to change water to ice and vice versa
        mWaterMaterial = mRefWater.renderer.material;
				
		// Gett the ice material
        mIceMaterial = (Material)Resources.Load("IceMaterial");
		
		// Get the pipe object
        mVapourPipe = GameObject.Find("VapourSourceForPipe");
		
		mWaterScript = (WaterSimple)mRefWater.GetComponent(typeof(WaterSimple));
		
        // Access the vapour emitter object as it has been checked in the above step
        mVapourEmitter = mVapourPipe.transform.Find("VapourEmitter").gameObject;
		
		// Access the water drop script
        mWaterDropScript = (InstantiateWaterDrop)mVapourPipe.GetComponent(typeof(InstantiateWaterDrop));
				
		mColorChangeScript = GameObject.Find("Heater").GetComponent<ColorControl>();
		
		mColorChangeScript.SetColorParams(ColorControl.PrimaryColor.r, 1.5f, 0.001f, false);
		
		
		mPullyScript = GameObject.Find("WaterCollector").GetComponent<PullyMovement>();
		
		
		particleEmitter = gameObject.transform.parent.gameObject.particleEmitter;
		audio = gameObject.transform.parent.gameObject.audio;
		
	}
	
	private void Start()
	{
		GetComponent<ToggleMonitor>().OnToggleChanged += UpdateState;
		
		UpdateState();
		
		SetVapourParticleDefault();
	}
	
	private void Update()
    {
        // If the hot button is clicked, m_hot is set to true
        if(HotPipe.ToggleState)
        {
			SetHot();            
		}
        else
        {
			SetCold();			
        }
		
		if(ColdPipe.ToggleState)
			SetCold();

    }
	
	private void UpdateState()
	{
		DisplayMolecules = GetComponent<ToggleMonitor>().ToggleState;
	}
	
	internal bool DisplayMolecules
    {
		set
		{
			
			if(value)
			{
			
				if(ColdPipe != null && ColdPipe.ToggleState)
				{
					
					moleculeInstantiator.ShowIceMolecule();
				}
				else if(HotPipe != null && HotPipe.ToggleState)
				{
					moleculeInstantiator.ShowMolecule(true);
				}
				else
				{
					
					moleculeInstantiator.ShowMolecule(false);
				}
			}
			else
			{
				moleculeInstantiator.CleanSpace();
			}
		}
    }
	
	private void OnMouseDown()
	{
		if(HotPipe != null && mouseOver)
		{
			GetComponent<ToggleMonitor>().Toggle();
		}
	}
	
	private void OnMouseEnter()
	{
		mouseOver = true;
	}
	
	private void OnMouseExit()
	{
		mouseOver = false;
	}
	
	
	
	private void SetHot()
	{
		// Play the boiling sound if is not being played
        if(!audio.isPlaying)
			audio.Play();
        
        // Slowly increase the number of vapours
        IncreaseVapoursPerFrame();
        
        // Start the vapour after  trigger time has elapsed
        if(Time.time > mTriggerTime && !particleEmitter.emit)
        {
			mColorChangeScript.Go();			
            // Starts the vapours from water
            StartVapoursFromWater();

            // Starts vapours from pipe
            StartVapoursFromPipe();
			
			mWaterDropScript.TurnOnWater();
			mPullyScript.SetHot();
        }
	}
	
	
	private void SetCold()
	{
		if(particleEmitter.emit)
		{
			StopWaterDropFromPipe();
			mPullyScript.SetCold();
			StopVapoursFromWater();
			StopVapoursFromPipe();
			mColorChangeScript.Stop();
			
			// Stop boiling sound if water is not being heated
        	if(audio.isPlaying)
				audio.Stop();
		}
	}
		
	private void ChangeWaterToIce()
    {
		mWaterScript.enabled = false;
        mRefWater.renderer.material = mIceMaterial;
    }

    private void ChangeIceToWater()
    {
        mRefWater.renderer.material = mWaterMaterial;
		mWaterScript.enabled = true;
    }
	
	// Sets the default values of the vapour particles
	private void SetVapourParticleDefault()
	{
		particleEmitter.emit = false;

		particleEmitter.minEnergy = 0.0f;

		particleEmitter.maxEnergy = 0.0f;		
	}
	
	// Starts emitting the vapours
	private void StartVapoursFromWater()
	{
		particleEmitter.emit = true;
		
		// Set the minimum energy of particle
		particleEmitter.minEnergy = ParticleMinEnergy;				
	}
	
	private void IncreaseVapoursPerFrame()
	{
		// Increase the max energy till it reaches the max threshold energy
		if(particleEmitter.maxEnergy < ParticleMaxEnergy)
			particleEmitter.maxEnergy += 0.01f;	
	}
	
	// Stops vapours
	private void StopVapoursFromWater()
	{
		particleEmitter.emit = false;		
	}
	
	// Informs the vapour emitter script to stop vapours
    private void StopVapoursFromPipe()
    {
        mVapourEmitter.particleEmitter.emit = false;
    }
	
	// Informs the vapour emitter script to start vapours
    private void StartVapoursFromPipe()
    {
       mVapourEmitter.particleEmitter.emit = true;  
    }
	
	// Informs the water drop emitter script to stop water drop
    private void StopWaterDropFromPipe()
    {
       mWaterDropScript.TurnOffWater();
    }

    // Informs the water drop emitter script to start water drop
    private void StartWaterDropFromPipe()
    {
        mWaterDropScript.TurnOnWater();
    }
	
	#endregion
	
	// Represent the particle emitter of parent
	private ParticleEmitter particleEmitter;
	
	// Represent the audio source of parent
	private AudioSource audio;
	
	// Represents the trigger time to trigger the vapours
	private float mTriggerTime = 0;	
	
	// Represents the wait time for triggering vapours
    private const int WAIT = 10;

    // Represents the minimum energy of vapour particle
    private float ParticleMinEnergy = .01f;

    // Represents the maximum energy of vapour particle
    private float ParticleMaxEnergy = 10.0f;
	
	// flag to keep track if mouse is over object or not
	private bool mouseOver = false;
	private bool displaySphere = false;
	
	
	// Represents the vapour pipe Game object
    private GameObject mVapourPipe = null;
	
	// Represents the vapour emitter Game object
    private GameObject mVapourEmitter = null;
	
	// Represents the water container game object
    private GameObject mWaterContainer = null;
		
    // Represents the water object
    private GameObject mRefWater = null;
	
	// Represent the water script
    private WaterSimple mWaterScript;	
	
	// Represents the water drop script
    private InstantiateWaterDrop mWaterDropScript;
	
	// Represent heater color change script
    private ColorControl mColorChangeScript;
	
	// Represents the pully movement script
    private PullyMovement mPullyScript = null;
	
	// Represents the water material
    private Material mWaterMaterial = null;

    // Represents the ice material
    private Material mIceMaterial = null;
	
	
}
