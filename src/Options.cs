using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace OpenInWindowsTerminal
{
    public class Options : DialogPage
    {
        [Category("General")]
        [DisplayName("Command line arguments")]
        [Description("Command line arguments to pass to wt.exe")]
        public string CommandLineArguments { get; set; }

        [Category("General")]
        [DisplayName("Open solution/project as regular file")]
        [Description("When true, opens solutions/projects as regular files and does not load folder path into wt.")]
        public bool OpenSolutionProjectAsRegularFile { get; set; }

        protected override void OnApply(PageApplyEventArgs e)
        {
            base.OnApply(e);
        }
    }
}
