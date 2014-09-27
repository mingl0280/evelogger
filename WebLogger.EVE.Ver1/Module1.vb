Imports System.IO
Imports System.Text
Imports System.Threading
Imports System.Text.Encoding
Imports System.Data
Imports System.Net
Imports System.Web
Imports System.Windows.Forms
Imports System.Security.Cryptography
Imports System.Convert
Imports System.Globalization
Imports System.Math
Imports System.Runtime.InteropServices

Module Module1

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
    Structure AccountInfo
        Dim user As String
        Dim password_encrypted As String
    End Structure

    ''' <summary>
    ''' 定义全局存储账户信息数组
    ''' </summary>
    ''' <remarks></remarks>
    Public aAccounts As AccountInfo()

    Public GUID, captcha As String
    Public CookieStr, LoginAddr As String

    ''' <summary>
    ''' 常量定义
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ZToN As String = "0123456789"
    Public Const LoginPageURI As String = "https://auth.eve-online.com.cn/Account/LogOn?ReturnUrl=%2foauth%2fauthorize%3fclient_id%3deveclient%26scope%3deveClientLogin%26response_type%3dtoken%26redirect_uri%3dhttps%253A%252F%252Fauth.eve-online.com.cn%252Flauncher%253Fclient_id%253Deveclient%26lang%3dzh%26mac%3dNone&client_id=eveclient&scope=eveClientLogin&response_type=token&redirect_uri=https%3A%2F%2Fauth.eve-online.com.cn%2Flauncher%3Fclient_id%3Deveclient&lang=zh&mac=None"
    Public Const PostContentType As String = "application/x-www-form-urlencoded"
    Public Const PostContentHeader As String = "Content-Type"
    Public Const UserAgentHeader As String = "UserAgent"
    Public Const UserAgentValue As String = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:32.0) Gecko/20100101 Firefox/32.0"
    Public Const captchaURLHeader As String = "https://captcha.tiancity.com:442/CheckSwitch.ashx?jsoncallback=jQuery"
    Public Const GUIDURL As String = "https://auth.eve-online.com.cn/Account/GenerateGuid/"
    Public Const captchaImageURL As String = "https://captcha.tiancity.com:442/getimage.ashx?tid="
    Public Const vFailString As String = "<div class=""validation-summary-errors"">"

#End Region

#Region "数据处理"

    ''' <summary>
    ''' 验证客户端版本号
    ''' </summary>
    ''' <param name="i">返回值</param>
    ''' <remarks></remarks>
    Sub ValidClientVersion(ByRef i As Integer)
        Dim dnf As WebClient = New WebClient
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
        Return chrtemp
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
            Dim str As String
            str = ""
            str = LSet(str, 32767)
            Dim currentdir As String = Application.StartupPath + "\LaunchSET.ini"
            NativeMethods.GetPrivateProfileString("main", key, "", str, Len(str), currentdir)
            dststr = Left(str, InStr(str, Chr(0)) - 1)
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
            File.Create(Application.StartupPath + "\LaunchSET.ini")
            'MsgBox("无法打开LaunchSET.ini,请确定本程序已经放置于EVE Online根目录下!!!")
            'Return 0
        Else
            Dim path As String
            path = Application.StartupPath + "\LaunchSET.ini"
            NativeMethods.WritePrivateProfileString("main", key, str, path)
        End If
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
        Dim jdownloader As WebClient = New WebClient
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
            Guid = ""
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
        Dim webReq As WebRequest = HttpWebRequest.Create(LoginAddr)
        Dim param As String = "UserName=" + username + "&Password=" + password + "&CaptchaToken=" + Guid + "&Captcha=" + captcha
        Dim bytes() = Encoding.ASCII.GetBytes(param)
        With webReq
            SetCookieHeaders(webReq, CookieStr)
            .Method = "POST"
            .ContentType = "application/x-www-form-urlencoded"
            .Headers.Add("Accept-Language", "zh-cn,zh;q=0.8,en-us;q=0.5,en;q=0.3")
            .ContentLength = param.Length
            .Headers.Add(UserAgentHeader, UserAgentValue)
            Dim reqStream As Stream = webReq.GetRequestStream()
            reqStream.Write(bytes, 0, bytes.Length)
            reqStream.Flush()
        End With
        Dim sData As Stream
        Dim webResp As HttpWebResponse = webReq.GetResponse()
        sData = webResp.GetResponseStream()
        Dim innerHTML = New StreamReader(sData, Encoding.GetEncoding("UTF-8")).ReadToEnd()
        sData.Close()
        Dim respURI = webResp.ResponseUri.AbsoluteUri
        Dim LINFO(2) As String

        If innerHTML.IndexOf("已登录") > 0 Then
            Dim st, ed As Integer
            Dim ss As String = New String("")
            st = respURI.IndexOf("access_token=") + Len("access_token=")
            ed = respURI.IndexOf("&token_type") - 1
            For i As Integer = st To ed
                ss += respURI(i)
            Next
            LINFO(0) = "1"
            LINFO(1) = ss
        Else
            Dim sp As Integer = innerHTML.IndexOf(vFailString) + vFailString.Length
            Dim endp As Integer = innerHTML.IndexOf("</div>", sp)
            Dim info As String = innerHTML.Substring(sp, endp - sp).Replace("<span>", "").Replace("</span>", "").Replace("<ul>", "").Replace("<li>", vbCrLf).Replace("</li>", "").Replace("</ul>", "")
            LINFO(0) = "0"
            LINFO(1) = info
        End If
        Return LINFO
    End Function

    ''' <summary>
    ''' 获得Session和登录地址
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetASPNetSessionIDAndLoginAddr() As String
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
    End Function

    ''' <summary>
    ''' 设定登录Cookie
    ''' </summary>
    ''' <param name="hClient"></param>
    ''' <param name="CookieString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetCookieHeaders(ByRef hClient As HttpWebRequest, ByVal CookieString As String)
        Dim CookieParams() As String = CookieString.Split(";")
        hClient.CookieContainer = New CookieContainer()
        Dim addr As New Uri("https://auth.eve-online.com.cn/")
        For i As Integer = 0 To UBound(CookieParams)
            Dim CookItem() As String = CookieParams(i).Split("=")
            Dim CK As Cookie
            If UBound(CookItem) >= 1 Then
                If CookItem(0).IndexOf("path") >= 0 Or CookItem(0).IndexOf("secure") >= 0 Then Continue For
                CK = New Cookie(CookItem(0).Replace(",", ""), CookItem(1).Replace(",", ""))
                hClient.CookieContainer.Add(addr, CK)
            End If
        Next
    End Function

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
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function exitprog()
        Dim ccul, chcul As CultureInfo
        ccul = CultureInfo.CurrentCulture
        chcul = New CultureInfo("zh-cn")
        msgshown = True
        If ccul.Name = chcul.Name And Not msgshown Then
            MsgBox(My.Resources.MSG_CH)
        Else
            MsgBox(My.Resources.MSG_EN)
        End If
        If Debugger.IsAttached Then
            msgshown = True
            Exit Function
        End If
        Application.Exit()
    End Function

#End Region

End Module
