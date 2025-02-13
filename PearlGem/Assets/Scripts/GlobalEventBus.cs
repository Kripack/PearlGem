using System;
using UnityEngine;

namespace PearlGem
{
    [DefaultExecutionOrder(-10)]
    public class GlobalEventBus : MonoBehaviour
    {
        public static GlobalEventBus Instance { get; private set; }
        
        public event Action OnLevelWin;
        public event Action OnLevelLose;
        public event Action OnProjectilesAreOver;
        public event Action<int> OnScoreIncrease;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            } 
            else 
            {
                Instance = this;
            }
        }

        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void TriggerLevelLose()
        {
            Debug.Log("Lose!");
            OnLevelLose?.Invoke();
        }

        public void TriggerLevelWin()
        {
            OnLevelWin?.Invoke();
        }

        public void TriggerScoreIncrease(int amount)
        {
            OnScoreIncrease?.Invoke(amount);
        }

        public void TriggerProjectilesAreOver()
        {
            OnProjectilesAreOver?.Invoke();
        }
    }
}