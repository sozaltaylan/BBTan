using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUIController : MonoBehaviour
{
    public void DoShake()
    {
        transform.DOKill(true);
        transform.DOShakeScale(.1f, .1f, 5, 1000);

    }

}
