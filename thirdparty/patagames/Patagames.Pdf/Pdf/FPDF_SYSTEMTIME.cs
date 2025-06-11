// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FPDF_SYSTEMTIME
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>Declares of a struct type to the local system time.</summary>
public struct FPDF_SYSTEMTIME
{
  /// <summary>years since 1900</summary>
  [MarshalAs(UnmanagedType.I2)]
  public ushort wYear;
  /// <summary>months since January - [0,11]</summary>
  [MarshalAs(UnmanagedType.I2)]
  public ushort wMonth;
  /// <summary>days since Sunday - [0,6]</summary>
  [MarshalAs(UnmanagedType.I2)]
  public ushort wDayOfWeek;
  /// <summary>day of the month - [1,31]</summary>
  [MarshalAs(UnmanagedType.I2)]
  public ushort wDay;
  /// <summary>hours since midnight - [0,23]</summary>
  [MarshalAs(UnmanagedType.I2)]
  public ushort wHour;
  /// <summary>minutes after the hour - [0,59]</summary>
  [MarshalAs(UnmanagedType.I2)]
  public ushort wMinute;
  /// <summary>seconds after the minute - [0,59]</summary>
  [MarshalAs(UnmanagedType.I2)]
  public ushort wSecond;
  /// <summary>milliseconds after the second - [0,999]</summary>
  [MarshalAs(UnmanagedType.I2)]
  public ushort wMilliseconds;
}
