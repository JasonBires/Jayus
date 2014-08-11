using UnityEngine;
using System.Collections;
using Jayus.Core;
using Jayus.Events;

namespace Jayus.TimeControl
{
    public interface ITimeController
    {
        float TimeSpeed { get; set; }
    }

    public class TimeController : ITimeController
    {
        public float TimeSpeed { get; set; }

        private readonly IEventManager _eventManager;

        public TimeController(IEventManager eventManager)
        {
            TimeSpeed = 1.0F;
            _eventManager = eventManager;

            _eventManager.Subscribe<TimeManipulationChangedEvent>(HandleTimeManipulationEvent);
        }

        //TODO - Move out into another class
        private void HandleTimeManipulationEvent(TimeManipulationChangedEvent evnt)
        {
            Debug.Log("Caught time manipulation event, setting new speed");
            if (evnt.TimeKeyIsPressed)
            {
                TimeSpeed = -1.0F;
            }
            else
            {
                TimeSpeed = 1.0F;
            }
        }
    }
}