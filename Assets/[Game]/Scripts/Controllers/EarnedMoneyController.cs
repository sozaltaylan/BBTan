using UnityEngine;
using DG.Tweening;
using TMPro;


namespace BBTanClone.Controllers

{
    public class EarnedMoneyController : MonoBehaviour
    {
        #region Variables

        private Vector3 _firstPosition;
        [SerializeField] private TextMeshPro text;

        #endregion
        #region Methods
        private void Start()
        {
            _firstPosition = transform.position;
        }
        private void OnEnable()
        {
            Sequence _sequnce = DOTween.Sequence();
            _sequnce.Append(transform.DOMoveY(1, 1).OnComplete(() => { this.transform.position = _firstPosition; }));
            _sequnce.Join(text.DOFade(1, 0));


        }
        #endregion
    }

}
