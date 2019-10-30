using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// static file manager which allows the reading and writing of binary SaveData
/// </summary>
public static class xFileManager
{
    /// <summary>
    /// save function, uses binary formatting to serialize SaveData
    /// </summary>
    /// <param name="saveData">a custom class containing raw save data</param>
    /// <param name="path">the path to save (e.g. Application.persistentDataPath + "data.sav")</param>
    public static void Save(xSaveData saveData, string path)
    {
        // create file stream for writing
        FileStream file = new FileStream(path, FileMode.Create);

        // save the data in binary format
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(file, saveData);

        // done with file stream
        file.Close();
    }

    /// <summary>
    /// load function, uses deserializes formatted binary save data into a SaveData
    /// container
    /// </summary>
    /// <param name="path">the path to save (e.g. Application.persistentDataPath + "data.sav")</param>
    /// <returns>a custom class containing raw save data</returns>
    public static xSaveData Load(string path)
    {
        // check if save data exists
        if (!File.Exists(path))
            return null; // no save data exists

        // create file stream for reading
        FileStream file = new FileStream(path, FileMode.Open);

        // open the binary data
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        xSaveData saveData = binaryFormatter.Deserialize(file) as xSaveData;

        // done with file stream
        file.Close();
        return saveData;
    }
}