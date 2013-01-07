namespace RegExpose
{
    public enum ParseStepType
    {
        Pass,
        Capture,
        CaptureDiscarded,
        Fail,
        Match,
        StateSaved,
        Break,
        Backtrack,
        EndOfString,
        BeginParse,
        AdvanceIndex,
        ResetIndex,
        LookaroundAdvanceIndex,
        LookaroundResetIndex,
        LookaroundStart,
        LookaroundEnd,
        Error
    }
}