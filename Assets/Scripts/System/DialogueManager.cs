using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class DialogueManager : MonoBehaviour
{
    public GameObject Nelly, Bruttia;
    public Animator dialogueAnimator;
    public TextMeshProUGUI nellyText, bruttiaText;
    public string[] nellyLines;
    public string[] bruttiaLines;
    public int index;
    public bool active = true;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Typewriter());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AdvanceDialogue();
        }
    }

    void AdvanceDialogue()
    {
        if (index+1 > nellyLines.Length-1)
        {
            active = false;
        }
        else
        {
            index++;
            Nelly.SetActive(!Nelly.activeInHierarchy);
            Bruttia.SetActive(!Bruttia.activeInHierarchy);
            StartCoroutine(Typewriter());
        }
    }

    IEnumerator Typewriter()
    {
        nellyText.text = "";
        foreach (char c in nellyLines[index])
        {
            nellyText.text += c;
            yield return new WaitForSeconds(0.05f);
        }
        bruttiaText.text = "";
        foreach (char c in bruttiaLines[index])
        {
            bruttiaText.text += c;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
