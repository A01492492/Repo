using UnityEngine;
using System.Collections;

public class HotSpotTOBEDELETED : MonoBehaviour 
{
	void Awake()
	{
		//m_snowBall = GameObject.Find("SnowBall");
	}
	
	void OnMouseEnter() 
	{
        renderer.material.color = Color.red;		
    }
	
	void OnMouseExit() 
	{
        renderer.material.color = Color.white;
    }
	
	void OnMouseDown() 
	{
        renderer.material.color = Color.green;
		Debug.LogError("Mouse Down");
		
		
		if(! m_windowExists)
		{
			m_childWindow = (GameObject)Instantiate(Resources.Load("childWindow"));
			
			if(m_childWindow.transform.Find("molecule") != null)
			{
				m_molecule = m_childWindow.transform.Find("molecule").gameObject;
				
				if(m_molecule != null && m_molecule.GetComponent(typeof(MoleculeMovement)) != null)
				{
					MoleculeMovement molecularMovement = (MoleculeMovement)m_molecule.GetComponent(typeof(MoleculeMovement));
					molecularMovement.SetSpeed(3);
				}
					
			}
						
			if(m_molecule == null)
				Debug.LogError("Get Component returned null");
			else
				Debug.LogError("Get Component returned object");
		
			
			m_windowExists = true;
		}
		else
		{
			Destroy(m_childWindow);
			m_windowExists = false;
		}
		
    }
	
	private GameObject m_childWindow = null;
	private GameObject m_molecule = null;
			
	private bool m_windowExists = false;
}
