using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;


namespace Skymare
{
    public class StandCtrl : MonoBehaviour
    {
        private Transform _congligh;
        private bool _hasOpen;

        public UnityEvent EventOn, EventOff;

        // Start is called before the first frame update
        void Start()
        {
            _congligh = this.transform.GetChild(0);
        }



        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!_hasOpen)
            {
                _hasOpen = true;
                _congligh.DOLocalMoveY(-0.155f, 0.2f).OnComplete(() =>
                {
                    EventOn?.Invoke();
                });
            }           
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (_hasOpen)
            {
                _hasOpen = false;
                _congligh.DOLocalMoveY(0f, 0.2f).OnComplete(() =>
                {
                    EventOff?.Invoke();
                });
            }
        }
    }
}