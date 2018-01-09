namespace Nuggeter.Console
{
    using System.IO;

    internal class FromFolder
    {
        private readonly string _directoryPath;

        public FromFolder(string directoryPath)
        {
            this._directoryPath = directoryPath;
        }

        public string MapAbsolutePath(string fileName)
        {
            return new FileInfo(Path.Combine(this._directoryPath, fileName)).FullName;
        }
    }
}