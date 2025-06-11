// Decompiled with JetBrains decompiler
// Type: RegExt.FileAssociations.DefaultAppHashHelper
// Assembly: RegExt, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: DBF16820-DB7E-4C29-8C11-DD0B94851B7F
// Assembly location: C:\Program Files\PDFgear\RegExt.exe

using CommomLib.Config;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

#nullable disable
namespace RegExt.FileAssociations;

internal static class DefaultAppHashHelper
{
  public static string GetHash(string progId, string ext)
  {
    if (string.IsNullOrEmpty(progId) || string.IsNullOrEmpty(ext))
      return (string) null;
    string userSid = DefaultAppHashHelper.GetUserSid();
    if (string.IsNullOrEmpty(userSid))
      return (string) null;
    string experienceString = DefaultAppHashHelper.GetUserExperienceString();
    if (string.IsNullOrEmpty(experienceString))
      return (string) null;
    string hexDateTime = DefaultAppHashHelper.GetHexDateTime();
    return DefaultAppHashHelper.GetHashCore((ext + userSid + progId + hexDateTime + experienceString).ToLowerInvariant());
  }

  private static string GetHashCore(string baseInfo)
  {
    byte[] bytes1 = Encoding.Unicode.GetBytes(baseInfo);
    Array.Resize<byte>(ref bytes1, bytes1.Length + 2);
    bytes1[bytes1.Length - 2] = (byte) 0;
    bytes1[bytes1.Length - 1] = (byte) 0;
    byte[] hash;
    using (MD5 md5 = MD5.Create())
      hash = md5.ComputeHash(bytes1);
    int num1 = baseInfo.Length * 2 + 2;
    long num2 = (long) ((num1 & 4) <= 1) + DefaultAppHashHelper.GetShiftRight((long) num1, 2) - 1L;
    string hashCore = "";
    if (num2 > 1L)
    {
      DefaultAppHashHelper.Map map = new DefaultAppHashHelper.Map()
      {
        CACHE = 0,
        OUTHASH1 = 0,
        PDATA = 0,
        MD51 = (DefaultAppHashHelper.GetLong(hash) | 1L) + 1778057216L,
        MD52 = (DefaultAppHashHelper.GetLong(hash, 4) | 1L) + 333119488L,
        INDEX = DefaultAppHashHelper.GetShiftRight(num2 - 2L, 1)
      };
      for (map.COUNTER = map.INDEX + 1L; map.COUNTER > 0L; --map.COUNTER)
      {
        map.R0 = DefaultAppHashHelper.ConvertInt32(DefaultAppHashHelper.GetLong(bytes1, map.PDATA) + (long) map.OUTHASH1);
        map.R1.V1 = (long) DefaultAppHashHelper.ConvertInt32(DefaultAppHashHelper.GetLong(bytes1, map.PDATA + 4));
        map.PDATA += 8;
        map.R2.V1 = (long) DefaultAppHashHelper.ConvertInt32((long) map.R0 * map.MD51 - 284857861L * DefaultAppHashHelper.GetShiftRight((long) map.R0, 16 /*0x10*/));
        map.R2.V2 = (long) DefaultAppHashHelper.ConvertInt32(2046337941L * map.R2.V1 + 1755016095L * DefaultAppHashHelper.GetShiftRight(map.R2.V1, 16 /*0x10*/));
        map.R3 = DefaultAppHashHelper.ConvertInt32(3935764481L * map.R2.V2 - 1007687017L * DefaultAppHashHelper.GetShiftRight(map.R2.V2, 16 /*0x10*/));
        map.R4.V1 = (long) DefaultAppHashHelper.ConvertInt32((long) map.R3 + map.R1.V1);
        map.R5.V1 = (long) DefaultAppHashHelper.ConvertInt32(map.CACHE + (long) map.R3);
        map.R6.V1 = (long) DefaultAppHashHelper.ConvertInt32(map.R4.V1 * map.MD52 - 1021897765L * DefaultAppHashHelper.GetShiftRight(map.R4.V1, 16 /*0x10*/));
        map.R6.V2 = (long) DefaultAppHashHelper.ConvertInt32(1505996589L * map.R6.V1 - 573759729L * DefaultAppHashHelper.GetShiftRight(map.R6.V1, 16 /*0x10*/));
        map.OUTHASH1 = DefaultAppHashHelper.ConvertInt32(516489217L * map.R6.V2 + 901586633L * DefaultAppHashHelper.GetShiftRight(map.R6.V2, 16 /*0x10*/));
        map.OUTHASH2 = DefaultAppHashHelper.ConvertInt32(map.R5.V1 + (long) map.OUTHASH1);
        map.CACHE = (long) map.OUTHASH2;
      }
      byte[] bytes2 = new byte[16 /*0x10*/];
      BitConverter.GetBytes(map.OUTHASH1).CopyTo((Array) bytes2, 0);
      BitConverter.GetBytes(map.OUTHASH2).CopyTo((Array) bytes2, 4);
      map = new DefaultAppHashHelper.Map()
      {
        CACHE = 0L,
        OUTHASH1 = 0,
        PDATA = 0,
        MD51 = DefaultAppHashHelper.GetLong(hash) | 1L,
        MD52 = DefaultAppHashHelper.GetLong(hash, 4) | 1L,
        INDEX = DefaultAppHashHelper.GetShiftRight(num2 - 2L, 1)
      };
      for (map.COUNTER = map.INDEX + 1L; map.COUNTER > 0L; --map.COUNTER)
      {
        map.R0 = DefaultAppHashHelper.ConvertInt32(DefaultAppHashHelper.GetLong(bytes1, map.PDATA) + (long) map.OUTHASH1);
        map.PDATA += 8;
        map.R1.V1 = (long) DefaultAppHashHelper.ConvertInt32((long) map.R0 * map.MD51);
        map.R1.V2 = (long) DefaultAppHashHelper.ConvertInt32(2970681344L * map.R1.V1 - 812076783L * DefaultAppHashHelper.GetShiftRight(map.R1.V1, 16 /*0x10*/));
        map.R2.V1 = (long) DefaultAppHashHelper.ConvertInt32(1537146880L * map.R1.V2 - 2029495393L * DefaultAppHashHelper.GetShiftRight(map.R1.V2, 16 /*0x10*/));
        map.R2.V2 = (long) DefaultAppHashHelper.ConvertInt32(315537773L * DefaultAppHashHelper.GetShiftRight(map.R2.V1, 16 /*0x10*/) - 1184038912L * map.R2.V1);
        map.R3 = DefaultAppHashHelper.ConvertInt32(495124480L * map.R2.V2 + 629022083L * DefaultAppHashHelper.GetShiftRight(map.R2.V2, 16 /*0x10*/));
        map.R4.V1 = (long) DefaultAppHashHelper.ConvertInt32(map.MD52 * ((long) map.R3 + DefaultAppHashHelper.GetLong(bytes1, map.PDATA - 4)));
        map.R4.V2 = (long) DefaultAppHashHelper.ConvertInt32(385155072L * map.R4.V1 - 1569450251L * DefaultAppHashHelper.GetShiftRight(map.R4.V1, 16 /*0x10*/));
        map.R5.V1 = (long) DefaultAppHashHelper.ConvertInt32(2533294080L * map.R4.V2 - 746350849L * DefaultAppHashHelper.GetShiftRight(map.R4.V2, 16 /*0x10*/));
        map.R5.V2 = (long) DefaultAppHashHelper.ConvertInt32(730398720L * map.R5.V1 + 2090019721L * DefaultAppHashHelper.GetShiftRight(map.R5.V1, 16 /*0x10*/));
        map.OUTHASH1 = DefaultAppHashHelper.ConvertInt32(2674458624L * map.R5.V2 - 1079730327L * DefaultAppHashHelper.GetShiftRight(map.R5.V2, 16 /*0x10*/));
        map.OUTHASH2 = DefaultAppHashHelper.ConvertInt32((long) map.OUTHASH1 + map.CACHE + (long) map.R3);
        map.CACHE = (long) map.OUTHASH2;
      }
      BitConverter.GetBytes(map.OUTHASH1).CopyTo((Array) bytes2, 8);
      BitConverter.GetBytes(map.OUTHASH2).CopyTo((Array) bytes2, 12);
      byte[] inArray = new byte[8];
      long num3 = DefaultAppHashHelper.GetLong(bytes2, 8) ^ DefaultAppHashHelper.GetLong(bytes2);
      long num4 = DefaultAppHashHelper.GetLong(bytes2, 12) ^ DefaultAppHashHelper.GetLong(bytes2, 4);
      BitConverter.GetBytes((int) num3).CopyTo((Array) inArray, 0);
      BitConverter.GetBytes((int) num4).CopyTo((Array) inArray, 4);
      hashCore = Convert.ToBase64String(inArray);
    }
    return hashCore;
  }

  private static long GetShiftRight(long value, int count)
  {
    return (value & 2147483648L /*0x80000000*/) != 0L ? value >> count ^ 4294901760L : value >> count;
  }

  private static long GetLong(byte[] bytes, int index = 0)
  {
    return (long) BitConverter.ToInt32(bytes, index);
  }

  private static int ConvertInt32(long value)
  {
    return BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
  }

  private static string GetUserSid()
  {
    return new NTAccount(Environment.UserName).Translate(typeof (SecurityIdentifier)).Value.ToLowerInvariant();
  }

  private static string GetUserExperienceString()
  {
    try
    {
      string str1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), "shell32.dll");
      if (File.Exists(str1))
      {
        string key = "shell32_ue_" + FileVersionInfo.GetVersionInfo(str1).FileVersion.Replace(" ", "").Trim();
        string experienceString1;
        if (ConfigUtils.TryGet<string>(key, out experienceString1) && !string.IsNullOrEmpty(experienceString1))
          return experienceString1;
        string str2 = "User Choice set via Windows User Experience";
        byte[] bytes;
        using (FileStream input = File.Open(str1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
          using (BinaryReader binaryReader = new BinaryReader((Stream) input, (Encoding) new UTF8Encoding(false), true))
            bytes = binaryReader.ReadBytes(5242880 /*0x500000*/);
        }
        string str3 = "";
        int num1 = 0;
        do
        {
          try
          {
            str3 = Encoding.Unicode.GetString(bytes, 0, bytes.Length - num1);
          }
          catch
          {
          }
          ++num1;
        }
        while (string.IsNullOrEmpty(str3) && num1 < bytes.Length);
        int startIndex = str3.IndexOf(str2);
        if (startIndex >= 0)
        {
          int num2 = str3.IndexOf("}", startIndex);
          string str4 = "";
          if (num2 == -1)
            str4 = str2;
          string experienceString2 = str3.Substring(startIndex, num2 - startIndex + 1);
          ConfigUtils.TrySet<string>(key, experienceString2);
          return experienceString2;
        }
      }
    }
    catch
    {
    }
    return string.Empty;
  }

  private static string GetHexDateTime()
  {
    DateTime now = DateTime.Now;
    long fileTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0).ToFileTime();
    return $"{fileTime >> 32 /*0x20*/:x8}{fileTime & (long) uint.MaxValue:x8}";
  }

  private struct Map
  {
    public int PDATA;
    public long CACHE;
    public long COUNTER;
    public long INDEX;
    public long MD51;
    public long MD52;
    public int OUTHASH1;
    public int OUTHASH2;
    public int R0;
    public DefaultAppHashHelper.Map.R R1;
    public DefaultAppHashHelper.Map.R R2;
    public int R3;
    public DefaultAppHashHelper.Map.R R4;
    public DefaultAppHashHelper.Map.R R5;
    public DefaultAppHashHelper.Map.R R6;
    public DefaultAppHashHelper.Map.R R7;

    public struct R
    {
      public long V1;
      public long V2;
    }
  }
}
