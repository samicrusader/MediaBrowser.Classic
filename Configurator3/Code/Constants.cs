using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configurator
{
    public static class Constantsa
    {
        public const float MAX_ASPECT_RATIO_STRETCH = 10000;
        public const float MAX_ASPECT_RATIO_DEFAULT = 0.05F;

        public static readonly String ENTRYPOINTS_REGISTRY_PATH = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Media Center\Extensibility\Entry Points";
        public static readonly String CATEGORIES_REGISTRY_PATH = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Media Center\Extensibility\Categories";
        public static readonly String HIDDEN_CATEGORIES_GUID = @"MediaBrowserHidden";
        public static readonly String APPLICATION_ID = @"{4EC70CB4-5070-492F-90E0-EA0D290FB63B}";
        public static readonly String MB_MAIN_ENTRYPOINT_GUID = @"{AF7F9BF3-06F1-46BC-B3B6-98624018966E}";
        public static readonly String MB_CONFIG_ENTRYPOINT_GUID = @"{4C6D60AF-02AF-402A-8E5E-275C95BF59F9}";

        public static readonly String HKEY_LOCAL_MACHINE = @"HKEY_LOCAL_MACHINE";
    }
}
