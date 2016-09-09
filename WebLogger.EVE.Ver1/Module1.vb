Imports System.IO
Imports System.Text
Imports System.Threading
Imports System.Text.Encoding
Imports System.Net
Imports System.Web
Imports System.Windows.Forms
Imports System.Security.Cryptography
Imports System.Convert
Imports System.Globalization
Imports System.Math
Imports System.Runtime.InteropServices
Imports Microsoft.Win32

Public Module Module1

#Const T = True

#Region "API函数及全局变量声明"

    Class NativeMethods

        Private Function NativeMethods()

        End Function

        Private Function NativeMethods(ByVal i As Integer)
            Dim xtendarr(i) As String
            xtendarr(0) = "0-Methods Created"
            Return xtendarr
        End Function

        ''' <summary>
        ''' WindowsAPI，用于获取ini文件中的值
        ''' </summary>
        ''' <param name="lpApplicationName">应用程序名称，通常在[]一栏内</param>
        ''' <param name="lpKeyName">键名称，通常为KeyName=</param>
        ''' <param name="lpDefault">默认返回值，通常为空</param>
        ''' <param name="lpReturnedString">用于返回值存储的空间</param>
        ''' <param name="nSize">返回字符串长度</param>
        ''' <param name="lpFileName">要读取的ini文件名</param>
        ''' <returns>一个整数值，用于表明是否成功读取了该ini文件的值</returns>
        ''' <remarks></remarks>
        Public Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" _
        (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, _
         ByVal nSize As Int32, ByVal lpFileName As String) _
         As Int32

        ''' <summary>
        ''' WindowsAPI,用于写入ini文件键值
        ''' </summary>
        ''' <param name="lpApplicationName">应用程序名称，通常在[]一栏内</param>
        ''' <param name="lpKeyName">键名称，通常为KeyName=</param>
        ''' <param name="lpString">试图写入的字符串</param>
        ''' <param name="lpFileName">要写入的ini文件名</param>
        ''' <returns>一个整数值，用于表明是否成功写入了该ini文件</returns>
        ''' <remarks></remarks>
        Public Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" _
            (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) _
            As <MarshalAsAttribute(UnmanagedType.Bool)> Boolean

    End Class
    Public PingSent, PingRecv As Integer
    Public desenc(8) As Byte

    ''' <summary>
    ''' 账户信息存储结构
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure AccountInfo
        Dim user As String
        Dim password_encrypted As String
    End Structure

    Public Structure ProxyInfo
        Private m_ip As String
        Private m_user As String
        Private m_pass As String
        Private m_port As Integer
        Public Property IP As String
            Get
                Return m_ip
            End Get
            Set(value As String)
                m_ip = value
            End Set
        End Property
        Public Property User As String
            Get
                Return m_user
            End Get
            Set(value As String)
                m_user = value
            End Set
        End Property
        Public Property Pass As String
            Get
                Return m_pass
            End Get
            Set(value As String)
                m_pass = value
            End Set
        End Property

        Public Property Port As Integer
            Get
                Return m_port
            End Get
            Set(value As Integer)
                m_port = value
            End Set
        End Property

        Public Sub New(u As String, p As String, i As String, pt As Integer)
            m_user = u
            m_pass = p
            m_ip = i
            m_port = pt
        End Sub
    End Structure

    Public Class WebProxy_My
        Implements IWebProxy

        Private webProxyUri As Uri
        Private iCredentials As ICredentials

        Public Property Credentials As ICredentials Implements IWebProxy.Credentials
            Get
                Return iCredentials
            End Get
            Set(value As ICredentials)
                iCredentials = value
            End Set
        End Property

        Public Function GetProxy(destination As Uri) As Uri Implements IWebProxy.GetProxy
            Return webProxyUri
        End Function

        Public Function IsBypassed(host As Uri) As Boolean Implements IWebProxy.IsBypassed
            Return False
        End Function
    End Class


    Structure Headers
        Public Sub New(ByVal Header As String, ByVal Content As String)
            head = Header
            cont = Content
        End Sub
        Public head As String
        Public cont As String
    End Structure

    Structure htmlDocumentResponse
        Public iHTML As String
        Public uri As String
        Public iCookies As CookieCollection
    End Structure

    ''' <summary>
    ''' 定义全局存储账户信息数组
    ''' </summary>
    ''' <remarks></remarks>
    Public aAccounts As AccountInfo()

    Public proxy As WebProxy
    Public useProxy As Boolean = False
    Public GUID, captcha As String
    Public CookieStr, LoginAddr As String

    ''' <summary>
    ''' 常量定义
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ZToN As String = "0123456789"
    Public Const LoginPageURI As String = "https://auth.eve-online.com.cn/Account/LogOn?ReturnUrl=%2foauth%2fauthorize%3fclient_id%3deveLauncherSerenity%26lang%3dzh%26response_type%3dtoken%26redirect_uri%3dhttps%3a%2f%2fauth.eve-online.com.cn%2flauncher%3fclient_id%3deveLauncherSerenity%26scope%3deveClientToken%2520user&client_id=eveLauncherSerenity&lang=zh&response_type=token&redirect_uri=https://auth.eve-online.com.cn/launcher?client_id=eveLauncherSerenity&scope=eveClientToken%20user"
    'Public Const LoginPageURI As String = "https://auth.eve-online.com.cn/Account/LogOn?ReturnUrl=%2foauth%2fauthorize%3fclient_id%3deveclient%26scope%3deveClientLogin%26response_type%3dtoken%26redirect_uri%3dhttps%253A%252F%252Fauth.eve-online.com.cn%252Flauncher%253Fclient_id%253Deveclient%26lang%3dzh%26mac%3dNone&client_id=eveclient&scope=eveClientLogin&response_type=token&redirect_uri=https%3A%2F%2Fauth.eve-online.com.cn%2Flauncher%3Fclient_id%3Deveclient&lang=zh&mac=None"
    Public Const PostContentType As String = "application/x-www-form-urlencoded"
    Public Const PostContentHeader As String = "Content-Type"
    Public Const UserAgentHeader As String = "UserAgent"
    Public Const UserAgentValue As String = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0"
    Public Const captchaURLHeader As String = "https://captcha.tiancity.com:442/CheckSwitch.ashx?jsoncallback=jQuery"
    Public Const GUIDURL As String = "https://auth.eve-online.com.cn/Account/GenerateGuid/"
    Public Const captchaImageURL As String = "https://captcha.tiancity.com:442/getimage.ashx?tid="
    Public Const PassCardPostBackURI As String = "https://auth.eve-online.com.cn/Account/TwoFactor?ReturnUrl=%2Foauth%2Fauthorize%3Fclient_id%3DeveLauncherSerenity%26lang%3Dzh%26response_type%3Dtoken%26redirect_uri%3Dhttps%3A%2F%2Fauth.eve-online.com.cn%2Flauncher%3Fclient_id%3DeveLauncherSerenity%26scope%3DeveClientToken%2520user"
    Public Const vFailString As String = "<div class=""validation-summary-errors"">"
    Public Const AcceptString As String = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"

#End Region

#Region "数据处理"

    ''' <summary>
    ''' 验证客户端版本号
    ''' </summary>
    ''' <param name="i">返回值</param>
    ''' <remarks></remarks>
    Sub ValidClientVersion(ByRef i As Integer)
        Dim dnf As WebClient = New WebClient

        If useProxy Then
            dnf.Proxy = proxy
        End If
        Dim ss, sst(), sync, build As String
        Dim bbyte As Byte()
        ss = ""
        sync = ss
        build = ss
        Try
            bbyte = dnf.DownloadData("http://client.eve-online.com.cn/patches/premium_patchinfoserenity_inc.txt")
            ss = Encoding.ASCII.GetChars(bbyte)
            ReDim sst(UBound(ss.Split(",")))
            sst = ss.Split(",")
            sst(0) = sst(0).Split(":")(1)
            sst(1) = sst(1).Split(" ")(1)
            readini("sync", sync)
            readini("build", build)
            dnf.Dispose()
        Catch ex As Exception
            MsgBox("网络连接错误，程序即将退出。。。")
            exitprog()
            i = -1
        End Try
        Try
            If sync <> sst(1) And build <> sst(0) Then
                MsgBox("你的客户端需要更新，点击""确定""启动官方启动器以完成更新。")
                Shell("eve.exe")
                i = 2
            End If
        Catch ex As Exception
            MsgBox("无法打开eve.exe,请确定本程序已经放置于EVE Online根目录下!!!")
            exitprog()
            i = -1
        End Try
        i = 1
    End Sub

    ''' <summary>
    ''' Xor加密
    ''' </summary>
    ''' <param name="pToEncrypt"></param>
    ''' <param name="sKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Encrypt(ByVal pToEncrypt As String, ByVal sKey As Byte()) As Byte()
        Dim pByte As Byte() = ASCII.GetBytes(pToEncrypt)
        If pByte.Length < 8 Then
            ReDim pByte(8)
            For l = pByte.Length To 8
                pByte(l) = 0
            Next
        End If
        Randomize(Date.Now.ToOADate)
        Dim enc(8) As Byte

        For i = 0 To 7
            enc(i) = pByte(i) Xor sKey(i)
        Next
        Return enc
    End Function

    ''' <summary>
    ''' Xor解密
    ''' </summary>
    ''' <param name="pToDecrypt"></param>
    ''' <param name="sKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Decrypt(ByVal pToDecrypt As Byte(), ByVal sKey As Byte()) As String
        Dim pByte As Byte() = pToDecrypt
        Dim enc(8) As Byte
        For i = 0 To 7
            enc(i) = pByte(i) Xor sKey(i)
        Next
        Dim str As String = ASCII.GetString(enc)
        Return str
    End Function

    ''' <summary>
    ''' Base64编码
    ''' </summary>
    ''' <param name="stra">输入字符串</param>
    ''' <returns>输出字符串</returns>
    ''' <remarks></remarks>
    Function encode(ByVal stra As String) As String
        Dim r As Random = New Random()
        Dim key As Integer = r.Next(30, 255)
        Dim strb As String = ""
        Dim retstrA(2), retstr As String
        Dim bbyte() As Byte
        For Each i In stra
            strb = strb + (Asc(i) Xor key).ToString + " "
        Next
        retstrA(0) = key.ToString
        retstr = retstrA(0) + " " + strb
        strb = retstr
        For k = 1 To 5
            bbyte = ASCII.GetBytes(strb)
            strb = System.Convert.ToBase64String(bbyte)
        Next
        retstr = strb
        Return retstr
    End Function

    ''' <summary>
    ''' Base64解码
    ''' </summary>
    ''' <param name="stra">输入字符串</param>
    ''' <returns>输出字符串</returns>
    ''' <remarks></remarks>
    Function decode(ByVal stra As String) As String
        Dim strtemp(), chrtemp As String
        Dim inttemp() As Integer
        Dim bbyte() As Byte
        'Try
        For j As Integer = 1 To 5
            bbyte = System.Convert.FromBase64String(stra)
            stra = ASCII.GetString(bbyte)
        Next
        'Catch ex As Exception
        'End Try
        strtemp = stra.Split(" ")
        ReDim inttemp(UBound(strtemp))
        chrtemp = ""
        Dim key As Integer = Val(strtemp(0))
        For i As Integer = 1 To UBound(strtemp) - 1
            inttemp(i) = Val(strtemp(i))
            chrtemp = chrtemp + Chr(inttemp(i) Xor key)
        Next
        'Debug.WriteLine(chrtemp)
        Return chrtemp

    End Function

    Private Function encode_adv(ByVal stra As String) As Byte()
        Dim DesEncryptor As New TripleDESCryptoServiceProvider
    End Function

    Public Function PromoteProxy() As Boolean
        Dim r = MessageBox.Show("是否使用代理服务器？", "代理服务器", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If r = DialogResult.Yes Then
            Dim pd = ProxyDialog.ShowDialog()
            If pd.BaseResult = DialogResult.OK Then
                proxy = New WebProxy(pd.ProxyData.IP, pd.ProxyData.Port)
                proxy.BypassProxyOnLocal = True
                proxy.Credentials = New NetworkCredential(pd.ProxyData.User, pd.ProxyData.Pass)
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

#End Region

#Region "INI文件读取"

    ''' <summary>
    ''' 读取服务器IP
    ''' </summary>
    ''' <param name="dststr">返回值</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function readIPaddr(ByRef dststr As String) As String
        Dim isex As Integer
        isex = IO.File.Exists("start.ini")
        If Not isex Then
            MsgBox("无法打开Start.ini,请确定本程序已经放置于EVE Online根目录下!!!")
            exitprog()
            Return 0
        Else
            Dim str As String
            str = ""
            str = LSet(str, 256)
            Dim currentdir As String = Application.StartupPath + "\start.ini"
            NativeMethods.GetPrivateProfileString("main", "server", "", str, Len(str), currentdir)
            dststr = Left(str, InStr(str, Chr(0)) - 1)
            Return 1
        End If
    End Function

    ''' <summary>
    ''' 读取start.ini
    ''' </summary>
    ''' <param name="key">键值</param>
    ''' <param name="dststr">目标字符串</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function readini(ByVal key As String, ByRef dststr As String) As String
        Dim isex As Integer
        isex = IO.File.Exists("start.ini")
        If Not isex Then
            MsgBox("无法打开Start.ini,请确定本程序已经放置于EVE Online根目录下!!!")
            exitprog()
            Return 0
        Else
            Dim str As String
            str = ""
            str = LSet(str, 512)
            Dim currentdir As String = Application.StartupPath + "\start.ini"
            NativeMethods.GetPrivateProfileString("main", key, "", str, Len(str), currentdir)
            dststr = Left(str, InStr(str, Chr(0)) - 1)
            Return 1
        End If
    End Function

    ''' <summary>
    ''' 读取LaunchSet.ini
    ''' </summary>
    ''' <param name="key">键值</param>
    ''' <param name="dststr">目标字符串-ByRef</param>
    ''' <returns>是否读取成功</returns>
    ''' <remarks></remarks>
    ''' 
    Public Function readini2(ByVal key As String, ByRef dststr As String) As String
        Dim isex As Integer
        isex = IO.File.Exists("LaunchSET.ini")
        If Not isex Then
            MsgBox("无法打开LaunchSET.ini,请确定本程序已经放置于EVE Online根目录下!!!")
            IO.File.Create(Application.StartupPath + "\LaunchSET.ini").Dispose()
            Return 0
        Else
            Dim str, tmpstr As String
            str = ""
            str = LSet(str, 32767)
            tmpstr = ""
            Dim currentdir As String = Application.StartupPath + "\LaunchSET.ini"
            NativeMethods.GetPrivateProfileString("main", key, "", str, Len(str), currentdir)
            dststr = Left(str, InStr(str, Chr(0)) - 1)
            WriteRegistry(key, dststr)
            tmpstr = ReadRegistry(key)
            Return 1
        End If
    End Function

    ''' <summary>
    ''' 写入LaunchSet.ini
    ''' </summary>
    ''' <param name="key">键值</param>
    ''' <param name="str">内容</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function writeini(ByVal key As String, ByVal str As String) As Boolean
        Dim isex As Integer
        isex = File.Exists("LaunchSET.ini")
        If Not isex Then
            File.Create(Application.StartupPath + "\LaunchSET.ini").Close()
            'MsgBox("无法打开LaunchSET.ini,请确定本程序已经放置于EVE Online根目录下!!!")
            'Return 0
        End If
        Dim path As String
        path = Application.StartupPath + "\LaunchSET.ini"
        NativeMethods.WritePrivateProfileString("main", key, str, path)
        WriteRegistry(key, str)
    End Function

    Public Function WriteRegistry(ByVal key As String, ByVal val As String) As Boolean
        'If Content = "" Then Content = SaveFileDir
        Try
            Dim HK_SOFTWARE = Registry.LocalMachine.OpenSubKey("Software", True)

            Dim KeyBase0 = HK_SOFTWARE.OpenSubKey("PMX", True)
            If KeyBase0 Is Nothing Then
                KeyBase0 = HK_SOFTWARE.CreateSubKey("PMX")
            End If

            Dim KeyBase1 = KeyBase0.OpenSubKey("EvELogger", True)
            If KeyBase1 Is Nothing Then
                KeyBase1 = KeyBase0.CreateSubKey("EvELogger")
            End If

            KeyBase1.SetValue(key, val.Replace("\", "\\"))

            KeyBase1.Flush()
            KeyBase0.Flush()
            HK_SOFTWARE.Flush()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function ReadRegistry(ByVal Key As String) As String
        Dim tval As String
        Try
            tval = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("PMX").OpenSubKey("EvELogger").GetValue(Key)
        Catch ex As Exception
            tval = ""
        End Try
        Return tval
    End Function

#End Region

#Region "登录控制"

    ''' <summary>
    ''' 获取账户信息
    ''' </summary>
    ''' <returns>Accounts info array</returns>
    ''' <remarks>Account_Names=123|123|123|234|345
    '''          Passwords=abc|bcd|nil|bcd|bcd</remarks>
    Public Function FillAccountsInfo() As AccountInfo()
        encode_adv("")
        Dim sSrcStr, sSrcPwd As String
        Dim sUserStr(), sPwdStr() As String
        Dim RetArray(0) As AccountInfo
        sSrcStr = ""
        sSrcPwd = ""
        readini2("Account_Names", sSrcStr)
        readini2("Passwords", sSrcPwd)
        sUserStr = sSrcStr.Split("|")
        sPwdStr = sSrcPwd.Split("|")
        If UBound(sUserStr) > 0 Then
            ReDim RetArray(sUserStr.Length)
            For i As Integer = 0 To UBound(sUserStr)
                RetArray(i + 1).user = sUserStr(i)
                RetArray(i + 1).password_encrypted = sPwdStr(i)
            Next
            Return RetArray
        Else
            If sUserStr(0) <> Nothing Then
                ReDim RetArray(1)
                RetArray(1).user = sUserStr(0)
                RetArray(1).password_encrypted = sPwdStr(0)
            End If
        End If
        Try
            If RetArray(RetArray.Length).user = Nothing Then
                ReDim RetArray(RetArray.Length - 1)
            End If
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
        Return RetArray
    End Function

    ''' <summary>
    ''' 判断是否需要输入验证码
    ''' </summary>
    ''' <param name="Username"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function isRequestCaptcha(ByVal Username As String) As Boolean
        Return False
        Dim jdownloader As WebClient = New WebClient
        If useProxy Then
            jdownloader.Proxy = proxy
        End If

        Dim xrandnum As Random = New Random()
        Dim randString As String = ""
        Dim timeValue = DateAndTime.Now

        For i As Integer = 0 To 20
            randString += ZToN(xrandnum.Next(10))
        Next
        Dim timeValueText As String = timeValue.Ticks.ToString.Substring(5)
        Dim ReqAddr As String = captchaURLHeader + randString + "_" + timeValueText + "&fid=100&uid=" + Username + "&_=" + timeValueText
        Dim jcontent As String = jdownloader.DownloadString(ReqAddr)
        If jcontent.IndexOf("{""R"":""0""}") > 1 Then
            GUID = ""
            captcha = ""
            Return False
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' 获得GUID
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getGUID() As String
        Dim xweb As WebClient = New WebClient
        Dim GUIDString = xweb.DownloadString(GUIDURL)
        xweb.Dispose()
        Return GUIDString
        xweb.Dispose()
    End Function

    ''' <summary>
    ''' 尝试登录过程
    ''' </summary>
    ''' <param name="username"></param>
    ''' <param name="password"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TryLogin(ByVal username As String, ByVal password As String) As String()
        '获取Launcher Token
        Dim param As String = "UserName=" + username + "&Password=" + password + "&CaptchaToken=" + Guid + "&Captcha=" + captcha
        Dim hArr() As Headers = {New Headers(UserAgentHeader, UserAgentValue), New Headers("Accept-Language", "zh-cn,zh;q=0.8,en-us;q=0.5,en;q=0.3")}
        Dim WebResponse As htmlDocumentResponse
        Dim innerHTML As String
        Dim respURI As String
        Dim LINFO(2) As String
        '错误处理
        Try
            WebResponse = GetWebResponse(LoginAddr, True, True, , , param, "", hArr)
            innerHTML = WebResponse.iHTML
            respURI = WebResponse.uri
        Catch ex As Exception
            LINFO(0) = "0"
            LINFO(1) = ex.Message
            Return LINFO
        End Try

        Dim tmpCkStr As String = ""
        For i = 0 To WebResponse.iCookies.Count - 1
            Dim t = WebResponse.iCookies(i)
            tmpCkStr += t.Name + "=" + t.Value
        Next

        If innerHTML.IndexOf("eula") >= 0 Then
            Dim decl_1 As String = "eulaHash"
            Dim decl_2 As String = "returnUrl"
            Dim st_1 As Integer = innerHTML.IndexOf(decl_1) + decl_1.Length + 39
            Dim st_2 As Integer = innerHTML.IndexOf(decl_2) + decl_2.Length + 40
            Dim end_1 As Integer = innerHTML.IndexOf("""", st_1)
            Dim end_2 As Integer = innerHTML.IndexOf("""", st_2)
            Dim QueryStringPost As String = "eulaHash=" + innerHTML.Substring(st_1, end_1 - st_1) + "&returnUrl=" + innerHTML.Substring(st_2, end_2 - st_2) + "&action=%E6%8E%A5%E5%8F%97"
            Dim newResponse As htmlDocumentResponse
            Dim NCKStr As String = CookieStr.Split(";")(0) + "; UserNames=" + username + "; path=/; secure; HttpOnly"
            Dim currCk As String = CookieStr
            CookieStr = NCKStr
            newResponse = GetWebResponse("https://auth.eve-online.com.cn/oauth/Eula", True, True, , , QueryStringPost)
            LINFO(0) = "3"
            LINFO(1) = "Restart Login Process."
            Return LINFO
        End If

        If innerHTML.IndexOf("TwoFactorAuthenticationType") > 0 Then
            Dim firstindex As Integer = innerHTML.IndexOf("<div class=""block"">") + "<div class=""block"">".Length
            Dim firstindexEnd As Integer = innerHTML.IndexOf("<input", firstindex)
            Dim secondindex As Integer = innerHTML.IndexOf("<p>", firstindexEnd) + "<p>".Length
            Dim secondindexEnd As Integer = innerHTML.IndexOf("</p>", secondindex)
            Dim thirdIndex As Integer = innerHTML.IndexOf("<div class=""block"">", secondindexEnd) + "<div class=""block"">".Length
            Dim thirdIndexEnd As Integer = innerHTML.IndexOf("<input", thirdIndex)
            Dim LocStr1 As String = innerHTML.Substring(firstindex, firstindexEnd - firstindex).Trim
            Dim LocStr2 As String = innerHTML.Substring(secondindex, secondindexEnd - secondindex).Trim
            Dim locStr3 As String = innerHTML.Substring(thirdIndex, thirdIndexEnd - thirdIndex).Trim
            Dim sCardForm As New SecCard(LocStr1, LocStr2, locStr3)
            Dim sCardResult = sCardForm.ShowDialog()
            If sCardResult = DialogResult.OK Then
                Dim QueryString As String = "TwoFactorAuthenticationType=MatrixCard&MatrixCardOne=" + sCardForm.Value1 + "&MatrixCardTwo=" + sCardForm.Value2 + "&MatrixCardThree=" + sCardForm.Value3
                WebResponse = GetWebResponse(PassCardPostBackURI, True, True,,, QueryString)
                innerHTML = WebResponse.iHTML
                respURI = WebResponse.uri
            Else
                LINFO(0) = "4"
                LINFO(1) = "未正确输入密保卡信息，退出登录"
                Return LINFO
            End If
        End If

        If innerHTML.IndexOf("已登录") > 0 Then
            Dim st, ed As Integer
            Dim ss As String = New String("")
            st = respURI.IndexOf("access_token=") + Len("access_token=")
            ed = respURI.IndexOf("&token_type") - 1
            For i As Integer = st To ed
                ss += respURI(i)
            Next
            '获取客户端Client Token
            If innerHTML.IndexOf("Bearer") > 0 Or respURI.IndexOf("Bearer") > 0 Then
                ReDim Preserve LINFO(3)
                Dim ClientToken As String
                '如果网络出错
                Try
                    ClientToken = GetClientToken(ss)
                Catch ex As Exception
                    ClientToken = "Error:" + ex.Message
                End Try
                If ClientToken.StartsWith("Error") Then
                    LINFO(0) = "0"
                    LINFO(1) = ClientToken
                End If
                LINFO(0) = "2"
                LINFO(1) = ss
                LINFO(2) = ClientToken
            Else
                LINFO(0) = "1"
                LINFO(1) = ss
            End If
        Else
            '登录报错处理
            Dim sp As Integer = innerHTML.IndexOf(vFailString) + vFailString.Length
            Dim endp As Integer = innerHTML.IndexOf("</div>", sp)
            Dim info As String = innerHTML.Substring(sp, endp - sp).Replace("<span>", "").Replace("</span>", "").Replace("<ul>", "").Replace("<li>", vbCrLf).Replace("</li>", "").Replace("</ul>", "")
            LINFO(0) = "0"
            LINFO(1) = info
        End If
        Return LINFO
    End Function

    ''' <summary>
    ''' 获取客户端Token
    ''' </summary>
    ''' <param name="BearerToken">获得的远端Token</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetClientToken(ByVal BearerToken As String) As String
        Dim url = "https://auth.eve-online.com.cn//launcher/token?accesstoken=" + BearerToken
        Dim headerArray() As Headers = {New Headers(UserAgentHeader, UserAgentValue)}
        Dim Resp As htmlDocumentResponse = GetWebResponse(url, False, False, , , , , headerArray)
        Dim uri = Resp.uri
        Dim innerHTML = Resp.iHTML
        Dim st, ed As Integer
        Dim ss As String = New String("")
        st = innerHTML.IndexOf("access_token=") + Len("access_token=")
        ed = innerHTML.IndexOf("&", st) - 1
        For i As Integer = st To ed
            ss += innerHTML(i)
        Next
        Return ss
    End Function

    ''' <summary>
    ''' 获取Web页面信息和URI
    ''' </summary>
    ''' <param name="uri">获取Web页面信息和URI</param>
    ''' <param name="Method">访问模式，True=POST;False=GET</param>
    ''' <param name="AllowRedirect">是否允许重定向</param>
    ''' <param name="SendEncodingStr">(可选)发送字符串编码</param>
    ''' <param name="ReturnEncodingStr">(可选)接收字符串编码</param>
    ''' <param name="POSTData">(可选)Post参数串</param>
    ''' <param name="AcceptStr">(可选)Accept类型字符串</param>
    ''' <param name="HeadersArray">(可选)其它HTTP头</param>
    ''' <returns>返回页面的HTML代码和URI</returns>
    ''' <remarks></remarks>
    Private Function GetWebResponse(ByVal uri As String, _
                                    ByVal Method As Boolean, _
                                    ByVal AllowRedirect As Boolean, _
                                    Optional SendEncodingStr As String = "ASCII", _
                                    Optional ReturnEncodingStr As String = "UTF-8", _
                                    Optional POSTData As String = "", _
                                    Optional AcceptStr As String = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8", _
                                    Optional ByVal HeadersArray() As Headers = Nothing) As htmlDocumentResponse
        Dim webReq As HttpWebRequest = HttpWebRequest.Create(uri)
        If useProxy Then
            webReq.Proxy = proxy
        End If
        SetCookieHeaders(webReq, CookieStr)
        With webReq
            .Accept = AcceptStr
            If Not IsNothing(HeadersArray) Then
                For Each it In HeadersArray
                    If Not IsNothing(it.head) Then .Headers.Add(it.head, it.cont)
                Next
            Else
                .Headers.Add(UserAgentHeader, UserAgentValue)
            End If
            .AllowAutoRedirect = AllowRedirect
        End With
        If Method = True Then
            Dim bytes() = Encoding.GetEncoding(SendEncodingStr).GetBytes(POSTData)
            With webReq
                .Method = "POST"
                .ContentType = "application/x-www-form-urlencoded"
                .ContentLength = POSTData.Length
                Dim reqstream As Stream = .GetRequestStream()
                reqstream.Write(bytes, 0, bytes.Length)
                reqstream.Flush()
            End With
        ElseIf Method = False Then
            With webReq
                .Method = "GET"
            End With
        End If
        Dim sData As Stream
        Dim webResp As HttpWebResponse = webReq.GetResponse()
        sData = webResp.GetResponseStream()
        Dim innerHTML = New StreamReader(sData, Encoding.GetEncoding(ReturnEncodingStr)).ReadToEnd()
        sData.Close()
        Dim RetValue As htmlDocumentResponse
        RetValue.uri = webResp.ResponseUri.AbsoluteUri
        RetValue.iHTML = innerHTML
        RetValue.iCookies = webResp.Cookies
        Return RetValue
    End Function

    ''' <summary>
    ''' 获得Session和登录地址
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Sub GetASPNetSessionIDAndLoginAddr()
        On Error GoTo Stt
Stt:
        Dim wDownloader As WebClient = New WebClient
        wDownloader.Headers.Add(PostContentHeader, PostContentType)
        wDownloader.Headers.Add(UserAgentHeader, UserAgentHeader)
        Dim innerHTML As String = wDownloader.UploadString(LoginPageURI, "POST", "")
        Dim cookieKeys As String() = wDownloader.ResponseHeaders.AllKeys
        For i As Integer = 0 To UBound(cookieKeys)
            If cookieKeys(i).Equals("Set-Cookie") Then
                CookieStr = wDownloader.ResponseHeaders.Get(i)
            End If
        Next
        Dim startp As Integer = innerHTML.IndexOf("action=") + 8
        Dim endp As Integer = innerHTML.IndexOf("""", startp)
        LoginAddr = "https://auth.eve-online.com.cn" + innerHTML.Substring(startp, endp - startp)
        wDownloader.Dispose()

    End Sub

    ''' <summary>
    ''' 设定登录Cookie
    ''' </summary>
    ''' <param name="hClient"></param>
    ''' <param name="CookieString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Sub SetCookieHeaders(ByRef hClient As HttpWebRequest, ByVal CookieString As String)
        Dim CookieParams() As String = CookieString.Split(";")
        hClient.CookieContainer = New CookieContainer()
        Dim addr As New Uri("https://auth.eve-online.com.cn/")
        For i As Integer = 0 To UBound(CookieParams)
            Dim CookItem() As String = CookieParams(i).Split("=")
            Dim CK As Cookie
            If UBound(CookItem) >= 1 Then
                If CookItem(0).IndexOf("path") >= 0 Or CookItem(0).IndexOf("secure") >= 0 Then Continue For
                CK = New Cookie(CookItem(0).Trim().Replace(",", "%2c"), CookItem(1).Trim().Replace(",", "%2c"))
                hClient.CookieContainer.Add(addr, CK)
            End If
        Next
    End Sub

#End Region

#Region "异常终止处理"

    ''' <summary>
    ''' debug时不显示MessageBox
    ''' </summary>
    ''' <remarks></remarks>
#If DEBUG Then
    Public Function msgbox(ByVal s As String, Optional ByVal o1 As Object = Nothing, Optional ByVal o2 As Object = Nothing)
        Debug.WriteLine(s)
    End Function
#End If
    Dim msgshown As Boolean = False
    ''' <summary>
    ''' 退出提示的参数
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub exitprog()
        msgshown = True
        MsgBox(My.Resources.MSG_CH)
        If Debugger.IsAttached = True Then
            Exit Sub
        End If

        Application.Exit()
    End Sub

#End Region

End Module
