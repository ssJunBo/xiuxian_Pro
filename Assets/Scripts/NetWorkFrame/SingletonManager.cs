﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetWorkFrame
{
	public class SingletonManager : MonoBehaviour
	{
		private static GameObject _rootObj;
		private static List<Action> _singletonReleaseList = new List<Action>();
		void Awake()
		{
			_rootObj = gameObject;
			DontDestroyOnLoad(_rootObj);

			InitSingletons();
		}

		/// <summary>
		/// 销毁所有单例
		/// </summary>
		public void OnApplicationQuit() 
		{
            for (int i = _singletonReleaseList.Count - 1; i >= 0; i--)
            {
				_singletonReleaseList[i]();
            }
		}

		/// <summary>
		/// 所有单例的初始化
		/// </summary>
		void InitSingletons() 
		{
            AddSingleton<MessageDispatcher>();
            AddSingleton<NetWorkManager>();
        }

        private static void AddSingleton<T>() where T : Singleton<T> 
		{
			if (_rootObj.GetComponent<T>() == null)
			{
				T t = _rootObj.AddComponent<T>();
				t.SetInstance(t);
				t.Init();

				_singletonReleaseList.Add(delegate () {
					t.Release();
				});
            }
		}

		public static T GetSingleton<T>() where T : Singleton<T> 
		{
			T t = _rootObj.GetComponent<T>();
            if (t==null)
            {
				AddSingleton<T>();
            }
			return t;
		}

	}
}
