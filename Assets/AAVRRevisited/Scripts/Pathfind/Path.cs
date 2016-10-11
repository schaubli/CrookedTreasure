using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pathfind {
    public class Path
    {
        public Tile LastStep { get; private set; }
        public Path PreviousSteps { get; private set; }
        public double TotalCost { get; private set; }
        private Path(Tile lastStep, Path previousSteps, double totalCost)
        {
            LastStep = lastStep;
            PreviousSteps = previousSteps;
            TotalCost = totalCost;
        }
        public Path(Tile start) : this(start, null, 0) {}
        public Path AddStep(Tile step, double stepCost)
        {
            return new Path(step, this, TotalCost + stepCost);
        }

        public List<Tile> TilesOnPath() { //Gathers all tiles on path recursively and returns them
            if(PreviousSteps == null) {
                return new List<Tile>(); //return empty list if ther is no PreviousStep
            } else {
                List<Tile> childList = PreviousSteps.TilesOnPath(); // Calls it self recursively to get all tiles of the path
                childList.Add(LastStep); //Add own Last Step
                return childList;
            }
        }
    }
}