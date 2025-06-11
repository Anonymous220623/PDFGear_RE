// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.SharedUtilities
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

#nullable disable
namespace Ionic.Zip;

internal static class SharedUtilities
{
  private static Regex doubleDotRegex1 = new Regex("^(.*/)?([^/\\\\.]+/\\\\.\\\\./)(.+)$");
  private static Encoding utf8 = Encoding.GetEncoding("UTF-8");

  public static long GetFileLength(string fileName)
  {
    if (!File.Exists(fileName))
      throw new FileNotFoundException($"Could not find file '{fileName}'.", fileName);
    FileShare share = FileShare.ReadWrite | FileShare.Delete;
    using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, share))
      return fileStream.Length;
  }

  private static string SimplifyFwdSlashPath(string path)
  {
    if (path.StartsWith("./"))
      path = path.Substring(2);
    path = path.Replace("/./", "/");
    path = SharedUtilities.doubleDotRegex1.Replace(path, "$1$3");
    return path;
  }

  public static string NormalizePathForUseInZipFile(string pathName)
  {
    if (string.IsNullOrEmpty(pathName))
      return pathName;
    if (pathName.Length >= 2 && pathName[1] == ':' && pathName[2] == '\\')
      pathName = pathName.Substring(3);
    pathName = pathName.Replace('\\', '/');
    while (pathName.StartsWith("/"))
      pathName = pathName.Substring(1);
    return SharedUtilities.SimplifyFwdSlashPath(pathName);
  }

  public static string SanitizePath(string path)
  {
    List<string> stringList = new List<string>();
    int index1 = 0;
    string str1 = path;
    char[] chArray = new char[1]{ '/' };
    foreach (string str2 in str1.Split(chArray))
    {
      if (str2 == "..")
      {
        if (index1 != 0)
          --index1;
      }
      else
      {
        if (stringList.Count - 1 < index1)
          stringList.Add(str2);
        else
          stringList[index1] = str2;
        ++index1;
      }
    }
    path = "";
    for (int index2 = 0; index2 < index1; ++index2)
    {
      if (index2 > 0)
        path += "/";
      path += stringList[index2];
    }
    return path;
  }

  internal static byte[] StringToByteArray(string value, Encoding encoding)
  {
    return encoding.GetBytes(value);
  }

  internal static byte[] StringToByteArray(string value)
  {
    Encoding encoding = (Encoding) null;
    try
    {
      encoding = Encoding.GetEncoding("IBM437");
    }
    catch (Exception ex)
    {
    }
    if (encoding == null)
    {
      try
      {
        encoding = Encoding.GetEncoding(1252);
      }
      catch (Exception ex)
      {
      }
    }
    return SharedUtilities.StringToByteArray(value, encoding);
  }

  internal static string Utf8StringFromBuffer(byte[] buf)
  {
    return SharedUtilities.StringFromBuffer(buf, SharedUtilities.utf8);
  }

  internal static string StringFromBuffer(byte[] buf, Encoding encoding) => encoding.GetString(buf);

  internal static int ReadSignature(Stream s)
  {
    int num = 0;
    try
    {
      num = SharedUtilities._ReadFourBytes(s, "n/a");
    }
    catch (BadReadException ex)
    {
    }
    return num;
  }

  internal static int ReadEntrySignature(Stream s)
  {
    int num = 0;
    try
    {
      num = SharedUtilities._ReadFourBytes(s, "n/a");
      if (num == 134695760)
      {
        s.Seek(12L, SeekOrigin.Current);
        num = SharedUtilities._ReadFourBytes(s, "n/a");
        if (num != 67324752)
        {
          s.Seek(8L, SeekOrigin.Current);
          num = SharedUtilities._ReadFourBytes(s, "n/a");
          if (num != 67324752)
          {
            s.Seek(-24L, SeekOrigin.Current);
            num = SharedUtilities._ReadFourBytes(s, "n/a");
          }
        }
      }
    }
    catch (BadReadException ex)
    {
    }
    return num;
  }

  internal static int ReadInt(Stream s)
  {
    return SharedUtilities._ReadFourBytes(s, "Could not read block - no data!  (position 0x{0:X8})");
  }

  private static int _ReadFourBytes(Stream s, string message)
  {
    byte[] buffer = new byte[4];
    if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
      throw new BadReadException(string.Format(message, (object) s.Position));
    return (((int) buffer[3] * 256 /*0x0100*/ + (int) buffer[2]) * 256 /*0x0100*/ + (int) buffer[1]) * 256 /*0x0100*/ + (int) buffer[0];
  }

  internal static long FindSignature(Stream stream, int SignatureToFind)
  {
    long position1 = stream.Position;
    byte[] numArray = new byte[4]
    {
      (byte) (SignatureToFind >> 24),
      (byte) ((SignatureToFind & 16711680 /*0xFF0000*/) >> 16 /*0x10*/),
      (byte) ((SignatureToFind & 65280) >> 8),
      (byte) (SignatureToFind & (int) byte.MaxValue)
    };
    byte[] buffer = new byte[65536 /*0x010000*/];
    bool flag = false;
    do
    {
      int num = stream.Read(buffer, 0, buffer.Length);
      if (num != 0)
      {
        for (int index = 0; index < num; ++index)
        {
          if ((int) buffer[index] == (int) numArray[3])
          {
            long position2 = stream.Position;
            stream.Seek((long) (index - num), SeekOrigin.Current);
            flag = SharedUtilities.ReadSignature(stream) == SignatureToFind;
            if (!flag)
              stream.Seek(position2, SeekOrigin.Begin);
            else
              break;
          }
        }
      }
      else
        break;
    }
    while (!flag);
    if (flag)
      return stream.Position - position1 - 4L;
    stream.Seek(position1, SeekOrigin.Begin);
    return -1;
  }

  internal static DateTime AdjustTime_Reverse(DateTime time)
  {
    if (time.Kind == DateTimeKind.Utc)
      return time;
    DateTime dateTime = time;
    if (DateTime.Now.IsDaylightSavingTime() && !time.IsDaylightSavingTime())
      dateTime = time - new TimeSpan(1, 0, 0);
    else if (!DateTime.Now.IsDaylightSavingTime() && time.IsDaylightSavingTime())
      dateTime = time + new TimeSpan(1, 0, 0);
    return dateTime;
  }

  internal static DateTime PackedToDateTime(int packedDateTime)
  {
    if (packedDateTime == (int) ushort.MaxValue || packedDateTime == 0)
      return new DateTime(1995, 1, 1, 0, 0, 0, 0);
    int num1 = (int) (short) (packedDateTime & (int) ushort.MaxValue);
    short num2 = (short) (((long) packedDateTime & 4294901760L) >> 16 /*0x10*/);
    int year = 1980 + (((int) num2 & 65024) >> 9);
    int month = ((int) num2 & 480) >> 5;
    int day = (int) num2 & 31 /*0x1F*/;
    int hour = (num1 & 63488) >> 11;
    int minute = (num1 & 2016) >> 5;
    int second = (num1 & 31 /*0x1F*/) * 2;
    if (second >= 60)
    {
      ++minute;
      second = 0;
    }
    if (minute >= 60)
    {
      ++hour;
      minute = 0;
    }
    if (hour >= 24)
    {
      ++day;
      hour = 0;
    }
    DateTime dateTime = DateTime.Now;
    bool flag = false;
    try
    {
      dateTime = new DateTime(year, month, day, hour, minute, second, 0);
      flag = true;
    }
    catch (ArgumentOutOfRangeException ex1)
    {
      if (year == 1980 && (month == 0 || day == 0))
      {
        try
        {
          dateTime = new DateTime(1980, 1, 1, hour, minute, second, 0);
          flag = true;
        }
        catch (ArgumentOutOfRangeException ex2)
        {
          try
          {
            dateTime = new DateTime(1980, 1, 1, 0, 0, 0, 0);
            flag = true;
          }
          catch (ArgumentOutOfRangeException ex3)
          {
          }
        }
      }
      else
      {
        try
        {
          while (year < 1980)
            ++year;
          while (year > 2030)
            --year;
          while (month < 1)
            ++month;
          while (month > 12)
            --month;
          while (day < 1)
            ++day;
          while (day > 28)
            --day;
          while (minute < 0)
            ++minute;
          while (minute > 59)
            --minute;
          while (second < 0)
            ++second;
          while (second > 59)
            --second;
          dateTime = new DateTime(year, month, day, hour, minute, second, 0);
          flag = true;
        }
        catch (ArgumentOutOfRangeException ex4)
        {
        }
      }
    }
    if (!flag)
      throw new ZipException($"Bad date/time format in the zip file. ({$"y({year}) m({month}) d({day}) h({hour}) m({minute}) s({second})"})");
    return DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
  }

  internal static int DateTimeToPacked(DateTime time)
  {
    time = time.ToLocalTime();
    return (int) (ushort) (time.Day & 31 /*0x1F*/ | time.Month << 5 & 480 | time.Year - 1980 << 9 & 65024) << 16 /*0x10*/ | (int) (ushort) (time.Second / 2 & 31 /*0x1F*/ | time.Minute << 5 & 2016 | time.Hour << 11 & 63488);
  }

  public static void CreateAndOpenUniqueTempFile(string dir, out Stream fs, out string filename)
  {
    for (int index = 0; index < 3; ++index)
    {
      try
      {
        filename = Path.Combine(dir, SharedUtilities.InternalGetTempFileName());
        fs = (Stream) new FileStream(filename, FileMode.CreateNew);
        return;
      }
      catch (IOException ex)
      {
        if (index == 2)
          throw;
      }
    }
    throw new IOException();
  }

  public static string InternalGetTempFileName()
  {
    return $"DotNetZip-{Path.GetRandomFileName().Substring(0, 8)}.tmp";
  }

  internal static int ReadWithRetry(
    Stream s,
    byte[] buffer,
    int offset,
    int count,
    string FileName)
  {
    int num1 = 0;
    bool flag = false;
    int num2 = 0;
    do
    {
      try
      {
        num1 = s.Read(buffer, offset, count);
        flag = true;
      }
      catch (IOException ex)
      {
        if (new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).IsUnrestricted())
        {
          if (SharedUtilities._HRForException((Exception) ex) != 2147942433U)
            throw new IOException($"Cannot read file {FileName}", (Exception) ex);
          ++num2;
          if (num2 > 10)
            throw new IOException($"Cannot read file {FileName}, at offset 0x{offset:X8} after 10 retries", (Exception) ex);
          Thread.Sleep(250 + num2 * 550);
        }
        else
          throw;
      }
    }
    while (!flag);
    return num1;
  }

  private static uint _HRForException(Exception ex1) => (uint) Marshal.GetHRForException(ex1);
}
