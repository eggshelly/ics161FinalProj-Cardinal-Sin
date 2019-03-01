using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetAllData : MonoBehaviour
{

    public void DeleteAllData()
    {
        foreach(Transform t in transform)
        {
            SaveFileScript.DeleteAllData(t.gameObject.name);
        }
    }
}
