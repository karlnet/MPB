using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MPB
{
    public static class TExtensions
    {
        [DebuggerStepThrough]
        public static void MustBeTrue(this bool b)
        {
            if (!b) throw new ArgumentException();
        }
        [DebuggerStepThrough]
        public static void MustBeFalse(this bool b)
        {
            if (b) throw new ArgumentException();
        }
        [DebuggerStepThrough]
        public static void MustBeNonNull<T>(this T arg) where T : class
        {
            if (arg == null) throw new ArgumentNullException();
        }
        [DebuggerStepThrough]
        public static void MustBeNonEmpty<T>(this T[] array)
        {
            if (array.Length == 0) throw new ArgumentException();
        }
        [DebuggerStepThrough]
        public static void MustBeNonEmpty(this string arg)
        {
            if (String.IsNullOrEmpty(arg)) throw new ArgumentException();
        }
        [DebuggerStepThrough]
        public static void MustEqual<T>(this T arg, T value) where T : IComparable<T>
        {
            if (arg.CompareTo(value) != 0) throw new ArgumentOutOfRangeException();
        }
        [DebuggerStepThrough]
        public static void MustBeGreaterThan<T>(this T arg, T value) where T : IComparable<T>
        {
            if (arg.CompareTo(value) <= 0) throw new ArgumentOutOfRangeException();
        }
        [DebuggerStepThrough]
        public static void MustBeGreaterThanOrEqualTo<T>(this T arg, T value) where T : IComparable<T>
        {
            if (arg.CompareTo(value) == -1) throw new ArgumentOutOfRangeException();
        }
        [DebuggerStepThrough]
        public static void MustBeLessThan<T>(this T arg, T value) where T : IComparable<T>
        {
            if (arg.CompareTo(value) >= 0) throw new ArgumentOutOfRangeException();
        }
        [DebuggerStepThrough]
        public static void MustBeLessThanOrEqualTo<T>(this T arg, T value) where T : IComparable<T>
        {
            if (arg.CompareTo(value) == 1) throw new ArgumentOutOfRangeException();
        }
    }
}
