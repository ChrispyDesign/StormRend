using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// blizzard class responsible for incrementing and decrementing the blizzard meter and performing blizzards when necessary
/// </summary>
public class BlizzardManager : MonoBehaviour
{
	//Rename to BlizzardController

    // blizzard meter
    [Tooltip("Blizzard meter transform, contains toggle child elements")]
    [SerializeField] Transform m_blizzardMeter = null;

    [Space]
    [SerializeField] UnityEvent OnIncrement;
    [SerializeField] UnityEvent OnReset;
    [SerializeField] UnityEvent OnPrepare;
    [SerializeField] UnityEvent OnExecute;

    #region Variables
    // group of blizzard toggles
    Toggle[] m_blizzardNodes;
    #endregion

    #region Properties
    public static int blizzardCount {get; private set; } = 0;
    #endregion

    /// <summary>
    /// caches references to blizzard nodes
    /// </summary>
    private void Start()
    {
        Debug.Assert(m_blizzardMeter != null, "Blizzard meter not assigned to Blizzard Manager!");

        if (m_blizzardMeter != null)
            m_blizzardNodes = m_blizzardMeter.GetComponentsInChildren<Toggle>();

        blizzardCount = 0;
    }

    /// <summary>
    /// use this function to increment the blizzard meter by one! Performs an animation on
    /// the blizzard toggle node and executes & resets the blizzard if the meter is full
    /// </summary>
    public void IncrementBlizzardMeter()
    {
        OnIncrement.Invoke();

        // all blizzard nodes are toggled, start blizzard
        if (blizzardCount == m_blizzardNodes.Length)
        {
            ExecuteBlizzard(); // start blizzard
            ResetBlizzardMeter(); // reset blizzard meter
            return;
        }

        // get blizzard toggle
        Toggle blizzardNode = m_blizzardNodes[blizzardCount];
        blizzardNode.isOn = true; // toggle on

        // increment
        blizzardCount++;

        // cache reference to the node's animator
        Animator animator = blizzardNode.GetComponent<Animator>();
        
        if (blizzardCount == m_blizzardNodes.Length)
            PrepareBlizzard();
        else
            animator.SetTrigger("NotifyOnce"); // notify once
    }

    /// <summary>
    /// use this function to reset & empty the blizzard meter! 
    /// </summary>
    public void ResetBlizzardMeter()
    {
        OnReset.Invoke();

        // iterate over blizzard toggles
        for (int i = 0; i < m_blizzardNodes.Length; i++)
        {
            // get blizzard toggle
            Toggle blizzardNode = m_blizzardNodes[i];
            blizzardNode.isOn = false; // toggle off
        }
        blizzardCount = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    private void PrepareBlizzard()
    {
        OnPrepare.Invoke();

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
        OnExecute.Invoke();

        // do blizzard stuffs here
        Debug.Log("Blizzard");
    }
}