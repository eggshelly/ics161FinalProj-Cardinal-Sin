using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectiblesStageUI : MonoBehaviour
{
    [SerializeField] GameObject Panel;
    [SerializeField] Sprite keySpriteFilled;
    [SerializeField] Sprite keySpriteEmpty;

    //Called by StageHubScript when a stage is "entered". Creates the collectible icons on the StagePanel 
    public void CreateCollectibles(bool[] collectibles)
    {
        DeleteCurrent();
        RectTransform r = Panel.GetComponent<RectTransform>();
        float width = r.rect.width;
        Vector3 leftBorder = new Vector3(r.rect.xMin, r.transform.position.y);
        float distanceBetween = width / (collectibles.Length * 2);
        for (int i = 0; i < collectibles.Length; i++)
        {
            GameObject newImage = new GameObject();
            newImage.transform.SetParent(Panel.transform, false);
            RectTransform rect = newImage.AddComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(leftBorder.x + distanceBetween + distanceBetween* 2 * i , newImage.transform.localPosition.y);
            Image image = newImage.AddComponent<Image>();
            if (collectibles[i])
                image.sprite = keySpriteFilled;
            else
                image.sprite = keySpriteEmpty;
        }

    }


    //Deletes all the existing collectible icons.
    void DeleteCurrent()
    {
        foreach (Transform t in Panel.transform)
        {
            Destroy(t.gameObject);
        }
    }
}
