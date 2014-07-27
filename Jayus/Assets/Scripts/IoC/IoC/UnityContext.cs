using UnityEngine;
using System;
using Jayus.Core;

public class UnityContext : MonoBehaviour
{
    public static UnityContext Instance;
    public virtual void OnComponentAdded(MonoBehaviour component) { }
}

public class UnityContext<T> : UnityContext where T : IContextRoot, new()
{
	virtual protected void Awake()
	{
        if (UnityContext.Instance != null)
        {
            throw new Exception("Unity context already exists, cannot create another!");
        }

        UnityContext.Instance = this;
		Debug.Log("UnityContext Awakened");
		
		_applicationRoot = new T();
	}
	
	//
	// Defining OnEnable as fix for UnityEngine execution order bug
	//
	void OnEnable()
	{	
		Debug.Log("UnityContext Enabled");
	}
	
	public override void OnComponentAdded(MonoBehaviour component)
	{
		DesignByContract.Check.Require(_applicationRoot != null && _applicationRoot.container != null, "Container not initialized correctly, possible script execution order problem");
		_applicationRoot.container.Inject(component);
	}
	
	protected IoC.IContainer container { get { return _applicationRoot.container;} } 
	
	private T _applicationRoot;
}