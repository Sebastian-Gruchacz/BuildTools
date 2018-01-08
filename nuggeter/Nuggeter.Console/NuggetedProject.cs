namespace Nuggeter.Console
{
    using System.IO;

    using Microsoft.Build.Construction;

    internal class NuggetedProject
    {
        private NuggetedProject(ProjectInSolution project = null)
        {
            if (project == null)
            {
                return;
            }

            this.Path = project.AbsolutePath;
            this.Name = project.ProjectName;

            // ...
        }

        public bool Exists { get; private set; } = true;

        public bool IsNuggetable { get; private set; } = false;
        public string Path { get; }
        public string Name { get; }

        #region Static Part

        public static NuggetedProject Load(ProjectInSolution project)
        {
            if (!File.Exists(project.AbsolutePath))
            {
                return new NuggetedProject
                {
                    Exists = false
                };
            }

            return new NuggetedProject(project);
        }

        #endregion Static Part
    }
}