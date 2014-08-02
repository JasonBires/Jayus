using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Jayus.Core;

namespace Jayus.TimeControl
{
    //Using this class to wrap a dictionary so that we can easily make micro-optimizations if needed later on
	public class StateTracker<T> where T : ObjectState
	{
        private Dictionary<int, T> _stateBuffer;
        private int _currentTick;

        public StateTracker()
        {
            _currentTick = 0;
            _stateBuffer = new Dictionary<int, T>(9000); //Should be just as efficient as an array at this point
        }

        public T GetSavedStateFromTick(int tick)
        {
            ChangeCurrentTick(tick);
            T result = null;
            if (!_stateBuffer.TryGetValue(_currentTick, out result))
            {
                var firstState = GetFirstKnownState();
                SaveStateForCurrentTick(firstState);
                return firstState;
            }
            return result;
        }

        public T ReadPreviousTick()
        {
            return GetSavedStateFromTick(-1);
        }

        public void SaveStateForCurrentTick(T state)
        {
            _stateBuffer[_currentTick] = state;
            ChangeCurrentTick(1);
        }

        //TODO - Improve algorithm
        //Will probably have huge performance costs. We can either limit this operation by not jumping around in time too much, or we can perfect a better algorithm.
        private T GetFirstKnownState()
        {
            var lowestTick = _stateBuffer.Keys.Min();
            return _stateBuffer[lowestTick];
        }

        private void ChangeCurrentTick(int amount)
        {
            _currentTick += amount;
        }
	}
}
