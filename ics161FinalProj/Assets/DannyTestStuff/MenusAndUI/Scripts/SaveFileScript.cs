
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveFileScript
{
    public static void SaveLevel(string buttonName, PlayerData player, StageHubScript stageHub)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + string.Format("/{0}.csin", buttonName);
        FileStream stream = new FileStream(path, FileMode.Create);

        LevelData l = new LevelData(player, stageHub);

        formatter.Serialize(stream, l);
        stream.Close();
    }

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

    public static bool CheckSaveFile(string buttonName)
    {
        string path = Application.persistentDataPath + string.Format("/{0}.csin", buttonName);
        if (File.Exists(path))
        {
            return true;
        }
        return false;
    }

    public static void DeleteAllData(string buttonName)
    {
        string path = Application.persistentDataPath + string.Format("/{0}.csin", buttonName);
        if(File.Exists(path))
        {
            File.Delete(path);
        }
    }


}
