using System.Collections;
using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this List<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                // Swap elements at i and j
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
    }
}
