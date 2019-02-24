using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectiblesScript : MonoBehaviour
{
    [SerializeField] GameObject Panel;
    [SerializeField] Sprite keySpriteFilled;
    [SerializeField] Sprite keySpriteEmpty;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateCollectibles(int collectibles, int totalC)
    {
        DeleteCurrent();
        RectTransform r = Panel.GetComponent<RectTransform>();
        float width = r.rect.width;
        Vector3 leftBorder = new Vector3(r.rect.xMin, r.transform.position.y);
        float distanceBetween = width / (totalC * 2);
        for (int i = 0; i < totalC; i++)
        {
            GameObject newImage = new GameObject();
            newImage.transform.SetParent(Panel.transform, false);
            RectTransform rect = newImage.AddComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(leftBorder.x + distanceBetween + distanceBetween* 2 * i , newImage.transform.localPosition.y);
            Image image = newImage.AddComponent<Image>();
            if (i < (totalC -collectibles))
                image.sprite = keySpriteFilled;
            else
                image.sprite = keySpriteEmpty;
        }

    }

     /*void SetInformation(int collectibles, int totalC)
    {
        totalCollectibles = totalC;
        collectiblesLeft = collectibles;
        RectTransform r = GetComponent<RectTransform>();
        width = r.rect.width;
        leftBorder = new Vector3(r.rect.xMin, r.transform.position.y);
    }*/

    void DeleteCurrent()
    {
        foreach (Transform t in Panel.transform)
        {
            Destroy(t.gameObject);
        }
    }
}
