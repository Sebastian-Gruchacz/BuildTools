namespace Nuggeter.Console
{
    using System.IO;

    internal class FileReference
    {
        public FileReference(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                this.Path = path;
                this.Exists = File.Exists(path);
            }
        }

        public bool Exists { get; private set; }

        public string Path { get; private set; }


        private static FileReference _notExist = 
            new FileReference(null)
            {
                Exists = false
            };

        public static FileReference NotExists => _notExist;
    }
}