using System;
using TMPro;
using UnityEngine;

namespace UI_Scripts
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nextLevelText;
        [SerializeField] private TextMeshProUGUI currentLevelText;
        [SerializeField] private GameController gameController;

        private void Start()
        {
            UpdateProgressBar();
        }

        public void UpdateProgressBar()
        {
            gameController.prgImg.fillAmount = 0f;
            
            int lvlNumber = GameController.currentLvlNumber;
            lvlNumber++;
            currentLevelText.text = lvlNumber.ToString();
            lvlNumber++;
            nextLevelText.text = lvlNumber.ToString();
        }
    }
}