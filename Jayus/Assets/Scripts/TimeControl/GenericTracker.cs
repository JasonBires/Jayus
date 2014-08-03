using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Jayus.Core;
using Jayus.ObjectStates;

namespace Jayus.TimeControl
{
	public class GenericTracker<T> : ObjectBehavior where T : ObjectState
	{
        protected StateTracker<T> _stateTracker { get; set; }

        [IoC.Inject]
        protected ITimeController _timeControl { get; set; }

        public override void Start()
        {
            base.Start();

            //Newing this up instead of injecting it because the current IoC framework does not allow for transient lifestyles
            _stateTracker = new StateTracker<T>();
        }

        protected virtual void Update()
        {
            if (_timeControl.TimeSpeed < 0)
            {
                var newState = _stateTracker.ReadPreviousTick();
                LoadState(newState);
            }
            else
            {
                _stateTracker.SaveStateForCurrentTick(SaveState());
            }
        }

        protected virtual void LoadState(T state) { }
        protected virtual T SaveState() { return null; }
	}
}
