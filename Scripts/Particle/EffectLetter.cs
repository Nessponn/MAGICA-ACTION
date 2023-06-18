using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EffectLetter : MonoBehaviour
{
    private GameObject EffectWhite;
    private GameObject EffectWhite_Small;
    private GameObject EffectYellow;
    private GameObject EffectYellow_Small;

    [Range(0f, 10f)] public float Speed;
    [Range(1f, 10f)] public float Range_x;
    [Range(1f, 10f)] public float Range_y;

    Tween tween;

    private void Start()
    {
        var GM = TitleManager.Instance;

        EffectWhite = GM.EffectWhite;
        EffectWhite_Small = GM.EffectWhite_Small;
        EffectYellow = GM.EffectYellow;
        EffectYellow_Small = GM.EffectYellow_Small;

        InstantEffect();

        tween = DOVirtual.DelayedCall(3f, () =>
        {
            gameObject.AddComponent<EffectSystem_Title>();
        });
    }

    private void InstantEffect()
    {
        int num = Random.Range(0, 3);

        float rangex = Random.Range(-Range_x, -1);
        float rangey = Random.Range(-Range_y, Range_y);

        GameObject obj = null;
        if (num == 0) obj = Instantiate(EffectWhite);
        else if (num == 1) obj = Instantiate(EffectYellow);
        else if (num == 2) obj = Instantiate(EffectWhite_Small);
        else if (num == 3) obj = Instantiate(EffectYellow_Small);

        //obj.transform.parent = gameObject.transform;

        //obj.transform.DOLocalMove(new Vector3(Direction_x, Direction_y, 0), 1).SetLink(obj);

        obj.transform.position = gameObject.transform.position;

        tween = obj.transform.DOLocalMove(new Vector3(obj.transform.localPosition.x + rangex, obj.transform.localPosition.y + rangey, 0), 1 / Speed).SetLink(obj);

        Destroy(obj, 1 / Speed);

        if (TitleManager.Instance.Effect_End) return;

        DOVirtual.DelayedCall(0.1f, () =>
        {
            InstantEffect();
        });
    }

    private void OnDisable()
    {
        
    }
}
