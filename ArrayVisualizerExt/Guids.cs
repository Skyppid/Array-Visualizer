// Guids.cs
// MUST match guids.h

using System;

namespace ArrayVisualizerExt
{
    static class GuidList
    {
        public const string GuidArrayVisualizerExtPkgString = "d22013ee-b366-456f-9291-7a2d201d7e24";
        public const string GuidArrayVisualizerExtCmdSetString = "0375ad9d-d952-4631-bb41-c9c0ab3a0e36";
        public const string GuidToolWindowPersistanceString = "c0023666-5bf2-4e35-8732-6490c0736f6b";

        public static readonly Guid GuidArrayVisualizerExtCmdSet = new Guid(GuidArrayVisualizerExtCmdSetString);
    };
}