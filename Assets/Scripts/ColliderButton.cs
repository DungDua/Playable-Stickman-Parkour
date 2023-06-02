using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Skymare
{
    public class ColliderButton : MonoBehaviour
    {

        [SerializeField] private GameObject _icAds;
        [SerializeField] private TextMeshPro _txtPrice;

        private Animator _myAnim;
        private UnityAction _evtClick;

        // Start is called before the first frame update
        void Awake()
        {
            _myAnim = this.GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _myAnim.Play("show");
        }

        private void OnMouseDown()
        {
            _evtClick?.Invoke();
            _myAnim.SetTrigger("click");
        }

        public void OnSetPrice(Unlock type)
        {
            if(type.Type == UnlockTypes.Ads)
            {
                _icAds.SetActive(true);
                _txtPrice.text = "WATCH AD";
            }
            else
            {
                _icAds.SetActive(false);
                _txtPrice.text = type.Price.ToString() + " <sprite=3>";
            }
        }

        public void SetEventClick(UnityAction evt)
        {
            _evtClick = evt;
        }

        public void OnHide()
        {
            _myAnim.Play("hide");
            StartCoroutine(IEHieMe());

            IEnumerator IEHieMe()
            {
                yield return new WaitForSeconds(0.55f);
                this.gameObject.SetActive(false);
            }
        }
    }
}