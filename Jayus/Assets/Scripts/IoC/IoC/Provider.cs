using System;

namespace IoC
{
	public interface IProvider
	{
		object Create();
        object Create(object[] constructorParams);
		
		Type contract { get; }
	}
	
	public class StandardProvider:IProvider
	{
		public StandardProvider(System.Type type)
		{
			_type = type;
		}
		
		public object Create()
		{
			return Activator.CreateInstance(_type);
		}

        public object Create(object[] constructorParams)
        {
            return Activator.CreateInstance(_type, constructorParams);
        }
		
		public Type contract { get { return _type; } }
		
		private System.Type _type;
	}
	
	public class StandardProvider<T>:IProvider where T:new()
	{
		public object Create()
		{
			return new T();
		}

        public object Create(object[] constructorParams)
        {
            return Activator.CreateInstance(typeof(T), constructorParams);
        }
		
		public System.Type contract { get { return typeof(T); } }
	}

}

