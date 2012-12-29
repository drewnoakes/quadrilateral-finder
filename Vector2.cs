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

namespace DrewNoakes.QuadrilateralFinder
{
    /// <summary>
    /// A basic two dimensional vector.
    /// </summary>
    public struct Vector2
    {
        public double X { get; private set; }
        public double Y { get; private set; }

        public Vector2(double x, double y) : this()
        {
            X = x;
            Y = y;
        }

        public double Angle
        {
            get { return Math.Atan2(Y, X); }
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", X, Y);
        }

        public static Vector2 operator -(Vector2 a)
        {
            return new Vector2(
                -a.X,
                -a.Y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(
                a.X - b.X,
                a.Y - b.Y);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(
                a.X + b.X,
                a.Y + b.Y);
        }

        public static Vector2 operator *(Vector2 v, double scale)
        {
            return new Vector2(
                v.X*scale,
                v.Y*scale);
        }

        public double Cross(Vector2 vector2)
        {
            return (X*vector2.Y) - (Y*vector2.X);
        }
    }
}