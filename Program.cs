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
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DrewNoakes.QuadrilateralFinder
{
    internal static class Program
    {
        #region Test Data

        private static readonly LineSegment2[] _sampleLineSegments = new[]
            {
                // TOP
                new LineSegment2(new Vector2(335, 120), new Vector2(517, 144)),
                // BOTTOM
                new LineSegment2(new Vector2(287, 604), new Vector2(558, 619)),
                // LEFT
                new LineSegment2(new Vector2(323, 131), new Vector2(275, 587)),
                // RIGHT
                new LineSegment2(new Vector2(589, 473), new Vector2(580, 606)),

                // NOISE
                new LineSegment2(new Vector2(368, 39), new Vector2(489, 108)),
                new LineSegment2(new Vector2(53, 286), new Vector2(293, 406)),
                new LineSegment2(new Vector2(299, 347), new Vector2(214, 538)),
                new LineSegment2(new Vector2(200, 370), new Vector2(149, 528)),
                new LineSegment2(new Vector2(6, 446), new Vector2(68, 449)),
                new LineSegment2(new Vector2(66, 444), new Vector2(150, 525)),
                new LineSegment2(new Vector2(389, 514), new Vector2(518, 644))
            };

        #endregion

        private static void Main()
        {
            Console.Out.WriteLine(string.Join(",", Enumerable.Range(0, 3).Permutations().Select(c => string.Format("{{{0}}}", string.Join(",", c)))));
            var stopWatch = Stopwatch.StartNew();

            var intersections = QuadrilateralFinder.FindQuadrilateral(_sampleLineSegments);

            Console.Out.WriteLine("Completed in {0} ms", stopWatch.Elapsed.TotalMilliseconds);

            Render(_sampleLineSegments, intersections);
        }

        private static void Render(IEnumerable<LineSegment2> allLineSegments, Intersection[] intersections)
        {
            var allPoints = allLineSegments.SelectMany(l => new[] { l.End1, l.End2 }).Concat(intersections.Select(i => i.Point)).ToList();

            var width = (int)Math.Ceiling(allPoints.Max(p => p.X));
            var height = (int)Math.Ceiling(allPoints.Max(p => p.Y));

            var linePen = new Pen(Color.White, 5);
            var selectedLinePen = new Pen(Color.Yellow, 8);
            var quadrilateralPen = new Pen(Color.Red, 5);

            var form = new Form { ClientSize = new Size(width, height) };
            var bitmap = new Bitmap(width, height);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.Black);

                foreach (var line in _sampleLineSegments)
                    g.DrawLine(linePen, (float)line.End1.X, (float)line.End1.Y, (float)line.End2.X, (float)line.End2.Y);
                foreach (var line in intersections.SelectMany(i => new[] { i.Line1, i.Line2 }).Distinct())
                    g.DrawLine(selectedLinePen, (float)line.End1.X, (float)line.End1.Y, (float)line.End2.X, (float)line.End2.Y);
                g.DrawPolygon(quadrilateralPen, intersections.Select(i => new PointF((float)i.Point.X, (float)i.Point.Y)).ToArray());
            }

            form.Controls.Add(new PictureBox { Image = bitmap, Width = width, Height = height });
            form.ShowDialog();
        }
    }
}