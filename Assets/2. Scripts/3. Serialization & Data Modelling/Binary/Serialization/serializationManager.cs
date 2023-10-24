using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
public class serializationManager : MonoBehaviour
{
    public static bool Save(string oldSaveName, string newSaveName, object saveData)
    {
        BinaryFormatter formatter = GetBinaryFormatter();
        string saveFolder = Application.persistentDataPath + "/saves";
        //Create Folder
        if (!Directory.Exists(saveFolder)) Directory.CreateDirectory(saveFolder);
        //Delete Old Save File
        string oldSaveFile = saveFolder + "/" + oldSaveName + ".save";
        if (File.Exists(Path.Combine(Application.persistentDataPath, oldSaveFile))) File.Delete(Path.Combine(Application.persistentDataPath, oldSaveFile));
        //Create New Save File
        string newSaveFile = saveFolder + "/" + newSaveName + ".save";
        FileStream file = File.Create(newSaveFile);
        formatter.Serialize(file, saveData);
        file.Close();
        return true;
    }
    public static object Load(string saveName)
    {
        string saveFolder = Application.persistentDataPath + "/saves";
        string saveFile = saveFolder + "/" + saveName + ".save";
        if (!File.Exists(saveFile)) return null;
        BinaryFormatter formatter = GetBinaryFormatter();
        FileStream file = File.Open(saveFile, FileMode.Open);
        try
        {
            object saveData = formatter.Deserialize(file);
            file.Close();
            return saveData;
        }
        catch
        {
            Debug.LogErrorFormat("Failed to load save file at {0}.", saveFile);
            file.Close();
            return null;
        }
    }
    public static void Delete(string saveName)
    {
        string saveFolder = Application.persistentDataPath + "/saves";
        if (!Directory.Exists(saveFolder)) Directory.CreateDirectory(saveFolder);
        string oldSaveFile = saveFolder + "/" + saveName + ".save";
        if (File.Exists(Path.Combine(Application.persistentDataPath, oldSaveFile))) File.Delete(Path.Combine(Application.persistentDataPath, oldSaveFile));
    }
    public static BinaryFormatter GetBinaryFormatter()
    {
        //Formatter
        BinaryFormatter formatter = new BinaryFormatter();
        //Selector
        SurrogateSelector selector = new SurrogateSelector();
        //Surrogates
        Vector3SerializationSurrogate vector3Surrogate = new Vector3SerializationSurrogate();
        QuaternionSerializationSurrogate quaternionSurrogate = new QuaternionSerializationSurrogate();
        StoredItemSerializationSurrogate storedItemDataSurrogate = new StoredItemSerializationSurrogate();
        //Attaching the Surrogates to the Selector
        selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);
        selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSurrogate);
        selector.AddSurrogate(typeof(storedItemData), new StreamingContext(StreamingContextStates.All), storedItemDataSurrogate);
        return formatter;
    }
}
