using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Houseplants.Model.Helpers
{
    internal static class MathHelper
    {
        public static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        public static int GCD(IEnumerable<int> numbers)
        {
            Contract.Requires(numbers != null);
            Contract.Requires(numbers.Any());

            return numbers.Aggregate(GCD);
        }
    }
}