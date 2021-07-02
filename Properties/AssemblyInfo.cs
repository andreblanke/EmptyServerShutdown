using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle        ("EmptyServerShutdown")]
[assembly: AssemblyDescription  ("TShock plugin to shut down empty servers")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyProduct      ("EmptyServerShutdown")]
[assembly: AssemblyCopyright    ("Copyright © Andre Blanke 2021")]
[assembly: ComVisible           (false)]
[assembly: Guid                 ("06054E55-7E7D-48EC-B874-5E08D88F0E0C")]
[assembly: AssemblyVersion      ("1.1.0.0")]
