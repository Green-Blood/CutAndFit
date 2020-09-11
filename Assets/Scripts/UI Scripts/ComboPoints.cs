using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI_Scripts
{
    public class ComboPoints : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI comboText;
        private Sequence _sequence;

        private void Start()
        {
            _sequence = DOTween.Sequence();
        }

        public IEnumerator UpdateComboPoints(int comboPoints, float tweenTime)
        {
            comboText.text = "X " + comboPoints;
            _sequence.Append(comboText.transform.DOScale(1, tweenTime));
            yield return new WaitForSeconds(tweenTime);
            _sequence.Append(comboText.transform.DOScale(0, tweenTime));
        }
    }
}