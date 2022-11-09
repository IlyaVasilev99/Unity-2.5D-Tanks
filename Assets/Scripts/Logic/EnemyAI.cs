using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Logic
{
	
	internal class EnemyAI : Tank
	{

		public IEnumerator Think()
		{
			//float attackTime = 15;
			var whereToMove = new List<Vector2Int>()
			{
				Vector2Int.zero, Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right,  Vector2Int.down,
			};

			yield return TryMove(whereToMove[UnityEngine.Random.Range(0, whereToMove.Count)]);


			//if (attackTime > 0)
			//{
			//	attackTime -= Time.deltaTime;

			//}
			//else
			//{
				//this.Fire();

				//attackTime = (float)(15);
			//}




		}



	}
}


