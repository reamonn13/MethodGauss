using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace lab1_SLAU_MethodGauss
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKey key = Console.ReadKey().Key;
            do{
                
                string path = @"D:\Study\3 course\MathCuntor\lab1_SLAU_MethodGauss\a.dat";
                string[] readfile = File.ReadAllLines(path);  //чтение матрицы А из файла
                string[] str = null;
                int N = Convert.ToInt32(readfile[0]);
                double[,] A = new double[N, N];
                double[,] E = new double[N, N];
                double[] B = new double[N];
                double[] X = new double[N];
                double[] R = new double[N];
                int num_var;
                for (int i = 1; i < readfile.Count(); i++)
                {
                    str = readfile[i].Split(' ');
                    for (int j = 0; j < readfile.Count() - 1; j++)
                    {
                        A[i - 1, j] = Convert.ToDouble(str[j]);  //запись матрицы А в двумерный массив
                    }
                }

                path = @"D:\Study\3 course\MathCuntor\lab1_SLAU_MethodGauss\b4.dat";
                readfile = File.ReadAllLines(path);  //чтение матрицы B из файла
                str = readfile[0].Split(' ');
                for (int i = 0; i < N; i++)
                {
                    B[i] = Convert.ToDouble(str[i]);  //запись матрицы B в одномерный массив
                }

                for (int i = 0; i < N; i++)  //заполнение единичной матрицы
                {
                    for (int j = 0; j < N; j++)
                    {
                        if (i == j)
                            E[i, j] = 1;

                        else
                            E[i, j] = 0;
                    }
                }

                Console.WriteLine("Система линейных алгебраических уравнений: \n");
                PrintSystem(N, A, B);  //вывод СЛАУ на консоль
                do{
                    Console.Write("\nВыберите процедуру решения: \nВведите \"1\" для решения системы методом Гаусса с выбором главного элемента;\nВведите \"2\" для решения системы методом Гаусса без процедуры выбора главного элемента: ");
                    num_var = int.Parse(Console.ReadLine());
                    if (num_var == 1 || num_var == 2)
                    {
                        if (num_var == 1)
                        {
                            WithMainElement(N, A, B, X, R, E);
                        }
                        else if (num_var == 2)
                        {
                            WithoutMainElement(N, A, B, X, R, E);
                        }
                    }

                    else
                    {
                        Console.WriteLine("Введено некорректное значение, повторите ввод.");
                    }
                    Console.WriteLine("===========================================================================================================\n\n\n");
                } while (num_var != 1 && num_var != 2);


            } while (key != ConsoleKey.Escape);
            return;
        }

        static void PrintSystem(int N, double[,] A, double[] B)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (A[i, j] < 0)
                    {
                        Console.Write(" " + A[i, j] + "*x" + (j + 1));
                    }

                    else if (A[i, j] > 0)
                    {
                        Console.Write(" +" + A[i, j] + "*x" + (j + 1));
                    }

                    else
                    {
                        Console.Write("");
                    }
                }
                Console.Write(" = " + B[i] + ";");
                Console.WriteLine();
            }
        }

        static void PrintMatrix(int N, double[,] E)
        {
            for (int i = 0; i < N; i++)
            {
                Console.Write("(");
                for (int j = 0; j < N; j++)
                {
                    Console.Write(E[i, j] + "     ");
                }
                Console.Write(")\n");
            }
            Console.WriteLine();
        }

        static void WithMainElement(int N, double[,] A, double[] B, double[] X, double[] R, double [,] E)
        {
            double det;
            det = MakeTriangleWithME(N, A, B, X, R, E);
            if (det == 0)
            {
                Console.WriteLine("Матрица A вырождена, т. к. в нижнетреугольной матрице в главной диагонали встретился ноль, либо столбец матрицы нулевой.\n|A| = " + det);
                return;
            }
            else
            {
                Console.WriteLine("\nВеличина определителя матрицы А:   " + det);
                Console.WriteLine("\nРешение СЛАУ: ");
                for (int i = 0; i < N; i++)
                {
                    Console.Write("x" + (i + 1) + " = " + X[i] + ";\n");
                }
                Console.WriteLine("");
                Nevyazka(N, R, A, B, X);
                Console.WriteLine("Невязка: ");
                for (int i = 0; i < N; i++)
                {
                    Console.Write("r" + (i + 1) + " = " + R[i] + ";\n");
                }
                Console.WriteLine("");
                Inverse(N, A, X, E);
                Console.WriteLine("Обратная матрица матрицы А: ");
                PrintMatrix(N, E);
                //Console.WriteLine("\n\n");
                //PrintMatrix(N, A);
            }
        }

        static void WithoutMainElement(int N, double[,] A, double[] B, double[] X, double[] R, double[,] E)
        {
            double det;
            det = MakeTriangle(N, A, B, X, E);
            if (det == 0)
            {
                Console.WriteLine("Матрица A вырождена, т. к. в нижнетреугольной матрице в главной диагонали встретился ноль, либо столбец матрицы нулевой.\n|A| = " + det);
                return;
            }
            else
            {
                Console.WriteLine("\nВеличина определителя матрицы А:   " + det);
                Console.WriteLine("\nРешение СЛАУ: ");
                for (int i = 0; i < N; i++)
                {
                    Console.Write("x" + (i + 1) + " = " + X[i] + ";\n");
                }
                Console.WriteLine("");
                Nevyazka(N, R, A, B, X);
                Console.WriteLine("Невязка: ");
                for (int i = 0; i < N; i++)
                {
                    Console.Write("r" + (i + 1) + " = " + R[i] + ";\n");
                }
                Console.WriteLine("");
                Inverse(N, A, X, E);
                Console.WriteLine("Обратная матрица матрицы А: ");
                PrintMatrix(N, E);
            }
        }

        /*В методе вычисляется определитель;
         * приводится к треугольному виду расширенная матрица (А | В) - прямой ход
         * и матрица (А | Е) - для вычисления обратной матрицы;
         * также ведётся обратный ход;
         * вычисляется решение системы*/
        static double MakeTriangle(int N, double[,] A, double[] B, double[] X, double[,] E)  //
        {
            double res, factor, s = 0, det = 1;
            int current_str = 0, sign_det = 1;
            bool not_null = false;
            for (int j = 0; j < N - 1; j++)
            {
                for (int i = j; i < N - 1; i++)
                {
                    if (i == j)
                    {
                        if (A[i, j] == 0)  //если элемент на главной диагонали равен нулю
                        {
                            for (int k = i + 1; k < N; k++)  //проверяем элементы, стоящие ниже этого элемента
                            {
                                if (A[k, j] != 0)  //если нашёлся ненулевой
                                {
                                    for (int m = 0; m < N; m++)  //меняем строки местами
                                    {
                                        res = A[i, m];
                                        A[i, m] = A[k, m];
                                        A[k, m] = res;

                                        res = E[i, m];
                                        E[i, m] = E[k, m];
                                        E[k, m] = res;
                                    }

                                    res = B[i];
                                    B[i] = B[k];
                                    B[k] = res;

                                    sign_det *= -1;  //поменяли строки местами, не забыли поменять знак определителя
                                    not_null = true;  //флаг ненулевого элемента
                                    break;  //нашли ненулевой, выходим из цикла
                                }
                            }

                            if (not_null == false)  //если не нашли ненулевой элемент в столбце, то определитель равен нулю, а значит матрица вырождена
                            {
                                return 0;
                            }
                        }
                    }
                    factor = A[i + 1, j] / A[current_str, j];  //находим масштабирующий множитель
                    for (int c = j; c < N; c++)
                    {
                        A[i + 1, c] = A[i + 1, c] - A[current_str, c] * factor;  //вычитаем из (i + 1) уравнения текущее, умноженное на масштабирующий множитель
                        E[i + 1, c] = E[i + 1, c] - E[current_str, c] * factor;  //вычитаем из (i + 1) уравнения текущее, умноженное на масштабирующий множитель
                    }
                    B[i + 1] = B[i + 1] - B[current_str] * factor;
                }
                current_str++;  //счётчик текущего уравнения
            }

            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    if (i == j)
                        det *= A[i, j];  //считаем определитель, перемножив в нём элементы на главной диагонали
            det *= sign_det;  //домножаем определитель на знак, если строки в нём менялись местами - нашли значение определителя
            X[N - 1] = B[N - 1] / A[N - 1, N - 1];  //обратный ход метода Гаусса
            for (int i = N - 2, j; i >= 0; i--)
            {
                s = 0;
                for (j = i + 1; j < N; j++)
                {
                    s = s + A[i, j] * X[j];
                }
                X[i] = (B[i] - s) / A[i, i];
            }
            return det;       
        }

        static void Inverse(int N, double[,] A, double[] X, double[,] E)
        {
            double factor;
            int current_str = N - 1;
            for (int j = N - 1; j > 0; j--)  //получаем нули выше главной диагонали
            {
                for (int i = j; i > 0; i--)
                {
                    factor = A[i - 1, j] / A[current_str, j];  //находим масштабирующий множитель
                    for (int c = N - 1; c >= 0; c--)
                    {
                        A[i - 1, c] = A[i - 1, c] - A[current_str, c] * factor;  //вычитаем из (i - 1) уравнения текущее, умноженное на масштабирующий множитель
                        E[i - 1, c] = E[i - 1, c] - E[current_str, c] * factor;  //вычитаем из (i - 1) уравнения текущее, умноженное на масштабирующий множитель
                    }
                }
                current_str--;  //счётчик текущего уравнения
            }

            for (int i = 0; i < N; i++)  //получаем единицы на главной диагонали
            {
                for (int j = 0; j < N; j++)
                {
                    if (i == j)
                    {
                        for (int k = 0; k < N; k++)
                        {
                            E[i, k] = E[i, k] / A[i, j];
                        }

                        A[i, j] = A[i, j] / A[i, j];
                    }
                }
            }
        }

        static void Nevyazka(int N, double[] R, double[,] A, double[] B, double[] X)
        {
            double eps = 0.000001;
            for (int i = 0; i < N; i++)
            {
		        R[i] = B[i];
		        if (Math.Abs(R[i]) < eps)
                    R[i] = 0;
		        for (int j = 0; j < N; j++)
                {
			        R[i] -= A[i, j] * X[j];
			        if (Math.Abs(R[i]) < eps)
                        R[i] = 0;
                }
            }
        }

        static double MakeTriangleWithME(int N, double[,] A, double[] B, double[] X, double[] R, double[,] E)
        {
            int i, j, current_str = 0, sign_det = 1, sign;
            double factor, det = 1, s = 0;
            for (j = 0; j < N - 1; j++)
            {
                for (i = j; i < N - 1; i++)
                {
                    if (i == j)
                    {
                        sign = MaxOfColumn(N, A, B, E, i, j);
                        if (sign == 0)
                            return 0;
                        else
                            sign_det *= sign;
                    }

                    factor = A[i + 1, j] / A[current_str, j];  //находим масштабирующий множитель
                    for (int c = j; c < N; c++)
                    {
                        A[i + 1, c] = A[i + 1, c] - A[current_str, c] * factor;  //вычитаем из (i + 1) уравнения текущее, умноженное на масштабирующий множитель
                        E[i + 1, c] = E[i + 1, c] - E[current_str, c] * factor;  //вычитаем из (i + 1) уравнения текущее, умноженное на масштабирующий множитель
                    }
                    B[i + 1] = B[i + 1] - B[current_str] * factor;
                }
                current_str++;  //счётчик текущего уравнения
            }

            for (i = 0; i < N; i++)
                for (j = 0; j < N; j++)
                    if (i == j)
                        det *= A[i, j];  //считаем определитель, перемножив в нём элементы на главной диагонали
            det *= sign_det;  //домножаем определитель на знак, если строки в нём менялись местами - нашли значение определителя
            X[N - 1] = B[N - 1] / A[N - 1, N - 1];  //обратный ход метода Гаусса
            for (i = N - 2; i >= 0; i--)
            {
                s = 0;
                for (j = i + 1; j < N; j++)
                {
                    s = s + A[i, j] * X[j];
                }
                X[i] = (B[i] - s) / A[i, i];
            }
            return det;
        }

        static int MaxOfColumn(int N, double[,] A, double[] B, double[,] E, int istr, int jcol)
        {
            double max, res;
            int current_column = jcol, index = istr;
            bool checkswap = false;
            max = Math.Abs(A[current_column, current_column]);
            for (int j = current_column; j < N - 1; j++)
            {
                for (int i = j; i < N; i++)
                {
                    if (Math.Abs(A[i, j]) > max)
                    {
                        max = Math.Abs(A[i, j]);
                        index = i;
                        checkswap = true;
                    }
                }

                if (max == 0)
                {
                    return 0;
                }

                if (checkswap == true)
                {
                    for (int k = 0; k < N; k++)
                    {
                        res = A[current_column, k];
                        A[current_column, k] = A[index, k];
                        A[index, k] = res;

                        res = E[current_column, k];
                        E[current_column, k] = E[index, k];
                        E[index, k] = res;
                    }

                    res = B[index];
                    B[index] = B[j];
                    B[j] = res;

                    return -1;
                }

                break;
            }
            
            return 1;
        }
    }
}
