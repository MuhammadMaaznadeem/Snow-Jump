using UnityEngine;
using UnityEngine.Events;

namespace PLAYERTWO.PlatformerProject
{
	public abstract class EntityState<T> : MonoBehaviour where T : Entity<T>
	{
		public UnityEvent onEnter;
		public UnityEvent onExit;

		public void Enter(T entity)
		{
			onEnter?.Invoke();
			OnEnter(entity);
		}

		public void Exit(T entity)
		{
			onExit?.Invoke();
			OnExit(entity);
		}

		public void Step(T entity) => OnStep(entity);

		/// <summary>
		/// Called when this State is invoked.
		/// </summary>
		protected abstract void OnEnter(T entity);

		/// <summary>
		/// Called when this State changes to another.
		/// </summary>
		protected abstract void OnExit(T entity);

		/// <summary>
		/// Called every frame where this State is activated.
		/// </summary>
		protected abstract void OnStep(T entity);
	}
}
