using System.Collections.Generic;
using PearlGem.ColorPlanet;
using PearlGem.Projectile;
using UnityEngine;

namespace PearlGem.Utils
{
    public class LevelBuilder : MonoBehaviour
    {
        [Header("Level Settings")] 
        [SerializeField] int planetSegments = 300;
        [SerializeField] float planetRadius = 2f;
        [SerializeField] int numberOfColors = 3;
        [SerializeField] int numberOfColorClusters = 4;
        [SerializeField] int ballsPerColor = 5;

        [Header("Component Links")]
        [SerializeField] ColorGenerator colorGenerator;
        [SerializeField] PlanetGenerator planetGenerator;
        [SerializeField] ProjectilePool projectilePool;
        [SerializeField] Score levelScore;

        void Start()
        {
            SetupLevel();
        }
        
        [ContextMenu("SetupLevel")]
        void SetupLevel()
        {
            List<Color> uniqueColors = colorGenerator.GenerateUniqueColors(numberOfColors);
            
            planetGenerator.UpdateParameters(uniqueColors, planetSegments, planetRadius, numberOfColorClusters);
            var computedSegmentRadius = planetGenerator.GeneratePlanet();
            
            SetupProjectiles(uniqueColors, computedSegmentRadius);
            SetupLevelScore();
        }

        void SetupProjectiles(List<Color> uniqueColors, float computedSegmentRadius)
        {
            List<ColorBallCount> inventory = new List<ColorBallCount>();
            foreach (Color col in uniqueColors)
            {
                ColorBallCount item = new ColorBallCount(col, ballsPerColor);
                inventory.Add(item);
            }
            projectilePool.SetBallInventory(inventory, computedSegmentRadius);
        }

        void SetupLevelScore()
        {
            levelScore.SetNeededLevelScore(planetSegments);
        }
    }

}