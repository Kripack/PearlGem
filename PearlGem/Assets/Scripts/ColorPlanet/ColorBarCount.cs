using UnityEngine;

namespace PearlGem.ColorPlanet
{
    [System.Serializable]
    public class ColorBallCount
    {
        public readonly Color Color;
        public readonly int Count;

        public ColorBallCount(Color color, int count)
        {
            Color = color;
            Count = count;
        }
    }

}