Public Class BO

    Public Shared counter As Integer = 0 ' keep track continus array
    Public Shared newcounter As Integer = 0 ' ignore previous number after equal button
    Public Shared nextnum As Boolean = False ' scan number or operator after equal button

    Public Shared Function Reset()
        ' Delete 0 if numeric button is pressed
        Try
            If UI.TextBox2.Text = "0" Then
                UI.TextBox2.Text = ""
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Shared Function updateKey(ByVal keys As String)
        ' Handle numeric, negative and dot button key press
        Try
            If keys = "." Then
                ' Add dot after current number
                If Not UI.TextBox2.Text.Contains(".") Then
                    UI.TextBox2.Text = UI.TextBox2.Text + keys
                End If
            ElseIf keys = "-" Then
                ' Convert current number to negative or positive
                If Not UI.TextBox2.Text.Contains("-") Then
                    UI.TextBox2.Text = "-" + UI.TextBox2.Text
                Else
                    UI.TextBox2.Text = UI.TextBox2.Text.Remove(0, 1)
                End If
            Else
                If nextnum Then
                    ' Start next formula
                    UI.TextBox2.Text = keys
                    nextnum = False
                Else
                    ' Normal operation
                    Reset()
                    UI.TextBox2.Text = UI.TextBox2.Text + keys
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Shared Function history()
        Try
            UI.TextBox3.Text = "" ' clear history display
            Dim newline As Boolean = False
            For i = 0 To counter
                UI.TextBox3.Text += DA.array(i)
                If newline Then
                    ' Add new line after the answer after an equal operator
                    UI.TextBox3.Text += Environment.NewLine
                    newline = False
                End If
                If DA.array(i) = "=" Then
                    ' Check equal operator and apply next line in the next loop
                    newline = True
                End If
            Next
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Shared Function cancel()
        ' Reset everything and start with clean array
        Try
            counter = 0
            UI.TextBox2.Text = 0
            UI.TextBox3.Text = ""
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Shared Function cancelEntry()
        ' cancel latest entry and wait for new number
        Try
            UI.TextBox2.Text = 0
            history()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Shared Function delete()
        ' Delete latest number and reset to 0 after last number is deleted
        Try
            If UI.TextBox2.TextLength < 2 Then
                UI.TextBox2.Text = "0"
            Else
                UI.TextBox2.Text = UI.TextBox2.Text.Remove(UI.TextBox2.TextLength - 1, 1)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Shared Function operation(ByVal operators As String)
        ' Add operator to the array
        Try
            DA.array.SetValue(UI.TextBox2.Text, counter)
            counter += 1
            If operators = "+" Then
                DA.array.SetValue("+", counter)
            ElseIf operators = "-" Then
                DA.array.SetValue("-", counter)
            ElseIf operators = "*" Then
                DA.array.SetValue("*", counter)
            ElseIf operators = "/" Then
                DA.array.SetValue("/", counter)
            End If
            UI.TextBox2.Text = "0"
            history()
            counter += 1
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Shared Function calculate()
        ' Calculate result from the array with latest cut point of newcounter
        Try
            Dim answer As Double
            answer = DA.array(newcounter)
            For i = newcounter To counter
                If DA.array(i) = "+" Then
                    answer += DA.array(i + 1)
                ElseIf DA.array(i) = "-" Then
                    answer -= DA.array(i + 1)
                ElseIf DA.array(i) = "*" Then
                    answer *= DA.array(i + 1)
                ElseIf DA.array(i) = "/" Then
                    answer /= DA.array(i + 1)
                End If
            Next
            UI.TextBox2.Text = answer
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Shared Function equalOperation()
        ' Add second value, operator and answer into array and do cut point for calculation
        Try
            DA.array.SetValue(UI.TextBox2.Text, counter) ' save second operational num into array
            counter += 1
            DA.array.SetValue("=", counter) ' save equal operator into array
            calculate()
            counter += 1
            DA.array.SetValue(UI.TextBox2.Text, counter) ' save answer into array
            history()
            counter += 1
            ' cut point
            newcounter = counter
            nextnum = True
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

End Class
