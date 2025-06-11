// Decompiled with JetBrains decompiler
// Type: XmpCore.IXmpDateTime
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Sharpen;
using System;

#nullable disable
namespace XmpCore;

public interface IXmpDateTime : IComparable
{
  int Year { get; set; }

  int Month { get; set; }

  int Day { get; set; }

  int Hour { get; set; }

  int Minute { get; set; }

  int Second { get; set; }

  int Nanosecond { get; set; }

  TimeZoneInfo TimeZone { get; set; }

  TimeSpan Offset { get; set; }

  bool HasDate { get; }

  bool HasTime { get; }

  bool HasTimeZone { get; }

  Calendar Calendar { get; }

  string ToIso8601String();
}
