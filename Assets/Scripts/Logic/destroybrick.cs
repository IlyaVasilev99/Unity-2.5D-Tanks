using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Assets.Scripts.Logic
{
    //public bool isWall;
    internal class destroybrick : MonoBehaviour
    {
        public bool bulletTime = false;

        private void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);

        }
    }
}


