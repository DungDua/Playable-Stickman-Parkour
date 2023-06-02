using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

namespace Skymare
{
    public class PetFollower : MonoBehaviour
    {
        [SerializeField]private SkeletonAnimation _skeletonElement;

        private Transform _witcher;
        private string _cacheAnim;
        private float _timeCount, _timeIdle;
        private bool _randomState, _moveAround;
        private Vector3 _positionAround;
        private string _petName;


        // Start is called before the first frame update
        void Start()
        {
            _skeletonElement = this.transform.GetChild(0).GetComponent<SkeletonAnimation>();
        }

        // Update is called once per frame
        void Update()
        {
            float distance = Vector2.Distance(this.transform.position, _witcher.position);

            if(distance > 10f)
            {
                this.transform.position = _witcher.position;
            }
            if (distance >= 2f)
            {
                this.PlayAnim( "Action");
                Vector3 pos = Vector3.MoveTowards(this.transform.position, _witcher.position, Time.deltaTime * 5f);
                this.transform.position = pos;

                float euler = 1f;
                if (_witcher.position.x - this.transform.position.x <= -0.1f) euler = -1f;
                _skeletonElement.transform.localScale = new Vector3(euler, 1f, 1f);
                _randomState = false;
            }
            else
            {
                if (!_randomState)
                {
                    _randomState = true;
                    this.RandomMoveround();
                    _timeIdle = Random.Range(2f, 5f);
                }

                _timeCount += Time.deltaTime;
                if (_timeCount >= _timeIdle && !_moveAround)
                {
                    _randomState = false;
                    _timeCount = 0f;
                }

                if (_moveAround)
                {
                    Vector3 pos = Vector3.MoveTowards(this.transform.position, _positionAround, Time.deltaTime * 5f);
                    pos.z = pos.y;
                    this.transform.position = pos;

                    float euler = 1f;
                    if (_positionAround.x - this.transform.position.x <= -0.1f) euler = -1f;
                    _skeletonElement.transform.localScale = new Vector3(euler, 1f, 1f);

                    if (Vector2.Distance(this.transform.position, _positionAround) <= 0.2f)
                    {
                        this.RandomMoveround();
                    }

                    this.PlayAnim("Action");
                }
                else
                {
                    this.PlayAnim("Idle");
                }
            }
        }

        private void RandomMoveround()
        {
            _timeCount = 0f;
            _moveAround = Random.Range(0, 100) > 30;
            if (_moveAround)
            {
                _positionAround.x = _witcher.position.x + Random.Range(-1.9f, 1.9f);
                _positionAround.y = _witcher.position.y + Random.Range(-1f, 1f);
            }
        }

        private void PlayAnim(string anim)
        {
            if (anim.Equals(_cacheAnim)) return;
            _skeletonElement.AnimationName = _petName + "/" + anim;
            _cacheAnim = anim;
        }

        public void SetWitcher(Transform witcher, PetTypes pet)
        {
            _witcher = witcher;
            string skin = string.Empty;
            switch (pet)
            {
                case PetTypes.Axolotl:
                    skin = "axolotl";
                    _petName = "Axolotl";
                    break;
                case PetTypes.Camel:
                    skin = "camel";
                    _petName = "Camel";
                    break;
                case PetTypes.Chicken:
                    skin = "chicken";
                    _petName = "Chicken";
                    break;
                case PetTypes.Pig:
                    skin = "pig";
                    _petName = "Pig";
                    break;
                case PetTypes.Salmon:
                    skin = "fish";
                    _petName = "Fish";
                    break;
                case PetTypes.Bee:
                    skin = "bee";
                    _petName = "Bee";
                    break;
                case PetTypes.Fox:
                    skin = "fox";
                    _petName = "Fox";
                    break;
                case PetTypes.Parrot:
                    skin = "parrot";
                    _petName = "Parrot";
                    break;
                case PetTypes.Squid:
                    skin = "squid";
                    _petName = "Squid";
                    break;
                case PetTypes.Tiger:
                    skin = "tiger";
                    _petName = "Tiger";
                    break;
            }

            _skeletonElement.initialSkinName = skin;
            _skeletonElement.Initialize(true);
        }


    }
}