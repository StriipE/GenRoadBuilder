using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Resources.Scripts
{
    public class Incubator
    {
        private List<Road> generatedRoads;

        public void addRoadToIncubator(Road road)
        {
            generatedRoads.Add(road);
        }
    }
}
