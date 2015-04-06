﻿using SanAndreasUnity.Importing.Vehicles;
using UnityEngine;
using VConsts = SanAndreasUnity.Behaviours.Vehicles.VehiclePhysicsConstants;

namespace SanAndreasUnity.Behaviours.Vehicles
{
    public partial class Vehicle
    {
        private Rigidbody _rigidBody;

        [Range(-1, 1)]
        public float Accelerator;

        [Range(-1, 1)]
        public float Steering;

        public Handling.Car HandlingData { get; private set; }

        private void InitializePhysics()
        {
            _geometryParts.AttachCollisionModel(transform, true);

            _rigidBody = gameObject.AddComponent<Rigidbody>();

            HandlingData = Handling.Get<Handling.Car>(Definition.HandlingName);

            VConsts.Changed += UpdateValues;

            _rigidBody.mass = HandlingData.Mass;
            _rigidBody.centerOfMass = HandlingData.CentreOfMass;

            foreach (var wheel in _wheels)
            {
                wheel.Collider = wheel.Parent.gameObject.AddComponent<WheelCollider>();
                wheel.Collider.radius = 0.35f;
                wheel.Collider.suspensionDistance = 0.1f;

                var spring = wheel.Collider.suspensionSpring;
                spring.targetPosition = 0.5f;
                spring.damper = HandlingData.SuspensionDampingLevel * VConsts.Instance.SuspensionDampingScale;
                spring.spring = HandlingData.SuspensionForceLevel * VConsts.Instance.SuspensionForceScale;
                wheel.Collider.suspensionSpring = spring;

                var friction = wheel.Collider.forwardFriction;
                friction.extremumSlip = 0.6f;
                friction.extremumValue = 1.2f;
                friction.asymptoteSlip = 2.0f;
                friction.asymptoteValue = 0.2f;
                friction.stiffness = 1;

                wheel.Collider.forwardFriction = friction;
                wheel.Collider.sidewaysFriction = friction;

            }
        }

        private void UpdateValues(VConsts vals)
        {
            _rigidBody.drag = HandlingData.Drag * vals.DragScale;
            _rigidBody.mass = HandlingData.Mass * vals.MassScale;

            foreach (var wheel in _wheels) {
                var spring = wheel.Collider.suspensionSpring;
                spring.damper = HandlingData.SuspensionDampingLevel * VConsts.Instance.SuspensionDampingScale;
                spring.spring = HandlingData.SuspensionForceLevel * VConsts.Instance.SuspensionForceScale;
                wheel.Collider.suspensionSpring = spring;
            }
        }

        private void FixedUpdate()
        {
            foreach (var wheel in _wheels)
            {
                if (wheel.Alignment == WheelAlignment.RightFront ||
                    wheel.Alignment == WheelAlignment.LeftFront)
                {
                    wheel.Collider.steerAngle = HandlingData.SteeringLock * Steering;
                }
               
                wheel.Collider.motorTorque = Accelerator
                    * HandlingData.TransmissionEngineAccel
                    * VConsts.Instance.AccelerationScale;
            }
        }
    }
}