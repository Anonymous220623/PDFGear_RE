// Decompiled with JetBrains decompiler
// Type: NLog.Targets.DateAndSequenceArchive
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace NLog.Targets;

internal class DateAndSequenceArchive
{
  private readonly string _dateFormat;

  public string FileName { get; private set; }

  public DateTime Date { get; private set; }

  public int Sequence { get; private set; }

  public bool HasSameFormattedDate(DateTime date)
  {
    return string.Equals(date.ToString(this._dateFormat), this.Date.ToString(this._dateFormat), StringComparison.Ordinal);
  }

  public DateAndSequenceArchive(string fileName, DateTime date, string dateFormat, int sequence)
  {
    if (fileName == null)
      throw new ArgumentNullException(nameof (fileName));
    if (dateFormat == null)
      throw new ArgumentNullException(nameof (dateFormat));
    this.Date = date;
    this._dateFormat = dateFormat;
    this.Sequence = sequence;
    this.FileName = fileName;
  }
}
