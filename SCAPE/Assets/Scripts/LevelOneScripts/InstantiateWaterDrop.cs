using UnityEngine;
using System.Collections;

/// <summary>
/// This script instantiate water drops from the end of the vapour pipe.
/// This script ic called from the Controller
/// </summary>
public class InstantiateWaterDrop : MonoBehaviour 
{
    // Represents the water drop model. Drag and drop water drop model over it in the editor
    public GameObject waterDrop;

    // Initialize all the objects required in the script
    void Awake() 
    {
        // Get the point of instantiation as the position of water drop emitter
        m_instantiatePosition = transform.position;        

        // change the y co ordinate so that the drop does not appear to come from inside of the water drop emitter
        m_instantiatePosition.y -= 2.3f; 
    }

	// Use this for initialization
	void Start() 
    {
	    
	}
	
	// Update is called once per frame
	void Update() 
    {
        // Wait for trigger time
        if(Time.time > m_triggerTime)
        {
            if (m_dripping)
            {
                // instantiate the water drop
                m_waterDrop = (GameObject)Instantiate(waterDrop, Vector3.zero, Quaternion.identity);
                m_waterDrop.transform.parent = transform;
                m_waterDrop.transform.position = m_instantiatePosition;

                // wait for some time before instantiting another drop
                m_triggerTime = Time.time + 2;
            }
        }
	}

    // set the dripping status to true
    internal void TurnOnWater()
    {
        Debug.Log("TurnOnWater");

        m_dripping = !m_dripping;

        // Set the trigger time
        m_triggerTime = Time.time + WAIT;
    }

    // set the dripping status to false
    internal void TurnOffWater()
    {
        Debug.LogError("TurnOffWater");
        m_dripping = false;      
    }

    // Represents the flag representing the dripping status
    private bool m_dripping = false;

    // Represents the trigger time
    private float m_triggerTime = 0f;

    // Represents the wait time. Drops start after this time has passed
    private const float WAIT = 20f;

    // Represents the instantiate position of water drop
    private Vector3 m_instantiatePosition = new Vector3();

    // Represents the water drop
    private GameObject m_waterDrop = null;
}
