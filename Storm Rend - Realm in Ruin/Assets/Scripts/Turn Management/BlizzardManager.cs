using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// blizzard class responsible for managing the blizzard meter and performing blizzards
/// when necessary
/// </summary>
public class BlizzardManager : MonoBehaviour
{
    // group of blizzard toggles
    private Toggle[] m_blizzardMeter;

    /// <summary>
    /// constructor which caches references to toggle UI elements
    /// </summary>
    private void Start()
    {
        // cache references to child toggles
        m_blizzardMeter = GetComponentsInChildren<Toggle>();
    }

    /// <summary>
    /// use this function to increment the blizzard meter by one! Performs an animation on
    /// the blizzard toggle node and executes & resets the blizzard if the meter is full
    /// </summary>
    public void IncrementBlizzardMeter()
    {
        // iterate over blizzard toggles
        for (int i = 0; i < m_blizzardMeter.Length; i++)
        {
            // get blizzard toggle
            Toggle blizzardNode = m_blizzardMeter[i];
            
            // untoggled blizzard node found
            if (!blizzardNode.isOn)
            {
                blizzardNode.isOn = true; // toggle on

                // cache reference to the node's animator
                Animator animator = blizzardNode.GetComponent<Animator>();
                animator.SetTrigger("Notify"); // do juicy notify animation
                return;
            }
        }

        // all blizzard nodes are toggled, start blizzard
        ExecuteBlizzard();

        // reset blizzard meter
        ResetBlizzardMeter();
    }

    /// <summary>
    /// use this function to reset & empty the blizzard meter! 
    /// </summary>
    public void ResetBlizzardMeter()
    {
        // iterate over blizzard toggles
        for (int i = 0; i < m_blizzardMeter.Length; i++)
            m_blizzardMeter[i].isOn = false; // toggle off
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
