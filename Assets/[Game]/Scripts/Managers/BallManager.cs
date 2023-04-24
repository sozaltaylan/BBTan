using BBTanClone.Controllers;
using BBTanClone.Exceptions;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using BBTanClone.Managers;

public class BallManager : MonoSingleton<BallManager>
{
    #region Variables
    [SerializeField] private List<BallController> ballsOnScene = new List<BallController>();
    [SerializeField] private GameObject ballPrefab;
    [Range(0, 30)]
    [SerializeField] private float ballSpeed;

    #region Properties

    public float BallSpeed => ballSpeed;
    #endregion
    #endregion


    #region Methods

    private void Start()
    {
        AddListOnSceneBalls();
    }

    private void AddListOnSceneBalls()
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<BallController>(out BallController ball))
            {
                if (ballsOnScene.Count < transform.childCount && !ballsOnScene.Contains(ball))
                {
                    ballsOnScene.Add(ball);
                }

            }
        }
    }

    public void SetShakeForceBalls()
    {
        for (int i = 0; i < ballsOnScene.Count; i++)
        {
            var ball = ballsOnScene[i];
            ball.SetShakeForce();
        }
    }


    public void AddBall(int level)
    {
        if (!UIManager.Instance.IsCanAddBall()) return;

        var newBall = Instantiate(ballPrefab, this.transform.position, Quaternion.identity);
        newBall.transform.parent = this.transform;
        AddListOnSceneBalls();

        newBall.TryGetComponent<BallController>(out BallController ball);
        ball.ballLevel = level;

        if (level == 2)
        {
            ball.ballYellow.gameObject.SetActive(false);
            ball.ballRed.gameObject.SetActive(true);
        }

        if (level == 3)
        {
            ball.ballRed.gameObject.SetActive(false);
            ball.ballBlue.gameObject.SetActive(true);
        }

        var newBallRb = newBall.GetComponent<Rigidbody>();
        newBallRb.isKinematic = false;
        var randomX = Random.Range(-5, 5);
        var xForce = new Vector3(randomX, ballSpeed, 0);
        newBallRb.AddForce(xForce, ForceMode.Impulse);
    }

    public void AddSpeed()
    {
        if (UIManager.Instance.isAddSpeedButtonActive == false) return;

        StartCoroutine(DefaultSpeed());
        IEnumerator DefaultSpeed()
        {
            ballSpeed += 10;
            yield return new WaitForSeconds(1.5f);
            ballSpeed -= 9;

        }
    }

    private void SetBallMerch(List<BallController> newBalls)
    {
        if (!UIManager.Instance.IsCanMerchBall()) return;

        for (int i = 0; i < newBalls.Count; i++)
        {
            if (newBalls[i].TryGetComponent(out BallController ball))
            {
                ball.SetMove(this.transform);

                for (int k = 0; k < newBalls.Count - 1; k++)
                {
                    Destroy(newBalls[k].gameObject, 1);
                    ballsOnScene.Remove(newBalls[k]);
                }
                ball.SetMerch();
            }

        }
    }

    public void ConvertBall()
    {

        int _ballLevel = 1;
        int _maxLevel = 3;
        bool isActive = false;

        _ballLevel++;

        for (int i = 1; i < _maxLevel; i++)
        {
            if (isActive) break;

            List<BallController> _newBalls = new List<BallController>();

            for (int k = 0; k < ballsOnScene.Count; k++)
            {

                if (ballsOnScene[k].TryGetComponent<BallController>(out BallController ball))
                {

                    if (ball.ballLevel == i)
                    {
                        _newBalls.Add(ball);
                    }

                    if (_newBalls.Count == 3)
                    {
                        SetBallMerch(_newBalls);
                        isActive = true;
                        break;
                    }
                   
                }
            }

        }
    }

    public bool CheckMergeCondition()
    {

        int level1Count = 0;
        int level2Count = 0;
        int level3Count = 0;

        foreach (BallController ball in ballsOnScene)
        {
            if (ball.ballLevel == 1)
                level1Count++;
            else if (ball.ballLevel == 2)
                level2Count++;
            else if (ball.ballLevel == 3)
                level3Count++;
        }

        if (level1Count >= 3 || level2Count >= 3 || level3Count >= 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    public void CallAllBalls()
    {
        for (int i = 0; i < ballsOnScene.Count; i++)
        {
            var ball = ballsOnScene[i];
            ball.SetMove(this.transform);
        }
    }
}


#endregion