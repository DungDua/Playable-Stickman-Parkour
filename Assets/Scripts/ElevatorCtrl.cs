using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skymare
{

    public enum Direction
    {
        Vertical, Horizontal, Circle
    }

    public class ElevatorCtrl : MonoBehaviour
    {
        [SerializeField] private Direction _direct;
        [SerializeField] private float _speed;
        [SerializeField] private float _timeDelay;
        [SerializeField] private float _radius;
        [SerializeField] private List< ElevatorCtrl> _nearElevator;


        private Transform _elevator;
        private Transform _pointA;
        private Transform _pointB;
        private bool _isTargetA;
        private Transform _currentTarget;
        private float _abs;
        private float _countDelay;
        private LineRenderer _lineCircle;
        private bool _hasDelay;

        // Start is called before the first frame update
        void Start()
        {
            _elevator = this.transform.GetChild(0);
            _pointA = this.transform.GetChild(1);
            _pointB = this.transform.GetChild(2);

            _isTargetA = true;
            _currentTarget = _pointA;
            _abs = _direct == Direction.Vertical ? 1f : 0.05f;
        }

        // Update is called once per frame
        void Update()
        {
            if (_hasDelay) return;

            if(Vector2.Distance(_elevator.position, _currentTarget.position) <= _abs)
            {
                _countDelay += Time.deltaTime;
                if (_countDelay >= _timeDelay)
                {
                    _isTargetA = !_isTargetA;
                    _currentTarget = _isTargetA ? _pointA : _pointB;
                    _countDelay = 0f;
                }
            }
            else
            {
                if (_direct == Direction.Circle)
                {
                    _elevator.Rotate(Vector3.forward * _speed * Time.deltaTime);
                    _elevator.GetChild(0).localEulerAngles = Vector3.forward * ( 360f - _elevator.localEulerAngles.z );
                }
                else
                    _elevator.position = Vector2.MoveTowards(_elevator.position, _currentTarget.position, Time.deltaTime * _speed);
            }
        }


        private void OnValidate()
        {
            _elevator = this.transform.GetChild(0);
            _lineCircle = this.GetComponent<LineRenderer>();
            GameObject line = this.transform.GetChild(3).gameObject;

            if(_direct == Direction.Circle)
            {
                this.DrawCircle(100, _radius);
                _elevator.GetChild(0).localPosition = Vector3.right * _radius;
                line.SetActive(false);
            }
            else
            {
                _lineCircle.positionCount = 0;
                _elevator.GetChild(0).localPosition = Vector3.zero;
                _elevator.GetChild(0).localEulerAngles = Vector3.forward * 0f;
                line.SetActive(true);
            }
        }


        private void DrawCircle(int step, float radius)
        {
            _lineCircle.positionCount = step;
            for(int i = 0; i < step; i++)
            {
                float progress = (float)i / step;
                float radian = progress * 2 * Mathf.PI;
                float xScale = Mathf.Cos(radian);
                float yScale = Mathf.Sin(radian);

                float x = xScale * radius;
                float y = yScale * radius;

                Vector3 v1 = new Vector3(x, y, 0);
                _lineCircle.SetPosition(i, v1);
            }
        }

        public void OnSetDelay(bool delay)
        {
            _hasDelay = delay;
            if(_nearElevator.Count > 0)
            {
                for(int i = 0; i < _nearElevator.Count; i++)
                {
                    _nearElevator[i]?.OnDelayToo(delay);
                }
            }
            
        }

        public void OnDelayToo(bool delay)
        {
            _hasDelay = delay;
        }
    }
}