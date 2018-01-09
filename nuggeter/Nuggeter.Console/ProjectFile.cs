namespace Nuggeter.Console
{
    using System.IO;

    internal class ProjectFile
    {
        public ProjectFile(string fileName, string projectFilePath)
        {
            try
            {
                string absoluteName = new FromFolder(System.IO.Path.GetDirectoryName(projectFilePath)).MapAbsolutePath(fileName);
                this.Path = absoluteName;
                this.FileName = System.IO.Path.GetFileName(absoluteName);
                this.Exists = File.Exists(absoluteName);
            }
            catch
            {
                this.IsValid = false;
            }
        }

        public bool Exists { get; private set; }

        public bool IsValid { get; private set; } = true;

        public string Path { get; private set; }

        public string FileName { get; private set; }

        public FileType FileType { get; internal set; }
    }
}