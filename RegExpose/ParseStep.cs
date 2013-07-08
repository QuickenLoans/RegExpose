﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RegExpose.Nodes;
using RegExpose.Nodes.Parens;

namespace RegExpose
{
    public class ParseStep
    {
#if DEBUG
        private bool _alreadyAdvanced;
#endif

        private ParseStep()
        {
        }

        internal static ParseStep Pass(RegexNode node, string matchedText, State initialState, State currentState)
        {
            return new ParseStep
            {
                Type = ParseStepType.Pass,
                Node = node,
                MatchedText = matchedText,
                InitialState = initialState,
                CurrentState = currentState
            }.WithMessage(step => node.GetPassMessage(step.MatchedText, step.InitialState));
        }

        internal static ParseStep Capture(RegexNode node, string capturedText, int captureNumber, State initialState, State currentState)
        {
            return new ParseStep
            {
                Type = ParseStepType.Capture,
                Node = node,
                MatchedText = capturedText,
                CaptureNumber = captureNumber,
                InitialState = initialState,
                CurrentState = currentState
            }.WithMessage(step => 
                string.Format(
                    "Captured '{0}' (capture number: {1}) starting at index {2}",
                    step.MatchedText,
                    step.CaptureNumber,
                    step.InitialState.Index));
        }

        internal static ParseStep CaptureDiscarded(RegexNode node, string capturedText, int captureNumber)
        {
            return new ParseStep
            {
                Type = ParseStepType.CaptureDiscarded,
                Node = node,
                MatchedText = capturedText,
                CaptureNumber = captureNumber
            }.WithMessage(step =>
                string.Format(
                    "Discarded captured text, '{0}' (capture number: {1})",
                    step.MatchedText,
                    step.CaptureNumber));
        }

        internal static ParseStep Fail(RegexNode node, State initialState, State currentState, string additionalMessage = null)
        {
            return new ParseStep
            {
                Type = ParseStepType.Fail,
                Node = node,
                InitialState = initialState,
                CurrentState = currentState
            }.WithMessage(step => (additionalMessage != null ? additionalMessage + ": " : "") + node.GetFailMessage(step.InitialState));
        }

        internal static ParseStep Match(Regex regex, State initialState, string matchedText, IList<IList<ParenCapture>> captures)
        {
            return new ParseStep
            {
                Type = ParseStepType.Match,
                Node = regex,
                MatchedText = matchedText,
                InitialState = initialState,
                Captures = captures
            }.WithMessage(step => regex.GetPassMessage(step.MatchedText, step.InitialState));
        }

        internal static ParseStep AdvanceIndex(RegexNode node, State state)
        {
            return new ParseStep
            {
                Type = ParseStepType.AdvanceIndex,
                Node = node,
                CurrentState = state,
            }.WithMessage(step => string.Format("Advanced index to {0}", step.CurrentState.Index));
        }

        internal static ParseStep Break(RegexNode node)
        {
            return new ParseStep
            {
                Type = ParseStepType.Break,
                Node = node
            };
        }

        public static ParseStep StartLookaround(RegexNode node, State currentState)
        {
            return new ParseStep
            {
                Type = ParseStepType.LookaroundStart,
                Node = node,
                CurrentState = currentState
            }.WithMessage(step => "Look-around started");
        }

        public static ParseStep EndLookaround(RegexNode node)
        {
            return new ParseStep
            {
                Type = ParseStepType.LookaroundEnd,
                Node = node
            }.WithMessage(step => "Look-around ended");
        }

        internal static ParseStep StateSaved(RegexNode node, State currentState, string message)
        {
            return new ParseStep
            {
                Type = ParseStepType.StateSaved,
                Node = node,
                CurrentState = currentState
            }.WithMessage(step => message);
        }

        internal static ParseStep Backtrack(RegexNode node, State initialState, State backtrackState)
        {
            return new ParseStep
            {
                Type = ParseStepType.Backtrack,
                Node = node,
                InitialState = initialState,
                CurrentState = backtrackState
            }.WithMessage(step => string.Format("Backtracking to {0}", step.CurrentState.Index));
        }

        internal static ParseStep EndOfString(RegexNode node, State currentState)
        {
            return new ParseStep
            {
                Type = ParseStepType.EndOfString,
                Node = node,
                CurrentState = currentState
            }.WithMessage(step => "End of string");
        }

        public static ParseStep BeginParse(RegexNode node, State initialState)
        {
            return new ParseStep
            {
                Type = ParseStepType.BeginParse,
                Node = node,
                InitialState = initialState
            }.WithMessage(step => "Parse started");
        }

        public static ParseStep ResetIndex(RegexNode node, State initialState, State currentState)
        {
            return new ParseStep
            {
                Type = ParseStepType.ResetIndex,
                Node = node,
                InitialState = initialState,
                CurrentState = currentState
            }.WithMessage(step => string.Format("Resetting index to {0}", step.InitialState.Index));
        }

        public static ParseStep Error(Exception exception)
        {
            return new ParseStep
            {
                Type = ParseStepType.Error,
                Exception = exception
            };
        }

        private Func<ParseStep, string> _getMessage = step => null;

        public ParseStepType Type { get; private set; }
        public RegexNode Node { get; private set; }
        public string MatchedText { get; private set; }
        public State InitialState { get; private set; }
        public State CurrentState { get; private set; }
        public int CaptureNumber { get; private set; }
        public Exception Exception { get; private set; }

        public string Message
        {
            get { return _getMessage(this); }
        }

        public string NodeType
        {
            get { return Node != null ? Node.NodeType : ""; }
        }

        public string Pattern
        {
            get { return Node != null ? Node.Pattern : ""; }
        }

        internal IList<IList<ParenCapture>> Captures { get; private set; }

        internal int InitialStateIndex
        {
            get { return InitialState.Index; }
        }

        public bool SkipAdvanceOnFail { get; private set; }

        public State Advance()
        {
#if DEBUG
            Debug.Assert(Type == ParseStepType.Pass, "Cannot call Advance() unless the Type is Pass");
            Debug.Assert(!_alreadyAdvanced, "Can only call Advance() once");
            _alreadyAdvanced = true;
#endif
            // Call Advance() on the State for each character in the match
            return MatchedText.Aggregate(InitialState, (current, t) => current.Advance());
        }

        public override string ToString()
        {
            return string.Format("{0}{1}{2}{3}",
                Type.ToString().PadRight(15),
                NodeType.PadRight(25),
                Pattern.PadRight(35),
                Message);
        }

        private ParseStep WithMessage(Func<ParseStep, string> getMessage)
        {
            _getMessage = getMessage;
            return this;
        }

        public ParseStep ConvertToOuterContext(string input, int indexModifier, RegexNode regexNode, Func<RegexNode, bool> changeNodePredicate, Func<string, string> modifyMessageFunction)
        {
            if (changeNodePredicate(Node))
            {
                Node = regexNode;
                var originalGetMessage = _getMessage;
                _getMessage = step => modifyMessageFunction(originalGetMessage(step));
            }
            InitialState = InitialState == null ? null : new State(input, InitialState.Index + indexModifier);
            CurrentState = CurrentState == null ? null : new State(input, CurrentState.Index + indexModifier);
            return this;
        }

        public ParseStep AsLookaround()
        {
            if (Type == ParseStepType.AdvanceIndex)
            {
                Type = ParseStepType.LookaroundAdvanceIndex;
            }
            else if (Type == ParseStepType.ResetIndex)
            {
                Type = ParseStepType.LookaroundResetIndex;
            }

            return this;
        }

        public ParseStep WithSkipAdvanceOnFail(bool skipAdvanceOnFail)
        {
            SkipAdvanceOnFail = skipAdvanceOnFail;
            return this;
        }
    }
}