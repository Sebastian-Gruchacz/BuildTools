namespace Nuggeter.Console
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

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

        public FileReference NuggetConfig { get; private set; }

        private void LoadSolution(string path)
        {
            var sln = SolutionFile.Parse(path);
            this.Name = System.IO.Path.GetFileNameWithoutExtension(path);
            var rootFolder = System.IO.Path.GetDirectoryName(path);

            var projects = sln.ProjectsInOrder;
            var projectsTree = this.LoadProjectsStack(projects, rootFolder);
            
            this.FindNugetConfig(sln, rootFolder);
        }

        /// <summary>
        /// Tries to locate Nugget.config in solution folder
        /// </summary>
        /// <param name="sln"></param>
        /// <param name="rootFolder"></param>
        private void FindNugetConfig(SolutionFile sln, string rootFolder)
        {
            var configFile = Directory.EnumerateFiles(rootFolder, "nuget.config", SearchOption.TopDirectoryOnly).FirstOrDefault();
            if (configFile != null)
            {
                this.NuggetConfig = new FileReference(configFile);
            }
            else
            {
                this.NuggetConfig = FileReference.NotExists;
            }
        }

        private ProjectsProcessingStack LoadProjectsStack(IReadOnlyList<ProjectInSolution> projects, string rootFolder)
        {
            var stack = new ProjectsProcessingStack();

            foreach (var project in projects)
            {
                if (project.ProjectType == SolutionProjectType.KnownToBeMSBuildFormat)
                {
                    Console.WriteLine($"Analyzing {project.ProjectName}...");

                    var nuggetedProject = NuggetedProject.Load(project);
                    if (nuggetedProject.Exists && nuggetedProject.IsValid && nuggetedProject.IsNuggetable && nuggetedProject.IsSupported)
                    {
                        stack.Append(nuggetedProject);
                    }
                }
            }

            stack.Build();

            return stack;
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
}