using UnityEngine;
using System.Collections;

/// <summary>
/// This script handles the movement of the pully system which comprises of the door and water collector
/// 
/// The doors starts to open when water starts dripping from the vapour pipe
/// </summary>

public class PullyMovement : MonoBehaviour
{
	#region Internal Methods
		
	/// <summary>
    /// This method is called by the Controller to signal heating of water
    /// </summary>
	internal void SetHot()
	{
        m_hot = !m_hot;
        m_triggerTime = Time.time + WAIT;
	}

    /// <summary>
    /// This method is called by the Controller to signal cooling of water
    /// </summary>
	internal void SetCold()
    {
        m_hot = false;
        Debug.Log("SetCold");
    }
	
	internal void Reset()
	{
		Debug.Log("IN RESET PULLEY");
		m_waterTank.transform.position = m_collectorOrig;
		m_mainDoor.transform.position = m_doorOrig;
		
		AssignPositions();
		m_keepGoing = true;
	}
	
	#endregion
	
	#region Private methods
	
    /// <summary>
    /// This method initilaizes all the required objects
    /// </summary>
	private void Awake()
	{
        // get the water tank object.
		m_waterTank = GameObject.Find("WaterCollector");

        // Get the main door object
		m_mainDoor = GameObject.Find("Door");

        if (m_waterTank == null || m_mainDoor == null)
        {
            Debug.LogError("Water tank or door is missing");
            return;
        }
        
        // Get the initial position of the door. Door moves in the upward direction
        m_doorOrig = m_mainDoor.transform.position;
		
		// Get the initial position of the water tank. Tank moves in the downward direction
        m_collectorOrig = m_waterTank.transform.position;
		
		AssignPositions();
		
	}
	
	// Use this for initialization
	private void Start () 
	{
		
	}
	
	// Update is called once per frame
	private void Update ()
    {
        // Check if trigger time has passed
        if (Time.time > m_triggerTime)
        {
            // If the water flag is hot
            if (m_hot)
            {
				if (m_moveUp.y < MAX_HEIGHT && m_keepGoing) 
				{
	                // Add the offset to the door's postion
	                m_moveUp.y += OFFSET;
	
					m_mainDoor.transform.position = m_moveUp;
				}
				else
                    m_keepGoing = false;
				
                // Check if max depth has been reached. Continue if not
                if (m_moveDown.y > MAX_DEPTH && m_keepGoing) 
				{
					// Subtract the offset to the door's postion
                	m_moveDown.y -= OFFSET;
					
                    m_waterTank.transform.position = m_moveDown;
				}
                else
                    m_keepGoing = false;
            }
        }
	}
	
	private void AssignPositions()
	{
		m_moveUp = new Vector3(m_doorOrig.x, m_doorOrig.y, m_doorOrig.z);
		m_moveDown = new Vector3(m_collectorOrig.x, m_collectorOrig.y, m_collectorOrig.z);
	}
	
	#endregion
	
    // Represents the water status flag 
	private bool m_hot = false;
    private bool m_keepGoing = true;

    private float m_triggerTime = 0f;
    private const float WAIT = 20f;
	private const float OFFSET = 0.005f;
    private const float MAX_HEIGHT = 10f;
    private const float MAX_DEPTH = -5f;

    private Vector3 m_moveUp, m_moveDown, m_doorOrig, m_collectorOrig;
	
	private GameObject m_mainDoor = null;
	private GameObject m_waterTank = null;
}
