using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace BBTanClone.Controllers
{
    public class BallController : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Rigidbody rb;
        [SerializeField] private SphereCollider coll;

        private Vector3 _lastVelocity;
        private Vector3 _direction;

        private bool _isBallStop = true;

        public int ballLevel;

        public Transform ballRed;
        public Transform ballBlue;
        public Transform ballYellow;

        public float ballSpeedMultiplier = 1;

        #endregion

        #region Methods

        private void FixedUpdate()
        {

            SetBallSpeed();
            BeginningForce();
            SetBallsProperties();
            _lastVelocity = rb.velocity;
        }
        private void SetBallSpeed()
        {
            if (!_isBallStop)
            {
                rb.velocity = rb.velocity.normalized * BallManager.Instance.BallSpeed * ballSpeedMultiplier;
            }
        }
        public void BeginningForce()
        {
            if (Input.GetMouseButton(0) && _isBallStop)
            {

                rb.isKinematic = false;
                var randomX = Random.Range(-5, 5);
                var xForce = new Vector3(randomX, 7, 0);
                rb.velocity = xForce;

                StartCoroutine(ChangeBallBool());
                IEnumerator ChangeBallBool()
                {
                    yield return new WaitForSeconds(0.1f);
                    _isBallStop = false;

                }


            }


        }

        public void SetShakeForce()
        {
            var randomX = Random.Range(-5, 5);
            var xForce = new Vector3(randomX, 7, 0);
            rb.velocity = xForce;
        }
        
        public void SetMove(Transform transform)
        {
            this.transform.DOMove(transform.position, 1, false);
            rb.isKinematic = true;
        }

        public void SetMerch()
        {
            ballLevel += 1;
            rb.isKinematic = false;
            var randomX = Random.Range(-5, 5);
            var xForce = new Vector3(randomX, 7, 0);
            rb.velocity = xForce;

            StartCoroutine(WaitBallColor());
            IEnumerator WaitBallColor()
            {
                yield return new WaitForSeconds(1);
                SetBallColor();
            }

        }

        private void SetBallsProperties()
        {
            switch (ballLevel)
            {
                case 1:
                    ballSpeedMultiplier = 1f;
                    break;
                case 2:
                    ballSpeedMultiplier = 2f;
                    break;
                case 3:
                    ballSpeedMultiplier = 2.5f;
                    break;
                default:
                    break;
            }
        }
        private void SetBallColor()
        {

            if (ballLevel == 2)
            {
                ballYellow.gameObject.SetActive(false);
                ballRed.gameObject.SetActive(true);
            }

            if (ballLevel == 3)
            {
                ballRed.gameObject.SetActive(false);
                ballBlue.gameObject.SetActive(true);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            var speed = _lastVelocity.magnitude;
            _direction = Vector3.Reflect(_lastVelocity.normalized, collision.contacts[0].normal);
            rb.velocity = _direction * speed;
        }
        #endregion
    }
}

