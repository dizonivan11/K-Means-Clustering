using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace KMeansClustering
{
    public class Cluster
    {
        public Brush color { get; set; }
        public Point centroid { get; set; }
        public List<Point> points { get; set; }

        public Cluster(MainForm mf)
        {
            color = new SolidBrush(Color.FromArgb(mf.rand.Next(200), mf.rand.Next(200), mf.rand.Next(200)));
            // Set random points for each centroids
            centroid = new Point(mf.rand.Next(mf.OFFSET, mf.Width - mf.OFFSET), mf.rand.Next(mf.OFFSET, mf.Height - mf.OFFSET));
            points = new List<Point>();
        }

        public void CenterCentroid()
        {
            int meanX = 0;
            int meanY = 0;
            for (int p = 0; p < points.Count; p++)
            {
                meanX += points[p].X;
                meanY += points[p].Y;
            }
            meanX /= points.Count;
            meanY /= points.Count;
            centroid = new Point(meanX, meanY);
        }

        public static Cluster Empty(MainForm mf)
        {
            Cluster ret = new Cluster(mf);
            ret.centroid = new Point(-1, -1);
            return ret;
        }
    }
}
