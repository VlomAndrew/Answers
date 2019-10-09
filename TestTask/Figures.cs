using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTask
{
    abstract public class Figures
    {
        protected List<double> edge;
        protected int edgeCount;
        public abstract double Area();
    }


    public class Triangle: Figures
    {
        public Triangle(double A, double B, double C)
        {
            edgeCount = 3;
            if (!(A + B > C && B + C > A && A + C > B))
            {
                throw new ArgumentException(string.Format("невозможно построить такой треугольник со сторонами {0},{1},{2}",A,B,C));
            }
            edge = new List<double>(){A,B,C};
        }

        public bool IsRectangle()
        {
            var maxEdge = edge.Max();
            var index = edge.IndexOf(maxEdge);
            var katets = new List<double>(edge);
            katets.Remove(maxEdge);

            return Math.Pow(katets[0], 2) * Math.Pow(katets[1], 2) == Math.Pow(maxEdge, 2);


        }
        public override double Area()
        {
            var p = (edge[0] + edge[1] + edge[2]) / 2;
            var area = Math.Sqrt(p * (p - edge[0]) * (p - edge[1]) * (p - edge[2]));
            return area;
        }
    }

    public class Rectangle : Figures
    {
        public Rectangle(double A, double B, double C, double D)
        {
            if (A!=C && B!=D)
                throw new ArgumentException(string.Format("невозможно построить квадрас с такими сторонами {0},{1},{2},{3}",A,B,C,D));
            edgeCount = 4;
            edge = new List<double>(){A,B,C,D};
        }

        public override double Area()
        {
            return edge[0] * edge[1];
        }
    }

    public class Circle : Figures
    {
        public Circle(double radius)
        {
            edgeCount = 1;
            edge = new List<double>{radius};
        }

        public override double Area()
        {
            return Math.PI * Math.Pow(edge[0], 2);
        }
    }
}
