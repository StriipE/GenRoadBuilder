using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Resources.Scripts
{
    public class Road : MonoBehaviour
    {
        private int RoadLength = 0;
      
        private GameObject[,] roadBlocks;

        public Road(int length, int width)
        {
        }

        public void setGridSize(int length, int width)
        {
            roadBlocks = new GameObject[length, width];
        }
        public GameObject[,] RoadBlocks { get { return roadBlocks; } set { roadBlocks = value; } }

        public void incrementRoadLength()
        {
            ++RoadLength;
        }

        public int getRoadLength()
        {
            return RoadLength;
        }
    }
}
