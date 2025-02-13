using UnityEngine;

namespace PearlGem.Projectile
{
    public class TrajectoryPrediction : MonoBehaviour

    {
        [Header("Prediction Trajectory Settings")]
        [SerializeField] LineRenderer trajectoryLine;
        [SerializeField] int trajectoryPointsCount = 30;
        [SerializeField] float trajectoryTimeStep = 0.1f;
        
        public void DrawTrajectory(Vector3 launchPoint, Vector3 initialVelocity)
        {
            Vector3[] trajectoryPoints = new Vector3[trajectoryPointsCount];
            Vector3 currentPosition = launchPoint;
            Vector3 currentVelocity = initialVelocity;

            for (int i = 0; i < trajectoryPointsCount; i++)
            {
                trajectoryPoints[i] = currentPosition;
                
                if (Physics.Raycast(currentPosition, currentVelocity.normalized, out RaycastHit hit, currentVelocity.magnitude * trajectoryTimeStep))
                {
                    trajectoryLine.positionCount = i + 1;
                    trajectoryLine.SetPositions(trajectoryPoints);
                    return;
                }
                
                currentPosition += currentVelocity * trajectoryTimeStep;
                currentVelocity += Physics.gravity * trajectoryTimeStep;
            }

            trajectoryLine.positionCount = trajectoryPointsCount;
            trajectoryLine.SetPositions(trajectoryPoints);
        }


        public void HideTrajectory()
        {
            trajectoryLine.positionCount = 0;
        }
    }
}