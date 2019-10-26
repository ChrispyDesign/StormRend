using UnityEngine;

/// <summary>
/// Save Data container class, contains relevant floats, ints and strings to be saved
/// </summary>
[System.Serializable]
public class xSaveData
{
    // the date/time of save
    [SerializeField] private string m_dateTime;

    #region getters
    
    public string GetDateTime() { return m_dateTime; }

    #endregion

    /// <summary>
    /// constructor which initialises all of the raw save data
    /// </summary>
    /// <param name="dateTime"></param>
    public xSaveData(string dateTime)
    {
        m_dateTime = dateTime;
    }
}