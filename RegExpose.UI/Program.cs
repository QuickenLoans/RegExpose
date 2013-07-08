using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RegExpose.UI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(IsPresentation(args), GetPattern(args), GetInput(args)));
        }

        private static bool IsPresentation(string[] args)
        {
            return args.Any(arg => arg.ToLower() == "/presentation");
        }

        private static string GetPattern(string[] args)
        {
            return
                args.Select(arg => System.Text.RegularExpressions.Regex.Match(arg, "^/pattern=(.+)$", RegexOptions.IgnoreCase))
                    .Where(match => match.Success)
                    .Select(match => match.Groups[1].Value)
                    .FirstOrDefault();
        }

        private static string GetInput(string[] args)
        {
            return
                args.Select(arg => System.Text.RegularExpressions.Regex.Match(arg, "^/input=(.+)$", RegexOptions.IgnoreCase))
                    .Where(match => match.Success)
                    .Select(match => match.Groups[1].Value)
                    .FirstOrDefault();
        }
    }
}
