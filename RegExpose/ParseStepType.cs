namespace RegExpose
{
    public enum ParseStepType
    {
        Pass,
        Capture,
        CaptureDiscarded,
        Fail,
        Info,
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