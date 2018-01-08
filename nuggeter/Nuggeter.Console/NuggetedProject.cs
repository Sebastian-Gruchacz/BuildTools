namespace Nuggeter.Console
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;

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
 
            this.LoadSpecialFiles(project);
            this.LoadDependencies(project);
            // ...
        }

        private void LoadSpecialFiles(ProjectInSolution project)
        {
            //this.NugetFile = project.Dependencies.Where(p => p.)
        }

        private void LoadDependencies(ProjectInSolution project)
        {
            
        }

        public bool Exists { get; private set; } = true;

        public bool IsNuggetable { get; private set; } = false;

        public bool IsSupported { get; private set; } = true;

        public bool IsValid { get; private set; } = true;

        public string Path { get; }

        public string Name { get; }



        private IEnumerable<Reference> LoadReferences(string content)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(content);
            var nsm = new XmlNamespaceManager(xDoc.NameTable);
            nsm.AddNamespace("msbuild", @"http://schemas.microsoft.com/developer/msbuild/2003");

            var refs = xDoc.DocumentElement.SelectNodes(@"//msbuild:ItemGroup/msbuild:Reference", nsm);
            if (refs != null)
            {
                foreach (XmlNode referrence in refs)
                {
                    yield return BuildReferrenceInfoFromXml(referrence);
                }
            }
        }

        private Reference BuildReferrenceInfoFromXml(XmlNode referrence)
        {
            var includeValue = referrence.Attributes["Include"].Value;
            var includeInfo = AnalyzeIncludeInfoString(includeValue);

            var referenceInfo = new Reference
            {
                Name = includeInfo.Name
            };

            foreach (XmlNode child in referrence.ChildNodes)
            {
                switch (child.Name)
                {
                    case "HintPath":
                        {
                            referenceInfo.HintPath = child.InnerText;
                            break;
                        }
                    case "SpecificVersion":
                        {
                            referenceInfo.SpecificVersion = bool.Parse(child.InnerText.ToLower());
                            break;
                        }
                    case "Private":
                        {
                            // TODO: ???
                            break;
                        }
                    case "RequiredTargetFramework":
                        {
                            // TODO: ???
                            break;
                        }
                    case "EmbedInteropTypes":
                        {
                            // TODO: ???
                            break;
                        }
                    default:
                        {
                            throw new Exception("Unknown Reference option in Project file - " + child.Name);
                        }
                }
            }

            return referenceInfo;
        }

        private IncludeInfo AnalyzeIncludeInfoString(string includeValue)
        {
            string[] parts = includeValue.Split(',');
            if (parts.Length == 1)
            {
                return new IncludeInfo
                {
                    Name = includeValue.Trim()
                };
            }
            else
            {
                var info = new IncludeInfo
                {
                    Name = parts[0].Trim()
                };

                // ... version


                return info;
            }
        }

        private class IncludeInfo
        {
            public string Name { get; internal set; }
        }

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

            if (!project.AbsolutePath.EndsWith("*.csproj"))
            {
                return new NuggetedProject
                {
                    IsSupported = false
                };
            }

            try
            {
                return new NuggetedProject(project);
            }
            catch (Exception)
            {
                return new NuggetedProject
                {
                    IsValid = false
                };
            }
        }

        #endregion Static Part
    }
}