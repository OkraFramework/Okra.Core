// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNetCore.Testing
{
    public static class TestPlatformHelper
    {
        public static bool IsMono { get { return false; } }
        public static bool IsWindows { get { return true; } }
        public static bool IsLinux { get { return false; } }
        public static bool IsMac { get { return false; } }
    }
}
