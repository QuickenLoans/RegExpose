#RegExpose

RegExpose is a regular expression engine that exposes its inner-workings. It is comprised of two parts: the engine itself, and a UI that visually displays those inner-workings.

#####Usage:

    // Create a regex compiler with default options.
    RegexCompiler compiler = new RegexCompiler();

    // Compile a regex that searches for "foo" followed by an optional "bar".
    Regex regex = compiler.Compile("foo(?:bar)?");

    // Create an engine that parses "foobar".
    RegexEngine engine = regex.Parse("foobar");

    // Get a list of the steps that the engine takes while parsing its input.
    // Note that the result is lazily evaluated, and you can perform LINQ queries on it.
    IEnumerable<ParseStep> steps = engine.GetParseSteps();

    // Get the first match found in the input string.
    // Note that the RegExpose.Match class mimics the System.Text.RegularExpressions.Match class.
    Match match = engine.GetMatch();

    // Get a collection of all matches. The collection is lazily evaluated.
    IEnumerable<Match> matches = engine.GetMatches();

    // Replaces all matches from the input string with "quxbaz".
    // Like the System.Text.RegularExpressions.Regex.Replace method, this has several overloads.
    string replacement = engine.Replace("quxbaz");

###UI

The UI uses RegExpose in order to:
* Visually display the tree-like structure of a regular expression.
* Allow the user to visually "debug" the steps that the engine takes while parsing its input string.
