using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;
using System;

public class GUIController : MonoBehaviour
{
    #region Private methods

	void Awake()
	{
		m_snowBall = GameObject.Find("SnowGlobe");
        
        if (m_snowBall == null)
        {
            Debug.LogError("ERROR - Some of the game objects not found found");
            return;
        }        
	}
	
    // Use this for initialization
	private void Start () 
    {
        m_heatSlider = gameObject.AddComponent<Slider>();

        m_heatSlider.CreateSlider("Heat - ",
                                new Rect(Screen.width - 190, (Screen.height - 65), 100, 30),
                                new Rect(Screen.width - 100, (Screen.height - 60), 100, 30),
                                100);

        m_pressureSlider = gameObject.AddComponent<Slider>();

        m_pressureSlider.CreateSlider("Pressure - ",
                                new Rect(Screen.width - 190, (Screen.height - 35), 100, 30),
                                new Rect(Screen.width - 100, (Screen.height - 30), 100, 30),
                                100);

        m_heatSlider.PropertyChanged += new PropertyChangedEventHandler(EventHandler);
	}

    private void OnGUI() 
    {
        if (GUI.Button(new Rect(10, 10, 50, 50), "Start"))
            ShowMolecule(1);

        if (GUI.Button(new Rect(60, 10, 50, 50), "Stop"))
            CleanSpace();
    }
    
    private void EventHandler(object sender ,PropertyChangedEventArgs e)
    {
		Debug.Log("Property changed event received");
		
        if(e.PropertyName == "SliderValue")
            IncreaseVelocity((m_heatSlider.SliderValue) / 10);

    }

    private void IncreaseVelocity(int p)
    {
        if (m_molecules == null || m_molecules.Count < 1)
        {
            Debug.LogError("Failed to clear. No Molecules found");
            return;
        }

        for (int i = 0; i < m_molecules.Count; i++)
        {
            m_gasMovementScript = (MoleculeMovement)m_molecules[i].GetComponent(typeof(MoleculeMovement));

            if (m_gasMovementScript != null)
                m_gasMovementScript.SetSpeed(p);
        }
           
    }
	
	private void ShowMolecule(float speed)
	{
		if(m_snowBall == null)
		{
			Debug.LogError("ERROR - NO SNOW BALL FOUND");
			return;
		}        

        //CleanSpace();
		
        int numberOfMolecules = 0;

        while(numberOfMolecules++ < 2)
        {
            GameObject molecule = null;

		    try
		    {
                molecule = (GameObject)Instantiate(Resources.Load("Molecule"), Vector3.zero, Quaternion.identity);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed To instantiate molecule");
                return;
            }

            molecule.transform.parent = m_snowBall.transform;
            molecule.transform.position = m_snowBall.transform.position;
            m_molecules.Add(molecule);
            
		}
					
	}

    private void CleanSpace()
    {
        if (m_molecules == null || m_molecules.Count < 1)
        {
            Debug.LogError("Failed to clear. No Molecules found");
            return;
        }

        for (int i = 0; i < m_molecules.Count; i++)
            Destroy(m_molecules[i]);
    }	

    #endregion




    #region Private members

    private Slider m_heatSlider = null;
    private Slider m_pressureSlider = null;
	
	// Represent the container for all molecules
    private List<GameObject> m_molecules = new List<GameObject>();
	
	// Represent the molecule movement script	
	private MoleculeMovement m_gasMovementScript;

    // Represents the snow ball game object
    private GameObject m_snowBall = null;

    #endregion

    
}
