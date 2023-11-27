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
    public Animator playerFightAreaAnimator;
    public Animator enemyFightAreaAnimator;
    CardManager cardManager => FindAnyObjectByType<CardManager>();
    GameManager gameManager => FindAnyObjectByType<GameManager>();

    public GameObject spiritMeterFill, spiritMeterBuffer;
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
        
    }

    private void FixedUpdate()
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
        //comboPlayText.text = "play " + cardManager.CardsInPlayArea() + " card combo";
    }

    public void AnimateSpiritMeter()
    {
        spiritMax = gameManager.Nelly.maxSpirit;
        currentSpirit = gameManager.Nelly.spirit;

        //scale meter based on how much spirit in play
        float targetScale = (currentSpirit - spiritInPlay) / spiritMax;
        float currentScaleFill = spiritMeterFill.GetComponent<Image>().fillAmount;
        float currentScaleBuffer = spiritMeterFill.GetComponent<Image>().fillAmount;
        bool raising = targetScale > currentScaleBuffer;
        float fillSpeed = 0.1f;
        float bufferSpeed = raising ? 0.5f : Time.fixedDeltaTime;
        float lerpedScale = Mathf.Lerp(currentScaleFill, targetScale, fillSpeed);
        float bufferScale = Mathf.Lerp(currentScaleBuffer, targetScale, bufferSpeed) + 0.01f;
        spiritMeterFill.GetComponent<Image>().fillAmount = lerpedScale;
        spiritMeterBuffer.GetComponent<Image>().fillAmount = bufferScale;

        //anchor meter to hand
        Vector3 meterPos = handAnimator.gameObject.transform.position;
        meterPos.x = spiritMeterFill.gameObject.transform.position.x;
        spiritMeterFill.transform.parent.parent.position = meterPos;

        //particles
        spiritMeterFill.GetComponent<ParticleSystem>().emissionRate = Mathf.Lerp(0, 50, spiritInPlay/currentSpirit);

        //update text showing how much spirit left
        spiritText.text = "" + (currentSpirit - spiritInPlay);
    }

    void AnimateHPBar()
    {
        CharacterManager Nelly = gameManager.Nelly;
        CharacterManager Bruttia = gameManager.Bruttia;
        float nellyPercent = Nelly.health / Nelly.maxHealth;
        float bruttiaPercent = Bruttia.health / Bruttia.maxHealth;
        playerHP.fillAmount = nellyPercent;
        enemyHP.fillAmount = bruttiaPercent;
    }

    public void NextGameState()
    {
        StartCoroutine(gameManager.SwitchGameState());
    }
}
