using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Skymare
{
    public class InputMgr : MonoBehaviour
    {
        public static UnityAction ActMoveLeft, ActMoveRight, ActUpMove,  ActJump;

        public void OnMoveLeft()
        {
            ActMoveLeft?.Invoke();
        }

        public void OnMoveRight()
        {
            ActMoveRight?.Invoke();
        }

        public void OnUpMove()
        {
            ActUpMove?.Invoke();
        }


        public void OnJump()
        {
            ActJump?.Invoke();
        }

    }
}