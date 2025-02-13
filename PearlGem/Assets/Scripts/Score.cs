using UnityEngine;

namespace PearlGem
{
    [DefaultExecutionOrder(-1)]
    public class Score : MonoBehaviour
    {
        [field:SerializeField] public int TotalPoints { get; private set; }
        [field:SerializeField] public int CurrentLevelPoints { get; private set; }
        [field:SerializeField] public int MaxLevelPoints { get; private set; }

        void OnEnable()
        {
            GlobalEventBus.Instance.OnScoreIncrease += IncreaseLevelScore;
        }

        void Start()
        {
            TotalPoints = PlayerPrefs.GetInt("TotalScore");
        }

        public void SetNeededLevelScore(int amount)
        {
            MaxLevelPoints = amount;
        }
        
        public void RefreshPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            TotalPoints = 0;
        }
        
        void IncreaseTotalScore(int amount)
        {
            TotalPoints += amount;
            PlayerPrefs.SetInt("TotalScore", TotalPoints);
        }

        void IncreaseLevelScore(int amount)
        {
            CurrentLevelPoints += amount;

            if (CurrentLevelPoints >= MaxLevelPoints)
            {
                IncreaseTotalScore(MaxLevelPoints);
                GlobalEventBus.Instance.TriggerLevelWin();
            }
        }
        
        void OnDisable()
        {
            GlobalEventBus.Instance.OnScoreIncrease -= IncreaseLevelScore;
        }
    }
}