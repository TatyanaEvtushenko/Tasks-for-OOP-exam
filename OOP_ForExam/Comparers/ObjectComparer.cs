using System.Collections;

namespace OOP_ForExam.Comparers
{
    class ObjectComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            var hashX = x.GetHashCode();
            var hashY = y.GetHashCode();
            if (hashX > hashY)
            {
                return 1;
            }
            if (hashX < hashY)
            {
                return -1;
            }
            return 0;
        }
    }
}
