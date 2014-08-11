using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jayus.TimeControl;
using UnityEngine;

namespace Jayus.Core
{
    public class Main : IContextRoot
    {
        public IoC.IContainer container { get; private set; }

        public Main()
        {
            SetupContainer();
            StartGame();
        }

        void SetupContainer()
        {
            container = new IoC.UnityContainer();

            //Manual registration
            container.Bind<IoC.IMonoBehaviourFactory>().AsSingle<IoC.MonoBehaviourFactory>();
            container.Bind<IStateTracker>().AsTransient<StateTracker>();
            container.Bind<IEventManager>().AsSingle<EventManager>();
            container.Bind<IInputHandler>().AsSingle<InputHandler>();

            //Registration for the namespace 
            container.AutoRegisterTypesFromNamespace("Jayus.TimeControl");

            //Example of component registration:
            //container.Bind<IService>().AsSingle<Service>();
        }

        void StartGame()
        {
            //TODO - Add anything that needs initializing
        }
    }

    //UnityContext must be executed before 
    //anything else that uses the container itself.
    //In order to achieve this, you can use
    //the execution order or the awake/start 
    //functions order

    public class GameContext : UnityContext<Main>
    {
        [IoC.Inject]
        private IInputHandler _inputHandler { get; set; }

        void Start()
        {
            this.Inject();
        }

        //Used for updating of "global" services, such as input
        void Update()
        {
            _inputHandler.UpdateKeys();
        }
    }
}
