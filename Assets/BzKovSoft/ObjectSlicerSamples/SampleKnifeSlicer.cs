using UnityEngine;
using BzKovSoft.ObjectSlicer;
using System.Diagnostics;
using System;
using System.Collections;

namespace BzKovSoft.ObjectSlicerSamples
{
	/// <summary>
	/// Test class for demonstration purpose
	/// </summary>
	public class SampleKnifeSlicer : MonoBehaviour
	{
#pragma warning disable 0649
		//[SerializeField]
		private GameObject _blade;
#pragma warning restore 0649

        private void Start()
        {
            _blade = GameObject.FindGameObjectWithTag("chopper");
        }

        public GameController gameController;
		void Update()
		{
			if (Input.GetMouseButtonDown(0)&&GameController.gameController.canMove)
			{
				var knife = _blade.GetComponentInChildren<BzKnife>();
                knife.BeginNewSlice();
				//StartCoroutine(SwingSword());

                gameController.cutAnim[0].Play();
				GameController.gameController.increaseMoveCount(LevelTyp.LimitedCut);
			}
		}

		IEnumerator SwingSword()
		{
			var transformB = _blade.transform;
			transformB.position = new Vector3(0f, 3f, 0f);
			transformB.rotation = Quaternion.identity;

			const float seconds = 0.1f;
			for (float f = 0f; f < seconds; f += Time.deltaTime)
			{
				float aY = (f / seconds);
				float aX = (f / seconds) * 60 - 30;
				//float aX = 0;

				var r = Quaternion.Euler(aX, -aY, 0);

				transformB.rotation = Quaternion.identity * r;
				yield return null;
			}
		}
	}
}