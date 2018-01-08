namespace Nuggeter.Console
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.Build.Construction;

    class NuggetedSolution
    {
        private NuggetedSolution(string path = null)
        {
            if (path == null)
            {
                return;
            }

            this.Path = new FileInfo(path).FullName; // this will also validate path and names

            this.LoadSolution(this.Path);
        }

        public bool Exists { get; private set; } = true;

        public bool IsValid { get; private set; } = true;

        public string Path { get; private set; } = String.Empty;

        public string Name { get; private set; } = String.Empty;


        private void LoadSolution(string path)
        {
            var sln = SolutionFile.Parse(path);
            this.Name = System.IO.Path.GetFileNameWithoutExtension(path);
            var rootFolder = System.IO.Path.GetDirectoryName(path);

            var projects = sln.ProjectsInOrder;
            var projectsTree = this.LoadProjectsTree(projects, rootFolder);
        }

        private ProjectsTree LoadProjectsTree(IReadOnlyList<ProjectInSolution> projects, string rootFolder)
        {
            var tree = new ProjectsTree();

            foreach (var project in projects)
            {
                var nuggetedProject = NuggetedProject.Load(project);
                if (nuggetedProject.Exists && nuggetedProject.IsNuggetable)
                {
                    tree.Append(nuggetedProject);
                }
            }

            tree.Build();

            return tree;
        }


        #region Static Part

        public static NuggetedSolution FromFile(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    return NuggetedSolution.NotFound;
                }

                return new NuggetedSolution(path);

            }
            catch (Exception)
            {
                return NuggetedSolution.Invalid;
            }
        }

        public static NuggetedSolution Invalid
        {
            get
            {
                return new NuggetedSolution
                {
                    IsValid = false
                };
            }
        }

        public static NuggetedSolution NotFound
        {
            get
            {
                return new NuggetedSolution
                {
                    Exists = false
                };
            }
        }

        #endregion
    }

    internal class ProjectsTree
    {
        public void Build()
        {
            throw new NotImplementedException();
        }

        public void Append(object nuggetedProject)
        {
            throw new NotImplementedException();
        }
    }
}