using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using DG.Tweening;
using UnityEngine.Events;

namespace Skymare
{
    public enum State
    {
        Idle, Move, Jump, Dead, Finish, WallJump, StandDanger, Fly, Push
    }

    public class StickmanCtrl : MonoBehaviour
    {
        [SerializeField] private SkeletonAnimation _skeleton;
        [SerializeField] private GroundCheck _groundCheck;
        [SerializeField] private Transform _enemyCheck;
        [SerializeField] private WallJump _wallCheck;
        [SerializeField] private StandCheck _standCheck;
        [SerializeField] private TopCheck _topCheck;
        [SerializeField] private Transform _openDoor;
        [SerializeField] private PetFollower _prfPet;
        [SerializeField] private Transform _stopWall;
        [SerializeField] private List<FxPremium> _fxPremiums;


        private State _state, _prevState;
        public State MyState
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    if (_state == State.Finish) return;
                    _prevState = _state;
                    _state = value;
                    this.UpdateStateAnimation();
                }
            }
        }

        private float _moveSpeed;
        private float _jumpForce;

        private float _timeAttack;
        private int _countAttack,_countCoin;

        private Rigidbody2D _rigid2D;
        private List<ParticleSystem> _listJumpFX;
        private bool _pressMoveLeft, _pressMoveRight;
        private int _heart;
        private BoxCollider2D _myCollider;
        private float _axisMove, _maxAxis;
        private bool _playSoundRun;
        private bool _justFall, _isSlippery , _isFly, _isIronBars;
        private bool _isPendulum,_hasTap;
        private GameObject _objPendulum, _oldPlatform;
        private float _timeIdle;
        private bool _isDefaulSkin, _wallHasAhead, _obstacleAhead;



        // Start is called before the first frame update
        private void Awake()
        {
            InputMgr.ActMoveLeft = this.OnPressLeft;
            InputMgr.ActMoveRight = this.OnPressRight;
            InputMgr.ActUpMove = this.OnUpMove;
            InputMgr.ActJump = this.OnJump;

            _rigid2D = this.GetComponent<Rigidbody2D>();
            _moveSpeed = Const.MOVE_SPEED;
            _myCollider = this.GetComponent<BoxCollider2D>();
            _jumpForce = Const.JUMP_FORCE;
            int skin = PlayerPrefs.GetInt(Const.KEY_HERO_SELECTED, 0) ;
            _isDefaulSkin = skin == 0;
            if (!_isDefaulSkin)
            {
                HeroType hero = (HeroType)skin;
                this.OnChangeSkin("Skin" + (skin + 2), Color.white,hero);
            }
            this.OnReset();
        }

        void Start()
        {
            _groundCheck.OnSetEvent(this.OnHitGround, this.OnExitGround);
            _wallCheck.OnSetEvent(this.OnWallJump);
            _topCheck.OnSetEvent(this.OnWallJump,this.OnPendulum);
            InputMgr.ActMoveLeft = this.OnMoveLeft;
            InputMgr.ActMoveRight = this.OnMoveRight;
            InputMgr.ActUpMove = this.OnStop;
            InputMgr.ActJump = this.OnClickJump;

        //    GameController.Instance?.OnSetEvent(this.OnControllMove, this.OnStop,this.OnClickJump);

            PetTypes pet = (PetTypes)PlayerPrefs.GetInt(Const.KEY_PET_SELECTED);
            if(pet != PetTypes.None)
            {
                PetFollower ctr = Instantiate(_prfPet, this.transform.position, Quaternion.identity);
                ctr.SetWitcher(this.transform, pet);
            }

        }

        // Update is called once per frame
        void Update()
        {
            if (_state == State.Finish || _state == State.Dead) return;
 #if UNITY_EDITOR
            if (Input.GetKey(KeyCode.A) || _pressMoveLeft)
            {
                this.OnMoveLeft();
            }
            else if (Input.GetKey(KeyCode.D) || _pressMoveRight)
            {
                this.OnMoveRight();
            }
            else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                this.OnUpMove();
            }
#else
            if (Mathf.Abs(_maxAxis) > 0.1f)
            {
                if (MyState == State.WallJump || MyState == State.Push) return;
                MyState = State.Move;
                _axisMove = Mathf.MoveTowards(_axisMove, _maxAxis, Time.deltaTime * 5f);
                _skeleton.transform.localEulerAngles = _maxAxis > 0 ? Vector3.zero : Vector3.up * 180f;
                float velocity = 1f;
                if (_isSlippery) velocity = 0.8f;
                if (_isIronBars) velocity = 0.3f;
                _rigid2D.velocity = new Vector2(_moveSpeed * _axisMove * velocity, _rigid2D.velocity.y);
            }



#endif
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
            {
                if (_groundCheck.OnGround || _isPendulum)
                    this.OnJump();
            }

            if (_state == State.Jump || _state == State.Move || _state == State.Fly)
            {
                if (_rigid2D.velocity.y < 0.1f && !_groundCheck.OnGround && !_justFall)
                {
                    _skeleton.AnimationName = $"Fall{Random.Range(1, 4)}";
                    _justFall = true;
                }

                 if (_isFly )
                {
                    _skeleton.AnimationName = _rigid2D.velocity.y > 0 ? $"Jump4" : "Fall3";
                }
            }

            _timeIdle += Time.deltaTime;
            if(_state == State.Idle && _timeIdle >= 4f)
            {
                _timeIdle = 0f;
                this.OnPlayAnim($"Idle{Random.Range(1, 3)}", "Idle1", null);
            }

        }

        private void FixedUpdate()
        {
            Collider2D col = Physics2D.OverlapPoint(_stopWall.position, 1 << 8);
            _wallHasAhead = col != null;
            Collider2D col2 = Physics2D.OverlapPoint(_stopWall.position, 1 << 10);
            ObstacleAhead = col2 != null;
        }

        private void OnControllMove(float axis)
        {
            if (_isPendulum)
            {
                if (!_hasTap)
                {
                    _objPendulum.GetComponent<Rigidbody2D>()?.AddRelativeForce(Vector2.right * 50f * axis);
                    _hasTap = true;
                }
                return;
            }

            _maxAxis = axis;
        }

        private void OnStop()
        {
            if (_isPendulum)
            {
                _hasTap = false;
                return;
            }

            _maxAxis = 0f;
            this.OnUpMove();
        }

        private void OnClickJump()
        {
            if (_groundCheck.OnGround || _isPendulum)
                this.OnJump();
        }
        #region public method
        public void OnBeHit()
        {
            _state = State.Dead;
            this.OnPlayAnim("Lose", "Lose", () =>
            {
                StartCoroutine(IESpawn(1f));
            });
        }
        #endregion
        #region private method
        private void UpdateStateAnimation()
        {
            switch (_state)
            {
                case State.Idle:
                    _skeleton.AnimationName = "Idle1";
                    _timeIdle = 0f;
                    break;
                case State.Move:
                    if (_groundCheck.OnGround)
                    {
                        string animMove = "Run";
                        if (_isSlippery) animMove = "Run_ice";
                        if (_isIronBars) animMove = "Careful_walk";
                        if (_obstacleAhead) animMove = "Push";
                        _skeleton.AnimationName = animMove;
                        SoundManager.Instance?.OnPlaySoundRun(true);
                    }
                    break;
                case State.Jump:
                case State.Fly:
                    Vector3 point = _topCheck.transform.position + Vector3.up * 0.5f;
                    RaycastHit2D hit = Physics2D.Raycast(point, Vector2.up);
                    //     if (_skeleton.AnimationName == "jump_up1" || _skeleton.AnimationName == "jump_up2") return;
                    string anim = $"Jump{Random.Range(1, 4)}"; // Random.Range(0, 100) < 80 ? "Jump1" : "Jump2";
                    if (_skeleton.AnimationName == "Jump1" && anim == "Jump2") return;
                    if (hit) anim = "Jump1";
                    _skeleton.AnimationState.SetAnimation(0, anim, false);
                    SoundManager.Instance?.OnPlaySoundRun(false);
                    SoundManager.Instance?.OnPlaySoundJump();
                    _justFall = false;
                    break;

                case State.Dead:
                    _skeleton.AnimationState.SetAnimation(0, "Fall Die", false);
                    break;
                case State.Finish:
                    _rigid2D.velocity = new Vector2(0,_rigid2D.velocity.y);
                    var current2 = _skeleton.AnimationState.SetAnimation(0, $"Win{Random.Range(1, 3)}", false);
                    current2.Complete += (t) =>
                    {
                        _skeleton.AnimationState.SetAnimation(0, $"Idle{Random.Range(1, 4)}", true);
                    };

                    break;
                case State.StandDanger:
                    var current = _skeleton.AnimationState.SetAnimation(0, "Unbalance", false);
                    current.Complete += (t) =>
                    {
                        MyState = State.Idle;
                    };
                    break;
                case State.Push:
                    StartCoroutine(IEIDle());
                    IEnumerator IEIDle()
                    {
                        yield return new WaitForSeconds(0.1f);
                        MyState = State.Idle;
                        _rigid2D.velocity = Vector2.zero;
                    }
                    break;
                default: break;
            }
        }

        private void OnHitGround()
        {
            if ( _state == State.Dead || _state == State.Finish) return;

            if (MyState == State.WallJump)
            {
                _rigid2D.velocity = new Vector2(0, _rigid2D.velocity.y);
            }

            MyState = State.Idle;
        //    _isFly = false;

            if (!_standCheck.OnGround())
            {
                MyState = State.StandDanger;
            }
            else
            {
                if (!_isSlippery)
                {
                    this.OnPlayAnim($"Land{Random.Range(1, 4)}", "Idle1", null);
                }
                else
                {
                    this.OnPlayAnim($"Unbalance", "Idle1", null);
                }
            }
            SoundManager.Instance?.OnPlaySoundLand();
            _topCheck.gameObject.SetActive(true);

        }

        private void OnExitGround()
        {
            if (_state != State.Fly && _state != State.Dead)
                MyState = State.Jump;

            _isIronBars = false;
            _isSlippery = false;
        }

        private void OnPressLeft()
        {
            _pressMoveLeft = true;
        }

        private void OnPressRight()
        {
            _pressMoveRight = true;
        }

        private void OnMoveLeft()
        {

            if (_isPendulum)
            {
                if (!_hasTap)
                {
                    _objPendulum.GetComponent<Rigidbody2D>()?.AddRelativeForce(Vector2.left * 50f);
                    _hasTap = true;
                }
                return;
            }

            if (MyState == State.WallJump || MyState == State.Push || MyState == State.Finish) return;

            _skeleton.transform.localEulerAngles = Vector3.up * 180f;
            if (_wallHasAhead)
            {
                _rigid2D.velocity = new Vector2(0f, _rigid2D.velocity.y);
                if (_groundCheck.OnGround)
                    MyState = State.Idle;
                return;
            }

            MyState = State.Move;
            float velocity = 1f; 
            if (_isSlippery) velocity = 0.8f;
            if (_isIronBars || _obstacleAhead) velocity = 0.3f;

            _rigid2D.velocity = new Vector2(_moveSpeed * Input.GetAxis("Horizontal") * velocity, _rigid2D.velocity.y);
        }

        private void OnMoveRight()
        {
            if (_isPendulum)
            {
                if (!_hasTap)
                {
                    _objPendulum.GetComponent<Rigidbody2D>()?.AddRelativeForce(Vector2.right * 50f);
                    _hasTap = true;
                }
                return;
            }

            if (MyState == State.WallJump || MyState == State.Push || MyState == State.Finish) return;

            
            _skeleton.transform.localEulerAngles = Vector3.zero;
            if (_wallHasAhead)
            {
                _rigid2D.velocity = new Vector2(0f, _rigid2D.velocity.y);
                if (_groundCheck.OnGround)
                    MyState = State.Idle;
                return;
            }

            MyState = State.Move;
            float velocity = 1f;
            if (_isSlippery) velocity = 0.8f;
            if (_isIronBars || _obstacleAhead) velocity = 0.3f;
            _rigid2D.velocity = new Vector2(_moveSpeed * Input.GetAxis("Horizontal") * velocity, _rigid2D.velocity.y);
        }

        private void OnUpMove()
        {
            if (_isPendulum)
            {
                _hasTap = false;
                return;
            }

            if (MyState == State.WallJump || MyState == State.Push || MyState == State.Finish) return;

            if (_groundCheck.OnGround)
            {
                MyState = State.Idle;
                SoundManager.Instance?.OnPlaySoundRun(false);
                //if (_standCheck.OnGround() && _isSlippery)
                //{
                //    this.OnPlayAnim("Slippery", "Idle1", null);
                //    _rigid2D.AddForce(_skeleton.transform.right * 50f);
                //}
            }


            _rigid2D.velocity = new Vector2(0, _rigid2D.velocity.y);
            _pressMoveLeft = _pressMoveRight = false;
            _axisMove = 0f;

            if (_groundCheck.OnGround && !_standCheck.OnGround())
            {
                MyState = State.StandDanger;
            }
        }

        private void OnJump()
        {
            if (_isPendulum)
            {
                _rigid2D.bodyType = RigidbodyType2D.Dynamic;
                MyState = State.Jump;
                _isPendulum = false;
                this.transform.parent = null;
                _topCheck.gameObject.SetActive(false);
                Vector2 direct = _objPendulum.GetComponent<Rigidbody2D>().velocity.normalized + Vector2.up;
                _rigid2D.AddForce(direct * _jumpForce * 0.8f);
                _objPendulum.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                return;
            }

            if (MyState == State.WallJump || MyState == State.Push || MyState == State.Fly) return;

            MyState = State.Jump;
            _rigid2D.AddForce(Vector2.up * _jumpForce);
        }

        private void OnWallJump(WallType type)
        {
            if (MyState == State.Push || MyState == State.Dead) return;
            if (!_groundCheck.OnGround && type == WallType.Dash) return;

                MyState = State.WallJump;
            _rigid2D.velocity = Vector2.zero;
            _rigid2D.bodyType = RigidbodyType2D.Kinematic;
            _myCollider.enabled = false;
            _groundCheck.gameObject.SetActive(false);

            switch (type)
            {
                case WallType.Ground:
                    StartCoroutine(IEWallJump());
                    break;
                case WallType.Climb:
                    StartCoroutine(IEClimbWall());
                    break;
                case WallType.Monkey:
                    StartCoroutine(IEMonkey());
                    break;
                case WallType.Candle:
                    StartCoroutine(IECandle());
                    break;
                case WallType.Dash:
                    
                    if(_groundCheck.OnGround )
                        StartCoroutine(IEDash());
                    break;
            }

        }

        private void OnPendulum(GameObject obj)
        {
            MyState = State.WallJump;
            _isPendulum = true;
            _objPendulum = obj;
            _rigid2D.velocity = Vector2.zero;
            _rigid2D.bodyType = RigidbodyType2D.Kinematic;
            _skeleton.AnimationName = "Hanging";
            this.transform.SetParent(obj.transform);
        }

        private IEnumerator IEWallJump()
        {
            _skeleton.AnimationState.SetAnimation(0, "Climb_up", false);
            yield return new WaitForSeconds(0.4f);
            _rigid2D.bodyType = RigidbodyType2D.Dynamic;
            Vector3 pos = _wallCheck.transform.position + _skeleton.transform.right.normalized * 0.2f + Vector3.up * 0.5f;
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.up, 2f, 1 << 8);
            if (hit)
            {
                _rigid2D.AddForce(Vector2.up * _jumpForce * 1.1f);
            }
            else
            {
                Vector2 dir = (Vector2.up + (Vector2)(_skeleton.transform.right * 0.06f)).normalized;
                _rigid2D.AddForce(dir * _jumpForce * 1.2f);
            }

            _skeleton.AnimationState.SetAnimation(0, "Jump1", false);
            yield return new WaitForSeconds(0.3f);
            _groundCheck.gameObject.SetActive(true);
            _myCollider.enabled = true;
        }

        private IEnumerator IEClimbWall()
        {
            _skeleton.AnimationName = "Climb_idle";
            float time = 0f;
            while(time < 0.3f)
            {
                _state = State.WallJump;
                _rigid2D.velocity = Vector2.zero;
                time += Time.deltaTime;
                yield return null;
            }
        //    yield return new WaitForSeconds(0.3f);
            _skeleton.AnimationName = "Climb_ladder";

            while (Physics2D.OverlapCircle(_wallCheck.transform.position, 0.1f, 1 << 8) != null)
            {
                this.transform.Translate(Vector3.up * Time.deltaTime * 3f);
                _state = State.WallJump;
                _rigid2D.velocity = Vector2.zero;
                yield return null;
            }
            _skeleton.AnimationName = "Climb_idle";
            yield return new WaitForSeconds(0.5f);
            _rigid2D.bodyType = RigidbodyType2D.Dynamic;
            Vector2 dir = (Vector2.up + (Vector2)(_skeleton.transform.right * 0.06f)).normalized;
            _rigid2D.AddForce(dir * _jumpForce * 1.2f);
            _skeleton.AnimationState.SetAnimation(0, "Jump1", false);
            yield return new WaitForSeconds(0.3f);
            _groundCheck.gameObject.SetActive(true);
            _myCollider.enabled = true;
        }

        private IEnumerator IEMonkey()
        {
            _skeleton.AnimationName =  "Monkey_climb_idle";
            _wallCheck.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _skeleton.AnimationName = "Monkey_climb";
            Vector2 pos = _wallCheck.transform.position + _skeleton.transform.right.normalized;
            while (Physics2D.OverlapCircle(pos, 0.5f, 1 << 8))
            {
                this.transform.DOMoveX(this.transform.position.x + Mathf.Sign(_skeleton.transform.right.x), 0.5f);
                yield return new WaitForSeconds(0.5f);
                pos = _wallCheck.transform.position + _skeleton.transform.right.normalized;
            }
            _skeleton.timeScale = 0f;

            yield return new WaitForSeconds(0.1f);
            _skeleton.timeScale = 1f;
            _rigid2D.bodyType = RigidbodyType2D.Dynamic;
            Vector2 dir = (Vector2)_skeleton.transform.right.normalized * 0.6f + Vector2.up;
            _rigid2D.AddForce(dir * _jumpForce * 0.6f);
            _skeleton.AnimationState.SetAnimation(0, "Jump3", false);
            yield return new WaitForSeconds(0.3f);
            _groundCheck.gameObject.SetActive(true);
            _myCollider.enabled = true;
            _wallCheck.gameObject.SetActive(true);
        }

        private IEnumerator IECandle()
        {
            _skeleton.AnimationName = "Hanging";
            _wallCheck.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
         //   this.transform.DOMoveY(this.transform.position.y + 1.8f, 0.7f);
            this.OnPlayAnim("Climb", "Idle1", () =>
            {
                _groundCheck.gameObject.SetActive(true);
                _myCollider.enabled = true;
                _wallCheck.gameObject.SetActive(true);
                _rigid2D.bodyType = RigidbodyType2D.Dynamic;
                MyState = State.Idle;
                this.transform.position += Vector3.up * 1.8f;
            });
        }

        private IEnumerator IEDash()
        {

            MyState = State.WallJump;
            this.OnStop();
            _topCheck.gameObject.SetActive(false);
            _wallCheck.gameObject.SetActive(false);
            _myCollider.enabled = false;
            _rigid2D.bodyType = RigidbodyType2D.Kinematic;
            Vector2 direct = _skeleton.transform.localEulerAngles.y == 0 ? Vector2.right : Vector2.left;
            _skeleton.AnimationName = "Dutch";
            bool _wall = true;

            while (_wall)
            {
                Vector3 pos = this._stopWall.position + Vector3.up ;
                Collider2D col = Physics2D.OverlapPoint(pos,1 << 8);
                _wall = col != null;
                this.transform.Translate(direct * Time.deltaTime * 10f);

                yield return null;
            }

            _skeleton.AnimationName = "Idle1";
            _topCheck.gameObject.SetActive(true);
            _wallCheck.gameObject.SetActive(true);
            _myCollider.enabled = true;
            _rigid2D.bodyType = RigidbodyType2D.Dynamic;
            _rigid2D.velocity = Vector2.zero;
            _groundCheck.gameObject.SetActive(true);
            MyState = State.Idle;
        }

        private IEnumerator IESpawn(float timeDelay)
        {
            yield return new WaitForSeconds(timeDelay);
            this.OnReset();

        }

        private void OnReset()
        {
            _rigid2D.bodyType = RigidbodyType2D.Kinematic;
            _myCollider.enabled = true;
            this.transform.localScale = Vector3.zero;
            this.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
            {
                _rigid2D.bodyType = RigidbodyType2D.Dynamic;
            });

            _state = State.Idle;
            MyState = State.Jump;
            this.transform.position = _openDoor.position;
            Vector3 myPos = this.transform.position;
            myPos.z = -10f;
            Camera.main.transform.position = myPos;
            
            _rigid2D.velocity = Vector2.zero;
            _maxAxis = 0f;
            _wallCheck.gameObject.SetActive(true);
            if(_isDefaulSkin)
                this.OnRandomSkin();
        }

        private void OnStandDanger()
        {
            if (_groundCheck.OnGround)
            {
                MyState = State.StandDanger;
            }
        }

        #endregion
        #region Collision
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_state == State.Dead || _state == State.Finish) return;
            if (collision.tag == Const.TAG_FINISH)
            {
                MyState = State.Finish;
                _rigid2D.velocity = new Vector2(0,_rigid2D.velocity.y);
                SoundManager.Instance?.OnPlaySound(SoundType.Portal);

                if (collision.name.Contains("lose"))
                {
                    _skeleton.AnimationState.SetAnimation(0, "Lose", false);
                    StartCoroutine(IENext());

                    IEnumerator IENext()
                    {
                        yield return new WaitForSeconds(1.5f);
                       
                    }
                }

                if (collision.name.Contains("Imprision"))
                {
                    this.OnPlayAnim("Set_free", $"Win{Random.Range(1,4)}", () =>
                    {
                        _skeleton.AnimationName = $"Win{Random.Range(1, 4)}";
                        
                    });
                }

                 
            }
            else if (collision.tag == Const.TAG_FLOOR)
            {
                MyState = State.Dead;
                StartCoroutine(IESpawn(0.5f));
                SoundManager.Instance?.OnPlaySound(SoundType.Fall);

            }
            else if (collision.tag == Const.TAG_THROW_GROUND)
            {
                collision.gameObject.GetComponent<ThrowGround>()?.OnPlayAnim();
                this.OnOtherPushMe(State.Push, Vector2.right * 600f);
                MyState = State.Push;
            }
            else if (collision.tag == Const.TAG_ARROW)
            {
                this.OnOtherPushMe(State.Push, collision.transform.up * -1000f);
                MyState = State.Push;
            }
            else if (collision.tag == Const.TAG_FADE_SWITCH)
            {
                FadeSwitchCtrl ctrl = collision.GetComponent<FadeSwitchCtrl>();
                if (!ctrl.HasShow)
                {
                    _rigid2D.velocity = new Vector2(0,_rigid2D.velocity.y);
                    MyState = State.WallJump;
                    _skeleton.AnimationName = "Idle1";
                    StartCoroutine(IEPressSwitch());

                    IEnumerator IEPressSwitch()
                    {
                        yield return new WaitForSeconds(0.5f);
                        this.OnPlayAnim("Press_button", "Idle1", () =>
                        {
                            MyState = State.Idle;
                            ctrl.OnShowPlatform();
                        });
                    }

                }
            }
            else if(collision.tag == Const.TAG_TRAP_BOX)
            {
                MyState = State.Dead;
                _myCollider.enabled = false;
                _wallCheck.gameObject.SetActive(false);
                _skeleton.AnimationName = "Die";
                StartCoroutine(IESpawn(2f));
            }
            else if(collision.tag == Const.TAG_CHECK_POINT)
            {
                _openDoor = collision.transform;
            }
            else if(collision.tag == Const.TAG_TRY_SKIN)
            {
                TrySkinCtrl ctrl = collision.GetComponent<TrySkinCtrl>();
                if(ctrl.Unlock == UnlockTypes.Free)
                {
                    OnGetSkin();
                }
                else
                {
                    this.OnUpMove();
                    
                }

                void OnGetSkin()
                {
                    this.OnChangeSkin(ctrl.SkinID, Color.white,ctrl.Hero);
                    ctrl.FxBuff.transform.position = this.transform.position + Vector3.up ;
                    ctrl.FxBuff.Play();
                    ctrl.OnChangeSkin();
                    _isDefaulSkin = false;
                }
            }
            else if(collision.tag == Const.TAG_PIKE)
            {
                this.OnDead();
            }
            else if(collision.tag == Const.TAG_ITEM)
            {
                if (collision.gameObject.name.Contains("WaterMelon"))
                    _countCoin += 5;
            }

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == Const.TAG_PUSH_GROUND )
            {
                if (_isFly) return;
                collision.gameObject.GetComponent<PushGround>()?.OnPlayAnim();
                this.OnOtherPushMe(State.Fly, Vector2.up * Const.JUMP_FORCE * 1.5f);
                SoundManager.Instance?.OnPlaySound(SoundType.Slime);
                _justFall = false;
            }
            else if (collision.gameObject.tag == Const.TAG_GROUND)
            {
                _isSlippery = collision.gameObject.name.Contains("ice");
                _isIronBars = collision.gameObject.name.Contains("iron");
                if (_state == State.Move)
                    this.UpdateStateAnimation();
            }
            else if (collision.gameObject.tag == Const.TAG_TNT)
            {
                _state = State.Dead;
                _myCollider.enabled = false;
                _wallCheck.gameObject.SetActive(false);
                //     Vector2 direct = (this.transform.position - collision.gameObject.transform.position).normalized;
                float sign = Mathf.Sign(this.transform.position.x - collision.gameObject.transform.position.x);
                Vector2 direct = new Vector2(sign, 1);
                _rigid2D.AddForce(direct * Const.JUMP_FORCE * 1.2f);
                _skeleton.AnimationName = "Fall Die";
                StartCoroutine(IESpawn(2f));
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            _oldPlatform = collision.gameObject;
        }

        private void OnOtherPushMe(State st, Vector2 force)
        {
            if (MyState == State.Fly || _state == State.Push) return;
            _isFly = true;
            MyState = st;
            _rigid2D.AddForce(force);

            StartCoroutine(IEFly());

            IEnumerator IEFly()
            {
                yield return new WaitForSeconds(0.2f);
                if (MyState == State.Fly)
                {
                    MyState = State.Jump;
                }
                _isFly = false;
            }
        }

        private void OnRandomSkin()
        {
            int sk = Random.Range(3, 70);
            this.OnChangeSkin("Skin" + sk, Color.white, HeroType.Default);

            //if (Random.Range(0, 100) > 60)
            //{
            //    this.OnChangeSkin("Skin2", new Color(1f, 0.394f, 0f, 1f),HeroType.Default);
            //}
            //else
            //{
            //    Color cl = Color.red;
            //    int random = Random.Range(0, 100);
            //    if (random < 25)
            //    {
            //        cl = Color.green;
            //    }
            //    else if (random < 50)
            //    {
            //        cl = Color.yellow;
            //    }
            //    else if (random < 75)
            //    {
            //        cl = new Color(37f / 255f, 193 / 255f, 1f, 1f);
            //    }
                
            //    this.OnChangeSkin("Skin1", cl, HeroType.Default);
            //}
        }

        private void OnChangeSkin(string skin,Color cl, HeroType hero)
        {
            _skeleton.Skeleton.SetSkin(skin);
            _skeleton.Skeleton.SetSlotsToSetupPose();
            _skeleton.LateUpdate();
            _skeleton.Skeleton.SetColor(cl);

            foreach(FxPremium fx in _fxPremiums)
            {
                fx.Fx.SetActive(fx.Hero == hero);
            }
        }


        private void OnPlayAnim(string curent, string next, UnityAction callback)
        {
            var current2 = _skeleton.AnimationState.SetAnimation(0, curent, false);
            current2.Complete += (t) =>
            {
                callback?.Invoke();
                if (_state == State.Idle)
                    _skeleton.AnimationState.SetAnimation(0, next, true);

            };

        }

        private void OnDead()
        {
            _state = State.Finish;
            this.OnStop();
            _rigid2D.velocity = Vector2.zero;
            //       _myCollider.enabled = false;
            _wallCheck.gameObject.SetActive(false);
            _skeleton.AnimationState.SetAnimation(0, "Lose", false);
            StartCoroutine(IESpawn(2f));

        }

        protected bool ObstacleAhead
        {
            get { return _obstacleAhead; }
            set
            {
                if (_obstacleAhead != value)
                {
                    _obstacleAhead = value;
                    this.UpdateStateAnimation();
                }
            }
        }

        public void OnSpringPush(float force)
        {
            _rigid2D.velocity = Vector2.zero;
            this.OnOtherPushMe(State.Fly, Vector2.up * Const.JUMP_FORCE * force);
            SoundManager.Instance?.OnPlaySound(SoundType.Slime);
            _justFall = false;
           

        }

        #endregion
    }

    [System.Serializable]
    public class FxPremium
    {
        public HeroType Hero;
        public GameObject Fx;
    }
}
