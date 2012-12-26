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
    public struct LineSegment2
    {
        public Vector2 End1 { get; private set; }
        public Vector2 End2 { get; private set; }

        public LineSegment2(Vector2 end1, Vector2 end2) : this()
        {
            End1 = end1;
            End2 = end2;
        }

        public Vector2 ToVector2()
        {
            return new Vector2(End2.X - End1.X, End2.Y - End1.Y);
        }

        public double AngleTo(LineSegment2 lineSegment)
        {
            return ToVector2().Angle - lineSegment.ToVector2().Angle;
        }

        public override string ToString()
        {
            return string.Format("({0}) -> ({1})", End1, End2);
        }
    }
}