using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// blizzard class responsible for incrementing and decrementing the blizzard meter and performing blizzards when necessary
/// </summary>
public class BlizzardManager : MonoBehaviour
{
    // blizzard meter
    [Tooltip("Blizzard meter transform, contains toggle child elements")]
    [SerializeField] private Transform m_blizzardMeter;

    // group of blizzard toggles
    private Toggle[] m_blizzardNodes;
    private static int m_blizzardCount = 0;

    #region getters

    public static int GetBlizzardCount() { return m_blizzardCount; }

    #endregion

    /// <summary>
    /// caches references to blizzard nodes
    /// </summary>
    private void Start()
    {
        Debug.Assert(m_blizzardMeter != null, "Blizzard meter not assigned to Blizzard Manager!");

        if (m_blizzardMeter != null)
            m_blizzardNodes = m_blizzardMeter.GetComponentsInChildren<Toggle>();

        m_blizzardCount = 0;
    }

    /// <summary>
    /// use this function to increment the blizzard meter by one! Performs an animation on
    /// the blizzard toggle node and executes & resets the blizzard if the meter is full
    /// </summary>
    public void IncrementBlizzardMeter()
    {
        // all blizzard nodes are toggled, start blizzard
        if (m_blizzardCount == m_blizzardNodes.Length)
        {
            ExecuteBlizzard(); // start blizzard
            ResetBlizzardMeter(); // reset blizzard meter
            return;
        }

        // get blizzard toggle
        Toggle blizzardNode = m_blizzardNodes[m_blizzardCount];
        blizzardNode.isOn = true; // toggle on

        // increment
        m_blizzardCount++;

        // cache reference to the node's animator
        Animator animator = blizzardNode.GetComponent<Animator>();
        
        if (m_blizzardCount == m_blizzardNodes.Length)
            PrepareBlizzard();
        else
            animator.SetTrigger("NotifyOnce"); // notify once
    }

    /// <summary>
    /// use this function to reset & empty the blizzard meter! 
    /// </summary>
    public void ResetBlizzardMeter()
    {
        // iterate over blizzard toggles
        for (int i = 0; i < m_blizzardNodes.Length; i++)
        {
            // get blizzard toggle
            Toggle blizzardNode = m_blizzardNodes[i];
            blizzardNode.isOn = false; // toggle off
        }
        
        m_blizzardCount = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    private void PrepareBlizzard()
    {
        // iterate over blizzard toggles
        for (int i = 0; i < m_blizzardNodes.Length; i++)
        {
            // get blizzard toggle
            Toggle blizzardNode = m_blizzardNodes[i];

            // cache reference to the node's animator
            Animator animator = blizzardNode.GetComponent<Animator>();
            animator.SetTrigger("NotifyLoop"); // play notification loop
        }
    }

    /// <summary>
    /// function which executes the blizzard when the meter is full, decreases all player health by one
    /// and immobilizes them
    /// </summary>
    private void ExecuteBlizzard()
    {
        // do blizzard stuffs here
        Debug.Log("Blizzard");
    }
}