// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.UtilityMethods
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public sealed class UtilityMethods
{
  private const int DEF_WRONG_DATE = 61;
  private const int DEF_EXCEL2007_MAX_ROW_COUNT = 1048576 /*0x100000*/;
  private const int DEF_EXCEL2007_MAX_COLUMN_COUNT = 16384 /*0x4000*/;
  private const int DEF_EXCEL97TO03_MAX_ROW_COUNT = 65536 /*0x010000*/;
  private const int DEF_EXCEL97TO03_MAX_COLUMN_COUNT = 256 /*0x0100*/;

  private UtilityMethods()
  {
  }

  public static bool Intersects(Rectangle rect1, Rectangle rect2)
  {
    return rect1.X <= rect2.X + rect2.Width && rect2.X <= rect1.X + rect1.Width && rect1.Y <= rect2.Y + rect2.Height && rect2.Y <= rect1.Y + rect1.Height;
  }

  public static bool Contains(Rectangle rect, int x, int y)
  {
    return rect.X <= x && x <= rect.X + rect.Width && rect.Y <= y && y < rect.Y + rect.Height;
  }

  public static int IndexOf(TBIFFRecord[] array, TBIFFRecord value)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    int index = 0;
    for (int length = array.Length; index < length; ++index)
    {
      if (array[index] == value)
        return index;
    }
    return -1;
  }

  public static int IndexOf(int[] array, int value)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    int index = 0;
    for (int length = array.Length; index < length; ++index)
    {
      if (array[index] == value)
        return index;
    }
    return -1;
  }

  public static int IndexOf(short[] array, short value)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    int index = 0;
    for (int length = array.Length; index < length; ++index)
    {
      if ((int) array[index] == (int) value)
        return index;
    }
    return -1;
  }

  public static double ConvertDateTimeToNumber(DateTime dateTime)
  {
    double oaDate = dateTime.ToOADate();
    if (oaDate < 61.0)
      --oaDate;
    return oaDate;
  }

  public static DateTime ConvertNumberToDateTime(double dNumber, bool is1904DateSystem)
  {
    if (is1904DateSystem)
      dNumber += 1462.0;
    else if (dNumber < 61.0)
      ++dNumber;
    DateTime dateTime = DateTime.FromOADate(dNumber);
    return dateTime.Millisecond > 500 ? dateTime.AddSeconds(1.0) : dateTime;
  }

  [CLSCompliant(false)]
  public static ICellPositionFormat CreateCell(int iRow, int iColumn, TBIFFRecord recordType)
  {
    ICellPositionFormat record = (ICellPositionFormat) BiffRecordFactory.GetRecord(recordType);
    record.Row = iRow;
    record.Column = iColumn;
    return record;
  }

  public static string RemoveFirstCharUnsafe(string value) => value.Substring(1, value.Length - 1);

  public static string Join(string separator, List<string> value)
  {
    if (separator == null)
      separator = string.Empty;
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    int num1 = 0;
    int count = value.Count;
    for (int index = 0; index < count; ++index)
    {
      string str = value[index];
      if (str != null)
        num1 += str.Length;
    }
    int num2 = num1 + (count - 1) * separator.Length;
    if (num2 < 0 || num2 + 1 < 0)
      throw new OutOfMemoryException();
    if (num2 == 0)
      return string.Empty;
    StringBuilder stringBuilder = new StringBuilder();
    string str1 = value[0];
    if (value != null)
      stringBuilder.Append(value[0]);
    for (int index = 1; index < count; ++index)
    {
      stringBuilder.Append(separator);
      string str2 = value[index] ?? string.Empty;
      stringBuilder.Append(str2);
    }
    return stringBuilder.ToString();
  }

  public static void GetMaxRowColumnCount(out int iRows, out int iColumns, ExcelVersion version)
  {
    switch (version)
    {
      case ExcelVersion.Excel97to2003:
        iRows = 65536 /*0x010000*/;
        iColumns = 256 /*0x0100*/;
        break;
      case ExcelVersion.Excel2007:
      case ExcelVersion.Excel2010:
      case ExcelVersion.Excel2013:
      case ExcelVersion.Excel2016:
      case ExcelVersion.Xlsx:
        iRows = 1048576 /*0x100000*/;
        iColumns = 16384 /*0x4000*/;
        break;
      default:
        throw new ArgumentException("Unknown version");
    }
  }

  public static void CopyStreamTo(Stream source, Stream destination)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    byte[] buffer = new byte[32768 /*0x8000*/];
    int count;
    while ((count = source.Read(buffer, 0, 32768 /*0x8000*/)) > 0)
      destination.Write(buffer, 0, count);
  }

  public static MemoryStream CloneStream(MemoryStream source)
  {
    MemoryStream destination = new MemoryStream((int) source.Length);
    long position = source.Position;
    source.Position = 0L;
    UtilityMethods.CopyStreamTo((Stream) source, (Stream) destination);
    destination.Position = source.Position = position;
    return destination;
  }

  public static XmlReader CreateReader(Stream data, bool skipToElement)
  {
    XmlReader reader = (XmlReader) new XmlTextReader(data);
    if (skipToElement)
    {
      while (reader.NodeType != XmlNodeType.Element)
        reader.Read();
    }
    return reader;
  }

  internal static XmlReader CreateReader(Stream data, string tag)
  {
    data.Position = 0L;
    XmlReader reader = (XmlReader) new XmlTextReader(data);
    while (!reader.EOF && reader.LocalName != tag)
      reader.Read();
    return reader;
  }

  internal static List<double> CloneList(List<double> list)
  {
    List<double> doubleList = new List<double>();
    foreach (double num in list)
      doubleList.Add(num);
    return doubleList;
  }

  public static XmlReader CreateReaderFromStreamPosition(Stream data)
  {
    return UtilityMethods.CreateReader(data, true);
  }

  public static XmlReader CreateReader(Stream data)
  {
    if (data.CanSeek && data.Position != 0L)
      data.Position = 0L;
    return UtilityMethods.CreateReader(data, true);
  }

  public static XmlWriter CreateWriter(Stream data, Encoding encoding)
  {
    return (XmlWriter) new XmlTextWriter(data, encoding);
  }

  public static XmlWriter CreateWriter(TextWriter data) => (XmlWriter) new XmlTextWriter(data);

  public static XmlWriter CreateWriter(TextWriter data, bool indent)
  {
    return (XmlWriter) new XmlTextWriter(data)
    {
      Formatting = Formatting.Indented
    };
  }
}
