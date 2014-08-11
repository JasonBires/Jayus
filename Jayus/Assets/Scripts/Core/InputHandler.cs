using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jayus.Events;
using UnityEngine;

namespace Jayus.Core
{
    public interface IInputHandler
    {
        void UpdateKeys();
    }

    //TODO - This needs to be restructured, but all key press events should be handled by this class.
	public class InputHandler : IInputHandler
	{
        private bool TKeyPressed { get; set; }

        private readonly IEventManager _eventManager;
        public InputHandler(IEventManager eventManager)
        {
            _eventManager = eventManager;
        }

        public void UpdateKeys()
        {
            var tKeyPressed = Input.GetKey(KeyCode.T);
            if (tKeyPressed != TKeyPressed)
            {
                TKeyPressed = tKeyPressed;
                _eventManager.RaiseEvent<TimeManipulationChangedEvent>(new TimeManipulationChangedEvent() { TimeKeyIsPressed = TKeyPressed });
            }
        }

	}
}
