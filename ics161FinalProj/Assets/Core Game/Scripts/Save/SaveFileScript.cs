
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveFileScript
{
    /*
     * Takes in the button name as the file path, the current player, and uses StageHubScript to access all the individual stage's collectibles for saving 
     * Creates a new level data object, which contains a series of public variables 
     * The public variables are then serialized then stored in the file
     */
    public static void SaveLevel(string buttonName, Vector3 playerPos, StageHubScript stageHub)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + string.Format("/{0}.csin", buttonName);
        FileStream stream = new FileStream(path, FileMode.Create);

        LevelData l = new LevelData(playerPos, stageHub);

        formatter.Serialize(stream, l);
        stream.Close();
    }

    /*
     * The buttonName is passed in as the name of the path 
     * If the path exists, then open the path and return the LevelData object that has the saved information 
     * Otherwise, returns null
     */

    public static LevelData LoadLevel(string buttonName)
    {
        string path = Application.persistentDataPath + string.Format("/{0}.csin", buttonName);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LevelData level = formatter.Deserialize(stream) as LevelData;
            stream.Close();

            return level;
        }
        else
        {
            Debug.LogError("woops");
            return null;
        }
    }

    //Checks if a file path exists
    public static bool CheckSaveFile(string buttonName)
    {
        string path = Application.persistentDataPath + string.Format("/{0}.csin", buttonName);
        if (File.Exists(path))
        {
            return true;
        }
        return false;
    }

    //Deletes all file paths
    public static void DeleteAllData(string buttonName)
    {
        string path = Application.persistentDataPath + string.Format("/{0}.csin", buttonName);
        if(File.Exists(path))
        {
            File.Delete(path);
        }
    }


}
