using System;
using System.Collections.Generic;
using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	public abstract class EntityStateManager<T> : MonoBehaviour where T : Entity<T>
	{
		private List<EntityState<T>> m_list = new List<EntityState<T>>();

		private Dictionary<Type, EntityState<T>> m_states = new Dictionary<Type, EntityState<T>>();

		/// <summary>
		/// Returns the instance of the current Entity State.
		/// </summary>
		/// <value></value>
		public EntityState<T> current { get; private set; }

		/// <summary>
		/// Return the index of the current Entity State.
		/// </summary>
		public int index => m_list.IndexOf(current);

		/// <summary>
		/// Return the instance of the Entity associated with this Entity State Manager.
		/// </summary>
		public T entity { get; private set; }

		protected abstract List<EntityState<T>> GetStateList();

		protected virtual void InitializeEntity() => entity = GetComponent<T>();

		protected virtual void InitializeStates()
		{
			m_list = GetStateList();

			foreach (var state in m_list)
			{
				var type = state.GetType();

				if (!m_states.ContainsKey(type))
				{
					m_states.Add(type, state);
				}
			}

			if (m_list.Count > 0)
			{
				current = m_list[0];
			}
		}

		/// <summary>
		/// Change to a given Entity State based on its index on the States list.
		/// </summary>
		/// <param name="to">The index of the State you want to change to.</param>
		public virtual void Change(int to)
		{
			if (to >= 0 && to < m_list.Count)
			{
				Change(m_list[to]);
			}
		}

		/// <summary>
		/// Change to a given Entity State based on its class type.
		/// </summary>
		/// <typeparam name="TState">The class of the state you want to change to.</typeparam>
		public virtual void Change<TState>() where TState : EntityState<T>
		{
			var type = typeof(TState);

			if (m_states.ContainsKey(type))
			{
				Change(m_states[type]);
			}
		}

		/// <summary>
		/// Changes to a given Entity State based on its instance.
		/// </summary>
		/// <param name="to">The instance of the Entity State you want to change to.</param>
		public virtual void Change(EntityState<T> to)
		{
			if (to)
			{
				if (current)
				{
					current.Exit(entity);
				}

				current = to;
				current.Enter(entity);
			}
		}

		/// <summary>
		/// Returns true if the type of the current State matches a given one.
		/// </summary>
		/// <param name="type">The type you want to compare to.</param>
		public virtual bool IsCurrentOfType(Type type) => current.GetType() == type;

		public virtual void Step()
		{
			if (current)
			{
				current.Step(entity);
			}
		}

		protected virtual void Start()
		{
			InitializeEntity();
			InitializeStates();
		}
	}
}
