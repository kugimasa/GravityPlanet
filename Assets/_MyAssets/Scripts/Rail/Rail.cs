using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rails
{
    public class Rail : MonoBehaviour
    {
        [SerializeField] private Transform m_goalPlanet;
        public Transform GoalPlanet => m_goalPlanet;
    }

}