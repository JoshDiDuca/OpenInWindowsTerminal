using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using Microsoft.Win32;
using Microsoft;
using System.Linq;

namespace OpenInWindowsTerminal
{
    internal sealed class OpenWindowsTerminalCommand
    {
        private readonly Package _package;
        private readonly Options _options;

        private OpenWindowsTerminalCommand(Package package, Options options)
        {
            _package = package;
            _options = options;

            var commandService = (OleMenuCommandService)ServiceProvider.GetService(typeof(IMenuCommandService));

            if (commandService != null)
            {
                var menuCommandID = new CommandID(PackageGuids.guidOpenInWindowsTerminalCmdSet, PackageIds.OpenInWindowsTerminal);
                var menuItem = new MenuCommand(OpenFolderInVs, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        public static OpenWindowsTerminalCommand Instance { get; private set; }

        private IServiceProvider ServiceProvider
        {
            get { return _package; }
        }

        public static void Initialize(Package package, Options options)
        {
            Instance = new OpenWindowsTerminalCommand(package, options);
        }

        private void OpenFolderInVs(object sender, EventArgs e)
        {
            try
            {
                var dte = (DTE2)ServiceProvider.GetService(typeof(DTE));
                Assumes.Present(dte);

                string path = ProjectHelpers.GetSelectedPath(dte, _options.OpenSolutionProjectAsRegularFile);

                if (!string.IsNullOrEmpty(path))
                {
                    int line = 0;

                    if (dte.ActiveDocument?.Selection is TextSelection selection)
                    {
                        line = selection.ActivePoint.Line;
                    }

                    OpenWindowsTerminal(path, line);
                }
                else
                {
                    MessageBox.Show("Couldn't resolve the folder");
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        private void OpenWindowsTerminal(string path, int line = 0)
        {
            bool isDirectory = Directory.Exists(path);

            /*string newPath = path;

            var ignoreEnding = new List<string> { "proj", ".json", ".png", ".jpg" };
            if (!isDirectory && (ignoreEnding.Select(end => path.EndsWith(end)).Any()))
                newPath = Path.GetDirectoryName(path);

            var args = isDirectory ? "." : line > 0 ? $"-g {newPath}:{line}" : $"{newPath}";
            if (!string.IsNullOrEmpty(_options.CommandLineArguments))
            {
                args = $"{args} {_options.CommandLineArguments}";
            }

            if (!isDirectory && (path.EndsWith("package.json")))
            {
                args = $"dotnet run " + args;
            }

            if (!isDirectory && (path.EndsWith(".ts")))
            {
                args = $"tsc " + args;
            }

            if (!isDirectory && (path.EndsWith(".cs")) || !isDirectory && (path.EndsWith(".csproj")))
            {
                args = $"dotnet run " + args;
            }*/

            if (!isDirectory)
                path = Path.GetDirectoryName(path);

            var start = new System.Diagnostics.ProcessStartInfo()
            {
                FileName = $"wt.exe",
                Arguments = " -d " + path,
                CreateNoWindow = true,
                UseShellExecute = false,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
            };

            using (System.Diagnostics.Process.Start(start))
            {

            }
        }
    }

}
