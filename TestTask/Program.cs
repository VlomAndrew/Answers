using System;

namespace TestTask
{

    sealed class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            try
            {
                Triangle tr1 = new Triangle(10.0, 5.0, 2.0);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
            Triangle tr2 = new Triangle(11, 10, 12);

            Rectangle sq1 = new Rectangle(10, 10, 10, 10), sq2 = new Rectangle(10, 25, 10, 25);

            Console.WriteLine(
                " Треугольник 2 прямоугольный - {0},Площадь второго треугольник = {1}",
                tr2.IsRectangle(),  tr2.Area());
            try
            {
                Rectangle rec1 = new Rectangle(12,13,14,15);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }

            Circle cr1 = new Circle(10);
            Console.WriteLine("Площадь круга {0}",cr1.Area());
        }
    }
}
