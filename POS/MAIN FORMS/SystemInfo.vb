Imports System.Management
Imports System.Management.Instrumentation
Imports System
Imports System.IO
Public Class SystemInfo
    Private Sub SystemInfo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Start()
        Try
            Dim i As System.Management.ManagementObject
            Dim searchInfo_Processor As New System.Management.ManagementObjectSearcher("Select * from Win32_Processor")
            For Each i In searchInfo_Processor.Get()
                txtProcessorName.Text = i("Name").ToString
                txtProcessorID.Text = i("ProcessorID").ToString
                txtProcessorDescription.Text = i("Description").ToString
                txtProcessorManufacturer.Text = i("Manufacturer").ToString
                txtProcessorL2CacheSize.Text = i("L2CacheSize").ToString
                txtProcessorClockSpeed.Text = i("CurrentClockSpeed").ToString & " Mhz"
                txtProcessorDataWidth.Text = i("DataWidth").ToString
                txtProcessorExtClock.Text = i("ExtClock").ToString & " Mhz"
                txtProcessorFamily.Text = i("Family").ToString
            Next
            Dim searchInfo_Board As New System.Management.ManagementObjectSearcher("Select * from Win32_BaseBoard")
            For Each i In searchInfo_Board.Get()
                txtBoardDescription.Text = i("Description").ToString
                txtBoardManufacturer.Text = i("Manufacturer").ToString
                txtBoardName.Text = i("Name").ToString
                txtBoardSerialNumber.Text = i("SerialNumber").ToString
            Next

            TextBox1.Text = Environment.UserDomainName.ToString
            TextBox2.Text = Environment.Version.ToString
            TextBox3.Text = Environment.OSVersion.ToString
            TextBox4.Text = String.Format(" {0} MBytes", System.Math.Round(My.Computer.Info.TotalPhysicalMemory / (1024 * 1024)), 2).ToString
            TextBox6.Text = getWMIInfo("OSArchitecture", "Win32_OperatingSystem")
            TextBox8.Text = getHD()
            Label23.Text = GetCurDrive()
            TextBox7.Text = GetSpace()

        Catch ex As Exception
            MsgBox(ex.tostring, MsgBoxStyle.Critical, "Error!")
            End
        End Try
    End Sub
    Public Function GetCurDrive() As String
        Label23.Text = Path.GetPathRoot(Environment.SystemDirectory)
        Dim res = Label23.Text(0)
        Label23.Text = res
        Return res.ToString
    End Function
    Public Function getHD() As String
        Dim dblSize As Double 'Store Size
        dblSize = Math.Round(GetHDSize(GetCurDrive) / 1024 / 1024 / 1024) 'Call GetHDSize Sub and Divide 3 Times By 1024 ( Byte ) To Give GB 
        '1 KB = 1024 - KiloByte
        '1 MB = 1024 ^ 2 - MegaByte
        '1 GB = 1024 ^ 3 - GigaByte
        '1 TB = 1024 ^ 4 - TeraByte
        '1 PB = 1024 ^ 5 - PetaByte
        '1 EB = 1024 ^ 6 - ExaByte
        '1 ZB = 1024 ^ 7 - ZettaByte
        '1 YB = 1024 ^ 8 - YottaByte
        '1 BB = 1024 ^ 9 - BrontoByte
        Return dblSize.ToString() & " GB" 'Display Result
    End Function
    Public Function GetHDFreeSpace(ByVal strDrive As String) As Double

        'Ensure Valid Drive Letter Entered, Else, Default To C
        If strDrive = "" OrElse strDrive Is Nothing Then

            strDrive = "C"

        End If

        'Make Use Of Win32_LogicalDisk To Obtain Hard Disk Properties
        Dim moHD As New ManagementObject("Win32_LogicalDisk.DeviceID=""" + strDrive + ":""")

        'Get Info
        moHD.[Get]()

        'Get Hard Disk Free Space
        Return Convert.ToDouble(moHD("FreeSpace"))

    End Function

    Public Function GetSpace() As String
        Dim dblFree As Double 'Store Size

        dblFree = Math.Round(GetHDFreeSpace(GetCurDrive) / 1024 / 1024 / 1024) 'Call GetHDFreeSpace Sub and Divide 3 Times By 1024 ( Byte ) To Give GB

        '1 KB = 1024 - KiloByte
        '1 MB = 1024 ^ 2 - MegaByte
        '1 GB = 1024 ^ 3 - GigaByte
        '1 TB = 1024 ^ 4 - TeraByte
        '1 PB = 1024 ^ 5 - PetaByte
        '1 EB = 1024 ^ 6 - ExaByte
        '1 ZB = 1024 ^ 7 - ZettaByte
        '1 YB = 1024 ^ 8 - YottaByte
        '1 BB = 1024 ^ 9 - BrontoByte

        Return (dblFree.ToString & " GB") 'Display Result

    End Function
    Public Function getWMIInfo(ByVal wmiObjectInfo As String, ByVal wmiRelativePath As String) As String
        Try
            '
            'Give it something to report in case something wrong happens.
            getWMIInfo = "Nothing!"

            Dim wmiClass As New System.Management.ManagementClass
            Dim wmiObject As New System.Management.ManagementObject

            wmiClass.Path.RelativePath = wmiRelativePath
            'This will go through each item in the requested info. I only care about
            'the 1st for the most part but remeber that each instance could have different values.
            For Each wmiObject In wmiClass.GetInstances
                getWMIInfo = (wmiObject(wmiObjectInfo))
                'I only want the first instance.
                Return getWMIInfo
            Next
        Catch exc As Exception
            Return exc.Message
        End Try

    End Function
    Public Function GetHDSize(ByVal strDrive As String) As Double 'Get Size of Specified Disk

        'Ensure Valid Drive Letter Entered, Else, Default To C
        If strDrive = "" OrElse strDrive Is Nothing Then

            strDrive = GetCurDrive()

        End If

        'Make Use Of Win32_LogicalDisk To Obtain Hard Disk Properties
        Dim moHD As New ManagementObject("Win32_LogicalDisk.DeviceID=""" + strDrive + ":""")

        'Get Info
        moHD.[Get]()

        'Get Hard Disk Size
        Return Convert.ToDouble(moHD("Size"))

    End Function
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        TextBox5.Text = String.Format(" {0} MBytes", System.Math.Round(My.Computer.Info.AvailablePhysicalMemory / (1024 * 1024)), 2).ToString
    End Sub
End Class