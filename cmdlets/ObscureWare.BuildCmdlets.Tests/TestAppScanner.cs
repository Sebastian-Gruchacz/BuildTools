namespace ObscureWare.BuildCmdlets.Tests
{
    using System.Linq;

    using Shouldly;

    using Xunit;

    public class TestAppScanner
    {
        [Theory]
        [InlineData(@".Net Framework")]
        [InlineData(@"Microsoft .Net")]
        public void app_scanner_shall_find_some_well_known_applications(string appNamePartial)
        {
            var apps = SystemHelpers.GetInstalledApplications().ToArray();

            var comparator = new StringContainsComparator(appNamePartial);
            apps.Select(app => app.Name).ShouldContain(s => comparator.IsSimilarTo(s));
        }

        [Fact]
        public void get_MSI_details()
        {
            var apps = SystemHelpers.GetInstalledApplications().ToList();
            var vsApps = apps.Where(app => app.Name.Contains("Visual Studio") || app.Name.Contains("Build Tools")).ToArray();

            foreach (var appInfo in vsApps)
            {
                var path = MsiHelper.GetProductInfo(appInfo.Id.ToString("B"));
                if (!string.IsNullOrWhiteSpace(path))
                {

                }
            }
        }
    }
}
