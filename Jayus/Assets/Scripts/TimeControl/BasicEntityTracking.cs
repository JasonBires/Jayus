using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jayus.Core;
using Jayus.ObjectStates;
using Jayus.TimeControl;
using UnityEngine;

namespace Jayus.TimeControl
{
	public class BasicEntityTracking : GenericTracker<RigidBodyState>
	{
        public override void Start()
        {
            base.Start();
            if (rigidbody == null)
            {
                throw new Exception("There is no rigidbody attached to this object! Behavior is invalid!");
            }
        }

        protected override void LoadState(RigidBodyState state)
        {
            transform.position = state.Position;
            transform.localEulerAngles = state.Rotation;
            rigidbody.velocity = state.Velocity;
            rigidbody.angularVelocity = state.AngularVelocity;
        }

        protected override RigidBodyState SaveState()
        {
            return new RigidBodyState() 
            {
                Position = transform.position,
                Rotation = transform.localEulerAngles,
                Velocity = rigidbody.velocity,
                AngularVelocity = rigidbody.angularVelocity 
            };
        }
	}
}
