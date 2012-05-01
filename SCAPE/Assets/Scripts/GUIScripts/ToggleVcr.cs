using UnityEngine;
using System.Collections;

/// <summary>
/// This script handles the hot key "r" or "R". It pops up the vcr when the r key
/// is hit and pops it down on again hitting the r or R key. Works as toggle
/// </summary>
public class ToggleVcr : MonoBehaviour 
{
    /// <summary>
    /// Initializes the objects required by the script at the later stages.
    /// </summary>
    void Awake()
    {
        // Get the Vcr object
        m_vcr = GameObject.Find("Vcr");

        if (m_vcr == null)
        {
            Debug.LogError("Faied to get Vcr");
            return;
        }

        // This script tries to access the Vcr object in each frame. See explanation below
        m_runnerScript = gameObject.GetComponent<PlayerDemoRunner>();

        if (m_runnerScript == null)
        {
            Debug.LogError("Faied to get runner script");
            return;
        }
    }
     
    // Use this for initialization
	void Start()
    {
        // call change visibility at the start
        changeVisibility();        
    }
	
	// Update is called once per frame
	void Update() 
    {
        // Change the visibility of the Vcr when "r" key is hit
        if(Input.GetKeyDown("r"))
            changeVisibility();
	}

    /// <summary>
    /// This methods handles the enabling and disabling of the Vcr. This scripts also enables 
    /// and disables the runner script as it accesses the Vcr object. If we will disable only the
    /// Vcr then this script will throw a null pointer exception
    /// 
    /// Note - We don't make the Vcr invisible and visible. If we do that then the player may
    /// unknowingly click the Vcr buttons resulting in undesirable recording etc
    /// </summary>
    private void changeVisibility()
    {
        // Toggle the visiblity flag
        m_visible = !m_visible;

        // If visibility flag is true, enable the Vcr else disable it
        if (m_visible)
        {
            m_vcr.SetActiveRecursively(m_visible);
            m_runnerScript.enabled = m_visible;   
        }
        else 
        {
            m_runnerScript.enabled = m_visible;
            m_vcr.SetActiveRecursively(m_visible);
        }        
    }

    // Represnts the visibility status
    private bool m_visible = true;

    // Represents the Vcr object
    GameObject m_vcr = null;

    // Reresents the runner script
    private PlayerDemoRunner m_runnerScript = null;
    
}