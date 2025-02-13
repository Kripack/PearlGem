using UnityEngine;

namespace PearlGem.ColorPlanet
{
    public interface IColorTarget
    {
        public bool TryToDrop(Color color);
    }
}