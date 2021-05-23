using System;
using lab1_SLAU_MethodGauss;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Linq;

namespace UnitTestGauss
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestDeterminate()
        {
            double expertedValueDeterminate = 25;
            string path = @"D:\Dlya kursovoj\lab1_SLAU_MethodGauss\a1.dat";
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
    }
}
