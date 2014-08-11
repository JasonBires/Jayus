using System;

namespace IoC
{
	public interface IBinder<Contractor>
	{
		void AsSingle();
        void AsTransient();
        void AsTransient<T>() where T : class, Contractor;
		void AsSingle<T>(T istance) where T:class, Contractor;
		void AsSingle<T>() where T:Contractor;
		void ToFactory<T>(IProvider provider) where T:IProvider, Contractor;
			
		void Bind<ToBind>(IInternalContainer container);
	}

}

