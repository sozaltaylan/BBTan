using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BBTanClone.Exceptions;
using TMPro;
using BBTanClone.Controllers;

namespace BBTanClone.Managers
{
    public class BlockManager : MonoSingleton<BlockManager>
    {
        #region Variables

        [SerializeField] private List<BlockController> blocksOnScene = new List<BlockController>();

        [SerializeField] private GameObject confetti;

        #endregion

        #region Methods
        private void Update()
        {
            if (blocksOnScene.Count == 0)
            {
                confetti.SetActive(true);
                BallManager.Instance.CallAllBalls();
            }
        }

        public void RemoveBlock(BlockController block)
        {
            if (blocksOnScene.Contains(block))
            {
                blocksOnScene.Remove(block);
            }
        }

        public void SetEarnedMoney()
        {
            for (int i = 0; i < blocksOnScene.Count; i++)
            {
                var block = blocksOnScene[i];
                var text = block.earnedMoneyText.GetComponent<TextMeshPro>();
                text.text = UIManager.Instance.EarnedMoney + "$";


            }
        }
    }
    #endregion
}
