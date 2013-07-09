using System.Collections.Generic;

namespace RegExpose.UI
{
    public class CachedStep
    {
        public CachedStep(ParseStep step, int stepIndex, int regexIndex, int currentIndex, int currentLookaroundIndex, IEnumerable<int> savedStatesIndexes)
        {
            Step = step;
            StepIndex = stepIndex;
            RegexIndex = regexIndex;
            CurrentIndex = currentIndex;
            CurrentLookaroundIndex = currentLookaroundIndex;
            SavedStatesIndexes = savedStatesIndexes;
        }

        public ParseStep Step { get; private set; }
        public int StepIndex { get; private set; }
        public int RegexIndex { get; private set; }
        public int CurrentIndex { get; private set; }
        public IEnumerable<int> SavedStatesIndexes { get; private set; }
        public int CurrentLookaroundIndex { get; private set; }
    }
}