// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontNameFormat0
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontNameFormat0(SystemFontOpenTypeFontSourceBase fontSource) : 
  SystemFontSystemFontName(fontSource)
{
  private ushort stringOffset;
  private SystemFontNameRecord[] nameRecords;
  private Dictionary<SystemFontNameRecord, string> strings;
  private string fontFamily;

  public override string FontFamily
  {
    get
    {
      if (this.fontFamily == null)
        this.fontFamily = this.ReadString(this.Reader, (ushort) 1033, (ushort) 1);
      return this.fontFamily;
    }
  }

  private string ReadString(SystemFontOpenTypeFontReader reader, ushort languageId, ushort nameId)
  {
    foreach (SystemFontNameRecord nameRecord in this.FindNameRecords(3, languageId, nameId))
    {
      Encoding encodingFromEncodingId = SystemFontIDs.GetEncodingFromEncodingID(nameRecord.EncodingID);
      if (encodingFromEncodingId != null)
        return this.ReadString(reader, nameRecord, encodingFromEncodingId);
    }
    return (string) null;
  }

  private string ReadString(
    SystemFontOpenTypeFontReader reader,
    SystemFontNameRecord record,
    Encoding encoding)
  {
    string str;
    if (!this.strings.TryGetValue(record, out str))
    {
      reader.BeginReadingBlock();
      long offset = this.Offset + (long) this.stringOffset + (long) record.Offset;
      reader.Seek(offset, SeekOrigin.Begin);
      byte[] numArray = new byte[(int) record.Length];
      reader.Read(numArray, (int) record.Length);
      reader.EndReadingBlock();
      str = encoding.GetString(numArray, 0, numArray.Length);
      this.strings[record] = str;
    }
    return str;
  }

  private IEnumerable<SystemFontNameRecord> FindNameRecords(
    int platformId,
    ushort languageId,
    ushort nameId)
  {
    return SystemFontEnumerable.Where<SystemFontNameRecord>((IEnumerable<SystemFontNameRecord>) this.nameRecords, (Func<SystemFontNameRecord, bool>) (r => (int) r.PlatformID == platformId && (int) r.LanguageID == (int) languageId && (int) r.NameID == (int) nameId));
  }

  internal override string ReadName(ushort languageID, ushort nameID)
  {
    return this.ReadString(this.Reader, languageID, nameID);
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    ushort length = reader.ReadUShort();
    this.stringOffset = reader.ReadUShort();
    this.nameRecords = new SystemFontNameRecord[(int) length];
    this.strings = new Dictionary<SystemFontNameRecord, string>();
    for (int index = 0; index < (int) length; ++index)
    {
      this.nameRecords[index] = new SystemFontNameRecord();
      this.nameRecords[index].Read(reader);
    }
  }
}
