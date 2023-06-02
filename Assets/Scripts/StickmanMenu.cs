using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

namespace Skymare
{
    public class StickmanMenu : MonoBehaviour
    {

        [SerializeField] protected SkeletonAnimation _skeleton;
        [SerializeField] protected Animator _animUpgrade;
        [SerializeField] protected List<Unlock> _unlocks;
        [SerializeField] protected ColliderButton _btnUpgrade;

        private float _countIdle, _timeIdle;
        private bool _idleDone;
        protected bool _hasUnlock;
        protected int _stepUpgrade;
        protected bool _cleaTrack;


        // Start is called before the first frame update
        protected virtual void Start()
        {
            _timeIdle = Random.Range(0f, 5f);
            _idleDone = true;
            _btnUpgrade?.SetEventClick(this.OnClickUpgrade);
            _btnUpgrade.gameObject.SetActive(false);
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (_hasUnlock) return;

            _countIdle += Time.deltaTime;
            if(_countIdle >= _timeIdle && _idleDone)
            {
                _idleDone = false;
                var current = _skeleton.AnimationState.SetAnimation(0, $"Menu Movement/Idle{Random.Range(2, 8)}", false);
                current.Complete += (t) =>
                {
                    if (_hasUnlock) return;
                    _timeIdle = Random.Range(0f, 5f);
                    _countIdle = 0f;
                    _idleDone = true;
                    _skeleton.AnimationState.SetAnimation(0, "Menu Movement/Idle1", true);
                };
            }
        }

        protected virtual void OnClickUpgrade()
        {

        }

        public virtual void ShowHint()
        {
            _btnUpgrade.gameObject.SetActive(true);
        }

        public void HideHint()
        {
            _animUpgrade.SetBool("hint", false);
            if(_btnUpgrade.gameObject.activeInHierarchy)
                _btnUpgrade.OnHide();
        }
    }

    [System.Serializable]
    public class Unlock
    {
        public UnlockTypes Type;
        public int Price;
    }
}