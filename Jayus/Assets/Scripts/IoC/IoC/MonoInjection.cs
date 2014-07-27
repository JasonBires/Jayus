using UnityEngine;
using Jayus.Core;

public static class MonoInjection
{
	public static void Inject(this MonoBehaviour script)
	{
        UnityContext.Instance.OnComponentAdded(script);
	}
}