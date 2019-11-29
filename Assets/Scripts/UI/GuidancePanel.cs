using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StormRend.UI
{
    public class GuidancePanel : MonoBehaviour
    {
        //Inspector
        [SerializeField] bool anyKeyCancel = true;
        [Range(0, 5f), SerializeField] float fadeInDuration = 2f;
        [Range(0, 5f), SerializeField] float duration = 2f;
        [Range(0, 5f), SerializeField] float fadeOutDuration = 2f;
        [Range(0, 5f), SerializeField] float quickfadeOutDuration = 1f;

        //Members
        Image BG = null;
        TextMeshProUGUI text;
        Color initBGCol = Color.black;
        Color initTextCol = Color.black;

        void Awake()
        {
            BG = GetComponent<Image>();
            text = GetComponentInChildren<TextMeshProUGUI>();

            //Record initial colors
            initBGCol = BG.color;
            initTextCol = text.color;

            //Clear to get ready for fade in
            BG.color = Color.clear;
            text.color = Color.clear;
        }

        void OnEnable()
        {
            StartCoroutine(FadeIn(fadeInDuration));
        }
        
        void Update()
        {
            if (anyKeyCancel && Input.anyKeyDown)
            {
                StopAllCoroutines();
                StartCoroutine(FadeOut(quickfadeOutDuration));
            }
        }

        IEnumerator FadeIn(float t)
        {
            float time = 0f;
            float rate = 1f / t;
            while (time < 1f)
            {
                time += rate * Time.deltaTime;

                BG.color = Color.Lerp(Color.clear, initBGCol, time);
                text.color = Color.Lerp(Color.clear, initTextCol, time);
                yield return null;
            }
            BG.color = initBGCol;
            text.color = initTextCol;
            StartCoroutine(Wait(duration));
        }

        IEnumerator Wait(float t)
        {
            yield return new WaitForSeconds(t);
            StartCoroutine(FadeOut(fadeOutDuration));
        }

        IEnumerator FadeOut(float t)
        {
            var currentBGCol = BG.color;
            var currentTextCol = text.color;

            float time = 0f;
            float rate = 1f / t;
            while (time < 1f)
            {
                time += rate * Time.deltaTime;

                BG.color = Color.Lerp(currentBGCol, Color.clear, time);
                text.color = Color.Lerp(currentTextCol, Color.clear, time);
                yield return null;
            }

            //Deactivate
            BG.color = Color.clear;
            text.color = Color.clear;
            this.gameObject.SetActive(false);
        }
    }
}