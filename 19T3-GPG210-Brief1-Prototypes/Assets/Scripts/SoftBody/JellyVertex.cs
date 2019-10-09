﻿using UnityEngine;

namespace SoftBody
{
    public class JellyVertex
    {
        public int verticeIndex;
        public Vector3 initialVertexPosition;
        public Vector3 currentVertexPosition;

        public Vector3 currentVelocity;


        public JellyVertex(int verticeIndex, Vector3 initialVertexPosition, Vector3 currentVertexPosition, Vector3 currentVelocity)
        {
            this.verticeIndex = verticeIndex;
            this.initialVertexPosition = initialVertexPosition;
            this.currentVertexPosition = currentVertexPosition;
            this.currentVelocity = currentVelocity;
        }

        public Vector3 GetCurrentDisplacement()
        {
            return currentVertexPosition - initialVertexPosition;
        }

        public void UpdateVelocity(float _bounceSpeed)
        {
            currentVelocity -= GetCurrentDisplacement() * _bounceSpeed * Time.deltaTime;
        }

        public void Settle(float _stiffness)
        {
            currentVelocity *= 1f - _stiffness * Time.deltaTime;
        }
        
        public void ApplyPressureToVertex(Transform _transform, Vector3 _position, float _pressure, bool ignoreY = false)
        {
            Vector3 distanceVerticePoint = currentVertexPosition - _transform.InverseTransformPoint(_position);
            if (ignoreY)
                distanceVerticePoint.y = 0;
            float adaptedPressure = _pressure / (1f + distanceVerticePoint.sqrMagnitude);
            float velocity = adaptedPressure * Time.deltaTime;

            currentVelocity += distanceVerticePoint.normalized * velocity;
        }
    }
}