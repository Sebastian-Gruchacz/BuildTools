namespace ObscureWare.BuildCmdlets
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Win32;

    public static class SystemHelpers
    {
        private static readonly HiveInfo[] _installationHives = new[]
        {
            new HiveInfo(Registry.CurrentUser, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"),
            new HiveInfo(Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"),
            new HiveInfo(Registry.LocalMachine, @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall")
        };

        /// <summary>
        /// Enumerate all installed applications from registry
        /// </summary>
        /// <returns></returns>
        /// <remarks> Info here:
        /// https://stackoverflow.com/questions/908850/get-installed-applications-in-a-system
        /// https://stackoverflow.com/questions/24909108/get-installed-software-list-using-c-sharp
        /// </remarks>
        public static IEnumerable<AppInfo> GetInstalledApplications()
        {
            foreach (var installationHive in _installationHives)
            {
                using (Microsoft.Win32.RegistryKey key = installationHive.Root.OpenSubKey(installationHive.Path))
                {
                    if (key == null)
                    {
                        continue;
                    }

                    foreach (string subkeyName in key.GetSubKeyNames())
                    {
                        Guid guid;
                        if (!Guid.TryParse(subkeyName, out guid))
                        {
                            continue;
                        }

                        using (var subkey = key.OpenSubKey(subkeyName))
                        {
                            if (subkey == null)
                            {
                                continue; // disappeared suddenly?
                            }

                            string name = subkey.GetValue(@"DisplayName")?.ToString();
                            string sourcePath = subkey.GetValue(@"InstallSource")?.ToString();
                            string installpath = subkey.GetValue(@"InstallLocation")?.ToString();

                            if (string.IsNullOrWhiteSpace(name) ||
                                (string.IsNullOrWhiteSpace(sourcePath) && string.IsNullOrWhiteSpace(installpath)))
                            {
                                continue;
                            }

                            yield return new AppInfo(guid, name, sourcePath, installpath);

                        }
                    }
                }
            }
        }
    }
}