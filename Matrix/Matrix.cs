using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;

namespace Matrix {
    class Matrix {
        private double[,] matrix;
        Matrix() {
            matrix = new double[3, 3];
            int k = 0;
            for (int i = 0; i < 3; ++i)
                for (int j = 0; j < 3; ++j) {
                    matrix[i, j] = k;
                    ++k;
                }
        }

        Matrix(int x, int y) {
            matrix = new double[y, x];
        }

        Matrix(double[,] m) {
            matrix = m;
        }

        public void ToFile(string path) {
            using (StreamWriter file = new StreamWriter(path)) {
                for (int i = 0; i < matrix.GetLength(0); ++i) {
                    for (int j = 0; j < matrix.GetLength(0); ++j) {
                        file.Write($"{ matrix[i, j]}\t");
                    }
                }
            }
        }

        public void FromFile(string path) {
            using (StreamReader file = new StreamReader(path)) {

                for (int i = 0; i < matrix.GetLength(0); ++i) {
                    for (int j = 0; j < matrix.GetLength(1); ++j) {
                        matrix[i, j] = file.Read();
                    }
                }
            }
        }

        public void ToJSon(string path) {
            using (StreamWriter file = System.IO.File.CreateText(path)) {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, matrix);
            }
        }

        public void FromJson(string path) {
            var x = System.IO.File.ReadAllText(path);
            matrix = JsonConvert.DeserializeObject<double[,]>(x);
        }

        public static double[,] operator +(Matrix m1, double[,] m) {
            for (int i = 0; i < m.GetLength(0); ++i) {
                for (int j = 0; j < m.GetLength(1); ++j) {
                    m1.matrix[i,j] += m[i,j];
                }
            }
            return m1.matrix;
        }

        public static double[,] operator *(Matrix m1, double[,] m) {
            double[,] m2 = new double[m.GetLength(0), m.GetLength(1)];
            for (int i = 0; i < m1.matrix.GetLength(1); i++) {
                for (int j = 0; j < m.GetLength(0); j++) {
                    m2[i,j] = 0;
                    for (int k = 0; k < m1.matrix.GetLength(1); k++) {
                        m2[i,j] += m1.matrix[i,k] * m[k,j];
                    }
                }
            }
            return m1.matrix;
        }

        public static double[,] operator *(Matrix m1, double d) {
            for(int i = 0; i < m1.matrix.GetLength(0); ++i) {
                for (int j = 0; j < m1.matrix.GetLength(1); ++j) {
                    m1.matrix[i, j] *= d;
                }
            }
            return m1.matrix;
        }

        public static bool operator==(Matrix m1, double[,] m2) {
            if (m1.matrix.GetLength(0) != m2.GetLength(0) && m1.matrix.GetLength(1) != m2.GetLength(1))
                return false;
            for (int i = 0; i < m1.matrix.GetLength(0); ++i) {
                for (int j = 0; j < m1.matrix.GetLength(1); ++j) {
                    if (m1.matrix[i, j] != m2[i, j])
                        return false;
                }
            }
            return true;
        }

        public static bool operator !=(Matrix m1, double[,] m2) {
            if (m1.matrix.GetLength(0) != m2.GetLength(0) && m1.matrix.GetLength(1) != m2.GetLength(1))
                return true;
            for (int i = 0; i < m1.matrix.GetLength(0); ++i) {
                for (int j = 0; j < m1.matrix.GetLength(1); ++j) {
                    if (m1.matrix[i, j] != m2[i, j])
                        return true;
                }
            }
            return false;
        }
    }
}