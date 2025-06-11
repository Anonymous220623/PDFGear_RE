// Decompiled with JetBrains decompiler
// Type: Patagames.Activation.Activation
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace Patagames.Activation;

/// <summary>Represents a class for working with activation of SDK</summary>
[LicenseProvider]
internal class Activation
{
  internal static byte[] PdfiumPublicKey = new byte[92]
  {
    (byte) 48 /*0x30*/,
    (byte) 90,
    (byte) 48 /*0x30*/,
    (byte) 13,
    (byte) 6,
    (byte) 9,
    (byte) 42,
    (byte) 134,
    (byte) 72,
    (byte) 134,
    (byte) 247,
    (byte) 13,
    (byte) 1,
    (byte) 1,
    (byte) 1,
    (byte) 5,
    (byte) 0,
    (byte) 3,
    (byte) 73,
    (byte) 0,
    (byte) 48 /*0x30*/,
    (byte) 70,
    (byte) 2,
    (byte) 65,
    (byte) 0,
    (byte) 168,
    (byte) 171,
    (byte) 45,
    (byte) 109,
    (byte) 194,
    (byte) 11,
    (byte) 124,
    (byte) 48 /*0x30*/,
    (byte) 251,
    (byte) 184,
    (byte) 82,
    (byte) 185,
    (byte) 105,
    (byte) 165,
    (byte) 61,
    (byte) 36,
    (byte) 142,
    (byte) 100,
    (byte) 209,
    (byte) 121,
    (byte) 31 /*0x1F*/,
    (byte) 18,
    (byte) 2,
    (byte) 185,
    (byte) 164,
    (byte) 167,
    (byte) 245,
    (byte) 212,
    (byte) 226,
    (byte) 40,
    (byte) 155,
    (byte) 149,
    (byte) 8,
    (byte) 75,
    (byte) 254,
    (byte) 126,
    (byte) 134,
    (byte) 246,
    (byte) 197,
    (byte) 182,
    (byte) 10,
    (byte) 22,
    (byte) 12,
    (byte) 33,
    (byte) 77,
    (byte) 195,
    (byte) 27,
    (byte) 4,
    (byte) 141,
    (byte) 102,
    (byte) 155,
    (byte) 246,
    (byte) 30,
    (byte) 193,
    (byte) 87,
    (byte) 247,
    (byte) 80 /*0x50*/,
    (byte) 4,
    (byte) 245,
    (byte) 83,
    (byte) 86,
    (byte) 221,
    (byte) 102,
    (byte) 135,
    (byte) 2,
    (byte) 1,
    (byte) 17
  };
  private static object _syncMT = new object();
  internal const string KeyMainPath = "Software\\Patagames Software\\Pdf.Net SDK";
  internal const string KeyExtendPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Operparam";
  internal const string ParamInstallPath = "InstllationPath";
  internal const string ParamProductKey = "ProductKey";
  internal const string ParamTrialDate = "53DC775367954AF2972F7BEDD921D02D";
  internal const string ParamHash = "Hash";
  internal const string ParamTestKey = "validate";
  internal const string Paramx64Path = "x64Path";
  internal const string Paramx86Path = "x86Path";
  internal const string ParamIcuPath = "icuPath";

  public Activation() => Patagames.Activation.Activation.SaveLicenseAtDesignTime();

  public static void SaveLicenseAtDesignTime()
  {
    try
    {
      if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
        return;
      Log.Info("Try to check activation in design mode");
      string key;
      if (Patagames.Activation.Activation.IsKeyActivated(out key, out byte[] _, out byte[] _))
      {
        Log.Info("Found key {0}", (object) key);
        LicenseManager.CurrentContext.SetSavedLicenseKey(typeof (Patagames.Activation.Activation), key);
        Log.Info("Key saverd into assembly");
      }
      else
        Log.Info("There is no activation");
    }
    catch (Exception ex)
    {
      Log.Error("Check Activation at DesignTime Error: {0} {1}", (object) ex.Message, (object) ex.StackTrace);
    }
  }

  public static string GetSavedKey(Assembly licAssembyly)
  {
    try
    {
      if (licAssembyly != (Assembly) null)
      {
        Log.Info("Search key in SpecifiedAssembly");
        string keyFromResource = Patagames.Activation.Activation.ExtractKeyFromResource(licAssembyly);
        if (keyFromResource != null)
          return keyFromResource;
      }
      string keyFromResource1 = Patagames.Activation.Activation.ExtractKeyFromResource((Assembly) null);
      if (keyFromResource1 != null)
        return keyFromResource1;
      Log.Info("Search key in EntryAssembly");
      string keyFromResource2 = Patagames.Activation.Activation.ExtractKeyFromResource(Assembly.GetEntryAssembly());
      if (keyFromResource2 != null)
        return keyFromResource2;
      Log.Info("Search key in assemblies");
      string str = (string) null;
      try
      {
        str = BitConverter.ToString(Assembly.GetExecutingAssembly().GetName().GetPublicKeyToken());
      }
      catch (Exception ex)
      {
        Log.Error("Cant't retrieve PublicToken for patagtames assembly: {0}", (object) ex.Message);
      }
      StackFrame[] frames = new StackTrace().GetFrames();
      if (frames != null)
      {
        Log.Info("Found {0} frames", (object) frames.Length);
        for (int index = 0; index < frames.Length; ++index)
        {
          if (frames[index] == null)
          {
            Log.Info("Frame {0} is null", (object) index);
          }
          else
          {
            MethodBase method = frames[index].GetMethod();
            if (method != (MethodBase) null)
            {
              Type reflectedType = method.ReflectedType;
              if (reflectedType != (Type) null)
              {
                Assembly assembly = reflectedType.Assembly;
                try
                {
                  byte[] publicKeyToken = assembly.GetName().GetPublicKeyToken();
                  if (publicKeyToken != null)
                  {
                    if (str != null)
                    {
                      if (str == BitConverter.ToString(publicKeyToken))
                      {
                        Log.Info("Skipped: {0}", (object) assembly.GetName().ToString());
                        continue;
                      }
                    }
                  }
                }
                catch (Exception ex)
                {
                  Log.Error("Cant't compare public tokens: {0}", (object) ex.Message);
                }
                string keyFromResource3 = Patagames.Activation.Activation.ExtractKeyFromResource(assembly);
                if (keyFromResource3 != null)
                  return keyFromResource3;
              }
              else
                Log.Info("Type is null", (object) index);
            }
            else
              Log.Info("Method is null", (object) index);
          }
        }
      }
      else
        Log.Info("There is no any frames");
      Log.Info("The license was not found");
    }
    catch (Exception ex)
    {
      Log.Error("Check Activation at RunTime Error: {0} {1}", (object) ex.Message, (object) ex.StackTrace);
    }
    return (string) null;
  }

  private static string ExtractKeyFromResource(Assembly a)
  {
    Log.Info("Try to extract key from {0}", a == (Assembly) null ? (object) "default place" : (object) a.Location);
    string savedLicenseKey = LicenseManager.CurrentContext.GetSavedLicenseKey(typeof (Patagames.Activation.Activation), a);
    if ((savedLicenseKey ?? "").Trim() == "")
    {
      Log.Info("Key not found");
      return (string) null;
    }
    Log.Info("Key is {0}", savedLicenseKey.Length > 25 ? (object) (savedLicenseKey.Substring(0, 24) + "...") : (object) "[hidden]");
    return savedLicenseKey;
  }

  /// <summary>Gets the path to installation folder</summary>
  /// <value>Returns null if the SDK was not installed</value>
  internal static string InstallationPath
  {
    get
    {
      lock (Patagames.Activation.Activation._syncMT)
      {
        string installationPath = RegistryWOW6432.GetRegKey32<string>(RegHive.HKEY_LOCAL_MACHINE, "Software\\Patagames Software\\Pdf.Net SDK", "InstllationPath");
        if (installationPath == null || installationPath.Trim() == "")
          installationPath = RegistryWOW6432.GetRegKey64<string>(RegHive.HKEY_LOCAL_MACHINE, "Software\\Patagames Software\\Pdf.Net SDK", "InstllationPath");
        return installationPath;
      }
    }
  }

  /// <summary>Gets the path to 64 bit pdfium dynamic library</summary>
  /// <value>Returns null if the SDK was not installed</value>
  internal static string x64Path
  {
    get
    {
      lock (Patagames.Activation.Activation._syncMT)
      {
        string x64Path = RegistryWOW6432.GetRegKey32<string>(RegHive.HKEY_LOCAL_MACHINE, "Software\\Patagames Software\\Pdf.Net SDK", nameof (x64Path));
        if (x64Path == null || x64Path.Trim() == "")
          x64Path = RegistryWOW6432.GetRegKey64<string>(RegHive.HKEY_LOCAL_MACHINE, "Software\\Patagames Software\\Pdf.Net SDK", nameof (x64Path));
        return x64Path;
      }
    }
  }

  /// <summary>Gets the path to 32 bit pdfium dynamic library</summary>
  /// <value>Returns null if the SDK was not installed</value>
  internal static string x86Path
  {
    get
    {
      lock (Patagames.Activation.Activation._syncMT)
      {
        string x86Path = RegistryWOW6432.GetRegKey32<string>(RegHive.HKEY_LOCAL_MACHINE, "Software\\Patagames Software\\Pdf.Net SDK", nameof (x86Path));
        if (x86Path == null || x86Path.Trim() == "")
          x86Path = RegistryWOW6432.GetRegKey64<string>(RegHive.HKEY_LOCAL_MACHINE, "Software\\Patagames Software\\Pdf.Net SDK", nameof (x86Path));
        return x86Path;
      }
    }
  }

  /// <summary>Gets the path to icudt dynamic library</summary>
  /// <value>Returns null if the SDK was not installed</value>
  internal static string icuPath
  {
    get
    {
      lock (Patagames.Activation.Activation._syncMT)
      {
        string icuPath = RegistryWOW6432.GetRegKey32<string>(RegHive.HKEY_LOCAL_MACHINE, "Software\\Patagames Software\\Pdf.Net SDK", nameof (icuPath));
        if (icuPath == null || icuPath.Trim() == "")
          icuPath = RegistryWOW6432.GetRegKey64<string>(RegHive.HKEY_LOCAL_MACHINE, "Software\\Patagames Software\\Pdf.Net SDK", nameof (icuPath));
        return icuPath;
      }
    }
  }

  /// <summary>Gets the trial date from registry</summary>
  /// <value>Returns null if TRIAL was not stored into registry</value>
  internal static DateTime? TrialDate
  {
    get
    {
      string s = RegistryWOW6432.GetRegKey64<string>(RegHive.HKEY_CURRENT_USER, "Software\\Microsoft\\Windows\\CurrentVersion\\Operparam", "53DC775367954AF2972F7BEDD921D02D");
      if (s == null || s.Trim() == "")
        s = RegistryWOW6432.GetRegKey32<string>(RegHive.HKEY_CURRENT_USER, "Software\\Microsoft\\Windows\\CurrentVersion\\Operparam", "53DC775367954AF2972F7BEDD921D02D");
      return s != null && s.Trim() != "" ? new DateTime?(DateTime.FromFileTime(long.Parse(s))) : new DateTime?();
    }
  }

  /// <summary>Calculates Machine ID</summary>
  internal static string MachineId => DeviceInfo.Instance.Id;

  /// <summary>Gets the path there assembly is placed</summary>
  /// <returns></returns>
  internal static string GetCodeBasePath(int type)
  {
    try
    {
      switch (type)
      {
        case 1:
          return Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().Location).Path));
        case 2:
          return Directory.GetCurrentDirectory();
        case 3:
          return AppDomain.CurrentDomain.BaseDirectory;
        case 4:
          return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bin");
        default:
          return (string) null;
      }
    }
    catch
    {
      return (string) null;
    }
  }

  /// <summary>Copy native dlls to specified folder</summary>
  /// <param name="base_path">Folder where the files will be copied</param>
  /// <param name="is_check">if true then check that patagames assemblies contains in base_path folder </param>
  /// <remarks>This method creates x64 and x86 subfolders and copies appropriet file to them.
  /// This method do nothing if SDK was not properly installed
  /// </remarks>
  internal static void CopyNative(string base_path, bool is_check = false)
  {
    lock (Patagames.Activation.Activation._syncMT)
    {
      try
      {
        if (is_check)
        {
          string[] strArray = (string[]) null;
          if (Directory.Exists(base_path))
            strArray = Directory.GetFiles(base_path, "Patagames.*");
          if (strArray == null || strArray.Length == 0)
          {
            Log.Error("Patagames assemblies not found at {0}", (object) base_path);
            return;
          }
        }
        if (Patagames.Activation.Activation.x64Path == null && Patagames.Activation.Activation.x86Path == null)
          Log.Error("Installation not found");
        if (Patagames.Activation.Activation.x64Path != null)
        {
          if (!Directory.Exists(Path.Combine(base_path, "x64\\")))
            Directory.CreateDirectory(Path.Combine(base_path, "x64\\"));
          File.Copy(Patagames.Activation.Activation.x64Path, Path.Combine(base_path, "x64\\" + Platform.PdfiumDllName), true);
        }
        if (Patagames.Activation.Activation.x86Path != null)
        {
          if (!Directory.Exists(Path.Combine(base_path, "x86\\")))
            Directory.CreateDirectory(Path.Combine(base_path, "x86\\"));
          File.Copy(Patagames.Activation.Activation.x86Path, Path.Combine(base_path, "x86\\" + Platform.PdfiumDllName), true);
        }
        if (Patagames.Activation.Activation.icuPath == null)
          return;
        File.Copy(Patagames.Activation.Activation.icuPath, Path.Combine(base_path, Platform.IcuDllName), true);
      }
      catch (Exception ex)
      {
        Log.Error("Can't copy to {0}: {1}", (object) base_path, (object) ex.Message);
      }
    }
  }

  /// <summary>Checks if key was activated on this machine</summary>
  /// <param name="key">Receive key if it was activated</param>
  /// <param name="data">Key+Machine_id</param>
  /// <param name="hash">Sign of data</param>
  /// <returns>Returns trie if key was activated, false otherwise</returns>
  /// <remarks>This method calculate machine_id and check the hash</remarks>
  internal static bool IsKeyActivated(out string key, out byte[] data, out byte[] hash)
  {
    data = hash = (byte[]) null;
    key = RegistryWOW6432.GetRegKey32<string>(RegHive.HKEY_LOCAL_MACHINE, "Software\\Patagames Software\\Pdf.Net SDK", "ProductKey");
    if (key == null || key.Trim() == "")
      key = RegistryWOW6432.GetRegKey64<string>(RegHive.HKEY_LOCAL_MACHINE, "Software\\Patagames Software\\Pdf.Net SDK", "ProductKey");
    if (key == null || key.Trim() == "")
      return false;
    hash = RegistryWOW6432.GetRegKey32<byte[]>(RegHive.HKEY_LOCAL_MACHINE, "Software\\Patagames Software\\Pdf.Net SDK", "Hash");
    if (hash == null || hash.Length == 0)
      hash = RegistryWOW6432.GetRegKey64<byte[]>(RegHive.HKEY_LOCAL_MACHINE, "Software\\Patagames Software\\Pdf.Net SDK", "Hash");
    if (hash == null || hash.Length == 0)
      return false;
    data = Encoding.UTF8.GetBytes((key ?? "").Replace("-", "").ToUpper() + Patagames.Activation.Activation.MachineId);
    if (data == null || data.Length == 0)
      return false;
    using (RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider())
    {
      RSAParameters rsaPublicKey = new AsnKeyParser(Patagames.Activation.Activation.PdfiumPublicKey).ParseRSAPublicKey();
      cryptoServiceProvider.ImportParameters(rsaPublicKey);
      return cryptoServiceProvider.VerifyData(data, (object) CryptoConfig.MapNameToOID("SHA1"), hash);
    }
  }

  internal static bool ExtractKeyInfo(
    string key,
    out string email,
    out DateTime expDate,
    out DateTime genDate,
    out string pCode)
  {
    email = (string) null;
    expDate = DateTime.MinValue;
    genDate = DateTime.MinValue;
    pCode = (string) null;
    byte[] bytes = Patagames.Activation.Activation.FromHex(key);
    if (BitConverter.ToUInt16(bytes, 0) != (ushort) 63214)
      return false;
    int int16_1 = (int) BitConverter.ToInt16(bytes, 2);
    int month1 = (int) bytes[4];
    int day1 = (int) bytes[5];
    int int16_2 = (int) BitConverter.ToInt16(bytes, 6);
    int month2 = (int) bytes[8];
    int day2 = (int) bytes[9];
    pCode = Pdfium.DefaultAnsiEncoding.GetString(bytes, 11, (int) bytes[10]);
    ushort uint16 = BitConverter.ToUInt16(bytes, 11 + (int) bytes[10]);
    email = Pdfium.DefaultAnsiEncoding.GetString(bytes, 11 + (int) bytes[10] + 2, (int) uint16);
    expDate = new DateTime(int16_1, month1, day1);
    genDate = new DateTime(int16_2, month2, day2);
    return true;
  }

  private static byte[] FromHex(string hex)
  {
    hex = hex.Replace("-", "");
    byte[] numArray = new byte[hex.Length / 2];
    for (int index = 0; index < numArray.Length; ++index)
      numArray[index] = Convert.ToByte(hex.Substring(index * 2, 2), 16 /*0x10*/);
    return numArray;
  }

  internal static DateTime GetTrialDate(bool isSaveMark)
  {
    try
    {
      string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
      if ((folderPath ?? "").Trim() == "")
        return DateTime.Now;
      string str1 = Path.Combine(folderPath, "Pdfium.Net.SDK");
      if (!Directory.Exists(str1))
      {
        if (!isSaveMark)
          return DateTime.Now;
        Directory.CreateDirectory(str1);
      }
      string str2 = Path.Combine(str1, "config.ini");
      if (!File.Exists(str2))
      {
        if (isSaveMark)
          Patagames.Activation.Activation.WriteDate(str2, DateTime.Now);
        return DateTime.Now;
      }
      try
      {
        return Patagames.Activation.Activation.ReadDate(str2);
      }
      catch
      {
        if (isSaveMark)
          Patagames.Activation.Activation.WriteDate(str2, DateTime.Now);
        return DateTime.Now;
      }
    }
    catch
    {
      return DateTime.Now;
    }
  }

  private static void WriteDate(string appDataFile, DateTime date)
  {
    using (BinaryWriter binaryWriter = new BinaryWriter((Stream) new FileStream(appDataFile, FileMode.Create, FileAccess.Write)))
    {
      binaryWriter.Write(date.Ticks);
      binaryWriter.Close();
    }
  }

  private static DateTime ReadDate(string appDataFile)
  {
    FileStream input = new FileStream(appDataFile, FileMode.Open, FileAccess.Read);
    DateTime dateTime = DateTime.Now;
    using (BinaryReader binaryReader = new BinaryReader((Stream) input))
    {
      if (binaryReader.PeekChar() > 0)
        dateTime = new DateTime(binaryReader.ReadInt64());
      binaryReader.Close();
    }
    return dateTime;
  }
}
