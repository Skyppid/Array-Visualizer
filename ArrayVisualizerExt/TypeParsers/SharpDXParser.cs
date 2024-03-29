﻿using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using LinqLib.Sequence;
using Microsoft.VisualStudio.Shell;

namespace ArrayVisualizerExt.TypeParsers
{
    public class SharpDxParser : ITypeParser
    {
        #region ITypeParser Members

        public char LeftBracket { get; set; }
        public char RightBracket { get; set; }
        public Func<string, int> ParseDimension { get; set; }

        public bool IsExpressionTypeSupported(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            string expressionType = expression.Type;
            return expressionType.StartsWith("SharpDX.Matrix") || expressionType.StartsWith("SharpDX.Vector");
        }

        public string GetDisplayName(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            ThreadHelper.ThrowIfNotOnUIThread();
            int elements = expression.Value.Count(c => c == ':');
            string formatter;
            switch (elements)
            {
                case 2:
                    formatter = "Vector{0}2{1}";
                    break;
                case 3:
                    formatter = "Vector{0}3{1}";
                    break;
                case 4:
                    formatter = "Vector{0}4{1}";
                    break;
                case 16:
                    formatter = "Matrix{0}4,4{1}";
                    break;
                case 6:
                    formatter = "Matrix{0}3,2{1}";
                    break;
                case 20:
                    formatter = "Matrix{0}5,4{1}";
                    break;
                default:

                    throw new ArgumentOutOfRangeException(nameof(expression),
                        "The number of ':' separators found in expression.Value.Count is invalid.");
            }

            return string.Format(formatter, LeftBracket, RightBracket);
        }

        public int[] GetDimensions(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            ThreadHelper.ThrowIfNotOnUIThread();
            switch (expression.Type)
            {
                case "SharpDX.Matrix":
                    return new[] {4, 4};
                case "SharpDX.Matrix3x2":
                    return new[] {3, 2};
                case "SharpDX.Matrix5x4":
                    return new[] {5, 4};
                case "SharpDX.Vector2":
                    return new[] {2};
                case "SharpDX.Vector3":
                    return new[] {3};
                case "SharpDX.Vector4":
                    return new[] {4};
                default:
                    throw new NotSupportedException($"'{expression.Type} is not supported.'");
            }
        }

        public int GetMembersCount(Expression expression)
        {
            int[] dims = GetDimensions(expression);
            if (dims.Length == 1)
                return dims[0];

            return dims[0] * dims[1];
        }

        public object[] GetValues(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            ThreadHelper.ThrowIfNotOnUIThread();

            Predicate<Expression> predicate;
            bool rotate = false;
            switch (expression.Type)
            {
                case "SharpDX.Matrix":
                case "SharpDX.Matrix3x2":
                case "SharpDX.Matrix5x4":
                    predicate = e => 'M' == e.Name[0];
                    break;
                case "SharpDX.Vector2":
                    predicate = e => "XY".Contains(e.Name.Last());
                    break;
                case "SharpDX.Vector3":
                    predicate = e => "XYZ".Contains(e.Name.Last());
                    break;
                case "SharpDX.Vector4":
                    predicate = e => "XYZW".Contains(e.Name.Last());
                    rotate = true;
                    break;
                default:
                    return Array.Empty<object>();
            }

            object[] values;
            IEnumerable<Expression> query = expression.DataMembers.Cast<Expression>().Where(e => predicate(e));

            if (rotate)
                query = query.RotateLeft(1);

            if (expression.DataMembers.Item(1).Type.Contains(LeftBracket))
                values = query.ToArray();
            else
                values = query.Select(e => e.Value).ToArray();
            return values;
        }

        #endregion
    }
}