using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jayus.Core;
using Jayus.TimeControl;
using UnityEngine;

namespace Jayus.TimeControl
{
	public class TimeTrackingBehavior : ObjectBehavior
	{
        private StateTracker<PositionState> _stateTracker { get; set; }

        public override void Start()
        {
            //base.Start();

            //Newing this up instead of injecting it because the current IoC framework does not allow for transient lifestyles
            _stateTracker = new StateTracker<PositionState>();
        }

        /* Update is called once per frame
        * If T is pressed and timeControl's backing array isn't spent, rewind time
        * Else change the object as normal and record the object's rotation and translation in timeControl
        */
        void Update()
        {
            if (Input.GetKey(KeyCode.T))
            {
                LoadState();
            }
            else
            {
                SaveState();
            }
        }

        void LoadState()
        {
            var newState = _stateTracker.ReadPreviousTick() as PositionState;

            transform.position = newState.Position;
            transform.localEulerAngles = newState.Rotation;
        }

        void SaveState()
        {
            var state = new PositionState() { Position = transform.position, Rotation = transform.localEulerAngles };
            _stateTracker.SaveStateForCurrentTick(state);
        }
	}
}
