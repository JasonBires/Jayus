using UnityEngine;
using System.Collections;

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