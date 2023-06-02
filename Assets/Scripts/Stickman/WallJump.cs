using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum WallType
{
    Ground,Climb,Monkey,Candle, Pendulum, Elevator, Dash
}

namespace Skymare
{
    public class WallJump : MonoBehaviour
    {

        private UnityAction<WallType> _evtWallJump;


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == Const.TAG_GROUND)
            {
                if (collision.name.Contains("ClimbWall"))
                {
                    _evtWallJump?.Invoke(WallType.Climb);
                }
                else if (collision.name.Contains("MonkeyWall"))
                {
                    _evtWallJump?.Invoke(WallType.Monkey);
                }
                else if (collision.name.Contains("Candle") || collision.name.Contains("pendulum"))
                {

                }
                else
                {
                    if (collision as BoxCollider2D)
                    {
                        Vector2 colPos = (Vector2)collision.transform.position + collision.offset;
                        BoxCollider2D box = (BoxCollider2D)collision;
                        float y = box.size.y / 2f;

                        if (this.transform.position.y + 0.2f > colPos.y + y)
                            _evtWallJump?.Invoke(WallType.Ground);

                        if (collision.gameObject.name.Contains("Container"))
                        {
                            Elevator ele = collision.gameObject.GetComponent<Elevator>();
                            ele?.OnStopElevator();
                        }
                        else if (collision.name.Contains("platform"))
                        {
                            AddChildPlatform platform = collision.GetComponent<AddChildPlatform>();
                            platform?.OnStop();
                        }
                    }
                }
            }
            else if(collision.tag == Const.TAG_DASH)
            {
                _evtWallJump?.Invoke(WallType.Dash);
            }

        }

        public void OnSetEvent(UnityAction<WallType> act)
        {
            _evtWallJump = act;
        }

    }
}