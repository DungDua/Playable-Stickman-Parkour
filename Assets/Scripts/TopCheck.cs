using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Skymare
{
    public class TopCheck : MonoBehaviour
    {

        private UnityAction<WallType> _evtWallJump;
        private UnityAction<GameObject> _evtPendulum;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == Const.TAG_GROUND)
            {
                if (collision.name.Contains("Candle"))
                {
                    _evtWallJump?.Invoke(WallType.Candle);
                }
                else if (collision.name.Contains("pendulum"))
                {
                    _evtPendulum?.Invoke(collision.gameObject);
                }
            }
        }

        public void OnSetEvent(UnityAction<WallType> evt, UnityAction<GameObject> pendulum)
        {
            _evtWallJump = evt;
            _evtPendulum = pendulum;
        }
    }
}