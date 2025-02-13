using PearlGem.UI;
using UnityEngine;

namespace PearlGem
{
    public class LevelLoseHandler : MonoBehaviour
    {
        [SerializeField] Popup losePopup;

        bool _isWin;
        
        void OnEnable()
        {
            GlobalEventBus.Instance.OnLevelLose += OnLose;
            GlobalEventBus.Instance.OnLevelWin += SetWin;
        }

        void SetWin()
        {
            _isWin = true;
        }

        void OnLose()
        {
            if (_isWin) return;
            losePopup.Show();
        }
        
        void OnDisable()
        {
            GlobalEventBus.Instance.OnLevelLose -= OnLose;
            GlobalEventBus.Instance.OnLevelWin -= SetWin;
        }
    }
}