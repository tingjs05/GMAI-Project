using UnityEngine;

namespace PGGE
{
    // The base class for all third-person camera controllers
    public abstract class TPCBase
    {
        protected Transform mCameraTransform;
        protected Transform mPlayerTransform;
        protected Vector3 desiredPosition;

        // reposition camera
        private float obstacleOffset = 0.5f;
        private LayerMask obstacleLayer = LayerMask.GetMask("Obstacles");

        public Transform CameraTransform
        {
            get
            {
                return mCameraTransform;
            }
        }
        public Transform PlayerTransform
        {
            get
            {
                return mPlayerTransform;
            }
        }

        public TPCBase(Transform cameraTransform, Transform playerTransform)
        {
            mCameraTransform = cameraTransform;
            mPlayerTransform = playerTransform;
        }

        public void RepositionCamera()
        {
            //-------------------------------------------------------------------
            // Implement here.
            //-------------------------------------------------------------------
            //-------------------------------------------------------------------
            // Hints:
            //-------------------------------------------------------------------
            // check collision between camera and the player.
            // find the nearest collision point to the player
            // shift the camera position to the nearest intersected point
            //-------------------------------------------------------------------

            Transform obstacleTransform = CheckObstacles();

            // get camera offset so camera does not clip into the obstacle
            if (obstacleTransform == null)
                return;

            // offset desired position away from the obstacle, and scale offset depending on player direction from the obstacle
            desiredPosition += GetOffset(obstacleTransform);
        }

        public void MoveToDesiredPosition()
        {
            // reposition camera after all calculations
            mCameraTransform.position = Vector3.Lerp(mCameraTransform.position, desiredPosition, Time.deltaTime * CameraConstants.Damping);
        }

        public abstract void Update();

        private Transform CheckObstacles()
        {
            // find starting point of raycast: match camera x and z positions, lock y-axis to player y position
            Vector3 rayStartPos = new Vector3(desiredPosition.x, mPlayerTransform.position.y, desiredPosition.z);
            // find direction and distance of raycast
            Vector3 raycastDirection = (desiredPosition - rayStartPos).normalized;
            float distanceFromTarget = Vector3.Distance(desiredPosition, rayStartPos) + obstacleOffset;

            // raycast variables
            RaycastHit hit;
            Transform obstacleTransform = null;

            // check vertical area for obstacles
            if (Physics.Raycast(rayStartPos, raycastDirection, out hit, distanceFromTarget, obstacleLayer) && hit.distance > obstacleOffset)
            {
                // show raycast
                Debug.DrawRay(rayStartPos, raycastDirection * hit.distance, Color.magenta);
                // set the vector to reposition camera y positions
                desiredPosition.y = hit.point.y - obstacleOffset;
                // set obstacle transform if obstacle is found
                obstacleTransform = hit.transform;
            }

            // get starting point, direction and distance of raycast
            rayStartPos = new Vector3(mPlayerTransform.position.x, desiredPosition.y, mPlayerTransform.position.z);
            raycastDirection = (desiredPosition - mPlayerTransform.position).normalized;
            raycastDirection = new Vector3(raycastDirection.x, 0f, raycastDirection.z);
            distanceFromTarget = Vector3.Distance(desiredPosition, rayStartPos) + obstacleOffset;

            // check horizontal area for obstacles
            if (Physics.Raycast(rayStartPos, raycastDirection, out hit, distanceFromTarget, obstacleLayer))
            {
                // show raycast
                Debug.DrawRay(rayStartPos, raycastDirection * hit.distance, Color.magenta);
                // set the vector to reposition camera x and z positions
                desiredPosition.x = hit.point.x;
                desiredPosition.z = hit.point.z;
                // set obstacle transform if obstacle is found
                obstacleTransform = hit.transform;
            }

            return obstacleTransform;
        }

        private Vector3 GetOffset(Transform obstacleTransform)
        {
            Vector3 offsetDirection;

            // get direction between player and camera
            Vector3 dir = mPlayerTransform.position - obstacleTransform.position;
            dir.y = 0f;
            // show direction
            Debug.DrawRay(obstacleTransform.position, dir, Color.yellow);
            // normalize direction
            dir = dir.normalized;
            
            // get dot product of horizontal axis
            float dot = Vector3.Dot(obstacleTransform.forward, dir);

            // check if player is in the front or back of wall
            if (!(dot > - 0.1 && dot < 0.1))
            {
                // set return vector
                offsetDirection = dot < 0? -obstacleTransform.forward : obstacleTransform.forward;
                // show direction
                Debug.DrawRay(obstacleTransform.position, offsetDirection * 10f, Color.blue);
                // exit method after getting one direction, and scale direction by offset and ratio of player direction from the obstacle
                return (1f - Vector3.Dot(mPlayerTransform.forward, offsetDirection)) * obstacleOffset * offsetDirection;
            }
            
            // check which side the player is beside the obstacle
            dot = Vector3.Dot(obstacleTransform.right, dir);
            // set return vector
            offsetDirection = dot < 0? -obstacleTransform.right : obstacleTransform.right;
            // show direction
            Debug.DrawRay(obstacleTransform.position, offsetDirection * 10f, Color.blue);
            // return direction after calculation, and scale direction by offset and ratio of player direction from the obstacle
            return (1f - Vector3.Dot(mPlayerTransform.forward, offsetDirection)) * obstacleOffset * offsetDirection;
        }
    }
}
