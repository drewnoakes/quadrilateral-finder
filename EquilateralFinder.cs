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
    public static class QuadrilateralFinder
    {
        /// <summary>
        /// Detects the most likely quadrilateral shape formed by four line segments from <paramref name="lineSegments"/>.
        /// Possible solutions are evaluated based upon the closeness of corner angles to 90°, and penalties are assigned
        /// if line segments would intersect within their spans.
        /// </summary>
        /// <param name="lineSegments"></param>
        /// <returns>The intersection points that form the found quadrilateral.</returns>
        public static Intersection[] FindQuadrilateral(IEnumerable<LineSegment2> lineSegments)
        {
            var smallestError = Double.MaxValue;
            Intersection[] bestIntersections = null;

            // Evaluate all possible choices of four edges from the input set
            foreach (var edgeSet in lineSegments.Combinations(4))
            {
                // Evaluate them all in order
                foreach (var edges in edgeSet.Permutations().Select(Enumerable.ToArray))
                {
                    // determine the intersections we'd have if we used these lines
                    var intersections = new[]
                        {
                            new Intersection(edges[0], edges[1]),
                            new Intersection(edges[1], edges[2]),
                            new Intersection(edges[2], edges[3]),
                            new Intersection(edges[3], edges[0])
                        };

                    // Check for parallel lines that didn't have an intersection
                    if (!intersections.All(i => i.HasIntersection))
                        continue;

                    var error = CalculateError(intersections);

                    if (error < smallestError)
                    {
                        smallestError = error;
                        bestIntersections = intersections;
                    }
                }
            }

            return bestIntersections;
        }

        private static double CalculateError(Intersection[] intersections)
        {
            var distanceError = intersections.Sum(
                intersection =>
                {
                    Func<double, double> calcDistancePenalty = d =>
                    {
                        if (d > 0 && d < 1)
                        {
                            // Itersection within the line segment is a bad sign, so penalise for it, proportionally
                            return -Math.Log(d > 0.5 ? 1 - d : d);
                        }
                        return 0;
                    };

                    return calcDistancePenalty(intersection.Distance1) + calcDistancePenalty(intersection.Distance2);
                }
                );

            var angles = new[]
                {
                    GetAcuteAngleBetweenPoints(intersections[0].Point, intersections[1].Point, intersections[2].Point),
                    GetAcuteAngleBetweenPoints(intersections[1].Point, intersections[2].Point, intersections[3].Point),
                    GetAcuteAngleBetweenPoints(intersections[2].Point, intersections[3].Point, intersections[0].Point),
                    GetAcuteAngleBetweenPoints(intersections[3].Point, intersections[0].Point, intersections[1].Point)
                };


            // Sum of squared errors
            const double piOnTwo = Math.PI/2;
            var angleError = angles.Sum(angle => Math.Pow(piOnTwo - Math.Abs(angle), 2));

            return angleError + distanceError;
        }

        private static double GetAcuteAngleBetweenPoints(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            var v1 = p2 - p1;
            var v2 = p2 - p3;
            var angle = v2.Angle - v1.Angle;
            if (angle < -Math.PI)
                angle += Math.PI*2;
            if (angle > Math.PI)
                angle -= Math.PI*2;
            return angle;
        }
    }
}