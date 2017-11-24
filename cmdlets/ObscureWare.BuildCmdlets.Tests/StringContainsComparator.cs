namespace ObscureWare.BuildCmdlets.Tests
{
    using System;

    public class StringContainsComparator
    {
        private readonly string _expected;

        public StringContainsComparator(string expected)
        {
            if (string.IsNullOrWhiteSpace(expected))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(expected));

            _expected = expected.ToUpper();
        }

        public bool IsSimilarTo(string other)
        {
            return (other?.ToUpper() ?? String.Empty).Contains(this._expected);
        }
    }
}