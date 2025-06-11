// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontScriptList
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontScriptList(SystemFontOpenTypeFontSourceBase fontFile) : SystemFontTableBase(fontFile)
{
  private Dictionary<uint, SystemFontScriptRecord> scriptRecords;
  private Dictionary<uint, SystemFontScript> scripts;

  private SystemFontScript ReadScript(
    SystemFontOpenTypeFontReader reader,
    SystemFontScriptRecord record)
  {
    reader.BeginReadingBlock();
    long offset = this.Offset + (long) record.ScriptOffset;
    reader.Seek(offset, SeekOrigin.Begin);
    SystemFontScript systemFontScript = new SystemFontScript(this.FontSource, record.ScriptTag);
    systemFontScript.Offset = offset;
    systemFontScript.Read(reader);
    reader.EndReadingBlock();
    return systemFontScript;
  }

  public SystemFontScript GetScript(uint tag)
  {
    SystemFontScript script;
    if (!this.scripts.TryGetValue(tag, out script) && this.scriptRecords != null)
    {
      SystemFontScriptRecord record;
      script = this.scriptRecords.TryGetValue(tag, out record) || this.scriptRecords.TryGetValue(SystemFontTags.DEFAULT_TABLE_SCRIPT_TAG, out record) ? this.ReadScript(this.Reader, record) : (SystemFontScript) null;
      this.scripts[tag] = script;
    }
    return script;
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    ushort capacity = reader.ReadUShort();
    this.scriptRecords = new Dictionary<uint, SystemFontScriptRecord>((int) capacity);
    this.scripts = new Dictionary<uint, SystemFontScript>((int) capacity);
    for (int index = 0; index < (int) capacity; ++index)
    {
      SystemFontScriptRecord fontScriptRecord = new SystemFontScriptRecord();
      fontScriptRecord.Read(reader);
      this.scriptRecords[fontScriptRecord.ScriptTag] = fontScriptRecord;
    }
  }

  internal override void Write(SystemFontFontWriter writer)
  {
    writer.WriteUShort((ushort) this.scriptRecords.Count);
    foreach (uint key in this.scriptRecords.Keys)
      this.ReadScript(this.Reader, this.scriptRecords[key]).Write(writer);
  }

  internal override void Import(SystemFontOpenTypeFontReader reader)
  {
    ushort capacity = reader.ReadUShort();
    this.scripts = new Dictionary<uint, SystemFontScript>((int) capacity);
    for (int index = 0; index < (int) capacity; ++index)
    {
      uint num = reader.ReadULong();
      if ((int) num != (int) SystemFontTags.NULL_TAG)
      {
        SystemFontScript systemFontScript = new SystemFontScript(this.FontSource, num);
        systemFontScript.Import(reader);
        this.scripts[num] = systemFontScript;
      }
    }
  }
}
