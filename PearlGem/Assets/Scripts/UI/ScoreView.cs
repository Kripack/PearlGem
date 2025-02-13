using TMPro;
using UnityEngine;

namespace PearlGem.UI
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] Score scoreComponent;
        
        void OnEnable()
        {
            GlobalEventBus.Instance.OnScoreIncrease += UpdateView;
        }

        void UpdateView(int amount)
        {
            scoreText.text = $"{scoreComponent.CurrentLevelPoints.ToString()}/{scoreComponent.MaxLevelPoints.ToString()}";
        }
        
        void OnDisable()
        {
            GlobalEventBus.Instance.OnScoreIncrease -= UpdateView;
        }
    }
}