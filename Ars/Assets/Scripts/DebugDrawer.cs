using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;
using Crowolf.Ars.Utilities;

namespace Crowolf.Ars.Sandbox
{
	public class DebugDrawer : SingletonMonoBehaviour<DebugDrawer>
	{
		[SerializeField, Tooltip("Set screen refresh span per seconds")]
		private float _refreshSpan = 0.2f;

		[SerializeField]
		private TextMeshProUGUI _text;
		public string Text { get; set; } = "";

		/// <summary>
		/// Text shows to screen
		/// </summary>
		/// <param name="text"></param>
		public static void Log( string text ) => Instance.Text = text;

		protected override void InitializeOnAwake()
		{
			// Do nothing
		}

		private void Start()
		{
			this.UpdateAsObservable()
				.Sample( TimeSpan.FromSeconds( (double)_refreshSpan ) )
				.Subscribe( _ => _text.text = Text )
				.AddTo( this );
		}
	}
}
