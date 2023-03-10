using System;
using UnityEngine;

namespace Crowolf.Ars.Utilities
{
	/// <summary>
	/// Singleton MonoBehaviour
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[DefaultExecutionOrder( -1 )]
	public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;
		public static T Instance
		{
			get
			{
				if ( _instance == null )
				{
					var message = $"No one attached '{typeof(T)}' to gameObject.";
					Debug.LogError( message );
					throw new NullReferenceException( message );
				}
				return _instance;
			}
		}

		virtual protected void Awake()
		{
			if ( !_instance.IsUnityNull() )
			{
				Destroy( this );
				return;
			}
			_instance = this as T;
			DontDestroyOnLoad( this.gameObject );
			InitializeOnAwake();
		}

		abstract protected void InitializeOnAwake();
	}
}