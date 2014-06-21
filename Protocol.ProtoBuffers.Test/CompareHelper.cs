using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.Test
{
    public static class CompareHelper
    {
        public static bool IsEqual(DateTime[] a, DateTime[] b)
        {
            if (a == null && b == null)
                return true;
            if (a == null)
                return false;
            if (b == null)
                return false;
            if (a.Length != b.Length)
                return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (!IsEqual(a[i], b[i]))
                    return false;
            }

            return true;
        }

        public static bool IsEqual(long[] a, long[] b)
        {
            if (a == null && b == null)
                return true;
            if (a == null)
                return false;
            if (b == null)
                return false;
            if (a.Length != b.Length)
                return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }

            return true;
        }

        public static bool IsEqual(int[] a, int[] b)
        {
            if (a == null && b == null)
                return true;
            if (a == null)
                return false;
            if (b == null)
                return false;
            if (a.Length != b.Length)
                return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }

            return true;
        }
        public static bool IsEqual(List<String> a, List<String> b)
        {
            if (a == null && b == null)
                return true;
            if (a == null)
                return false;
            if (b == null)
                return false;
            if (a.Count != b.Count)
                return false;

            for (int i = 0; i < a.Count; i++)
            {
                if (!IsEqual(a[i], b[i]))
                    return false;
            }

            return true;
        }

        public static bool IsEqual(String[] a, String[] b)
        {
            if (a == null && b == null)
                return true;
            if (a == null)
                return false;
            if (b == null)
                return false;
            if (a.Length != b.Length)
                return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (!IsEqual(a[i], b[i]))
                    return false;
            }

            return true;
        }

        public static bool IsEqual(String a, String b)
        {
            if (a == null && b == null)
                return true;
            if (a == null)
                return false;
            if (b == null)
                return false;

            return a.Equals(b);
        }

        public static bool IsEqual(DateTime a, DateTime b)
        {
            if (a.Kind != b.Kind)
                return false;

            return a.Ticks == b.Ticks;
        }

        public static bool IsEqual(DateTime? a, DateTime? b)
        {
            if (a == null && b == null)
                return true;
            if (a == null)
                return false;
            if (b == null)
                return false;
            return IsEqual(a.Value, b.Value);
        }

        public static bool IsEqualObject(Object a, Object b)
        {
            if (a == null && b == null)
                return true;
            if (a == null)
                return false;
            if (b == null)
                return false;
            if (a.GetType() != b.GetType())
                return false;

            if (a.GetType().IsArray)
            {
                return IsEqualArray((Array)a, (Array)b);
            }

            return a.Equals(b);
        }

        public static bool IsEqualArray(Array a, Array b)
        {
            if (a == null && b == null)
                return true;
            if (a == null)
                return false;
            if (b == null)
                return false;
            
            Type aType = a.GetType();
            Type bType = b.GetType();

            if (aType != bType)
                return false;

            if (!aType.IsArray)
                return false;

            var aEnumerator = a.GetEnumerator();
            var bEnumerator = b.GetEnumerator();

            while (true)
            {
                bool aHasElement = aEnumerator.MoveNext();
                bool bHasElement = bEnumerator.MoveNext();

                if (aHasElement != bHasElement)
                    return false;

                if (!aHasElement)
                    return true;

                if (!IsEqualObject(aEnumerator.Current, bEnumerator.Current))
                    return false;
            }
        }
    }
}
