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
        private XmlDocument _xDoc;
        private XmlNamespaceManager _namespaceMngr;

        private NuggetedProject(ProjectInSolution project = null)
        {
            if (project == null)
            {
                return;
            }


            this.Path = project.AbsolutePath;
            this.Name = project.ProjectName;

            this.LoadXml();

            this.LoadReferences();
            this.LoadSpecialFiles();
            this.LoadDependencies();
            // ...
        }

        private void LoadXml()
        {
            _xDoc = new XmlDocument();
            _xDoc.Load(this.Path);
            _namespaceMngr = new XmlNamespaceManager(_xDoc.NameTable);
            _namespaceMngr.AddNamespace("msbuild", @"http://schemas.microsoft.com/developer/msbuild/2003");

            this.SchemaVersion = this.RecognizeFormat();
        }

        private VsProjectSchemaVersion RecognizeFormat()
        {
            var fwkVersion = _xDoc.DocumentElement.SelectSingleNode(@"//PropertyGroup/TargetFramework", _namespaceMngr);
            if (fwkVersion != null)
            {
                return VsProjectSchemaVersion.New;
            }
            else
            {
                return VsProjectSchemaVersion.Clasic;
            }
        }

        public bool Exists { get; private set; } = true;

        public bool IsNuggetable { get; private set; } = false;

        public bool IsSupported { get; private set; } = true;

        public bool IsValid { get; private set; } = true;

        public string Path { get; }

        public string Name { get; }

        public ICollection<Reference> References { get; } = new List<Reference>();

        public VsProjectSchemaVersion SchemaVersion { get; private set; }

        private void LoadSpecialFiles()
        {
            //this.NugetFile = project.Dependencies.Where(p => p.)
        }

        private void LoadDependencies()
        {

        }

        private void LoadReferences()
        {
            if (this.SchemaVersion == VsProjectSchemaVersion.Clasic)
            {
                var refs = _xDoc.DocumentElement.SelectNodes(@"//msbuild:ItemGroup/msbuild:Reference", _namespaceMngr);
                if (refs != null)
                {
                    foreach (XmlNode referrence in refs)
                    {
                        References.Add(BuildReferenceInfoFromXml(referrence));
                    }
                }
            }
            else
            {
                var refs = _xDoc.DocumentElement.SelectNodes(@"//ItemGroup/ProjectReference", _namespaceMngr);

                // ... other types
            }
        }

        private Reference BuildReferenceInfoFromXml(XmlNode reference)
        {
            if (reference == null) throw new ArgumentNullException(nameof(reference));

            var includeValue = reference.Attributes["Include"]?.Value;
            var includeInfo = AnalyzeIncludeInfoString(includeValue);

            var referenceInfo = new Reference
            {
                Name = includeInfo.Name
            };

            foreach (XmlNode child in reference.ChildNodes)
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
            if (includeValue == null)
            {
                return null;
            }

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

            if (!project.AbsolutePath.EndsWith(".csproj"))
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