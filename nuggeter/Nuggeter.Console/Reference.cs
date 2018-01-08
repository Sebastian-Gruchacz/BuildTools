namespace Nuggeter.Console
{
    internal class Reference
    {
        public string Name { get; set; }
        public string Hints { get; set; }
        public bool SpecificVersion { get; internal set; }
        public string HintPath { get; internal set; }
    }
}