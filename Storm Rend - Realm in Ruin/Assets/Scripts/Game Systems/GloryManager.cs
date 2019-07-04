using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// glory management script responsible for incrementing and decrementing glory, and updating the UI's glory meter
/// </summary>
public class GloryManager : MonoBehaviour
{
    // glory meter
    [Tooltip("Glory meter transform, contains toggle child elements")]
    [SerializeField] private Transform m_gloryMeter;

    // group of glory toggles
    private static Toggle[] m_gloryNodes;
    private static int m_gloryCount = 0;

    #region getters

    public static int GetGloryCount() { return m_gloryCount; }

    #endregion

    /// <summary>
    /// caches references to glory nodes
    /// </summary>
    void Start()
    {
        Debug.Assert(m_gloryMeter != null, "Glory meter not assigned to Glory Manager!");

        if (m_gloryMeter != null)
            m_gloryNodes = m_gloryMeter.GetComponentsInChildren<Toggle>();

        m_gloryCount = 0;
    }
    
    /// <summary>
    /// use this function to gain glory!
    /// </summary>
    /// <param name="value">the amount of glory to gain</param>
    public static void GainGlory(int value)
    {
        // increment
        m_gloryCount += value;

        // ensure glory doesn't exceed max (disallow index overflow)
        m_gloryCount = Mathf.Clamp(m_gloryCount, 0, m_gloryNodes.Length);

        // turn on/off the right amount of glory nodes
        UpdateGloryMeter();

        // do other stuffs here

    }

    /// <summary>
    /// use this function to spend glory!
    /// </summary>
    /// <param name="value">the amount of glory to spend</param>
    /// <returns>false if there isn't enough glory, true if the glory was spent successfully</returns>
    public static bool SpendGlory(int value)
    {
        if (m_gloryCount - value < 0)
            return false; // not enough glory

        // decrement
        m_gloryCount -= value;

        // ensure glory doesn't fall below min (disallow index overflow)
        m_gloryCount = Mathf.Clamp(m_gloryCount, 0, m_gloryNodes.Length);

        // turn on/off the right amount of glory nodes
        UpdateGloryMeter();
        
        return true; // glory spent successfully
    }

    /// <summary>
    /// updates the UI toggle elements of the glory meter to match the amount of glory
    /// </summary>
    private static void UpdateGloryMeter()
    {
        // iterate through all glory nodes and turn on/off the relevant nodes
        for (int i = 0; i < m_gloryNodes.Length; i++)
        {
            // get glory toggle
            Toggle gloryNode = m_gloryNodes[i];

            if (i < m_gloryCount)
                gloryNode.isOn = true; // glory available
            else
                gloryNode.isOn = false; // glory unavailable
        }
    }
}