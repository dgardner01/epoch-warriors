using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Animator fogAnimator;
    public Animator cardsAnimator;
    public Animator handAnimator;

    CardManager cardManager => FindAnyObjectByType<CardManager>();
    GameManager gameManager => FindAnyObjectByType<GameManager>();

    public GameObject spiritMeter;
    public TextMeshProUGUI spiritText;
    float spiritMax;
    public float currentSpirit;
    float spiritInPlay = 0;

    public GameObject comboPlayButton;
    public TextMeshProUGUI comboPlayText;

    public Image playerHP, enemyHP;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AnimateSpiritMeter();
        AnimateHPBar();
    }

    public void SetAnimationState(Animator animator, string state, bool status)
    {
        animator.SetBool(state, status);
    }

    public void UpdateSpiritMeter(int spirit)
    {
        spiritInPlay = spirit;
    }

    public void TogglePlayButton(bool state)
    {
        comboPlayButton.SetActive(state);
        comboPlayText.text = "play " + cardManager.CardsInPlayArea() + " card combo";
    }

    public void AnimateSpiritMeter()
    {
        spiritMax = gameManager.Nelly.maxSpirit;
        currentSpirit = gameManager.Nelly.spirit;
        float lerpSpeed = 0.1f;

        //scale meter based on how much spirit in play
        Vector3 meterScale = spiritMeter.transform.localScale;
        meterScale.y = Mathf.Lerp(0, 1, (currentSpirit - spiritInPlay) / spiritMax);
        spiritMeter.transform.localScale = Vector3.Lerp(spiritMeter.transform.localScale,
            meterScale, lerpSpeed);

        //anchor meter to hand
        Vector3 meterPos = handAnimator.gameObject.transform.position;
        meterPos.x = spiritMeter.gameObject.transform.position.x;
        spiritMeter.transform.parent.parent.position = meterPos;

        //update text showing how much spirit left
        spiritText.text = "" + (currentSpirit - spiritInPlay);
    }

    void AnimateHPBar()
    {
        CharacterManager Nelly = gameManager.Nelly;
        CharacterManager Bruttia = gameManager.Bruttia;
        float nellyPercent = Nelly.health / Nelly.maxHealth;
        print(nellyPercent);
        float bruttiaPercent = Bruttia.health / Bruttia.maxHealth;
        print(bruttiaPercent);
        playerHP.fillAmount = nellyPercent;
        enemyHP.fillAmount = bruttiaPercent;
    }

    public void NextGameState()
    {
        StartCoroutine(gameManager.SwitchGameState());
    }
}
