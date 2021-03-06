﻿using UnityEngine;
using System.Collections;
using Jayus.Core;
using Jayus.TimeControl;
using Jayus.ObjectStates;
using System;
using Jayus.Events;

namespace Jayus.Character
{

    public class CameraMover : MonoBehaviour
    {
        public float speed;
        public float rotationSpeed;

        private float xRotation;
        private float yRotation;

        [IoC.Inject]
        private ITimeController _timeControl { set; get; }

        [IoC.Inject]
        private IStateTracker _stateTracker { get; set; }

        public AudioSource forwardMove;
        public AudioSource timeReverse;

        public void Start()
        {
            this.Inject();

            xRotation = 0;
            yRotation = 0;
        }

        /* Update is called once per frame
         * If T is pressed and timeControl's backing array isn't spent, rewind time
         * Else change the object as normal and record the object's rotation and translation in timeControl
         */
        void Update()
        {
            if (_timeControl.TimeSpeed < 0)
            {
                if (!timeReverse.isPlaying)
                {
                    timeReverse.Play();
                    forwardMove.Stop();
                }
                LoadState();
            }
            else
            {
                if (!forwardMove.isPlaying)
                {
                    timeReverse.Stop();
                    forwardMove.Play();
                }
                float translationZ = Input.GetAxis("Vertical") * speed;
                float translationX = Input.GetAxis("Horizontal") * speed;
                translationZ *= Time.deltaTime;
                translationX *= Time.deltaTime;
                Vector3 translateBy = new Vector3(translationX, 0, translationZ);
                transform.Translate(translationX, 0, translationZ);

                yRotation -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
                yRotation = Mathf.Clamp(yRotation, -80, 80);
                xRotation += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
                xRotation = xRotation % 360;
                Vector3 rotateBy = new Vector3(yRotation, xRotation, 0);
                transform.localEulerAngles = new Vector3(yRotation, xRotation, 0);

                SaveState();
            }
        }

        void LoadState()
        {
            var newState = _stateTracker.ReadPreviousTick<PositionState>();

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