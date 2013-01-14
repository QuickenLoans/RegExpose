using System.Collections.Generic;

namespace RegExpose.UI
{
    public class CachedStep
    {
        public CachedStep(ParseStep step, int stepIndex, int currentIndex, int currentLookaroundIndex, IEnumerable<int> savedStatesIndexes)
        {
            Step = step;
            StepIndex = stepIndex;
            CurrentIndex = currentIndex;
            CurrentLookaroundIndex = currentLookaroundIndex;
            SavedStatesIndexes = savedStatesIndexes;
        }

        public ParseStep Step { get; private set; }
        public int StepIndex { get; private set; }
        public int CurrentIndex { get; private set; }
        public IEnumerable<int> SavedStatesIndexes { get; private set; }
        public int CurrentLookaroundIndex { get; private set; }
    }
}