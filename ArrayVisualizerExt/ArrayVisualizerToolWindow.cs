 using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace ArrayVisualizerExt
{
  /// <summary>
  /// This class implements the tool window exposed by this package and hosts a user control.
  ///
  /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane, 
  /// usually implemented by the package implementer.
  ///
  /// This class derives from the ToolWindowPane class provided from the MPF in order to use its 
  /// implementation of the IVsUIElementPane interface.
  /// </summary>
  [Guid("c0023666-5bf2-4e35-8732-6490c0736f6b")]
  public class ArrayVisualizerToolWindow : ToolWindowPane
  {
    ArrayVisualizerToolControl arrayVisualizerToolControl;

    /// <summary>
    /// Standard constructor for the tool window.
    /// </summary>
    public ArrayVisualizerToolWindow() :
      base(null)
    {
      // Set the window title reading it from the resources.
      Caption = Resources.ToolWindowTitle;
      // Set the image that will appear on the tab of the window frame
      // when docked with an other window
      // The resource ID correspond to the one defined in the resx file
      // while the Index is the offset in the bitmap strip. Each image in  
      // the strip being 16x16.
      this.BitmapResourceID = 301;
      this.BitmapIndex = 1;

      // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
      // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on 
      // the object returned by the Content property.
      arrayVisualizerToolControl = new ArrayVisualizerToolControl();
      base.Content = arrayVisualizerToolControl;
    }

    public override void OnToolWindowCreated()
    {
      EnvDTE.DTE dte = (EnvDTE.DTE)GetService(typeof(EnvDTE.DTE));
      EnvDTE80.Events2 events = (EnvDTE80.Events2)dte.Events;

      var windowVisibility = events.get_WindowVisibilityEvents();

      windowVisibility.WindowShowing += WindowVisibility_WindowShowing;
      windowVisibility.WindowHiding += WindowVisibility_WindowHiding;
    }

    void WindowVisibility_WindowHiding(EnvDTE.Window Window)
    {
      if (Window.Kind == "Tool" && Window.Caption == Resources.ToolWindowTitle)
        arrayVisualizerToolControl.ToolDeactivated();
    }

    void WindowVisibility_WindowShowing(EnvDTE.Window Window)
    {
      if (Window.Kind == "Tool" && Window.Caption == Resources.ToolWindowTitle)
        arrayVisualizerToolControl.ToolActivated();
    }
  }
}
