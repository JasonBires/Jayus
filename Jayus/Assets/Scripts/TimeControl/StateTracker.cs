using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Jayus.ObjectStates;

namespace Jayus.TimeControl
{
    public interface IStateTracker
    {
        T ReadPreviousTick<T>() where T : ObjectState;
        T GetSavedStateFromTick<T>(int tick) where T : ObjectState;
        void SaveStateForCurrentTick<T>(T state) where T : ObjectState;
    }

    //Using this class to wrap a dictionary so that we can easily make micro-optimizations if needed later on
	public class StateTracker : IStateTracker
	{
        private Dictionary<int, ObjectState> _stateBuffer;
        private int _currentTick;

        public StateTracker()
        {
            _currentTick = 0;
            _stateBuffer = new Dictionary<int, ObjectState>(9000); //Should be just as efficient as an array at this point
        }

        public T ReadPreviousTick<T>() where T:ObjectState
        {
            return GetSavedStateFromTick<T>(-1);
        }

        public T GetSavedStateFromTick<T>(int tick) where T : ObjectState
        {
            ChangeCurrentTick(tick);
            ObjectState result = null;
            if (!_stateBuffer.TryGetValue(_currentTick, out result))
            {
                var firstState = GetFirstKnownState<T>();
                SaveStateForCurrentTick(firstState);
                return firstState;
            }
            return result as T;
        }

        public void SaveStateForCurrentTick<T>(T state) where T : ObjectState
        {
            _stateBuffer[_currentTick] = state;
            ChangeCurrentTick(1);
        }

        //TODO - Improve algorithm
        //Will probably have huge performance costs. We can either limit this operation by not jumping around in time too much, or we can perfect a better algorithm.
        private T GetFirstKnownState<T>() where T : ObjectState
        {
            var lowestTick = _stateBuffer.Keys.Min();
            return _stateBuffer[lowestTick] as T;
        }

        private void ChangeCurrentTick(int amount)
        {
            _currentTick += amount;
        }
	}
}
