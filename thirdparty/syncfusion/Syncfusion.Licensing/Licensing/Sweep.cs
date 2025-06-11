// Decompiled with JetBrains decompiler
// Type: Syncfusion.Licensing.Sweep
// Assembly: Syncfusion.Licensing, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=632609b4d040f6b4
// MVID: 46253E3A-52AF-49F3-BF00-D811A33B7BC6
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Licensing.dll

using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

#nullable disable
namespace Syncfusion.Licensing;

internal class Sweep
{
  internal static string ErrorMessage = "";
  internal static string assemblyName;
  internal ulong ver1;
  internal ulong ver2;
  internal ulong ver3;
  internal static string UK;
  internal static string s1 = "";
  private ulong rt;
  private DateTime expire = DateTime.Now;

  public void InitAssemblyInfo(Type t)
  {
    this.GetAssemblyInformation(t, ref this.ver1, ref this.ver2, ref this.ver3, ref Sweep.assemblyName);
  }

  public ulong GetRTResultForIsKeyValid2() => this.rt;

  public DateTime GetExpireResultForIsKeyValid2() => this.expire;

  private bool CheckCallStack(Type type, LicenseContext context)
  {
    if (context == null)
      return false;
    Sweep.UK = "INTERNAL";
    StackTrace stackTrace = new StackTrace(3, true);
    int index = 0;
    string str1 = type.Assembly.FullName;
    object[] customAttributes = type.Assembly.GetCustomAttributes(false);
    string newValue = "";
    bool flag1 = false;
    if (str1.StartsWith("Syncfusion."))
    {
      foreach (object obj in customAttributes)
      {
        if (obj.GetType() == typeof (AssemblyDefaultAliasAttribute))
        {
          newValue = ((AssemblyDefaultAliasAttribute) obj).DefaultAlias.Replace(".dll", "");
          if (newValue.StartsWith("Syncfusion."))
          {
            flag1 = true;
            break;
          }
          break;
        }
      }
    }
    if (flag1)
      str1 = str1.Replace(str1.Substring(0, str1.IndexOf(",")), newValue);
    for (StackFrame frame = stackTrace.GetFrame(index); frame != null; frame = stackTrace.GetFrame(index))
    {
      MethodBase method = frame.GetMethod();
      if (method != (MethodBase) null)
      {
        string str2 = "";
        if (method.ReflectedType != (Type) null)
        {
          try
          {
            str2 = method.ReflectedType.Assembly.FullName;
            if (this.GetEncLic(method.ReflectedType.Assembly, type, context))
              return true;
          }
          catch
          {
          }
          bool flag2 = false;
          if (str2.StartsWith("Syncfusion."))
          {
            foreach (object obj in customAttributes)
            {
              if (obj.GetType() == typeof (AssemblyDefaultAliasAttribute))
              {
                newValue = ((AssemblyDefaultAliasAttribute) obj).DefaultAlias.Replace(".dll", "");
                if (newValue.StartsWith("Syncfusion."))
                {
                  flag2 = true;
                  break;
                }
                break;
              }
            }
          }
          if (flag2)
            str2 = str2.Replace(str2.Substring(0, str2.IndexOf(",")), newValue);
          if (str2 != str1 && method.ReflectedType.Assembly.FullName.StartsWith("Syncfusion.") && !method.ReflectedType.Assembly.FullName.StartsWith("Syncfusion.Core") && method.Name != "GetLicense")
            return true;
        }
      }
      ++index;
    }
    return false;
  }

  private void GetAssemblyInformation(
    Type type,
    ref ulong ver1,
    ref ulong ver2,
    ref ulong ver3,
    ref string assemblyname)
  {
    bool flag = false;
    foreach (object customAttribute in type.Assembly.GetCustomAttributes(false))
    {
      if (customAttribute.GetType() == typeof (AssemblyDefaultAliasAttribute))
      {
        Sweep.assemblyName = ((AssemblyDefaultAliasAttribute) customAttribute).DefaultAlias;
        flag = true;
      }
      if (customAttribute.GetType() == typeof (AssemblyInformationalVersionAttribute))
      {
        string informationalVersion = ((AssemblyInformationalVersionAttribute) customAttribute).InformationalVersion;
        ver1 = Convert.ToUInt64(informationalVersion.Substring(0, informationalVersion.IndexOf(".")));
        string str1 = informationalVersion.Substring(informationalVersion.IndexOf(".") + 1);
        ver2 = Convert.ToUInt64(str1.Substring(0, str1.IndexOf(".")));
        string str2 = str1.Substring(str1.IndexOf(".") + 1);
        ver3 = Convert.ToUInt64(str2.Substring(0, str2.IndexOf(".")));
      }
    }
    if (flag)
      return;
    Sweep.assemblyName = type.Assembly.FullName;
    int startIndex = Sweep.assemblyName.IndexOf("Version=") + 8;
    string str3 = Sweep.assemblyName.Substring(startIndex);
    ver1 = Convert.ToUInt64(str3.Substring(0, str3.IndexOf(".")));
    string str4 = str3.Substring(str3.IndexOf(".") + 1);
    ver2 = Convert.ToUInt64(str4.Substring(0, str4.IndexOf(".")));
    string str5 = str4.Substring(str4.IndexOf(".") + 1);
    ver3 = Convert.ToUInt64(str5.Substring(0, str5.IndexOf(".")));
    int length = Sweep.assemblyName.IndexOf(",");
    Sweep.assemblyName = Sweep.assemblyName.Substring(0, length);
  }

  private Stream GetManifestResourceStream(Assembly satellite, string name)
  {
    CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;
    foreach (string manifestResourceName in satellite.GetManifestResourceNames())
    {
      if (compareInfo.Compare(manifestResourceName, name, CompareOptions.IgnoreCase) == 0)
      {
        name = manifestResourceName;
        break;
      }
    }
    return satellite.GetManifestResourceStream(name);
  }

  public static string ByteArray2String(byte[] bs)
  {
    StringBuilder stringBuilder = new StringBuilder();
    for (int index = 0; index < bs.Length; ++index)
      stringBuilder.Append(Convert.ToChar(bs[index]));
    return stringBuilder.ToString();
  }

  internal static void GetS1()
  {
    if (!(Sweep.s1 == ""))
      return;
    Sweep.s1 = "syncfusion.core,0;1;2;3;4;5;6;7;8;9;10;11;12;13;14;15;16;17;18;19;20;21;22;23;24;25;26;27;28;29;30;31;32;33;34|syncfusion.shared,0;1;2;3;4;5;6;7;8;9;10;11;12;13;14;15;16;17;18;19;20;21|syncfusion.aspnet Binary,0|syncfusion.aspnet Source,1|syncfusion.aspnetmvc Binary,2|syncfusion.aspnetmvc Source,3|syncfusion.silverlight Binary,4|syncfusion.silverlight Source,5|syncfusion.windowsforms Binary,6|syncfusion.windowsforms Source,7|syncfusion.windowsappsplatform Binary,8|syncfusion.windowsappsplatform Source,9|syncfusion.wpf Binary,10|syncfusion.wpf Source,11|syncfusion.javascript Binary,12|syncfusion.javascript Source,13|syncfusion.lightswitch Binary,14|syncfusion.fileformats Binary,15|syncfusion.fileformats Source,16|syncfusion.xamarin Binary,17|syncfusion.xamarin Source,18|syncfusion.lightswitch Source,19|syncfusion.android Binary,20|syncfusion.android Source,21|syncfusion.reportplatform Binary,22|syncfusion.dashboardplatform Binary,23|syncfusion.dashboardplatformsdk Binary,24|syncfusion.reportplatformsdk Binary,25|syncfusion.aspnetcore Binary,26|syncfusion.aspnetcore Source,27|syncfusion.php Binary,28|syncfusion.php Source,29|syncfusion.jsp Binary,30|syncfusion.jsp Source,31|syncfusion.flutter Binary,32|syncfusion.flutter Source,33|syncfusion.blazor Binary,34|syncfusion.blazor Source,35|syncfusion.winui Binary,36|syncfusion.winui Source,37|syncfusion.metrostudio Binary,38";
  }

  public string GetErrorMessage() => Sweep.ErrorMessage;

  public bool GetEncLic(Assembly asm, Type type, LicenseContext context)
  {
    try
    {
      string name = asm.GetName(false).Name;
      string str1 = asm.GetName(false).CodeBase.Replace("file:///", "").Replace("/", "\\");
      int num = str1.LastIndexOf("\\", StringComparison.CurrentCulture);
      string str2 = str1.Remove(num + 1, str1.Length - (num + 1));
      StreamReader streamReader = (StreamReader) null;
      try
      {
        if (str2.ToLower().IndexOf($"gac\\{Sweep.assemblyName.ToLower()}\\{this.ver1.ToString()}.{this.ver2.ToString()}.{this.ver3.ToString()}", StringComparison.CurrentCulture) > -1)
        {
          streamReader = new StreamReader(str2 + "__assemblyinfo__.ini");
          string lower = streamReader.ReadToEnd().ToLower();
          int startIndex = lower.IndexOf("url=file:///", StringComparison.CurrentCulture) + "url=file:///".Length;
          int length = lower.LastIndexOf(Sweep.assemblyName.ToLower() + ".dll", StringComparison.CurrentCulture) - startIndex;
          str2 = lower.Substring(startIndex, length);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
      finally
      {
        streamReader?.Close();
      }
      string str3 = context.UsageMode != LicenseUsageMode.Runtime ? str2 + "sl.slf" : str2 + "sr.slf";
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.ToString());
    }
    return false;
  }

  private static string DecryptString(string encrString)
  {
    try
    {
      return Encoding.ASCII.GetString(Convert.FromBase64String(encrString));
    }
    catch
    {
      throw;
    }
  }

  private static bool Compare(int[] _prod, string assembly)
  {
    if (assembly.ToLower().Contains("web"))
      assembly = "syncfusion.aspnet";
    else if (assembly.ToLower().Contains("mvc") && !assembly.ToLower().Contains("mobile"))
      assembly = "syncfusion.aspnetmvc";
    else if (assembly.ToLower().Contains("silverlight"))
      assembly = "syncfusion.silverlight";
    else if (assembly.ToLower().Contains("windows"))
      assembly = "syncfusion.windowsforms";
    else if (assembly.ToLower().Contains("phone"))
      assembly = "syncfusion.windowsphone";
    else if (assembly.ToLower().Contains("wpf"))
      assembly = "syncfusion.wpf";
    else if (assembly.ToLower().Contains("javascript"))
      assembly = "syncfusion.javascript";
    else if (assembly.ToLower().Contains("lightswitch"))
      assembly = "syncfusion.lightwsitch";
    else if (assembly.ToLower().Contains("fileformats"))
      assembly = "syncfusion.fileformats";
    else if (assembly.ToLower().Contains("xamarin"))
      assembly = "syncfusion.xamarin";
    else if (assembly.ToLower().Equals("bigdata"))
      assembly = "syncfusion.bigdata";
    else if (assembly.ToLower().Contains("bigdataplus"))
      assembly = "syncfusion.bigdataplus";
    else if (assembly.ToLower().Contains("android"))
      assembly = "syncfusion.android";
    else if (assembly.ToLower().Contains("dashboardplatform") && !assembly.ToLower().Contains("sdk"))
      assembly = "syncfusion.dashboardplatform";
    else if (assembly.ToLower().Contains("dashboardplatformsdk"))
      assembly = "syncfusion.dashboardplatformsdk";
    else if (assembly.ToLower().Contains("reportplatformsdk"))
      assembly = "syncfusion.reportplatformsdk";
    else if (assembly.ToLower().Contains("universalwindows"))
      assembly = "syncfusion.universalwindows";
    else if (assembly.ToLower().Contains("jsp"))
      assembly = "syncfusion.jsp";
    else if (assembly.ToLower().Contains("flutter"))
      assembly = "syncfusion.flutter";
    else if (assembly.ToLower().Contains("blazor"))
      assembly = "syncfusion.blazor";
    else if (assembly.ToLower().Contains("winui"))
      assembly = "syncfusion.winui";
    string s1 = Sweep.s1;
    char[] chArray1 = new char[1]{ '|' };
    foreach (string str1 in s1.Split(chArray1))
    {
      char[] chArray2 = new char[1]{ ',' };
      string[] strArray = str1.Split(chArray2);
      if (assembly.ToLower().IndexOf(strArray[0].ToLower().Replace(" source", "").Replace(" binary", "")) > -1)
      {
        string str2 = strArray[1];
        char[] chArray3 = new char[1]{ ';' };
        foreach (string str3 in str2.Split(chArray3))
        {
          if (_prod[Convert.ToUInt64(str3)] == 1)
            return true;
        }
      }
    }
    return false;
  }

  internal static bool CheckLicense(
    int ver1,
    int ver2,
    int ver3,
    string pszKey,
    string pszEmail,
    ref string pszAssembly,
    int unlock,
    ref IntPtr t,
    ref IntPtr a,
    ref IntPtr b,
    ref IntPtr c)
  {
    return false;
  }

  internal static string[] _Find_UnlockKey()
  {
    StringCollection sc = new StringCollection();
    Sweep.GetValuesFromKey(Sweep.TryGetLegacyKey(), sc);
    Sweep.GetValuesFromKey(Sweep.TryGetCurrentUserKey(), sc);
    Sweep.GetValuesFromKey(Sweep.TryGetLocalMachineKey(), sc);
    string[] array = new string[sc.Count];
    sc.CopyTo(array, 0);
    return array;
  }

  private static void GetValuesFromKey(RegistryKey hKey, StringCollection sc)
  {
    if (hKey == null)
      return;
    string[] valueNames = hKey.GetValueNames();
    hKey.Close();
    if (valueNames == null || valueNames.GetLength(0) <= 0)
      return;
    sc.AddRange(valueNames);
  }

  internal static RegistryKey TryGetLegacyKey()
  {
    RegistryKey legacyKey = (RegistryKey) null;
    try
    {
      legacyKey = Registry.ClassesRoot.OpenSubKey("CLSID\\{f82541ac-3346-4ffc-b975-a33807feec24}\\sf");
    }
    catch (Exception ex)
    {
    }
    return legacyKey;
  }

  internal static RegistryKey TryGetCurrentUserKey()
  {
    RegistryKey currentUserKey = (RegistryKey) null;
    try
    {
      currentUserKey = Registry.CurrentUser.OpenSubKey("Software\\Syncfusion\\Essential Suite\\InstalledVersions\\keys");
    }
    catch (Exception ex)
    {
    }
    return currentUserKey;
  }

  internal static RegistryKey TryGetLocalMachineKey()
  {
    RegistryKey localMachineKey = (RegistryKey) null;
    try
    {
      localMachineKey = Registry.LocalMachine.OpenSubKey("Software\\Syncfusion\\Essential Suite\\InstalledVersions\\keys");
    }
    catch (Exception ex)
    {
    }
    return localMachineKey;
  }

  public static void GetProdArray(ulong wProds, ref int[] _prod)
  {
    _prod = new int[64 /*0x40*/];
    if (((long) wProds & 1L) == 1L)
      _prod[0] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[1] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[2] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[3] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[4] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[5] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[6] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[7] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[8] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[9] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[10] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[11] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[12] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[13] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[14] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[15] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[16 /*0x10*/] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[17] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[18] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[19] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[20] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[21] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[22] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[23] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[24] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[25] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[26] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[27] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[28] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[29] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[30] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[31 /*0x1F*/] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[32 /*0x20*/] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[33] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[34] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[35] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) == 1L)
      _prod[36] = 1;
    wProds >>= 1;
    if (((long) wProds & 1L) != 1L)
      return;
    _prod[37] = 1;
  }

  private static int ParseHexDigit(char c)
  {
    if (c >= '0' && c <= '9')
      return (int) c - 48 /*0x30*/;
    if (c >= 'a' && c <= 'f')
      return (int) c - 97 + 10;
    if (c >= 'A' && c <= 'F')
      return (int) c - 65 + 10;
    throw new ArgumentException("Invalid   hex     char  acter");
  }

  private static string ParseHex(string hex)
  {
    char[] chArray1 = new char[hex.Length / 2];
    int num1 = 0;
    for (int index1 = 0; index1 < chArray1.Length; ++index1)
    {
      char[] chArray2 = chArray1;
      int index2 = index1;
      string str1 = hex;
      int index3 = num1;
      int num2 = index3 + 1;
      int num3 = Sweep.ParseHexDigit(str1[index3]) * 16 /*0x10*/;
      string str2 = hex;
      int index4 = num2;
      num1 = index4 + 1;
      int hexDigit = Sweep.ParseHexDigit(str2[index4]);
      int num4 = (int) (ushort) (num3 + hexDigit);
      chArray2[index2] = (char) num4;
    }
    return new string(chArray1);
  }

  private static int Checksum(string str)
  {
    char[] charArray = str.ToLower().ToCharArray();
    int num = 0;
    foreach (char ch in charArray)
      num += (int) Convert.ToInt16(ch);
    return num * str.Length;
  }

  public static string[] GetUnlockTextInformation()
  {
    UnlockKeyInfo[] unlockKeyInfos = Sweep.GetUnlockKeyInfos();
    string[] unlockTextInformation = new string[unlockKeyInfos.Length];
    for (int index = 0; index < unlockKeyInfos.Length; ++index)
      unlockTextInformation[index] = unlockKeyInfos[index].ToString();
    return unlockTextInformation;
  }

  public static UnlockKeyInfo[] GetUnlockKeyInfos()
  {
    Sweep.GetS1();
    string[] unlockKey = Sweep._Find_UnlockKey();
    ArrayList arrayList = new ArrayList();
    if (unlockKey != null)
    {
      foreach (string key in unlockKey)
      {
        UnlockKeyInfo unlockKeyInfo = new UnlockKeyInfo(key);
        arrayList.Add((object) unlockKeyInfo);
      }
    }
    return (UnlockKeyInfo[]) arrayList.ToArray(typeof (UnlockKeyInfo));
  }
}
