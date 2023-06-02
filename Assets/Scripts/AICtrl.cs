using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using DG.Tweening;

namespace Skymare
{
    public class AICtrl : MonoBehaviour
    {
        public enum AIState
        {
            Idle,Move,Gear,Ready,Climb, Finish
        }

        [SerializeField] private SkeletonAnimation _skeleton;
        [SerializeField] private GroundCheck _checkGround;
        [SerializeField] private WallJump _wallCheck;

        private float _countDelayJump, _timeDelayJump;
        private float _countDelayIdle, _timeIdle;

        private bool _isGround;
        private Rigidbody2D _rigid2D;
        private BoxCollider2D _myBox;
        private Vector2 _jumpPosition;
        private List<Collider2D> _listJumpBox;
        private AIState _state;
        private bool _canStart;
        private Vector2 _beginPosition;
        private bool _hasPush;
        private float _mySpeed;


        // Start is called before the first frame update
        void Start()
        {
            _checkGround.OnSetEvent(this.OnGround, OnExitGround);
            _wallCheck.OnSetEvent(this.OnCheckWall);
            _rigid2D = this.GetComponent<Rigidbody2D>();
            _myBox = this.GetComponent<BoxCollider2D>();
            _listJumpBox = new List<Collider2D>();
            _state = AIState.Move;

        //    _skeleton.Skeleton.SetColor(new Color(0.9f, 0, 1f, 1f));
            _beginPosition = this.transform.position;
            _mySpeed = Random.Range(3f, 6f);
        }

        // Update is called once per frame
        void Update()
        {
            if (!_canStart) return;

            switch (_state)
            {
                case AIState.Idle:

                    break;
                case AIState.Move:
                    _rigid2D.velocity = new Vector2(_mySpeed, _rigid2D.velocity.y);
                    if (_isGround)
                        _skeleton.AnimationName = "Run";
                    else
                        _skeleton.AnimationName = _rigid2D.velocity.y > 0 ? "Jump1" : "Fall1";
                    
                    
                    break;
                case AIState.Gear:
                    _rigid2D.velocity = new Vector2(_mySpeed * 1.2f, _rigid2D.velocity.y);
                    _skeleton.AnimationName = "Run" ;
                    if (Mathf.Abs(_jumpPosition.x - this.transform.position.x) < 0.05f )
                    {
                        _rigid2D.AddForce(Vector2.up * Random.Range(600f,700f));
                        _state = AIState.Move;
                    }
                       
                    break;
                case AIState.Ready:

                    break;
            }
            
        }



        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_listJumpBox.Contains(collision)) return;

            if(collision.tag == Const.TAG_JUMP_BOX)
            {
                _jumpPosition = this.transform.position;

                _listJumpBox.Add(collision);
                if(Random.Range(0,100) > 80)
                {
                    StartCoroutine(IEGear());
                }
                else
                {
                    _rigid2D.AddForce(Vector2.up * Random.Range(600f, 700f));
                }
            }
            else if(collision.tag == Const.TAG_FINISH)
            {
                _state = AIState.Finish;
                _rigid2D.velocity = new Vector2(0,_rigid2D.velocity.y);
                _skeleton.AnimationName = "Win" + Random.Range(1, 4);
                _canStart = false;

                if (collision.name.Contains("lose"))
                {
                    _skeleton.AnimationState.SetAnimation(0, "Lose", false);
                }
            }
            else if(collision.tag == Const.TAG_FLOOR)
            {
                this.transform.position = _beginPosition;
                _listJumpBox.Clear();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.tag == Const.TAG_PUSH_GROUND && !_hasPush)
            {
                collision.gameObject.GetComponent<PushGround>()?.OnPlayAnim();
                _rigid2D.AddForce(Vector2.up * Const.JUMP_FORCE * 1.6f);
                _hasPush = true;
            }
        }


        private IEnumerator IEGear()
        {
            _skeleton.AnimationName = "Idle1";
            _rigid2D.velocity = Vector2.zero;
            _state = AIState.Ready;

            yield return new WaitForSeconds(0.5f);
            _skeleton.timeScale = 0.6f;
            _skeleton.AnimationName = "Run";
            float timeGear = Random.Range(0.3f, 0.8f);
            this.transform.DOMoveX(this.transform.position.x - Random.Range(0.5f, 1f), timeGear);

            yield return new WaitForSeconds(timeGear);
            _skeleton.timeScale = 1f;
            _skeleton.AnimationName = "Idle1";
            yield return new WaitForSeconds(Random.Range(0,0.5f));
            _state = AIState.Gear;
        }



        private void OnGround()
        {
            _isGround = true;
            _state = AIState.Move;
            _hasPush = false;
        }

        private void OnExitGround()
        {
            _isGround = false;
        }

        private void OnCheckWall(WallType type)
        {
            _state = AIState.Climb;
            StartCoroutine(IEWallJump());
        }


        private IEnumerator IEWallJump()
        {
            _myBox.enabled = false;
            _rigid2D.bodyType = RigidbodyType2D.Kinematic;
            _rigid2D.velocity = Vector2.zero;
            _skeleton.AnimationState.SetAnimation(0, "Climb_up", false);
            yield return new WaitForSeconds(0.4f);
            _rigid2D.bodyType = RigidbodyType2D.Dynamic;
            Vector2 dir = (Vector2.up + (Vector2)(_skeleton.transform.right * 0.06f)).normalized;
            _rigid2D.AddForce(dir * 600f );

            _skeleton.AnimationState.SetAnimation(0, "Jump1", false);
            yield return new WaitForSeconds(0.3f);
            _rigid2D.velocity = new Vector2(0, _rigid2D.velocity.y);
            _myBox.enabled = true;

        }

        public void OnStart()
        {
            _canStart = true;
        }

    }
}