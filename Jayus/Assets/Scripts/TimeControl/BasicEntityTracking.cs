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
	public class BasicEntityTracking : ObjectBehavior
	{
        private StateTracker<RigidBodyState> _stateTracker { get; set; }

        [IoC.Inject]
        protected ITimeController _timeControl { get; set; }

        public override void Start()
        {
            base.Start();

            //Newing this up instead of injecting it because the current IoC framework does not allow for transient lifestyles
            _stateTracker = new StateTracker<RigidBodyState>();

            if (rigidbody == null)
            {
                throw new Exception("There is no rigidbody attached to this object! Behavior is invalid!");
            }
        }

        /* Update is called once per frame
        * If T is pressed and timeControl's backing array isn't spent, rewind time
        * Else change the object as normal and record the object's rotation and translation in timeControl
        */
        protected virtual void Update()
        {
            if (_timeControl.TimeSpeed < 0)
            {
                LoadState();
            }
            else
            {
                SaveState();
            }
        }

        protected virtual void LoadState()
        {
            var newState = _stateTracker.ReadPreviousTick();

            transform.position = newState.Position;
            transform.localEulerAngles = newState.Rotation;
            rigidbody.velocity = newState.Velocity;
            rigidbody.angularVelocity = newState.AngularVelocity;
        }

        protected virtual void SaveState()
        {
            var state = new RigidBodyState() 
            {
                Position = transform.position,
                Rotation = transform.localEulerAngles,
                Velocity = rigidbody.velocity,
                AngularVelocity = rigidbody.angularVelocity 
            };
            _stateTracker.SaveStateForCurrentTick(state);
        }
	}
}
