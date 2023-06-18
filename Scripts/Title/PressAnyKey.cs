using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class PressAnyKey : MonoBehaviour
{
    private bool OK;//‘€ì‰Â”\‚É‚È‚é‚½‚ß‚Ì•Ï”
    public GameObject Letter;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMeshProUGUI>().DOFade(0, 0.5f).SetLoops(-1, LoopType.Yoyo);

        DOVirtual.DelayedCall(4f, () =>
        {
            OK = true;
        }).SetLink(gameObject);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) return;

        if (Input.GetKeyDown(KeyCode.Space)&& OK )
        {
            PressButton();
        }
    }

    // Update is called once per frame
    public void PressButton()
    {
        DOVirtual.DelayedCall(0.2f, () =>
        {
            Letter.GetComponent<EffectSystem_Title>().ENDEFFECT();
            Letter.GetComponent<EffectSystem_Title>().enabled = false;
            TitleManager.Instance.TextAnimation();
            TitleManager.Instance.LetterAnim();
            gameObject.SetActive(false);
        }).SetLink(gameObject);
        
    }
}
