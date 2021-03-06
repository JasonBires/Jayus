using System;
using System.Collections.Generic;
using System.Reflection;
using IoC;
using System.Linq;

namespace IoC
{
    public class Container : IContainer, IInternalContainer
    {
        public Container()
        {
            _transientTypes = new List<Type>();
            _providers = new Dictionary<Type, IProvider>();
            _uniqueInstances = new Dictionary<Type, object>();
            _injectLater = new HashSet<object>();
        }

        //
        // IContainer interface
        //

        public IBinder<TContractor> Bind<TContractor>()
        {
            IBinder<TContractor> binder = BinderProvider<TContractor>();

            binder.Bind<TContractor>(this);

            return binder;
        }

        public TContractor Build<TContractor>() where TContractor : class
        {
            Type contract = typeof(TContractor);

            TContractor instance = Get(contract) as TContractor;

            DesignByContract.Check.Ensure(instance != null, "IoC.Container instance failed to be built (contractor not found)");

            return instance;
        }

        public void Release<TContractor>() where TContractor : class
        {
            Type type = typeof(TContractor);

            if (_providers.ContainsKey(type))
                _providers.Remove(type);

            if (_uniqueInstances.ContainsKey(type))
                _uniqueInstances.Remove(type);
        }

        public void AutoRegisterTypesFromNamespace(string Namespace)
        {
            var interfaces = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsInterface && t.Namespace == Namespace).ToList();
            var classes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && t.Namespace == Namespace).ToList();
            classes.ToList().ForEach(c =>
            {
                var matching = interfaces.FirstOrDefault(i => i.Name.EndsWith(c.Name));
                if(matching != null)
                    this.Register(matching, c);
            });
        }

        //
        // IInternalContainer interface
        //

        virtual public void Register(System.Type type, System.Type mapper)
        {
            DesignByContract.Check.Require(type.IsAssignableFrom(mapper));

            _providers[type] = new StandardProvider(mapper);
        }

        virtual public void Register(System.Type type, IProvider provider)
        {
            DesignByContract.Check.Require(type.IsAssignableFrom(provider.contract));

            _providers[type] = provider;
        }

        virtual public void Register(System.Type type)
        {
            _providers[type] = new StandardProvider(type);
        }

        virtual public void RegisterTransientType(System.Type type, System.Type mapper)
        {
            if(!_transientTypes.Contains(type))
                _transientTypes.Add(type);

            DesignByContract.Check.Require(type.IsAssignableFrom(mapper));
            _providers[type] = new StandardProvider(mapper);
        }

        virtual public void Map(System.Type type, System.Type mapper, object instance)
        {
            DesignByContract.Check.Require(instance != null, "IoC: Trying to register an null instance of type: " + type.FullName);
            DesignByContract.Check.Require(type.IsAssignableFrom(instance.GetType()), "IoC: Trying to register an invalid instance of type: " + type.FullName);

            _injectLater.Add(instance);

            _providers[type] = new StandardProvider(mapper);

            _uniqueInstances[mapper] = instance;
        }

        public void Inject<TContractor>(TContractor instance)
        {
            if (instance != null)
                InternalInject(instance);
        }

        //
        // Private Members
        //

        private void InternalInject(object injectable)
        {
            DesignByContract.Check.Require(injectable != null);

            Type contract = injectable.GetType();

            var properties = contract.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance);

            foreach (PropertyInfo member in properties)
            {
                object[] attrs = member.GetCustomAttributes(typeof(InjectAttribute), true);

                foreach (object attr in attrs)
                {
                    if (attr.GetType() == typeof(InjectAttribute))
                    {
                        PropertyInfo info = member as PropertyInfo;

                        if (info.PropertyType == typeof(IContainer)) //self inject
                            info.SetValue(injectable, this, null);
                        else
                        {
                            object valueObj = Get(info.PropertyType);

                            //inject in Injectable the valueObj
                            if (valueObj != null)
                                info.SetValue(injectable, valueObj, null);
                        }
                    }
                }
            }

            if (injectable is IInitialize)
                (injectable as IInitialize).OnInject();
        }

        private object TryInjectConstructorParams(IProvider provider)
        {
            var type = provider.contract;
            var parameters = type.GetConstructors().First().GetParameters().ToList();
            if (parameters.Count > 0)
            {
                var toInject = parameters.Select(x => Get(x.ParameterType));
                return provider.Create(toInject.ToArray());
            }
            return null;
        }

        virtual protected object Get(Type contract)
        {
            if (_providers.ContainsKey(contract) == true)
            {
                IProvider provider = _providers[contract];

                if (_transientTypes.Contains(contract))
                    return provider.Create();
                if (_uniqueInstances.ContainsKey(provider.contract) == false)
                    return CreateDependency(provider);
                else
                    return _uniqueInstances[provider.contract];
            }
            else
                if (_uniqueInstances.ContainsKey(contract) == true)
                {
                    object instance = _uniqueInstances[contract];

                    if (_injectLater.Contains(instance))
                    {
                        InternalInject(instance);

                        _injectLater.Remove(instance);
                    }

                    return instance;
                }

            return null;
        }

        virtual protected IBinder<TContractor> BinderProvider<TContractor>()
        {
            return new Binder<TContractor>();
        }

        private object CreateDependency(IProvider provider)
        {
            object obj = TryInjectConstructorParams(provider) ?? provider.Create();

            _uniqueInstances[provider.contract] = obj; //seriously, this must be done before obj is injected to avoid circular dependencies

            InternalInject(obj);

            return obj;
        }

        private readonly List<Type> _transientTypes;
        private readonly Dictionary<Type, IProvider> _providers;
        private readonly Dictionary<Type, object> _uniqueInstances;

        private readonly HashSet<object> _injectLater;
    }
}