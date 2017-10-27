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
        private Array _SystemArray;
        private long[,,] _intArray3DUnUsed;

        private static int[,] _intArray2D_S;
        private static Array _SystemArray_S;
        private static long[,,] _intArray3DUnUsed_S;

        #endregion

        #region Constructors and Destructors

        public Form1()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void buttonBreak_Click(object sender, EventArgs e)
        {
            _intArray2D_S = Enumerator.Generate(10, 5, 20).Shuffle().ToArray(4, 5);
            _SystemArray_S = Enumerator.Generate(1, 3.27, 40).Shuffle().ToArray(8, 5);

            _intArray2D = Enumerator.Generate(8, 4, 32).Shuffle().ToArray(4, 8);
            _SystemArray = Enumerator.Generate(1, 1.7, 24).Shuffle().ToArray(3, 8);

            Debugger.Break();

            _intArray2D_S = null;
            _SystemArray_S = null;

            _intArray2D = null;
            _SystemArray = null;
        }

        private void buttonSystemArray_Click(object sender, EventArgs e)
        {
            Array arr1 = Enumerator.Generate<long>(1, 1, 360).Shuffle().ToArray(3, 3, 4, 5);
            Array arr2 = Enumerator.Generate(15, (X) => new TestType() {a = X, b = X - 5}).Shuffle().ToArray(5, 3);
            Array arrJ1 = GetJaggedArray2();
            Array arrJ2 = GetJaggedArray3();

            Debugger.Break();
        }

        private void buttonStandard_Click(object sender, EventArgs e)
        {
            long[,,,] arr1 = Enumerator.Generate<long>(1, 1, 360).Shuffle().ToArray(3, 3, 4, 5);
            TestType[,] arr2 = Enumerator.Generate(15, (X) => new TestType() {a = X, b = X - 5}).Shuffle()
                .ToArray(5, 3);
            int[][] arrJ1 = GetJaggedArray2();
            int[][][] arrJ2 = GetJaggedArray3();

            Debugger.Break();
        }

        private void buttonChart_Click(object sender, EventArgs e)
        {
            const double RAD = 57.2957795;
            double[] D1 = new double[145];
            double[,] D2 = new double[2, 145];

            for (int i = 0; i < 145; i++)
            {
                D1[i] = Math.Sin(i * 5 / RAD);
                D2[0, i] = D1[i];
                D2[1, i] = D1[i] > 0 ? i * .02 : -i * .02;
            }
            Debugger.Break();
        }

        private void buttonSharpDX_Click(object sender, EventArgs e)
        {
            Array arr1 = Enumerator.Generate<long>(1, 1, 360).Shuffle().ToArray(3, 3, 4, 5);
            Array arr2 = Enumerator.Generate(15, (X) => new TestType() {a = X, b = X - 5}).Shuffle().ToArray(5, 3);
            Array arrJ1 = GetJaggedArray2();
            Array arrJ2 = GetJaggedArray3();

            var v1 = new Vector2(1, 2);
            var v2 = new Vector3(1, 2, 3);
            var v3 = new Vector4(1, 2, 3, 4);
            Matrix m1 = Matrix.Identity;
            Matrix3x2 m2 = Matrix3x2.Identity;
            Matrix5x4 m3 = Matrix5x4.Identity;

            var o = new VectorOther {w = new Vector3(1, 2, 3)};

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
            Array arr2 = Enumerator.Generate(15, (X) => new TestType() {a = X, b = X - 5}).Shuffle().ToArray(5, 3);
            Array arrJ1 = GetJaggedArray2();
            Array arrJ2 = GetJaggedArray3();

            var m1 = MatrixTopLevelOperators.matrix<IEnumerable<double>>(GetEnums(4, 5));
            var m2 = MatrixTopLevelOperators.matrix<IEnumerable<double>>(GetEnums(3, 4));

            var v1 = MatrixTopLevelOperators.vector(new double[] {1, 2, 3, 4, 5, 4, 3, 2, 1});
            var v2 = MatrixTopLevelOperators.vector(new double[] {1.11, 2.22, 3.33, 5.55, 4.44, 3.33, 1.11});

            Debugger.Break();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Methods and Operators

        private static int[][] GetJaggedArray2()
        {
            return new int[][]
            {
                new int[] {1, 2, 3, 4}, new int[] {1, 2, 3}, new int[] {1, 2, 3, 4, 5},
                new int[] {1, 2, 3, 4, 6, 7, 8, 89}
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

        public int a;
        public int b;

        #endregion

        #region Public Methods and Operators

        public override string ToString()
        {
            return string.Format("{0} of {1}", this.a, this.b);
        }

        #endregion
    }

    public class VectorOther
    {
        #region Public Properties

        public Vector3 w { get; set; }

        #endregion
    }
}