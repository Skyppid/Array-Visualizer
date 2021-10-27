using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using LinqLib.Array;
using LinqLib.Sequence;
using Microsoft.FSharp.Math;
using SharpDX;

namespace CsTester
{
    public partial class Form1 : Form
    {
        #region Fields

        private int[,] _intArray2D;
        private Array _systemArray;

        private static int[,] _intArray2Ds;
        private static Array _systemArrayS;

        #endregion

        #region Constructors and Destructors

        public Form1()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void buttonBreak_Click(object sender, EventArgs e)
        {
            _intArray2Ds = Enumerator.Generate(10, 5, 20).Shuffle().ToArray(4, 5);
            _systemArrayS = Enumerator.Generate(1, 3.27, 40).Shuffle().ToArray(8, 5);

            _intArray2D = Enumerator.Generate(8, 4, 32).Shuffle().ToArray(4, 8);
            _systemArray = Enumerator.Generate(1, 1.7, 24).Shuffle().ToArray(3, 8);

            Debugger.Break();

            _intArray2Ds = null;
            _systemArrayS = null;

            _intArray2D = null;
            _systemArray = null;
        }

        private void buttonSystemArray_Click(object sender, EventArgs e)
        {
            Array arr1 = Enumerator.Generate<long>(1, 1, 360).Shuffle().ToArray(3, 3, 4, 5);
            Array arr2 = Enumerator.Generate(15, (x) => new TestType() {A = x, B = x - 5}).Shuffle().ToArray(5, 3);
            Array arrJ1 = GetJaggedArray2();
            Array arrJ2 = GetJaggedArray3();

            Debugger.Break();
        }

        private void buttonStandard_Click(object sender, EventArgs e)
        {
            long[,,,] arr1 = Enumerator.Generate<long>(1, 1, 360).Shuffle().ToArray(3, 3, 4, 5);
            TestType[,] arr2 = Enumerator.Generate(15, (x) => new TestType() {A = x, B = x - 5}).Shuffle()
                .ToArray(5, 3);
            int[][] arrJ1 = GetJaggedArray2();
            int[][][] arrJ2 = GetJaggedArray3();

            Debugger.Break();
        }

        private void buttonChart_Click(object sender, EventArgs e)
        {
            const double rad = 57.2957795;
            double[] d1 = new double[145];
            double[,] d2 = new double[2, 145];

            for (int i = 0; i < 145; i++)
            {
                d1[i] = Math.Sin(i * 5 / rad);
                d2[0, i] = d1[i];
                d2[1, i] = d1[i] > 0 ? i * .02 : -i * .02;
            }
            Debugger.Break();
        }

        private void buttonSharpDX_Click(object sender, EventArgs e)
        {
            Array arr1 = Enumerator.Generate<long>(1, 1, 360).Shuffle().ToArray(3, 3, 4, 5);
            Array arr2 = Enumerator.Generate(15, (x) => new TestType() {A = x, B = x - 5}).Shuffle().ToArray(5, 3);
            Array arrJ1 = GetJaggedArray2();
            Array arrJ2 = GetJaggedArray3();

            var v1 = new Vector2(1, 2);
            var v2 = new Vector3(1, 2, 3);
            var v3 = new Vector4(1, 2, 3, 4);
            Matrix m1 = Matrix.Identity;
            Matrix3x2 m2 = Matrix3x2.Identity;
            Matrix5x4 m3 = Matrix5x4.Identity;

            var o = new VectorOther {W = new Vector3(1, 2, 3)};

            Debugger.Break();
        }

        private void buttonLargeArrays_Click(object sender, EventArgs e)
        {
            int[,,] acceptable = Enumerator.Generate(1, 1, 1000).ToArray(10, 10, 10);
            int[,,] large = Enumerator.Generate(1, 1, 4096).ToArray(16, 16, 16);
            int[,,] tooLarge = Enumerator.Generate(1, 1, 125000).ToArray(50, 50, 50);

            Debugger.Break();
        }

        private void buttonFsNatrix_Click(object sender, EventArgs e)
        {
            Array arr1 = Enumerator.Generate<long>(1, 1, 360).Shuffle().ToArray(3, 3, 4, 5);
            Array arr2 = Enumerator.Generate(15, (x) => new TestType() {A = x, B = x - 5}).Shuffle().ToArray(5, 3);
            Array arrJ1 = GetJaggedArray2();
            Array arrJ2 = GetJaggedArray3();

            var m1 = MatrixTopLevelOperators.matrix(GetEnums(4, 5));
            var m2 = MatrixTopLevelOperators.matrix(GetEnums(3, 4));

            var v1 = MatrixTopLevelOperators.vector(new double[] {1, 2, 3, 4, 5, 4, 3, 2, 1});
            var v2 = MatrixTopLevelOperators.vector(new[] {1.11, 2.22, 3.33, 5.55, 4.44, 3.33, 1.11});

            Debugger.Break();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region Methods and Operators

        private static int[][] GetJaggedArray2()
        {
            return new[]
            {
                new[] {1, 2, 3, 4}, new[] {1, 2, 3}, new[] {1, 2, 3, 4, 5},
                new[] {1, 2, 3, 4, 6, 7, 8, 89}
            };
        }

        private static int[][][] GetJaggedArray3()
        {
            return new[]
            {
                new[] {new[] {1, 2, 3}, new[] {1, 2, 3, 4, 5}, new[] {1}, new[] {1, 2, 3}, new[] {1, 2}},
                new[] {new[] {1, 2, 3}, new[] {1, 2, 3, 4, 5}, new[] {1}, new[] {1, 2, 3}},
                new[] {new[] {1, 2, 3}, new[] {1, 2, 3, 4, 5}, new[] {1}},
                new[]
                {
                    new[] {1, 2, 3},
                    new[]
                    {
                        1, 2, 3, 4, 5, 2, 3, 4, 5, 2, 3, 4, 5, 2, 3, 4, 5, 2, 3, 4, 5, 2, 3, 4, 5, 2, 3, 4, 5, 2, 3
                    },
                    new[] {1}, new[] {1, 2, 3}, new[] {1, 2}
                }
            };
        }

        #endregion

        private IEnumerable<IEnumerable<double>> GetEnums(int y, int x)
        {
            for (int i = 0; i < y; i++)
                yield return Enumerator.Generate<double>(y, y, x);
        }
    }

    public class TestType
    {
        #region Fields

        public int A;
        public int B;

        #endregion

        #region Public Methods and Operators

        public override string ToString()
        {
            return $"{A} of {B}";
        }

        #endregion
    }

    public class VectorOther
    {
        #region Public Properties

        public Vector3 W { get; set; }

        #endregion
    }
}