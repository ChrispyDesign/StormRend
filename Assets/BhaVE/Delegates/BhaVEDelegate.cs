using UnityEngine;
using BhaVE.Core;

namespace BhaVE.Delegates
{
	public abstract class BhaveDelegate<R> : ScriptableObject
	{
		//------------ Inits -----------
		public virtual void Awaken(BhaveAgent agent) { }		//Can't use awake because Scriptable Objects already has an Awake()
		public virtual void Initiate(BhaveAgent agent) { }		//Cant use start because Scriptable Objects also discreetly calls Start() (WTF?!)
		//------------ Mains ------------
		public virtual void Begin() { }
		public abstract R Execute(BhaveAgent agent);
		public virtual void Paused(bool paused) { }
		public virtual void End() { }
		//----------- Shutdowns ----------
		public virtual void Shutdown() { }
	}

	#region Experimentals: These can take in multiple parameters
	public abstract class BhaveDelegate<T1, R> : ScriptableObject
	{
		public abstract R Execute(BhaveAgent agent, T1 t1);
	}
	public abstract class BhaveDelegate<T1, T2, R> : ScriptableObject
	{
		public abstract R Execute(BhaveAgent agent, T1 t1, T2 t2);
	}
	public abstract class BhaveDelegate<T1, T2, T3, R> : ScriptableObject
	{
		public abstract R Execute(BhaveAgent agent, T1 t1, T2 t2, T3 t3);
	}
	public abstract class BhaveDelegate<T1, T2, T3, T4, R> : ScriptableObject
	{
		public abstract R Execute(BhaveAgent agent, T1 t1, T2 t2, T3 t3, T4 t4);
	}
	public abstract class BhaveDelegate<T1, T2, T3, T4, T5, R> : ScriptableObject
	{
		public abstract R Execute(BhaveAgent agent, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);
	}
	#endregion
}
