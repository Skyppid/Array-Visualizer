﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ArrayVisualizerControls.Properties
{


  /// <summary>
  ///   A strongly-typed resource class, for looking up localized strings, etc.
  /// </summary>
  // This class was auto-generated by the StronglyTypedResourceBuilder
  // class via a tool like ResGen or Visual Studio.
  // To add or remove a member, edit your .ResX file then rerun ResGen
  // with the /str option, or rebuild your VS project.
  [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
  internal class Resources
  {

    private static global::System.Resources.ResourceManager resourceMan;

    private static global::System.Globalization.CultureInfo resourceCulture;

    [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    internal Resources()
    {
    }

    /// <summary>
    ///   Returns the cached ResourceManager instance used by this class.
    /// </summary>
    [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
    internal static global::System.Resources.ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals(resourceMan, null))
        {
          global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ArrayVisualizerControls.Properties.Resources", typeof(Resources).Assembly);
          resourceMan = temp;
        }
        return resourceMan;
      }
    }

    /// <summary>
    ///   Overrides the current thread's CurrentUICulture property for all
    ///   resource lookups using this strongly typed resource class.
    /// </summary>
    [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
    internal static global::System.Globalization.CultureInfo Culture
    {
      get
      {
        return resourceCulture;
      }
      set
      {
        resourceCulture = value;
      }
    }

    /// <summary>
    ///   Looks up a localized string similar to source array must be a single dimension array..
    /// </summary>
    internal static string ArrayNot1DException
    {
      get
      {
        return ResourceManager.GetString("ArrayNot1DException", resourceCulture);
      }
    }

    /// <summary>
    ///   Looks up a localized string similar to source array must be two dimensional..
    /// </summary>
    internal static string ArrayNot2DException
    {
      get
      {
        return ResourceManager.GetString("ArrayNot2DException", resourceCulture);
      }
    }

    /// <summary>
    ///   Looks up a localized string similar to source array must be three dimensional..
    /// </summary>
    internal static string ArrayNot3DException
    {
      get
      {
        return ResourceManager.GetString("ArrayNot3DException", resourceCulture);
      }
    }

    /// <summary>
    ///   Looks up a localized string similar to source array must be four dimensional..
    /// </summary>
    internal static string ArrayNot4DException
    {
      get
      {
        return ResourceManager.GetString("ArrayNot4DException", resourceCulture);
      }
    }
  }
}
