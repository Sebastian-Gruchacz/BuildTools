namespace ObscureWare.BuildCmdlets
{
    using Microsoft.Win32;

    internal class HiveInfo
    {
        public HiveInfo(RegistryKey rootKey, string path)
        {
            this.Root = rootKey;
            this.Path = path;
        }

        public RegistryKey Root { get; }

        public string Path { get; }
    }
}