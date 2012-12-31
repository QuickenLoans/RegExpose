using System;
using Sprache;

namespace RegExpose.PatternParsing
{
    internal static class ParserExtensions
    {
        public static Parser<TRegexNode> ToRegexNode<T, TRegexNode>(this Parser<T> parser,
                                                                    Func<T, int, string, Parser<TRegexNode>> convert)
        {
            if (parser == null)
            {
                throw new ArgumentNullException("parser");
            }
            if (convert == null)
            {
                throw new ArgumentNullException("convert");
            }

            return input =>
                   parser(input)
                       .IfSuccess(
                           success =>
                           convert(
                               success.Result,
                               input.Position,
                               input.Source.Substring(input.Position, success.Remainder.Position - input.Position))(
                                   success.Remainder));
        }

        public static IResult<TResult> IfSuccess<T, TResult>(this IResult<T> result,
                                                             Func<ISuccess<T>, IResult<TResult>> next)
        {
            var s = result as ISuccess<T>;
            if (s != null)
            {
                return next(s);
            }

            var f = (IFailure<T>) result;
            return new Failure<TResult>(f.FailedInput, () => f.Message, () => f.Expectations);
        }

        public static IResult<T> IfFailure<T>(this IResult<T> result, Func<IFailure<T>, IResult<T>> next)
        {
            var s = result as ISuccess<T>;
            if (s != null)
            {
                return s;
            }
            var f = (IFailure<T>) result;
            return next(f);
        }
    }
}