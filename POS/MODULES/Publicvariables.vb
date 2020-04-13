Imports MySql.Data.MySqlClient
Imports System.Drawing.Printing
Module publicVariables
    'Connection
    Public localconn As MySqlConnection
    Public cloudconn As MySqlConnection
    Public cmd As MySqlCommand
    Public cloudcmd As New MySqlCommand
    Public da As MySqlDataAdapter
    Public dr As MySqlDataReader
    Public connectionstring As String
    '=============================================================================================================
    'Data Table
    Public dt As DataTable
    Public sql As String
    Public param As MySqlParameter
    Public ds As DataSet
    '=============================================================================================================
    'POS
    Public SINumber As Integer
    Public SiNumberToString As String
    Public transactionmode As String = "Walk-In"
    Public SeniorPWd As Decimal
    Public SeniorPWdDrinks As Decimal
    Public ThisIsMyInventoryID
    Public Shift As String
    Public BeginningBalance As Decimal
    Public EndingBalance As Decimal
    Public payment As Boolean = False
    Public posandpendingenter As Boolean = False
    Public productprice As Integer
    Public deleteitem As Boolean = False
    Public enterpressorbuttonpress As Boolean = False
    Public qtyisgreaterthanstock As Boolean = False
    Public hastextboxqty As Boolean = False
    Public productID
    Public getmunicipality
    Public getprovince
    Public Couponisavailable As Boolean
    Public DiscountType As String
    Public modeoftransaction As Boolean
    Public SyncIsOnProcess As Boolean
    '=============================================================================================================
    'POS INFORMATION
    Public ClientGuid As String
    Public ClientCrewID As String
    Public ClientStoreID As String
    Public ClientRole As String
    Public ClientBrand As String
    Public ClientLocation As String
    Public ClientPostalCode As String
    Public ClientAddress As String
    Public ClientBrgy As String
    Public ClientMunicipality As String
    Public ClientProvince As String
    Public ClientTin As String
    Public ClientTel As String
    Public ClientStorename As String
    Public ClientProductKey As String
    Public ClientMIN As String
    Public ClientMSN As String
    Public ClientPTUN As String
    '==CONNECTION STRINGS
    Public LocalConnectionString As String
    Public CloudConnectionString As String
    '==SETTINGS
    Public S_ExportPath As String
    Public S_Tax As String
    Public S_SIFormat As String
    Public S_Terminal_No As String
    Public S_ZeroRated As String
    Public S_Zreading As String
    '=============================================================================================================
    'btn click refresh
    Public btnperformclick As Boolean = False
    '=============================================================================================================
    'add module
    Public messageboxappearance As Boolean = False
    Public table As String
    Public fields As String
    Public value As String
    Public where As String
    Public successmessage As String
    Public errormessage As String
    Public returnvalrow As String
    Public mysqlcondition As String
    Public SystemLogType As String
    Public SystemLogDesc As String
    '=============================================================================================================
    'connection module
    Public myMySqlException As Boolean = True
    'Expenses
    Public hasbutton As Boolean = False
    '
    Public DatasourceOrAdd As Boolean = False
    'Payment Form
    Public TEXTBOXMONEYVALUE
    Public TEXTBOXTOTALPAYVALUE
    Public TEXTBOXCHANGEVALUE
    Public ifclose As Boolean
    'ModeOfTransactionDetails
    Public TEXTBOXFULLNAMEVALUE
    Public TEXTBOXREFERENCEVALUE
    Public TEXTBOXMARKUPVALUE
    '
    Public IfConnectionIsConfigured As Boolean
End Module
