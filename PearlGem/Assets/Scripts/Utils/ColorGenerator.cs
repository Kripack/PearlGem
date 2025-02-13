using System.Collections.Generic;
using UnityEngine;

namespace PearlGem.Utils
{
    public class ColorGenerator : MonoBehaviour
    {
        [SerializeField] List<Color> availableColors = new List<Color>()
        {
            Color.red,
            Color.green,
            Color.blue,
            Color.yellow,
            Color.magenta,
            Color.cyan,
            new Color(1f, 0.5f, 0f),
            new Color(0.5f, 0f, 1f)
        };
        
        public List<Color> GenerateUniqueColors(int count)
        {
            count = Mathf.Min(count, availableColors.Count);
            List<Color> available = new List<Color>(availableColors);
            List<Color> result = new List<Color>();

            for (int i = 0; i < count; i++)
            {
                int index = Random.Range(0, available.Count);
                result.Add(available[index]);
                available.RemoveAt(index);
            }

            return result;
        }
    }

}