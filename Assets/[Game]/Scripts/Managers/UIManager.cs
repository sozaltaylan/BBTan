using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BBTanClone.Exceptions;

namespace BBTanClone.Managers
{
    public class UIManager : MonoSingleton<UIManager>
    {
        #region  Variables


        [Header("Total Money")]
        [SerializeField] private float totalMoney;
        [SerializeField] private TMP_Text scoreUI;

        [Header("Earned Money")]
        [SerializeField] private float earnedMoney;
        [SerializeField] private TMP_Text earnedMoneyText;

        [Header("Ball Convertor Money")]
        [SerializeField] private float ballConvertorMoney;
        [SerializeField] private TMP_Text ballConvertorMoneyText;
        [SerializeField] private Button merchButton;

        [Header("Add Ball Button")]
        [SerializeField] private int addBalllevel = 1;
        [SerializeField] private TMP_Text addBalllevelText;
        [SerializeField] private float addBallCost;
        [SerializeField] private TMP_Text addBallCostText;

        [Header("Speed Ball Button")]
        [SerializeField] private int speedBalllevel;
        [SerializeField] private TMP_Text speedBallLevelText;
        [SerializeField] private float speedBallCost;
        [SerializeField] private TMP_Text speedBallCostText;


        [Header("Income Button")]
        [SerializeField] private float incomeMoneyLevel;
        [SerializeField] private TMP_Text incommeLevelText;
        [SerializeField] private float incomeMoneyCost;
        [SerializeField] private TMP_Text incomeMoneyCostText;



        [Header("Cost Multiplier")]
        [SerializeField] private float levelMultiplier;


        public bool isConvertorButtonActive = true;
        public bool isAddBallButtonActive = true;
        public bool isAddSpeedButtonActive = true;


        public float EarnedMoney => earnedMoney;
        #endregion
        #region Methods

        private void Start()
        {
            earnedMoneyText.text = EarnedMoney.ToString();   
        }
        private void Update()
        {

            HandleUpdate();


        }

        private void HandleUpdate()
        {
            scoreUI.text = totalMoney.ToString("F2") + "$";
            addBalllevelText.text = "LEVEL  " + addBalllevel;
            speedBallLevelText.text = "LEVEL " + speedBalllevel;
            incommeLevelText.text = "LEVEL " + incomeMoneyLevel;
            addBallCostText.text = "$" + addBallCost;
            speedBallCostText.text = "$" + speedBallCost;
            incomeMoneyCostText.text = "$" + incomeMoneyCost;
            earnedMoneyText.text = "$" + earnedMoney;
            ballConvertorMoneyText.text = "$" + ballConvertorMoney;
            SetUIColor();
        }
        public void SetBallConvertorMoneyUI()
        {
            bool isBallsEnough = BallManager.Instance.CheckMergeCondition();

            if (!IsCanMerchBall() || !isBallsEnough) return;

            
            StartCoroutine(IncreaseUIMoney());
            IEnumerator IncreaseUIMoney()
            {
                yield return new WaitForSeconds(1);
                totalMoney -= ballConvertorMoney;
                ballConvertorMoney *= levelMultiplier;
                
            }

        }

        private void SetUIColor() 
        {
            bool isBallsEnough = BallManager.Instance.CheckMergeCondition();
            if (!IsCanMerchBall() || !isBallsEnough)
            {
                merchButton.GetComponent<Image>().color = Color.gray;
            }
            else if (IsCanMerchBall() && isBallsEnough)
            {
                merchButton.interactable = true;
                merchButton.GetComponent<Image>().color = Color.green;
            }
        }
        
        public bool IsCanMerchBall()
        {
            var isEnough = ballConvertorMoney <= totalMoney ? true : false;
            {
                return isEnough;
            }
        }
        public bool IsCanAddBall()
        {
            var isEnough = addBallCost <= totalMoney ? true : false;
            {
                return isEnough;
            }
        }
        public void SetincreaseMoneyUI()
        {
            if (totalMoney < incomeMoneyCost) return;
            incomeMoneyLevel++;
            earnedMoney += 0.3f;
            earnedMoneyText.text = earnedMoney.ToString();
            totalMoney -= incomeMoneyCost;
            incomeMoneyCost += (levelMultiplier * incomeMoneyLevel);
        }

        public void SetAddBallUI()
        {
            if (!IsCanAddBall()) return;

            addBalllevel++;
            totalMoney -= addBallCost;
            addBallCost += (levelMultiplier * addBalllevel);

        }
        public void SetSpeedLevelUI()
        {
            if (totalMoney < speedBallCost)
            {
                isAddSpeedButtonActive = false;
                return;
            }
            speedBalllevel++;
            totalMoney -= speedBallCost;
            speedBallCost += (levelMultiplier * speedBalllevel);

        }

        public void IncreaseTotalMoney()
        {
            totalMoney += earnedMoney;
        }
    }

}
#endregion