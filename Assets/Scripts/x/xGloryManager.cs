using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// glory management script responsible for incrementing and decrementing glory, and updating the UI's glory meter
/// </summary>
public class xGloryManager : MonoBehaviour
{
    // glory meter
    [Tooltip("Glory meter transform, contains toggle child elements")]
    [SerializeField] private Transform m_gloryMeter;

    [Space]
    [SerializeField] UnityEvent OnGain;
    [SerializeField] UnityEvent OnSpend;
    
    // group of glory UI toggles
    #region Variables
    private static Toggle[] m_gloryNodes;
    public static int gloryCount { get; private set; } = 0;

    #endregion

    /// <summary>
    /// caches references to glory nodes
    /// </summary>
    void Start()
    {
        Debug.Assert(m_gloryMeter != null, "Glory meter not assigned to Glory Manager!");

        if (m_gloryMeter != null)
            m_gloryNodes = m_gloryMeter.GetComponentsInChildren<Toggle>();

        gloryCount = 0;
    }
    
    /// <summary>
    /// use this function to gain glory!
    /// </summary>
    /// <param name="value">the amount of glory to gain</param>
    public void GainGlory(int value)
    {
        OnGain.Invoke();       

        // increment
        gloryCount += value;

        // ensure glory doesn't exceed max (disallow index overflow)
        gloryCount = Mathf.Clamp(gloryCount, 0, m_gloryNodes.Length);

        // turn on/off the right amount of glory nodes
        UpdateGloryMeter();

        // do other stuffs here

    }

    /// <summary>
    /// use this function to spend glory!
    /// </summary>
    /// <param name="value">the amount of glory to spend</param>
    /// <returns>false if there isn't enough glory, true if the glory was spent successfully</returns>
    public bool SpendGlory(int value)
    {
        OnSpend.Invoke();

        if (gloryCount - value < 0)
            return false; // not enough glory

        // decrement
        gloryCount -= value;

        // ensure glory doesn't fall below min (disallow index overflow)
        gloryCount = Mathf.Clamp(gloryCount, 0, m_gloryNodes.Length);

        // turn on/off the right amount of glory nodes
        UpdateGloryMeter();
        
        return true; // glory spent successfully
    }

    /// <summary>
    /// updates the UI toggle elements of the glory meter to match the amount of glory
    /// </summary>
    private void UpdateGloryMeter()
    {
        // iterate through all glory nodes and turn on/off the relevant nodes
        for (int i = 0; i < m_gloryNodes.Length; i++)
        {
            // get glory toggle
            Toggle gloryNode = m_gloryNodes[i];

            if (i < gloryCount)
                gloryNode.isOn = true; // glory available
            else
                gloryNode.isOn = false; // glory unavailable
        }
    }
}