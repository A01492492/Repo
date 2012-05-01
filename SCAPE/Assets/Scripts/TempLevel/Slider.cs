using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class Slider : MonoBehaviour, INotifyPropertyChanged
{
    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    public void CreateSlider(string labelName, Rect labelRect, Rect sliderRect, int maxValue) 
    {
        if (!m_sliderInitialized)
        {
            m_sliderContainerRect = sliderRect;
            m_labelContainerRect = labelRect;

            m_lable = labelName;
            m_maxValue = maxValue;

            m_sliderInitialized = true;
        }
    }

    #region Public properties

    public int SliderValue
    {
        get { return m_sliderValue; }

        set
        {
            if (m_sliderValue != value)
            {
                m_sliderValue = value;

                OnPropertyChanged("SliderValue");
            }
        }
    }   

    #endregion

    protected void OnPropertyChanged(string name)
    {
        PropertyChangedEventHandler handler = PropertyChanged;

        if (handler != null)
        {
            handler(this, new PropertyChangedEventArgs(name));
        }
    }

	// Use this for initialization
	private void Start () 
    {
	
	}
	
	// Update is called once per frame
	private void Update () 
    {
	
	}

    private void OnGUI()
    {
        if (m_sliderInitialized)
        {
            GUI.Label(m_labelContainerRect, m_lable + SliderValue);
            SliderValue = (int)GUI.HorizontalSlider(m_sliderContainerRect, SliderValue, 0.0F, m_maxValue);
        }
    }

    private Rect m_labelContainerRect;
    private Rect m_sliderContainerRect;
    private string m_lable = string.Empty;
    private int m_maxValue = 0;
    private int m_sliderValue = 0;

    private bool m_sliderInitialized = false;    
}
