using System.Collections.Generic;
using UnityEngine;

namespace PearlGem.ColorPlanet
{
    public class PlanetGenerator : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int totalSegments = 100;
        [SerializeField] float planetRadius = 5f;
        [SerializeField] List<Color> availableColors;
        [SerializeField] int clusterCount = 5;
        [SerializeField] float minClusterDistance = 2f;
        [SerializeField] ColorSegment colorSegmentPrefab;
        
        [Header("Point distribution optimization")]
        [SerializeField] bool useOptimizePosition;
        [SerializeField] int optimizationIterations = 50;
        [SerializeField] float optimizationStep = 0.1f;
        [SerializeField] float repulsionExponent = 3f;
        
        float _computedSegmentRadius;
        
        List<ColorSegment> _segments = new ();
        List<ColorCluster> _clusters = new ();

        void Start()
        {
            GeneratePlanet();
        }

        public void UpdateParameters(List<Color> colors, int segments, float radius, int clusters)
        {
            availableColors = colors;
            totalSegments = segments;
            planetRadius = radius;
            clusterCount = clusters;
        }
        
        [ContextMenu("Generate Planet")]
        public float GeneratePlanet()
        {
            ClearOldSegments();
            
            _computedSegmentRadius = 2 * planetRadius / Mathf.Sqrt(totalSegments);

            GenerateColorClusters();
            
            Vector3[] positions = new Vector3[totalSegments];
            float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
            float angleIncrement = 2 * Mathf.PI * goldenRatio;

            for (int i = 0; i < totalSegments; i++)
            {
                float t = (float)i / totalSegments; // or (i + 0.5f) / totalSegments;
                float inclination = Mathf.Acos(1 - 2 * t);
                float azimuth = angleIncrement * i;

                float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth) * planetRadius;
                float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth) * planetRadius;
                float z = Mathf.Cos(inclination) * planetRadius;

                positions[i] = new Vector3(x, y, z);
            }
            
            if(useOptimizePosition) OptimizePositions(positions);
            
            for (int i = 0; i < totalSegments; i++)
            {
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, positions[i].normalized);
                CreateSegment(positions[i], rotation);
            }
            
            return _computedSegmentRadius;
        }

        void ClearOldSegments()
        {
            foreach (var segment in _segments)
            {
                Destroy(segment.gameObject);
            }
            _segments.Clear();
            _clusters.Clear();
        }
        
        void OptimizePositions(Vector3[] positions)
        {
            int n = positions.Length;
            for (int iter = 0; iter < optimizationIterations; iter++)
            {
                Vector3[] displacement = new Vector3[n];
                for (int i = 0; i < n; i++)
                {
                    displacement[i] = Vector3.zero;
                }
                
                for (int i = 0; i < n; i++)
                {
                    for (int j = i + 1; j < n; j++)
                    {
                        Vector3 diff = positions[i] - positions[j];
                        float dist = diff.magnitude;
                        float eps = 0.0001f;
                        if (dist < eps) dist = eps;
                        
                        float forceMagnitude = 1f / Mathf.Pow(dist, repulsionExponent);
                        Vector3 force = diff.normalized * forceMagnitude;
                        displacement[i] += force;
                        displacement[j] -= force;
                    }
                }
                
                for (int i = 0; i < n; i++)
                {
                    Vector3 normal = positions[i].normalized;
                    Vector3 tangentForce = displacement[i] - Vector3.Dot(displacement[i], normal) * normal;
                    
                    positions[i] += optimizationStep * tangentForce;
                    positions[i] = positions[i].normalized * planetRadius;
                }
            }
        }

        void CreateSegment(Vector3 position, Quaternion rotation)
        {
            var segment = Instantiate(colorSegmentPrefab, position, rotation, transform);
            segment.transform.localScale = Vector3.one * _computedSegmentRadius * 2;
            _segments.Add(segment);
            
            ColorCluster nearestColorCluster = null;
            float minDistance = Mathf.Infinity;
            foreach (var cluster in _clusters)
            {
                float distance = Vector3.Distance(position, cluster.Center);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestColorCluster = cluster;
                }
            }
            
            if (nearestColorCluster != null)
            {
                nearestColorCluster.Segments.Add(segment);
                segment.SetColorCluster(nearestColorCluster);
            }
        }

        void GenerateColorClusters()
        {
            for (int i = 0; i < clusterCount; i++)
            {
                Vector3 center = FindValidClusterPosition();
                ColorCluster colorCluster = new ColorCluster(center, GetUniqueClusterColor());
                _clusters.Add(colorCluster);
            }
        }

        Vector3 FindValidClusterPosition()
        {
            Vector3 center;
            int attempts = 0;
            do
            {
                center = Random.onUnitSphere * planetRadius;
                attempts++;
            } while (!IsClusterPositionValid(center) && attempts < 300);

            return center;
        }

        bool IsClusterPositionValid(Vector3 position)
        {
            foreach (var cluster in _clusters)
            {
                if (Vector3.Distance(position, cluster.Center) < minClusterDistance)
                {
                    return false;
                }
            }
            return true;
        }

        Color GetUniqueClusterColor()
        {
            List<Color> forbiddenColors = new List<Color>();
            foreach (var cluster in _clusters)
            {
                forbiddenColors.Add(cluster.Color);
            }

            List<Color> allowedColors = new List<Color>(availableColors);
            foreach (var color in forbiddenColors)
            {
                allowedColors.Remove(color);
            }

            return (allowedColors.Count > 0)
                ? allowedColors[Random.Range(0, allowedColors.Count)]
                : availableColors[Random.Range(0, availableColors.Count)];
        }
    }
}
