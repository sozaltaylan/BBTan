using BBTanClone.Managers;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BBTanClone.Controllers 
{
    public class BlockController : MonoBehaviour

    {

        #region Variables
        [SerializeField] private TextMeshPro blockPointText;
        private MeshRenderer _meshRenderer;

        [SerializeField] private List<BlockModels> blockModels = new List<BlockModels>();

        [SerializeField] private int stateColorIndex;
        [SerializeField] private int blockPoint;

        private MoneyUIController moneyUIController;

        public GameObject earnedMoneyText;
        [SerializeField] private GameObject hitParticle;
        [SerializeField] private GameObject destroyParticle;
        #endregion

        #region Methods

        private void Awake()
        {
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
            moneyUIController = FindObjectOfType<MoneyUIController>();
        }

        private void Start()
        {
            blockPointText.text = blockPoint.ToString();
        }
        private void OnCollisionEnter(Collision collision)
        {
            SetCubePoint();
            SetCubeColor();
            SetEarnedMoneyText();
            DoShake();
            moneyUIController.DoShake();
            UIManager.Instance.IncreaseTotalMoney();
            var particle = Instantiate(hitParticle, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(particle, 1);
        }


        private void SetCubePoint()
        {
            blockPoint--;
            blockPointText.text = blockPoint.ToString();

            if (blockPoint == 0)
            {
                var particle = Instantiate(destroyParticle, this.transform.position, Quaternion.identity);
                Destroy(gameObject);
                Destroy(particle, 1);
                BlockManager.Instance.RemoveBlock(this);
            }
        }

        private void SetCubeColor()
        {
            if (stateColorIndex == 3) stateColorIndex = 0;


            stateColorIndex++;
            for (int i = 0; i < blockModels.Count; i++)
            {
                var stateBlock = blockModels[i];

                if (stateBlock.colorIndex == stateColorIndex)
                {
                    stateBlock.Model.SetActive(true);
                }
                else
                {
                    stateBlock.Model.SetActive(false);
                }
            }


        }

        private void SetEarnedMoneyText()
        {
            earnedMoneyText.gameObject.SetActive(true);
            StartCoroutine(OpenCloseText());
            IEnumerator OpenCloseText()
            {
                yield return new WaitForSeconds(0.5f);
                if (earnedMoneyText != null)
                {
                    earnedMoneyText.gameObject.SetActive(false);
                }

            }


        }

        private void DoShake()
        {
            transform.DOKill(true);
            transform.DOShakeScale(.2f, .5f, 2, .3f);

        }
        #endregion
    }

    [Serializable]
    public class BlockModels
    {
        public GameObject Model;
        public int colorIndex;
    }



}
