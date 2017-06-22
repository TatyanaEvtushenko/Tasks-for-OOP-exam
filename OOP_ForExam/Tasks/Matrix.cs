using System;

namespace OOP_ForExam.Tasks
{
    //Разработать класс для представления матрицы произвольной размерности – Matrix.Класс:
    //- содержит элементы типа double, +
    //- принимает размеры матрицы на вход конструктора, +
    //- предоставляет типовые свойства(размеры матрицы, элемент на пересечении строки и столбца), +
    //- предоставляет типовые процедуры, применяемые при работе с матрицами(умножение, транспонирование), +
    //Продемонстрировать использование класса.

    class Matrix
    {
        private readonly double[,] _matrix;

        public int Height { get; }

        public int Width { get; }

        public Matrix(int n, int m)
        {
            n = n <= 0 ? 1 : n;
            m = m <= 0 ? 1 : m;
            _matrix = new double[n, m];
            Height = n;
            Width = m;
        }

        private bool IsCorrectIndex(int i, int j)
        {
            return i >= 0 && i < Height && j >= 0 && j < Width;
        }

        public double this[int i, int j]
        {
            get
            {
                if (IsCorrectIndex(i, j))
                {
                    return _matrix[i, j];
                }
                throw new ArgumentOutOfRangeException();
            }
            set
            {
                if (IsCorrectIndex(i, j))
                {
                    _matrix[i, j] = value;
                }
            }
        }

        public Matrix Multiply(Matrix otherMatrix)
        {
            if (Width != otherMatrix.Height) return null;
            var matrix = new Matrix(Height, otherMatrix.Width);
            double sum;
            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < otherMatrix.Width; j++)
                {
                    sum = 0;
                    for (ushort k = 0; k < Width; k++)
                    {
                        sum += _matrix[i, k] * otherMatrix[k, j];
                    }
                    matrix[i, j] = sum;
                }
            }
            return matrix;
        }

        public Matrix Transpose()
        {
            var matrix = new Matrix(Width, Height);
            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < Width; j++)
                {
                    matrix[j, i] = _matrix[i, j];
                }
            }
            return matrix;
        }

        public override string ToString()
        {
            var str = "";
            for (ushort i = 0; i < Height; i++)
            {
                for (ushort j = 0; j < Width; j++)
                {
                    str += $"{_matrix[i, j]} ";
                }
                str += "\n";
            }
            return str;
        }
    }
}
