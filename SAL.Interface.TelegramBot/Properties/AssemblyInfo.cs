using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: Guid("e273a361-7201-4014-93cb-5ba9fc546118")]
[assembly: System.CLSCompliant(false)]

#if NETCOREAPP
[assembly: AssemblyMetadata("ProjectUrl", "https://github.com/DKorablin/SAL.Interface.TelegramBot")]
#else

[assembly: AssemblyTitle("Interface.TelegramBot")]
[assembly: AssemblyDescription("Base interfaces for Telegram SAL plugins")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCompany("Danila Korablin")]
[assembly: AssemblyProduct("Interface.TelegramBot")]
[assembly: AssemblyCopyright("Copyright © Danila Korablin 2017-2020")]
#endif