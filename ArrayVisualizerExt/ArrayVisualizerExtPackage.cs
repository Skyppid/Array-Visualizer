using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace ArrayVisualizerExt
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("Array Visualizer",
        "Visualizes 1D, 2D, 3D and 4D arrays and SharpDX Vectors and Matrices", "1.7", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(ArrayVisualizerToolWindow))]
    [Guid(GuidList.GuidArrayVisualizerExtPkgString)]
    public sealed class ArrayVisualizerExtPackage : AsyncPackage
    {
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public ArrayVisualizerExtPackage()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}",
                ToString()));
        }

        /// <summary>
        /// This function is called when the user clicks the menu item that shows the 
        /// tool window. See the Initialize method to see how the menu item is associated to 
        /// this function using the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void ShowToolWindow(object sender, EventArgs e)
        {
            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            ToolWindowPane window = FindToolWindow(typeof(ArrayVisualizerToolWindow), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException(Resources.CanNotCreateWindow);
            }

            ThreadHelper.ThrowIfNotOnUIThread();
            IVsWindowFrame windowFrame = (IVsWindowFrame) window.Frame;
            ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }


        /////////////////////////////////////////////////////////////////////////////
        // Overridden Package Implementation

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}",
                ToString()));
            Initialize();

            progress.Report(new ServiceProgressData("Registering menu commands for ArrayVisualizer."));

            // Add our command handlers for menu (commands must exist in the .vsct file)
            if (await GetServiceAsync(typeof(IMenuCommandService)) is OleMenuCommandService mcs)
            {
                // Create the command for the tool window
                CommandID toolwndCommandId = new CommandID(GuidList.GuidArrayVisualizerExtCmdSet,
                    (int) PkgCmdIdList.ArrayVisualizerTool);
                MenuCommand menuToolWin = new MenuCommand(ShowToolWindow, toolwndCommandId);

                await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
                mcs.AddCommand(menuToolWin);
                //GlobalVars.menuToolWin = menuToolWin;
            }
        }

        #endregion
    }
}