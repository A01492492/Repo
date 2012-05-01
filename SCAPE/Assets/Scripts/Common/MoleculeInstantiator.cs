using UnityEngine;
using System.Collections.Generic;
using System;

public class MoleculeInstantiator : MonoBehaviour 
{
	
	internal void ShowMolecule(bool isVapour)
	{
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
            molecule.transform.parent = mSnowBall.transform;
			
            // set temp as center of snow ball
            temp = mSnowBall.transform.position;

            // get a random offset for temp
            temp.x += UnityEngine.Random.Range(-4, 4);
            temp.z += UnityEngine.Random.Range(-4, 4);
            temp.y += UnityEngine.Random.Range(-4, 4);

            //molecule.transform.position = m_snowBall.transform.position;
            molecule.transform.position = temp;
            
            // add the molecule to list of molecules
            mMolecules.Add(molecule);       
        }

        // set speed of each moelcule
        foreach (GameObject molecule in mMolecules)
        {
            mMoleculeMovementScript = (MoleculeMovement)molecule.GetComponent(typeof(MoleculeMovement));

            if (mMoleculeMovementScript != null)
                mMoleculeMovementScript.SetSpeed(speed);
			
        }
        		
	}
	
	/// <summary>
    /// This displays the ice molecule inside the snow globe
    /// </summary>
    internal void ShowIceMolecule() 
    {
        // Return if the m_cold is false
        //if (!m_cold)
        //{
            //Debug.Log("No ice found. Please click on cold button to generate ");
            //return;
        //}

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
        iceMolecule.transform.parent = mSnowBall.transform;

        // Change position of ice molecule to the center of snow globe
        iceMolecule.transform.position = mSnowBall.transform.position;
        
        //iceMolecule.transform.localPosition = offset;
        mMolecules.Add(iceMolecule);
    }
	
	/// <summary>This cleans up the molecules in the snow globe</summary>
	internal void CleanSpace()
	{
        if (mMolecules == null || mMolecules.Count < 1)
        {
            Debug.Log("Failed to clear. No Molecules found");
            return;
        }

        // Iterate the molecules list
        for (int i = 0; i < mMolecules.Count; i++)
            Destroy(mMolecules[i]);
        
        // Remove all the references
        mMolecules.Clear();
	}
	
	private void Awake()
	{
		// Get the snow globe object
		mSnowBall = GameObject.Find("SnowGlobe");
	}
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	// Represents the initial vapour molecule speed
	private float VapourMoleculeSpeed = 4f;
	
	// Represents the max number of vapour molecules
    private int MaxVapourMolecules = 30;
	
	// Represents the initial water molecule speed
    private float WaterMoleculeSpeed = 1f;
	
	// Represents the snow ball game object
	private GameObject mSnowBall = null;
	
	// Represent the container for all molecules
    private List<GameObject> mMolecules = new List<GameObject>();
	
	// Represent the molecule movement script	
	private MoleculeMovement mMoleculeMovementScript;
}
