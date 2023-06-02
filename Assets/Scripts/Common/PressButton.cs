using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;


namespace Skymare
{
    public class PressButton : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        public  void OnPointerDown(PointerEventData eventData)
        {
            this.transform.DOScale(Vector3.one * 1.1f, 0.1f);
        }

        public  void OnPointerUp(PointerEventData eventData)
        {
            this.transform.DOScale(Vector3.one, 0.1f);
        }
    }
}