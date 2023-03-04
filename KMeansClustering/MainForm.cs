using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KMeansClustering
{
    public partial class MainForm : Form
    {
        public Timer clock { get; set; }
        public Random rand { get; set; }
        public List<Cluster> clusters { get; set; }
        public List<Point> points { get; set; }
        public List<Cluster> parents { get; set; }
        public Cluster empty { get; set; }

        // Configurations
        public int OFFSET = 0;
        public int NUMBER_OF_CLUSTER = 10;
        public int NUMBER_OF_POINTS = 2000;
        public int SIZE = 12;

        public MainForm()
        {
            InitializeComponent();
            clock = new Timer();
            clock.Interval = 1;
            clock.Start();
            rand = new Random();
            clusters = new List<Cluster>();
            points = new List<Point>();
            parents = new List<Cluster>();
            empty = Cluster.Empty(this);

            // Initialization of Sample Data
            for (int c = 0; c < NUMBER_OF_CLUSTER; c++) clusters.Add(new Cluster(this));
            for (int p = 0; p < NUMBER_OF_POINTS; p++)
            {
                // User-defined data here
                points.Add(new Point(rand.Next(OFFSET, this.Width - OFFSET), rand.Next(OFFSET, this.Height - OFFSET)));
                parents.Add(empty);
            }

            // Form Events
            clock.Tick += Update;
            this.Paint += Render;
        }

        double DistanceOf(Point a, Point b)
        {
            return Math.Abs(Math.Pow((b.X - a.X), 2) + Math.Pow((b.Y - a.Y), 2));
        }

        void Update(object sender, EventArgs e)
        {
            for (int p = 0; p < points.Count; p++)
            {
                // Compare the distance of 'p' point to each centroid 'c'
                // The one with the nearest distance will be assign in the cluster
                Cluster selectedCluster = clusters[0];
                for (int c = 1; c < clusters.Count; c++)
                {
                    if (DistanceOf(points[p], clusters[c].centroid) < DistanceOf(points[p], selectedCluster.centroid))
                        selectedCluster = clusters[c];
                }
                Point movingPoint = points[p];
                Cluster clusterOfMovingPoint = parents[p];
                if (clusterOfMovingPoint != null) clusterOfMovingPoint.points.Remove(movingPoint);
                selectedCluster.points.Add(movingPoint);
                parents[p] = selectedCluster;
            }
            // Move centroids of each clusters in the centermost of their own domain
            for (int c = 0; c < clusters.Count; c++) clusters[c].CenterCentroid();
            this.Invalidate();
        }

        void Render(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int p = 0; p < points.Count; p++)
            {
                if (parents[p] == empty)
                {
                    g.DrawEllipse(new Pen(Color.Black, 4f), new Rectangle(points[p].X, points[p].Y, SIZE, SIZE));
                    g.DrawEllipse(new Pen(Color.White, 2f), new Rectangle(points[p].X, points[p].Y, SIZE, SIZE));
                    g.FillEllipse(Brushes.Gray, new Rectangle(points[p].X, points[p].Y, SIZE, SIZE));
                }
            }
            for (int c = 0; c < clusters.Count; c++)
            {
                for (int p = 0; p < clusters[c].points.Count; p++)
                {
                    g.DrawEllipse(new Pen(Color.Black, 4f), new Rectangle(clusters[c].points[p].X, clusters[c].points[p].Y, SIZE, SIZE));
                    g.DrawEllipse(new Pen(Color.White, 2f), new Rectangle(clusters[c].points[p].X, clusters[c].points[p].Y, SIZE, SIZE));
                    g.FillEllipse(clusters[c].color, new Rectangle(clusters[c].points[p].X, clusters[c].points[p].Y, SIZE, SIZE));
                }
                g.DrawRectangle(new Pen(Color.Black, 4f), new Rectangle(clusters[c].centroid.X, clusters[c].centroid.Y, SIZE, SIZE));
                g.DrawRectangle(new Pen(Color.White, 2f), new Rectangle(clusters[c].centroid.X, clusters[c].centroid.Y, SIZE, SIZE));
                g.FillRectangle(clusters[c].color, new Rectangle(clusters[c].centroid.X, clusters[c].centroid.Y, SIZE, SIZE));
            }
        }
    }
}
