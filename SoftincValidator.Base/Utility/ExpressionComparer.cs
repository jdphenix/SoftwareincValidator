using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace SoftwareincValidator.Utility
{
    public class ExpressionComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _expr;

        public ExpressionComparer(Func<T, T, bool> expr)
        {
            _expr = expr;
        } 

        public bool Equals(T x, T y)
        {
            return _expr(x, y);
        }

        public int GetHashCode(T obj)
        {
            return 0; // to force equality comparer to run.
        }
    }

    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Except<T>(this IEnumerable<T> left, IEnumerable<T> right, Func<T, T, bool> equality)
        {
            return left.Except(right, new ExpressionComparer<T>(equality));
        } 
    }
}
