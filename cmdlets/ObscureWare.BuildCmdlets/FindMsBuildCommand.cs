namespace ObscureWare.BuildCmdlets
{
    using System.Linq;
    using System.Management.Automation;

    [Cmdlet(VerbsCommon.Find, "MsBild")]
    public class FindMsBuildCommand : Cmdlet
    {
        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            var apps = SystemHelpers.GetInstalledApplications()
                .Where(app => app.Name.Contains("Visual Studio") || app.Name.Contains("Build Tools")).ToArray();


        }


    }
}
