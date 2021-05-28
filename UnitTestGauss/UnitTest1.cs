// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using lab1_SLAU_MethodGauss;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace UnitTestGauss
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestDeterminate() //кормим методу матрицу А, у которого определитель заведомо равен 0
        {
            double expertedValueDeterminate = 0;  //ожидаемый результат теста
            string path = @"D:\Dlya kursovoj\lab1_SLAU_MethodGauss\a2.dat";
            string[] readfile = File.ReadAllLines(path);  //чтение матрицы А из файла
            string[] str = null;
            int N = Convert.ToInt32(readfile[0]);
            double[,] A = new double[N, N];
            double[,] E = new double[N, N];
            double[] B = new double[N];
            double[] X = new double[N];
            double[] R = new double[N];
            for (int i = 1; i < readfile.Count(); i++)
            {
                str = readfile[i].Split(' ');
                for (int j = 0; j < readfile.Count() - 1; j++)
                {
                    A[i - 1, j] = Convert.ToDouble(str[j]);  //запись матрицы А в двумерный массив
                }
            }

            path = @"D:\Dlya kursovoj\lab1_SLAU_MethodGauss\b1.dat";
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
            double valueDeterminate = Program.MakeTriangleWithME(N, A, B, X, R, E);
            Assert.AreEqual(expertedValueDeterminate, valueDeterminate);
        }

        [TestMethod]
        public void TestNullElementOnMainDiagonal()
        {
            //создал массив с нулевым элементом на диагонали
            double[,] A = new double[3, 3] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } };
            double[,] E = new double[3, 3] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };
            double[] B = new double[3] { 1, 2, 3 };
            double[] X = new double[3] { 0, 0, 0 };
            int N = 3;
            double expertedNullDeterminate = 0, realDeterminate;
            //результат должен получиться 0
            realDeterminate = Program.MakeTriangle(N, A, B, X, E); 
            Assert.AreEqual(expertedNullDeterminate, realDeterminate);
        }

        [TestMethod]
        public void TestSolutionOfInverseMatrix()
        {
            string path = @"D:\Dlya kursovoj\lab1_SLAU_MethodGauss\a4.dat";
            string[] readfile = File.ReadAllLines(path);  //чтение матрицы А из файла
            string[] str = null;
            int N = Convert.ToInt32(readfile[0]);
            double[,] A = new double[N, N];
            double[,] E = new double[N, N];
            double[] B = new double[N];
            double[] X = new double[N];
            double[] R = new double[N];
            for (int i = 1; i < readfile.Count(); i++)
            {
                str = readfile[i].Split(' ');
                for (int j = 0; j < readfile.Count() - 1; j++)
                {
                    A[i - 1, j] = Convert.ToDouble(str[j]);  //запись матрицы А в двумерный массив
                }
            }

            path = @"D:\Dlya kursovoj\lab1_SLAU_MethodGauss\b.dat";
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
            /* после выполнения метода в матрицу Е будет записана обратная матрица 
             заранее посчитал результат на бумаге и через онлайн калькулятор, буду сравнивать список 
             значений полученных со списком значений ожидаемых 
             Обратная матрица ожидаемая: 
             { { -2, -2, 1 }, { 1.3333333, -0.3333333, 2 }, { 2, 0.3333333, -0.3333333 } }*/
            Program.WithoutMainElement(N, A, B, X, R, E);
            double[,] expertedMassiv = new double[3, 3] { { 1, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } };  //буду сравнивать с этим массивом
            Assert.AreEqual(E, expertedMassiv);
        }

        /* Метод переставляет строки с наибольшим элементов на главной диагонали наверх
         * В данном примере на главной диагонали расположены эл-ты 0, 4, 8, третья строка должна 
         * встать на место первой */
        [TestMethod]
        public void TestMaxOfColumn()
        {
            double[,] A = new double[3, 3] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } };
            double[,] E = new double[3, 3] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };
            double[] B = new double[3] { 1, 2, 3 };
            double[] X = new double[3] { 0, 0, 0 };
            int N = 3;
            int expertedOfMaxColumn = 6;
            Program.MaxOfColumn(N, A, B, E, 0, 0);
            Assert.AreEqual(expertedOfMaxColumn, A[0, 0]);
        }
    }
}
