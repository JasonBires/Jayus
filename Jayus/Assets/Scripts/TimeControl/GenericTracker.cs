using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Jayus.Core;
using Jayus.ObjectStates;
using UnityEngine;

namespace Jayus.TimeControl
{
    public class GenericTracker<T> : MonoBehaviour where T : ObjectState
	{
        [IoC.Inject]
        protected IStateTracker _stateTracker { get; set; }

        [IoC.Inject]
        protected ITimeController _timeControl { get; set; }

        public virtual void Start()
        {
            this.Inject();
        }

        protected virtual void Update()
        {
            if (_timeControl.TimeSpeed < 0)
            {
                var newState = _stateTracker.ReadPreviousTick<T>();
                LoadState(newState);
            }
            else
            {
                _stateTracker.SaveStateForCurrentTick<T>(SaveState());
            }
        }

        protected virtual void LoadState(T state) { }
        protected virtual T SaveState() { return null; }
	}
}
