namespace ObscureWare.BuildCmdlets
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("{Name}")]
    public class AppInfo
    {

        public AppInfo(Guid id, string name, string sourcePath, string instalPath)
        {
            this.Id = id;
            this.Name = name;
            this.SourcePath = sourcePath;
            this.InstallPath = instalPath;
        }

        public Guid Id { get; }

        public string Name { get; }

        public string SourcePath { get; }

        public string InstallPath { get; }
    }
}