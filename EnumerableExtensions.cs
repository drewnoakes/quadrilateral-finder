#region License

/* 
 * This file is part of QuadrilateralFinder.
 *
 * QuadrilateralFinder is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * QuadrilateralFinder is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with QuadrilateralFinder.  If not, see <http://www.gnu.org/licenses/>.
 */

#endregion

// Copyright Drew Noakes, http://drewnoakes.com

using System;
using System.Collections.Generic;
using System.Linq;

namespace DrewNoakes.QuadrilateralFinder
{
    internal static class EnumerableExtensions
    {
        /// <summary>
        /// Returns all combinations of size <paramref name="size"/> from the source <paramref name="elements"/>.
        /// Each returned combination is a unique unordered set.
        /// <code>Combinations({0,1,2,3}, 2) -> [{0,1},{0,2},{0,3},{1,2},{1,3},{2,3}]</code>
        /// </summary>
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int size)
        {
            return size == 0
                       ? new[] { Enumerable.Empty<T>() }
                       : elements.SelectMany((e, i) => elements.Skip(i + 1).Combinations(size - 1).Select(c => (new[] { e }).Concat(c)));
        }

        /// <summary>
        /// Returns all permutations of the source set.
        /// <code>Permutations({0,1,2}) -> [{0,1,2},{0,2,1},{1,0,2},{1,2,0},{2,0,1},{2,1,0}]</code>
        /// </summary>
        public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            return PermutationsInternal(source.ToArray());
        }

        private static IEnumerable<IEnumerable<T>> PermutationsInternal<T>(IEnumerable<T> source)
        {
            var c = source.Count();

            if (c == 1)
            {
                yield return source;
                yield break;
            }

            for (var i = 0; i < c; i++)
            {
                foreach (var p in PermutationsInternal(source.Take(i).Concat(source.Skip(i + 1))))
                {
                    yield return source.Skip(i).Take(1).Concat(p);
                }
            }
        }
    }
}