Imports LinqLib.Array
Imports LinqLib.Sequence
Imports SharpDX

Public Class Form1

#Region "Fields"

  Dim _intArray2D As Integer(,)
  Dim _SystemArray As Array
  Dim _intArray3DUnUsed As Long(,,)

  Shared _intArray2D_S As Integer(,)
  Shared _SystemArray_S As Array
  Shared _intArray3DUnUsed_S As Long(,,)

#End Region

  Private Sub buttonBreak_Click(sender As Object, e As EventArgs) Handles buttonBreak.Click

    _intArray2D_S = Enumerator.Generate(10, 5, 20).Shuffle().ToArray(4, 5)
    _SystemArray_S = Enumerator.Generate(1, 3.27, 40).Shuffle().ToArray(8, 5)

    _intArray2D = Enumerator.Generate(8, 4, 32).Shuffle().ToArray(4, 8)
    _SystemArray = Enumerator.Generate(1, 1.7, 24).Shuffle().ToArray(3, 8)

    Debugger.Break()

    _intArray2D_S = Nothing
    _SystemArray_S = Nothing

    _intArray2D = Nothing
    _SystemArray = Nothing

  End Sub

  Private Sub buttonSystemArray_Click(sender As Object, e As EventArgs) Handles buttonSystemArray.Click

    Dim arr1 As Array = Enumerator.Generate(Of Long)(1, 1, 360).Shuffle().ToArray(3, 3, 4, 5)
    Dim arr2 = Enumerator.Generate(Of TestType)(15, Function(X1) GetNewType(X1)).Shuffle().ToArray(5, 3)
    Dim arrJ1 As Array = GetJaggedArray2()
    Dim arrJ2 As Array = GetJaggedArray3()

    Debugger.Break()

  End Sub

  Private Sub buttonStandard_Click(sender As Object, e As EventArgs) Handles buttonStandard.Click

    Dim arr1 As Long(,,,) = Enumerator.Generate(Of Long)(1, 1, 360).Shuffle().ToArray(3, 3, 4, 5)
    Dim arr2 As TestType(,) = Enumerator.Generate(Of TestType)(15, Function(X1) GetNewType(X1)).Shuffle().ToArray(5, 3)
    Dim arrJ1 As Integer()() = GetJaggedArray2()
    Dim arrJ2 As Integer()()() = GetJaggedArray3()

    Debugger.Break()

  End Sub

  Private Sub buttonLargeArrays_Click(sender As Object, e As EventArgs) Handles buttonLargeArrays.Click

    Dim acceptable(,,) As Integer = Enumerator.Generate(1, 1, 1000).ToArray(10, 10, 10)
    Dim large(,,) As Integer = Enumerator.Generate(1, 1, 4096).ToArray(16, 16, 16)
    Dim tooLarge(,,) = Enumerator.Generate(1, 1, 125000).ToArray(50, 50, 50)

    Debugger.Break()

  End Sub

  Private Sub buttonChart_Click(sender As Object, e As EventArgs) Handles buttonChart.Click

    Const RAD As Double = 57.2957795
    Dim D1(145) As Double
    Dim D2(2, 145) As Double

    For i As Integer = 0 To 144
      D1(i) = Math.Sin(i * 5 / RAD)
      D2(0, i) = D1(i)
      If D1(i) > 0 Then
        D2(1, i) = i * 0.02
      Else
        D2(1, i) = -i * 0.02
      End If
    Next

    Debugger.Break()

  End Sub

  Private Sub buttonSharpDX_Click(sender As Object, e As EventArgs) Handles buttonSharpDX.Click

    Dim arr1 As Long(,,,) = Enumerator.Generate(Of Long)(1, 1, 360).Shuffle().ToArray(3, 3, 4, 5)
    Dim arr2 As TestType(,) = Enumerator.Generate(Of TestType)(15, Function(X1) GetNewType(X1)).Shuffle().ToArray(5, 3)
    Dim arrJ1 As Integer()() = GetJaggedArray2()
    Dim arrJ2 As Integer()()() = GetJaggedArray3()

    Dim v1 = New Vector2(1, 2)
    Dim v2 = New Vector3(1, 2, 3)
    Dim v3 = New Vector4(1, 2, 3, 4)
    Dim m1 = Matrix.Identity
    Dim m2 = Matrix3x2.Identity
    Dim m3 = Matrix5x4.Identity

    Dim o = New VectorOther()
    o.W = New Vector3(1, 2, 3)

    Debugger.Break()

  End Sub

  Private Sub buttonClose_Click(sender As Object, e As EventArgs) Handles buttonClose.Click
    Me.Close()
  End Sub

  Private Shared Function GetJaggedArray2() As Integer()()

    Dim arr = New Integer()() {New Integer() {1, 2, 3}, New Integer() {1, 2, 3, 4, 5}, New Integer() {1}, New Integer() {1, 2, 3}, New Integer() {1, 2}}
    Return arr

  End Function


  Private Shared Function GetJaggedArray3() As Integer()()()

    Dim arr()()() As Integer =
           {
             New Integer()() {New Integer() {1, 2, 3}, New Integer() {1, 2, 3, 4, 5}, New Integer() {1}, New Integer() {1, 2, 3}, New Integer() {1, 2}}, _
                New Integer()() {New Integer() {1, 2, 3}, New Integer() {1, 2, 3, 4, 5}, New Integer() {1}, New Integer() {1, 2, 3}}, _
                New Integer()() {New Integer() {1, 2, 3}, New Integer() {1, 2, 3, 4, 5}, New Integer() {1}}, _
                New Integer()() {New Integer() {1, 2, 3}, _
                                 New Integer() {1, 2, 3, 4, 5, 2, 3, 4, 5, 2, 3, 4, 5, 2, 3, 4, 5, 2, 3, 4, 5, 2, 3, 4, 5, 2, 3, 4, 5, 2, 3}, _
                                 New Integer() {1}, New Integer() {1, 2, 3}, New Integer() {1, 2}}}

    Return arr

  End Function

  Public Class TestType

    Public a As Integer
    Public b As Integer


    Public Overrides Function ToString() As String
      Return String.Format("{0} of {1}", Me.a, Me.b)
    End Function

  End Class

  Private Sub buttonFsNatrix_Click(sender As Object, e As EventArgs) Handles buttonFsNatrix.Click

    Dim arr1 As Long(,,,) = Enumerator.Generate(Of Long)(1, 1, 360).Shuffle().ToArray(3, 3, 4, 5)
    Dim arr2 As TestType(,) = Enumerator.Generate(Of TestType)(15, Function(X1) GetNewType(X1)).Shuffle().ToArray(5, 3)
    Dim arrJ1 As Integer()() = GetJaggedArray2()
    Dim arrJ2 As Integer()()() = GetJaggedArray3()

    Dim m1 = Microsoft.FSharp.Math.MatrixTopLevelOperators.matrix(Of IEnumerable(Of Double))(GetEnums(4, 5))
    Dim m2 = Microsoft.FSharp.Math.MatrixTopLevelOperators.matrix(Of IEnumerable(Of Double))(GetEnums(3, 4))

    Dim v1 = Microsoft.FSharp.Math.MatrixTopLevelOperators.vector(New Double() {1, 2, 3, 4, 5, 4, 3, 2, 1})
    Dim v2 = Microsoft.FSharp.Math.MatrixTopLevelOperators.vector(New Double() {1.11, 2.22, 3.33, 5.55, 4.44, 3.33, 1.11})

    Debugger.Break()

  End Sub

  Private Function GetNewType(X As Integer) As TestType

    Dim tt As New TestType()
    tt.a = X
    tt.b = X * 2

    Return tt

  End Function

  Public Class VectorOther

    Public W As Vector3

  End Class

  Private Iterator Function GetEnums(y As Integer, x As Integer) As IEnumerable(Of IEnumerable(Of Double))

    For i = 0 To y - 1
      Yield Enumerator.Generate(Of Double)(y, y, x)
    Next

  End Function

End Class