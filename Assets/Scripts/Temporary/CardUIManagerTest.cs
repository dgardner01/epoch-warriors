using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUIManagerTest : MonoBehaviour
{
    public bool hovering;
    public Vector3 backgroundStartPos;
    public Vector3 backgroundEndPos;
    public Vector3 cardsStartPos;
    public Vector3 cardsEndPos;
    public RectTransform cards;
    public RectTransform bg;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float lerpSpeed = Time.unscaledDeltaTime * 10;
        if (hovering)
        {
            Time.timeScale = 0.15f;
            bg.localPosition = Vector3.Lerp(bg.localPosition, backgroundEndPos, lerpSpeed);
            cards.localPosition = Vector3.Lerp(cards.localPosition, cardsEndPos, lerpSpeed);
        }
        else
        {
            Time.timeScale = 1;
            bg.localPosition = Vector3.Lerp(bg.localPosition, backgroundStartPos, lerpSpeed);
            cards.localPosition = Vector3.Lerp(cards.localPosition, cardsStartPos, lerpSpeed);
        }
    }
}
