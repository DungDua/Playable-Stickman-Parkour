using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace Skymare
{
    public class FadeSwitchCtrl : MonoBehaviour
    {

        private bool _hasShow;
        public bool HasShow => _hasShow;

        [SerializeField] private List<GameObject> _listPlatform;

        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < _listPlatform.Count; i++)
                _listPlatform[i].SetActive(false);
        }


        public void OnShowPlatform()
        {
            _hasShow = true;
            StartCoroutine(IEFadeinPlatform());
            StartCoroutine(IEFadeOutPlatform());
        }

        private IEnumerator IEFadeinPlatform()
        {
            for(int i = 0; i < _listPlatform.Count; i++)
            {
                GameObject platform = _listPlatform[i];
                platform.SetActive(true);
                platform.transform.localScale = Vector3.one * 0.8f;
                platform.transform.DOScale(Vector3.one, 0.5f);
                SpriteRenderer render = platform.GetComponent<SpriteRenderer>();
                render.color = new Color(1f, 1f, 1f, 0f);
                render.DOFade(1f, 0.5f);

                yield return new WaitForSeconds(0.7f);
            }
        }

        private IEnumerator IEFadeOutPlatform()
        {
            yield return new WaitForSeconds(3f);

            _hasShow = false;

            for (int i = 0; i < _listPlatform.Count; i++)
            {
                GameObject platform = _listPlatform[i];
                platform.transform.DOScale(Vector3.one * 0.8f, 0.5f);
                SpriteRenderer render = platform.GetComponent<SpriteRenderer>();
                render.DOFade(0f, 0.5f).OnComplete(()=> { platform.SetActive(false); });

                yield return new WaitForSeconds(0.7f);
            }
        }
    }
}