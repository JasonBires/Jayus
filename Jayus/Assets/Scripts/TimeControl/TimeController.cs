using UnityEngine;
using System.Collections;
using Jayus.Core;

namespace Jayus.TimeControl
{
    public interface ITimeController
    {
        float TimeSpeed { get; set; }
    }

    public class TimeController : ITimeController
    {
        public float TimeSpeed { get; set; }

        public TimeController()
        {
            TimeSpeed = 1.0F;
        }
    }
}