using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;

namespace LabAB03
{
    class Program
    {
        public struct MyStruct
        {
            public int chislo;

            public MyStruct(int i)
            {
                chislo = i;
            }
            
           
            public override string ToString()
            {
                return chislo.ToString();
            }
        }

        public interface IStrategy<T> where T : new()
        {

            void DoAlgorithm(MyMass<T> data, bool flagOfSort);
        }


        public int GetInt(Size s)
        {
            return s.Width;
        }

        public class IntComparep : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                if (x > y)
                    return 1;
                else if (x < y)
                    return -1;
                else
                    return 0;
            }
        }


        

        public class BubleSort <T>: Program.IStrategy<T> where T:new()
        { 
            private IComparer<int> _comparer;
            private Func<T, int> _selector;
            public BubleSort(IComparer<int> comparer,Func<T,int> selector)
            {
                _comparer = comparer;
                _selector = selector;

            }

            public void DoAlgorithm(MyMass<T> data, bool flagOfSort) 
            {
                bool fl = flagOfSort;
                List<T> mass = data.Mass;
                for(int j = 0;j<mass.Count;j++)
                for (int i = 0; i < mass.Count-1; i++)
                {
                    if (fl)
                    {
                        if (_comparer.Compare( _selector(mass[i]), _selector(mass[i + 1])) == 1)
                        {
                            var temp = mass[i + 1];
                            mass[i + 1] = mass[i];
                            mass[i] = temp;
                        }
                    }
                    else
                    {
                        if (_comparer.Compare(_selector(mass[i]), _selector(mass[i + 1])) == -1)
                        {
                            var temp = mass[i + 1];
                            mass[i + 1] = mass[i];
                            mass[i] = temp;
                        }
                    }
                }

                data.Mass = mass;
            }
        }


        public class IsertionSort<T> : Program.IStrategy<T> where T : new()
        {
            private IComparer<int> _comparer;
            private Func<T, int> _selector;
            public IsertionSort(IComparer<int> comparer, Func<T, int> selector)
            {
                _comparer = comparer;
                _selector = selector;

            }
            public void DoAlgorithm(MyMass<T> data, bool flagOfSort) 
            {
                var array = data.Mass;
                for (int i = 1; i < array.Count; i++)
                {
                    int j;
                    var buf = array[i];
                    for (j = i - 1; j >= 0; j--)
                    {
                        if (flagOfSort)
                        {

                            if ((_comparer.Compare(_selector(array[j]), _selector(buf)) == -1))
                                break;

                            array[j + 1] = array[j];
                        }
                        else
                        {
                            if ((_comparer.Compare(_selector(array[j]), _selector(buf)) == 1))
                                break;

                            array[j + 1] = array[j];
                        }
                    }
                    array[j + 1] = buf;
                }
            }
        }


        public class SelectionSort<T> : Program.IStrategy<T> where T : new()
        {
            private IComparer<int> _comparer;
            private Func<T, int> _selector;
            public SelectionSort(IComparer<int> comparer, Func<T, int> selector)
            {
                _comparer = comparer;
                _selector = selector;

            }
            public void DoAlgorithm(MyMass<T> data, bool flagOfSort) 
            {
                int min;
                T temp;
                var arr = data.Mass;
                int length = arr.Count;

                for (int i = 0; i < length - 1; i++)
                {
                    min = i;

                    for (int j = i + 1; j < length; j++)
                    {
                        if (flagOfSort)
                        {
                            if (_comparer.Compare(_selector(arr[j]), _selector(arr[min])) == -1)
                            {
                                min = j;
                            }
                        }
                        else
                        {
                            if (_comparer.Compare(_selector(arr[j]), _selector(arr[min])) == 1)
                            {
                                min = j;
                            }
                        }

                    }

                    if (min != i)
                    {
                        temp = arr[i];
                        arr[i] = arr[min];
                        arr[min] = temp;
                    }
                }
            }
        }

        public class MergeSort<T> : Program.IStrategy<T> where T : new()
        {
            private IComparer<int> _comparer;
            private Func<T, int> _selector;
            private bool flag;
            public MergeSort(IComparer<int> comparer, Func<T, int> selector)
            {
                _comparer = comparer;
                _selector = selector;

            }
            T[] Merge_Sort(T[] massive)
            {
                if (massive.Length == 1)
                    return massive;
                Int32 mid_point = massive.Length / 2;

                return Merge(Merge_Sort(massive.Take(mid_point).ToArray()), Merge_Sort(massive.Skip(mid_point).ToArray()));
            }

            T[] Merge(T[] mass1, T[] mass2)
            {

                int a = 0, b = 0;
                T[] merged = new T[mass1.Length + mass2.Length];
                int i = 0, j = 0, k = 0;
                while (i < mass1.Length && j < mass2.Length)
                {
                    if (flag)
                    {
                        if (_comparer.Compare(_selector(mass1[i]), _selector(mass2[j])) == -1)
                        {
                            merged[k] = mass1[i];
                            i++;
                        }
                        else
                        {
                            merged[k] = mass2[j];
                            j++;
                        }

                        k++;
                    }
                    else
                    {
                        if (_comparer.Compare(_selector(mass1[i]), _selector(mass2[j])) != -1)
                        {
                            merged[k] = mass1[i];
                            i++;
                        }
                        else
                        {
                            merged[k] = mass2[j];
                            j++;
                        }

                        k++;
                    }
                }

                while (i < mass1.Length)
                {

                    merged[k] = mass1[i];
                    i++;
                    k++;

                }

                while (j < mass2.Length)
                {

                    merged[k] = mass2[j];
                    k++;
                    j++;

                }

                return merged;
            }
            public void DoAlgorithm(MyMass<T> data, bool flagOfSort) 
            {
                var mass = data.Mass;
                flag = flagOfSort;
                var newMass = Merge_Sort(mass.ToArray());
                mass = new List<T>(newMass);
                data.Mass = mass;
            }
        }


        public class HeapSort<T> : Program.IStrategy<T> where T : new()
        {
            private bool flag;
            private IComparer<int> _comparer;
            private Func<T, int> _selector;
            public HeapSort(IComparer<int> comparer, Func<T, int> selector)
            {
                _comparer = comparer;
                _selector = selector;

            }
            int add2pyramid(T[] arr, int i, int N)
            {
                int imax;
                T buf;
                if ((2 * i + 2) < N)
                {

                    if (_comparer.Compare(_selector(arr[2 * i + 1]), _selector(arr[2 * i + 2])) == -1) imax = 2 * i + 2;
                    else imax = 2 * i + 1;

                }
                else imax = 2 * i + 1;
                if (imax >= N) return i;

                if (_comparer.Compare(_selector(arr[i]), _selector(arr[imax])) == -1)
                {
                    buf = arr[i];
                    arr[i] = arr[imax];
                    arr[imax] = buf;
                    if (imax < N / 2) i = imax;
                }

                return i;
            }

            T[] Pyramid_Sort(T[] arr, int len)
            {
                //step 1: building the pyramid
                for (int i = len / 2 - 1; i >= 0; --i)
                {
                    long prev_i = i;
                    i = add2pyramid(arr, i, len);
                    if (prev_i != i) ++i;
                }

                //step 2: sorting
                T buf;
                for (Int32 k = len - 1; k > 0; --k)
                {

                    buf = arr[0];
                    arr[0] = arr[k];
                    arr[k] = buf;


                    Int32 i = 0, prev_i = -1;
                    while (i != prev_i)
                    {
                        prev_i = i;
                        i = add2pyramid(arr, i, k);
                    }
                }

                if (!flag)
                {
                    arr = arr.ToArray().Reverse().ToArray();
                }

                return arr;
            }
            public void DoAlgorithm(MyMass<T> data, bool flagOfSort) 
            {
                flag = flagOfSort;
                var arr = data.Mass.ToArray();
                arr = Pyramid_Sort(arr, arr.Length);
                data.Mass = new List<T>(arr);
            }
        }

        public class QuickSort<T> : Program.IStrategy<T> where T : new()
        {
            private IComparer<int> _comparer;
            private Func<T, int> _selector;
            public QuickSort(IComparer<int> comparer, Func<T, int> selector)
            {
                _comparer = comparer;
                _selector = selector;

            }

            int partition(List<T> array, int start, int end, bool flag = true)
            {
                int marker = start;
                for (int i = start; i <= end; i++)
                {
                    if (flag)
                    {
                        if (_comparer.Compare(_selector(array[i]), _selector(array[end])) != 1)
                        {
                            var temp = array[marker]; // swap
                            array[marker] = array[i];
                            array[i] = temp;
                            marker += 1;
                        }
                    }
                    else
                    {
                        if (_comparer.Compare(_selector(array[i]), _selector(array[end])) != -1)
                        {
                            var temp = array[marker]; // swap
                            array[marker] = array[i];
                            array[i] = temp;
                            marker += 1;
                        }
                    }
                }
                return marker - 1;
            }

            void quicksort(List<T> array, int start, int end, bool flagOfsort)
            {
                if (start >= end)
                {
                    return;
                }
                int pivot = partition(array, start, end, flagOfsort);
                quicksort(array, start, pivot - 1, flagOfsort);
                quicksort(array, pivot + 1, end, flagOfsort);
            }


            public void DoAlgorithm(MyMass<T> data, bool flagOfSort) 
            {


                var arr = data.Mass;
                int start = 0;
                int end = arr.Count - 1;
                quicksort(arr, start, end, flagOfSort);

            }
        }

        


        public class MyMass<T> : ICloneable where T : new()
        {
            private List<T> _mass;
            private bool _flagOfSort;

            public MyMass(List<T> mass, bool flag =true)
            {
                _mass = new List<T>(mass);
                _flagOfSort = flag;
            }

            public bool FlagOfSort
            {
                get { return _flagOfSort; }
                set { _flagOfSort = value; }
            }
            public object Clone()
            {
                List<T> mass = new List<T>();
                foreach (var element in _mass)
                {
                    mass.Add(new T());
                    mass[mass.Count - 1] = element;
                }

                var res = new MyMass<T>(mass);
                return res;
            }

            public List<T> Mass
            {
                set { _mass = value;}
                get { return _mass; }
            }

            public override string ToString()
            {
                string res = string.Empty;
                foreach (var el in _mass)
                {
                    res += el.ToString()+" ";
                }

                return res;
            }
        }


        public static int GetInt(MyStruct str)
        {
            return str.chislo;
        }
        static void Main(string[] args)
        {
            ShowInfo();
            MyStruct[] arr = new[]
            {
                new MyStruct(4), new MyStruct(40), new MyStruct(87), new MyStruct(8), new MyStruct(4), new MyStruct(2),
                new MyStruct(78), new MyStruct(18), new MyStruct(5), new MyStruct(13), new MyStruct(6), new MyStruct(1)
            };
            List<MyStruct> mas = new List<MyStruct>(arr);
            MyMass<MyStruct> m = new MyMass<MyStruct>(mas);
           
           
            
            var mc = m.Clone();
            //var strat = new Context();


            Console.WriteLine("Не отсортированный массив " + m.ToString());

            //strat.SetStrategy(new BubleSort());
            //strat.DoSomeLogic(m, new IntComparep());
            var strat = new BubleSort<MyStruct>(new IntComparep(), GetInt);
            strat.DoAlgorithm(m, true);
            
            Console.WriteLine("Buble sort " + m.ToString());
           
            strat.DoAlgorithm(m, false);
            Console.WriteLine("Reverse buble sort " + m.ToString());

            
            var strat1 = new MergeSort<MyStruct>(new IntComparep(), GetInt);
            strat1.DoAlgorithm(m, true);

            Console.WriteLine("Merge sort " + m.ToString());

            strat1.DoAlgorithm(m, false);
            Console.WriteLine("Reverse merge sort " + m.ToString());


            var strat2 = new QuickSort<MyStruct>(new IntComparep(), GetInt);
            strat2.DoAlgorithm(m, true);

            Console.WriteLine("Quick sort " + m.ToString());

            strat2.DoAlgorithm(m, false);
            Console.WriteLine("Quick buble sort " + m.ToString());


            var strat3 = new HeapSort<MyStruct>(new IntComparep(), GetInt);
            strat3.DoAlgorithm(m, true);

            Console.WriteLine("heap sort " + m.ToString());

            strat3.DoAlgorithm(m, false);
            Console.WriteLine("Reverse heap sort " + m.ToString());


            



            var strat5 = new SelectionSort<MyStruct>(new IntComparep(), GetInt);
            strat5.DoAlgorithm(m, true);

            Console.WriteLine("Selection sort " + m.ToString());

            strat5.DoAlgorithm(m, false);
            Console.WriteLine("Reverse selection sort " + m.ToString());
            var strat4 = new IsertionSort<MyStruct>(new IntComparep(), GetInt);

            strat4.DoAlgorithm(m, true);

            Console.WriteLine("Insertion sort " + m.ToString());

            strat4.DoAlgorithm(m, false);
            Console.WriteLine("Reverse insertion sort " + m.ToString());




            //strat.SetStrategy(new SelectionSort());
            //strat.DoSomeLogic(m, new IntComparep());
            //Console.WriteLine("Selection sort " + m.ToString());
            //strat.DoSomeLogic(m, new IntComparep(), false);
            //Console.WriteLine("Reverse Selection sort " + m.ToString());

            //strat.SetStrategy(new IsertionSort());
            //strat.DoSomeLogic(m, new IntComparep());
            //Console.WriteLine("Insertion sort " + m.ToString());
            //strat.DoSomeLogic(m, new IntComparep(), false);
            //Console.WriteLine("Reverse insertion sort " + m.ToString());

            //strat.SetStrategy(new QuickSort());
            //strat.DoSomeLogic(m, new IntComparep());
            //Console.WriteLine("Quick sort " + m.ToString());
            //strat.DoSomeLogic(m, new IntComparep(), false);
            //Console.WriteLine("Reverse quick sort " + m.ToString());

            //strat.SetStrategy(new MergeSort());
            //strat.DoSomeLogic(m, new IntComparep());
            //Console.WriteLine("Merge sort " + m.ToString());
            //strat.DoSomeLogic(m, new IntComparep(), false);
            //Console.WriteLine("Reverse merge sort " + m.ToString());

            //strat.SetStrategy(new HeapSort());
            //strat.DoSomeLogic(m, new IntComparep());
            //Console.WriteLine("Heap sort " + m.ToString());
            //strat.DoSomeLogic(m, new IntComparep(), false);
            //Console.WriteLine("Reverse heap sort " + m.ToString());
        }

        static public string ShowInfo()
        {
            return (string.Format(
                @"Карпов Андрей Группа 8-Т3О-402Б-16 Задание 3 - Реализовать приложение для сортировки массивов данных. Алгоритмы
сортировки (bubbleSort, insertionSort, selectionSort, mergeSort, heapSort,
quickSort) должны быть представлены в виде классов, реализующих
паттерн “стратегия”. Данные в массиве могут быть любого типа. В рамках
процесса можно выполнить несколько различных сортировок массива
(объект, хранящий массив, должен реализовывать паттерн “прототип”).
Сортировать данные можно по возрастанию/убыванию. В качестве
параметров для создания объектов “стратегий” передаются селектор
ключа (поле или набор полей, по которому (-ым) производится сортировка)
и правило нахождения отношения порядка между ключами (делегат или
объект, реализующий интерфейс IComparer)."));
        }
    }

}

