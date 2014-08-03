using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Jayus.ObjectStates
{
	public class RigidBodyState : PositionState
	{
        public Vector3 Velocity { get; set; }
        public Vector3 AngularVelocity { get; set; }
	}
}
