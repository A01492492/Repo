using UnityEngine;
using System.Collections;

public class ColorControl : MonoBehaviour
{
    public enum PrimaryColor { r = 0, g, b };

    void Awake()
    {
        m_original = renderer.material.color;

        m_temp = new Color(m_original.r, m_original.g, m_original.b, m_original.a);
    }
	
	// Use this for initialization
	void Start() 
    {        
	
	}
	
	// Update is called once per frame
	void Update() 
    {
        if (m_on && Time.time > trigger && !m_changeColor)
            StartChangeColor();
	}
    
    private void StartChangeColor()
    {
        
        switch(m_colorToChange)
        {
            case PrimaryColor.r:
                if(m_temp.r < m_maxValue)
                    m_temp.r += m_offset;
                else
                    m_changeColor = true;

                break;

            case PrimaryColor.g:
                if (m_temp.g < m_maxValue)
                    m_temp.g += m_offset;
                else
                    m_changeColor = true;

                break;

            case PrimaryColor.b:
                if (m_temp.b < m_maxValue)
                    m_temp.b += m_offset;
                else
                    m_changeColor = true;

                break;
            
            default:
                Debug.Log("Value not supported");
                break;
        }

        if(m_repeat)
        {
            if (m_changeColor)
            {
                m_temp = m_original;
                m_changeColor = false;
            }
        }
        renderer.material.color = m_temp;
    }

    /// <summary>
    /// Changes the color of the object
    /// </summary>
    /// <param name="primaryColor">Component which is to be changed</param>
    /// <param name="end">End value of the component</param>
    /// <param name="offset">Increase factor</param>
    /// <param name="repeat">Whether the color change has to be repeated</param>
    public void SetColorParams(PrimaryColor primaryColorToChange, float end, float offset, bool repeat)
    {
        m_colorToChange = primaryColorToChange;
        m_maxValue = end;
        m_offset = offset;
        m_repeat = repeat;
    }

    public void ShowOriginal()
    {
        /*if(m_original == null)
            return;*/

        renderer.material.color = m_original;

        m_temp = m_original;
        m_changeColor = false;
    }

    public void Toggle()
    {
        trigger = Time.time + WAIT;

        m_on = !m_on;

        if(!m_on)
            ShowOriginal();

    }
	
	public void Go()
	{
		trigger = Time.time + WAIT;
		m_on = true;
	}

    public void Stop() 
    {
        m_on = false;

        ShowOriginal();
    }

    private const float WAIT = 1;
    private float trigger = 0;

    private PrimaryColor m_colorToChange;
    
    private float m_maxValue;
    private float m_offset;
    
    private bool m_repeat = false;
    private bool m_changeColor = false;
    private bool m_on = false;

    private Color m_original;
    private Color m_temp;    
}