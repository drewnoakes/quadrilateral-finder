#region License

/* 
 * This file is part of QuadrilateralFinder.
 *
 * QuadrilateralFinder is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * TinMan is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with QuadrilateralFinder.  If not, see <http://www.gnu.org/licenses/>.
 */

#endregion

// Copyright Drew Noakes, http://drewnoakes.com

namespace DrewNoakes.QuadrilateralFinder
{
    public struct Intersection
    {
        public LineSegment2 Line1 { get; private set; }
        public LineSegment2 Line2 { get; private set; }
        public Vector2 Point { get; private set; }
        public double Distance1 { get; private set; }
        public double Distance2 { get; private set; }
        public bool HasIntersection { get; private set; }

        public Intersection(LineSegment2 line1, LineSegment2 line2) : this()
        {
            Line1 = line1;
            Line2 = line2;

            var p = line1.End1;
            var q = line2.End1;
            var r = line1.ToVector2();
            var s = line2.ToVector2();

            var denom = r.Cross(s);

            if (denom == 0)
            {
                // The lines are parallel and there's no intersection
                HasIntersection = false;
                Distance1 = double.NaN;
                Distance2 = double.NaN;
                return;
            }

            var t = (q - p).Cross(s)/denom;
            var u = (q - p).Cross(r)/denom;

            Point = p + r*t;
            Distance1 = t;
            Distance2 = u;
            HasIntersection = true;
        }
    }
}