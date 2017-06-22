using System;
using System.Globalization;

namespace OOP_ForExam.Tasks
{
    //Разработать шаблон для представления матрицы произвольной размерности – Matrix<T>.Класс:
    //- содержит элементы типа T, +
    //- принимает размеры матрицы на вход конструктора,  +
    //- предоставляет типовые свойства(размеры матрицы, элемент на пересечении строки и столбца), +
    //- предоставляет типовые процедуры, применяемые при работе с матрицами(умножение, транспонирование), +
    //Продемонстрировать использование класса.

    interface IMatrixItem<T>
    {
        T MultyplyWith(T item);

        T AddWith(T item);
    }

    class DecimalMatrixItem : IMatrixItem<DecimalMatrixItem>
    {
        public decimal Data { get; set; }

        public decimal GetDefault => 0;

        public DecimalMatrixItem()
        {
            Data = GetDefault;
        }

        public DecimalMatrixItem(decimal data)
        {
            Data = data;
        }

        public DecimalMatrixItem MultyplyWith(DecimalMatrixItem item)
        {
            return new DecimalMatrixItem(Data * item.Data);
        }

        public DecimalMatrixItem AddWith(DecimalMatrixItem item)
        {
            return new DecimalMatrixItem(Data + item.Data);
        }

        public override string ToString()
        {
            return Data.ToString(CultureInfo.InvariantCulture);
        }
    }

    class Matrix<T>
        where T: IMatrixItem<T>, new ()
    {
        private readonly T[,] _matrix;

        public int Height { get; }

        public int Width { get; }

        public Matrix(int n, int m)
        {
            n = n <= 0 ? 1 : n;
            m = m <= 0 ? 1 : m;
            _matrix = new T[n, m];
            Height = n;
            Width = m;
        }

        private bool IsCorrectIndex(int i, int j)
        {
            return i >= 0 && i < Height && j >= 0 && j < Width;
        }

        public T this[int i, int j]
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

        public Matrix<T> Multiply(Matrix<T> otherMatrix)
        {
            if (Width != otherMatrix.Height) return null;
            var matrix = new Matrix<T>(Height, otherMatrix.Width);
            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < otherMatrix.Width; j++)
                {
                    var sum = new T();
                    for (var k = 0; k < Width; k++)
                    {
                        var mul = _matrix[i, k].MultyplyWith(otherMatrix[k, j]);
                        sum = sum.AddWith(mul);
                    }
                    matrix[i, j] = sum;
                }
            }
            return matrix;
        }

        public Matrix<T> Transpose()
        {
            var matrix = new Matrix<T>(Width, Height);
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
