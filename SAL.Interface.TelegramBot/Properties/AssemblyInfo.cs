﻿using System.Reflection;
using System.Runtime.InteropServices;

[assembly: Guid("e273a361-7201-4014-93cb-5ba9fc546118")]
[assembly: System.CLSCompliant(false)]

#if NETCOREAPP
[assembly: AssemblyMetadata("ProjectUrl", "https://github.com/DKorablin/SAL.Interface.TelegramBot")]
#else

[assembly: AssemblyDescription("Base interfaces for Telegram SAL plugins")]
[assembly: AssemblyCopyright("Copyright © Danila Korablin 2017-2025")]
#endif