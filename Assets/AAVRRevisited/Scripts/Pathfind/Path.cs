using UnityEngine;
using System.Collections;

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
    }
}