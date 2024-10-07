Imports System.Data.Odbc
Public Class Form1
    Dim conn As OdbcConnection
    Dim dr As OdbcDataReader
    Dim da As OdbcDataAdapter
    Dim cmd As OdbcCommand

    Sub koneksi()
        conn = New OdbcConnection("dsn=dsn_db_aplikasi")
        Try
            If conn.State = ConnectionState.Closed Then
                conn.Open()
                MsgBox("Koneksi Berhasil", MsgBoxStyle.Exclamation, "informasi")
            End If

        Catch ex As Exception
            MessageBox.Show("Koneksi Gagal" & ex.Message)
        End Try
    End Sub
    Sub bersihkan()
        txtnama.Clear()
        txtalamat.Clear()
        txttelp.Clear()
        txtnim.Clear()
    End Sub

    Sub tambahdata()
        Try
            Call koneksi()
            Dim input_nama As String = txtnama.Text
            Dim input_alamat As String = txtalamat.Text
            Dim input_telp As String = txttelp.Text

            Dim query As String = "INSERT INTO tbl_anggota (namaanggota, alamatanggota, telpanggota) VALUES (?,?,?)"

            cmd = New OdbcCommand(query, conn)
            cmd.Parameters.AddWithValue("namaanggota", input_nama)
            cmd.Parameters.AddWithValue("alamatanggota", input_alamat)
            cmd.Parameters.AddWithValue("telpanggota", input_telp)
            cmd.ExecuteNonQuery()
            MsgBox("Data berhasil disimpan!!")
            tampildata()
            bersihkan()

        Catch ex As Exception
            MsgBox("Terjadi kesalahan: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub

    Sub tampildata()
        Call koneksi()
        cmd = New OdbcCommand("select * from tbl_anggota", conn)
        dr = cmd.ExecuteReader
        DataGridView1.Rows.Clear()
        Do While dr.Read = True
            DataGridView1.Rows.Add(dr(0), dr(1), dr(2), dr(3))
        Loop
    End Sub
    Sub hapusdata()
        Call koneksi()
        Dim kodeanggota As String = txtnim.Text
        Dim sql As String = "DELETE FROM tbl_anggota WHERE kodeanggota = ?"
        Try
            Dim cmd As New OdbcCommand(sql, conn)
            cmd.Parameters.AddWithValue("kodeanggota", kodeanggota)
            cmd.ExecuteNonQuery()
            MsgBox("Data berhasil dihapus")
        Catch ex As Exception
            MsgBox("Gagal: " & ex.Message)
        End Try
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btmkoneksi.Click
        Call koneksi()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        txtnim.Text = DataGridView1.Rows(e.RowIndex).Cells(0).Value
        txtnama.Text = DataGridView1.Rows(e.RowIndex).Cells(1).Value
        txtalamat.Text = DataGridView1.Rows(e.RowIndex).Cells(2).Value
        txttelp.Text = DataGridView1.Rows(e.RowIndex).Cells(3).Value
    End Sub


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tampildata()
    End Sub


    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Call tambahdata()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Call hapusdata()
        Call bersihkan()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim kodeanggota As String = txtnim.Text
        Dim namaanggota As String = txtnama.Text
        Dim alamatanggota As String = txtalamat.Text
        Dim nomortelp As String = txttelp.Text
        Dim sql As String = "UPDATE tbl_anggota SET namaanggota = ?, alamatanggota = ?, telpanggota = ? WHERE kodeanggota = ?"
        Try
            Call koneksi()
            Dim cmd As New OdbcCommand(sql, conn)
            cmd.Parameters.AddWithValue("namaanggota", namaanggota)
            cmd.Parameters.AddWithValue("alamatanggota", alamatanggota)
            cmd.Parameters.AddWithValue("telpanggota", nomortelp)
            cmd.Parameters.AddWithValue("kodeanggota", kodeanggota)
            cmd.ExecuteNonQuery()
            MsgBox("Data berhasil diubah")
            tampildata()
            bersihkan()
        Catch ex As Exception
            MsgBox("Gagal: " & ex.Message)
        Finally
            conn.Close()
        End Try

        MsgBox("Pilih satu baris data yang akan diubah")

    End Sub
End Class
