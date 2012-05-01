using UnityEngine;
using System.Collections.Generic;
using System;
/// <summary>
/// This Script is the main controller of level one game. 
/// 
/// It controls the
/// 1. Hot spot for water bucket
/// 2. Hot spot for ice
/// 3. Display of relevant moelcules inside the snow globe
/// 4. Display of vapors
/// 5. Display of water drops
/// 6. Controls the pully movement
/// 
/// </summary>
public class Controller : MonoBehaviour
{

    #region Internal methods

    /// <summary>
    /// This method is called when the "cold" hot spot is clicked. 
    /// This sets the water status as cold.
    /// </summary>
    internal void SetCold()
    {
        m_cold = !m_cold;

        // Clean the snow globe space
        CleanSpace();

        // enable water script if no ice is present
        m_waterScript.enabled = !m_cold;

        // If ice
        if (m_cold)
        {
            m_hot = false;

            // change the color of heater
            m_colorChangeScript.Stop();

            // Change water to ice
            ChangeWaterToIce();
            //m_refWater.renderer.material = m_iceMaterial;

            // Stop vapours
            StopVapoursFromWater();
            StopVapoursFromPipe();

            // Indicate to the pully that set cold is clicked. Stops the pully movement
            m_pullyScript.SetCold();

            // Stop water drop
            StopWaterDropFromPipe();
        }
        else
        {
            //m_refWater.renderer.material = m_waterMaterial;
            ChangeIceToWater();
        }
    }


    /// <summary>
    /// This method is called when the "hot" hot spot is clicked. 
    /// This sets the water status as hot
    /// </summary>
    internal void SetHot()
    {
        m_hot = !m_hot;

        // Clean the snow globe space
        CleanSpace();

        if (m_hot)
        {
            // Set the trigger time for vapour and water drop
            m_triggerTime = Time.time + WAIT;
            m_cold = false;

            // change the color of heater. Begin is a toggle, so it sets and unsets
            m_colorChangeScript.Toggle();

            // Change ice to water
            ChangeIceToWater();
            //m_refWater.renderer.material = m_waterMaterial;

            //StartVapoursFromWater();
            //StartVapoursFromPipe();

            StartWaterDropFromPipe();
        }
        else
        {
            StopVapoursFromWater();
            StopVapoursFromPipe();

            StopWaterDropFromPipe();

            // change the color of heater
            m_colorChangeScript.Toggle();
        }

        // Indicate to the pully script that set hot has been called. Starts pully movement
        m_pullyScript.SetHot();
    }

    /// <summary>
    /// This method is called when the "vapour" hot spot is clicked. 
    /// This sets the water status as hot
    /// </summary>
    internal void SetHotSpotVapour()
    {
        Debug.Log("SetHotSpotVapour");

        if (m_hot)
        {
            CleanSpace();

            // If water is boiling then only activate the vapour hot spot
            m_hotSpotVapour = !m_hotSpotVapour;

            if (m_hotSpotVapour)
            {
                // if boolean is false - show vapour molecule
                ShowMolecule(true);
            }
            //else
            //CleanSpace();
        }
        else
            Debug.Log("Cannot set hot spot vapour. No Vapours found");
    }


    internal void SetHotSpotWater()
    {
        m_hotSpotWater = !m_hotSpotWater;
    }

    /// <summary>
    /// This is called when the hot spot "water bucket" is clicked.
    /// </summary>
    internal void SetHotSpotIce()
    {
        if (m_cold)
        {
            Debug.Log("SetHotSpotIce");

            // Toggle the value for hot spot ice as hot spot act as toggle
            m_hotSpotIce = !m_hotSpotIce;

            // if hot spot ice is true, display the ice molecule
            if (m_hotSpotIce)
            {
                // Displays the ice molecule
                ShowIceMolecule();
            }
            else
                CleanSpace();
        }
        else
        {
            Debug.Log("SetHotSpot water");

            // Toggle hot spot water
            m_hotSpotWater = !m_hotSpotWater;

            // If hot spot water is true then display water molecules
            if (m_hotSpotWater)
            {

                // if boolean is true - show ice molecule 
                ShowMolecule(false);
            }
            else
                CleanSpace();
        }

    }

    internal void Reset() 
    {
        CleanSpace();
        m_colorChangeScript.Stop();
        StopVapoursFromPipe();
        StopVapoursFromWater();
        StopWaterDropFromPipe();
        ChangeIceToWater();
    }

    #endregion



    #region Private Unity methods

    /// <summary>
    /// This method is used to initialize all the objects used in the script.
    /// </summary>
	private void Awake()
	{
        // Get the snow globe object
		m_snowBall = GameObject.Find("SnowGlobe");

        // Get the water container object(water bucket)
        m_waterContainer = GameObject.Find("WaterContainer");

        // Get the water drop collector object.
        m_waterCollector = GameObject.Find("WaterCollector");

        // Get the pipe object
        m_vapourPipe = GameObject.Find("VapourSourceForPipe");

        // Gett the ice material
        m_iceMaterial = (Material)Resources.Load("IceMaterial");

        // Get the heater object - TODO chaging the color of heater
        m_heater = GameObject.Find("Heater");

        if (m_snowBall == null || m_waterContainer == null || 
            m_iceMaterial == null || m_vapourPipe == null ||
            m_waterCollector == null || m_heater == null)
        {
            Debug.LogError("ERROR - Some of the game objects are missing");
            return;
        }

        // Check that water is present inside the bucket
        if(m_waterContainer.transform.Find("ReflectiveWater") == null)
        {
            Debug.LogError("ERROR - No Reflective water found");
            return;
        }

        // check for vapour emitter at the end where water drops are collected
        if(m_vapourPipe.transform.Find("VapourEmitter") == null)
        {
            Debug.LogError("ERROR - No vapour emitter found");
            return;
        }

        // Access the vapour emitter object as it has been checked in the above step
        m_vapourEmitter = m_vapourPipe.transform.Find("VapourEmitter").gameObject;

        // Access the water drop script
        m_waterDropScript = (InstantiateWaterDrop)m_vapourPipe.GetComponent(typeof(InstantiateWaterDrop));

        // Get the reflective water script
        m_refWater = m_waterContainer.transform.Find("ReflectiveWater").gameObject;

        // get the water material. This is used to change water to ice and vice versa
        m_waterMaterial = m_refWater.renderer.material;

        // Check if water script is attached. If found get a pointer to it
        if (m_refWater.GetComponent(typeof(WaterSimple)) != null)
            m_waterScript = (WaterSimple)m_refWater.GetComponent(typeof(WaterSimple));
        else
            Debug.LogError("ERROR - No water script found");

        // Get a pointer to pully movement script
        if (m_waterCollector.GetComponent(typeof(PullyMovement)) != null)
            m_pullyScript = (PullyMovement)m_waterCollector.GetComponent(typeof(PullyMovement));
        else
            Debug.LogError("ERROR - No Pully script found");

        if (m_heater.GetComponent(typeof(ColorControl)) != null)
        {
            m_colorChangeScript = (ColorControl)m_heater.GetComponent(typeof(ColorControl));
            m_colorChangeScript.SetColorParams(ColorControl.PrimaryColor.r, 1.5f, 0.001f, false);
        }
        else
            Debug.LogError("ERROR - No ColorControl script found");
        
	}
	
	private void Start()
	{
        // Set the size of vapour particles
		SetVapourParticleDefault();
	}
	
	private void Update()
    {
        // If the hot button is clicked, m_hot is set to true
        if(m_hot)
        {
            // Play the boiling sound if is not being played
            if (!audio.isPlaying)
                audio.Play();
            
            // Slowly increase the number of vapours
            IncreaseVapoursPerFrame();
            
            //Commented by me - TODO Change color of heater
            //IncreaseHeatPerFrame();

            // Start the vapour after  trigger time has elapsed
            if(Time.time > m_triggerTime && !particleEmitter.emit)
            {
                // Starts the vapours from water
                StartVapoursFromWater();

                // Starts vapours from pipe
                StartVapoursFromPipe();
            }
		}
        else
        {
            // Stop boiling sound if water is not being heated
            if(audio.isPlaying)
                audio.Stop();

            //Commented by me - TODO Change color of heater
            //DecreaseHeatPerFrame();
        }

    }

    #endregion

    #region Private methods

    private void ChangeWaterToIce()
    {
        m_refWater.renderer.material = m_iceMaterial;
    }

    private void ChangeIceToWater()
    {
        m_refWater.renderer.material = m_waterMaterial;
    }

    /// <summary>
	/// This cleans up the molecules in the snow globe
	/// </summary>
	private void CleanSpace()
	{
        if (m_molecules == null || m_molecules.Count < 1)
        {
            Debug.LogError("Failed to clear. No Molecules found");
            return;
        }

        // Iterate the molecules list
        for (int i = 0; i < m_molecules.Count; i++)
            Destroy(m_molecules[i]);
        
        // Remove all the references
        m_molecules.Clear();
	}

    /// <summary>
    /// This displays the ice molecule inside the snow globe
    /// </summary>
    private void ShowIceMolecule() 
    {
        // Return if no snow globe is found
        if (m_snowBall == null)
        {
            Debug.Log("ERROR - NO SNOW BALL FOUND");
            return;
        }

        // Return if the m_cold is false
        if (!m_cold)
        {
            Debug.Log("No ice found. Please click on cold button to generate ");
            return;
        }

        //CleanSpace();

        // Represents the ice molecule
        GameObject iceMolecule = null;

        // Offset to display the ice molecule at the center of snow globe.
        // Because the center if the model is slightly away from the actual center
        //Vector3 offset = new Vector3(.001f, .001f, .001f);

        try
        {
            iceMolecule = (GameObject)Instantiate(Resources.Load("IceMolecule"), Vector3.zero, Quaternion.identity);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed To instantiate molecule");
            return;
        }

        // Set snow globe as parent of the ice molecule
        iceMolecule.transform.parent = m_snowBall.transform;

        // Change position of ice molecule to the center of snow globe
        iceMolecule.transform.position = m_snowBall.transform.position;
        
        //iceMolecule.transform.localPosition = offset;
        m_molecules.Add(iceMolecule);
    }

    /// <summary>
    /// This displays the water or vapour molecule depending upon the flag isVapour
    /// </summary>
    /// <param name="isVapour"></param>
	public void ShowMolecule(bool isVapour)
	{
		if(m_snowBall == null)
		{
			Debug.LogError("ERROR - NO SNOW BALL FOUND");
			return;
		}

        if (!m_hot && !m_hotSpotWater)
        {
            Debug.LogError("No vapours found. Please click on hot button to generate vapours");
            return;
        }

        //CleanSpace();
		
        int numberOfMolecules = 0;

        float speed = 0;
        float drag = 0;
        int maxMolecules = 0;

        if (isVapour)
        {
            // Set the speed of molecule to the speed of vapour molecule
            speed = VapourMoleculeSpeed;

            // Set the drag factor
            drag = 0.2f;

            // Set the max number of molecules
            maxMolecules = MaxVapourMolecules;
        }
        else
        {
            // Set the speed of molecule to the speed of water molecule
            speed = WaterMoleculeSpeed;

            // Set the Drag factor
            drag = 1.8f;

            // Set the number of molecules to twice as much as vapour molecule
            maxMolecules = 2 * MaxVapourMolecules;
        }

        // Helper vector to vary the instantiation position of molecules
        Vector3 temp = new Vector3();

        while (numberOfMolecules++ < maxMolecules)
        {
            GameObject molecule = null;
            
            try
            {
                molecule = (GameObject)Instantiate(Resources.Load("Molecule"), Vector3.zero, Quaternion.identity);
                molecule.rigidbody.drag = drag;                
            }
            catch (Exception e)
            {
                Debug.LogError("Failed To instantiate molecule");
                return;
            }

            // Set snow ball as parent of molecule
            molecule.transform.parent = m_snowBall.transform;

            // set temp as center of snow ball
            temp = m_snowBall.transform.position;

            // get a random offset for temp
            temp.x += UnityEngine.Random.Range(-4, 4);
            temp.z += UnityEngine.Random.Range(-4, 4);
            temp.y += UnityEngine.Random.Range(-4, 4);

            //molecule.transform.position = m_snowBall.transform.position;
            molecule.transform.position = temp;
            
            // add the molecule to list of molecules
            m_molecules.Add(molecule);         
        }

        // set speed of each moelcule
        foreach (GameObject molecule in m_molecules)
        {
            m_moleculeMovement = (MoleculeMovement)molecule.GetComponent(typeof(MoleculeMovement));

            if (m_moleculeMovement != null)
                m_moleculeMovement.SetSpeed(speed);
        }
        		
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
	

	private void SetMoleculeSpeed(float speed)
	{
		if(m_moleculeMovement != null)
			m_moleculeMovement.SetSpeed(speed);
	}

    /*
    private void DecreaseHeatPerFrame()
    {
        
    }

    private void IncreaseHeatPerFrame()
    {
        
    }	
    

    private void ChangeIceToWater()
    {
        //renderer.material.color = m_originalContainerColor;
    }
    */

    // Informs the vapour emitter script to start vapours
    private void StartVapoursFromPipe()
    {
       m_vapourEmitter.particleEmitter.emit = true;  
    }

    // Informs the vapour emitter script to stop vapours
    private void StopVapoursFromPipe()
    {
        m_vapourEmitter.particleEmitter.emit = false;
    }

    // Informs the water drop emitter script to stop water drop
    private void StopWaterDropFromPipe()
    {
        if(m_waterDropScript != null)
            m_waterDropScript.TurnOffWater();
    }

    // Informs the water drop emitter script to start water drop
    private void StartWaterDropFromPipe()
    {
        if(m_waterDropScript != null)
            m_waterDropScript.TurnOnWater();
    }

    #endregion

    #region Private members

    // Represents the wait time for triggering vapours
    private const int WAIT = 10;

    // Represents the minimum energy of vapour particle
    private float ParticleMinEnergy = .01f;

    // Represents the maximum energy of vapour particle
    private float ParticleMaxEnergy = 10.0f;

    // Represents the initial vapour molecule speed
    private float VapourMoleculeSpeed = 4f;

    // Represents the initial water molecule speed
    private float WaterMoleculeSpeed = 1f;

    // Represents the max number of vapour molecules
    private int MaxVapourMolecules = 30;

	// Flag representing status of cold button
	private bool m_cold = false;
	
	// Flag representing status of hot button 
	private bool m_hot = false;
	
	// Flags representing  status of hot spot in water container
	private bool m_hotSpotWater = false;
	
	// Flags representing status of hot spot in vapour container
	private bool m_hotSpotVapour = false;

    // Flags representing status of hot spot in ice container
    private bool m_hotSpotIce = false;
	
	//private bool m_showingVapourMolecule = false;	
	
    // Represents the trigger time to trigger the vapours
	private float m_triggerTime = 0;

   	// Represents the snow ball game object
	private GameObject m_snowBall = null;

    // Represents the water container game object
    private GameObject m_waterContainer = null;

    // Represents the water collector object
    private GameObject m_waterCollector = null;

    // Represents the pully movement script
    private PullyMovement m_pullyScript = null;
    
    // Represents the water object
    private GameObject m_refWater = null;

    // Represents the vapour pipe Game object
    private GameObject m_vapourPipe = null;

    // Represents the heater game object
    private GameObject m_heater = null;

    // Represents the vapour emitter Game object
    private GameObject m_vapourEmitter = null;

    // Represents the water material
    private Material m_waterMaterial = null;

    // Represents the ice material
    private Material m_iceMaterial = null;
    
    // Represent the molecule movement script	
	private MoleculeMovement m_moleculeMovement;

    // Represent the water script
    private WaterSimple m_waterScript;

    // Represents the water drop script
    private InstantiateWaterDrop m_waterDropScript;

    private ColorControl m_colorChangeScript;

    // Represent the container for all molecules
    private List<GameObject> m_molecules = new List<GameObject>();

    //private float m_moleculeSpeed = 0f;

    #endregion

}