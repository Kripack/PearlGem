using System.Collections.Generic;
using UnityEngine;

namespace PearlGem.ColorPlanet
{
    public class ColorCluster
    {
        public List<ColorSegment> Segments;
        public readonly Vector3 Center;
        public readonly Color Color;

        public ColorCluster(Vector3 center, Color color)
        {
            Center = center;
            Color = color;
            Segments = new();
        }
    }
}