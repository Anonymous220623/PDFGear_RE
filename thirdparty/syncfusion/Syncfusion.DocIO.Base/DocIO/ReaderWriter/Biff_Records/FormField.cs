// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.FormField
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

internal class FormField
{
  internal const int DEF_VALUE = 25;
  private FieldType m_fieldType;
  private PICF m_picf;
  private ushort m_params;
  private ushort m_maxLength;
  private ushort m_checkBoxSize;
  private string m_title;
  private string m_defaultTextInputValue;
  private bool m_defaultCheckBoxValue;
  private ushort m_defaultDropDownValue;
  private string m_textFormat;
  private string m_help;
  private string m_tooltip;
  private string m_macroOnStart;
  private string m_macroOnEnd;
  private List<string> m_dropDownItems;
  private bool m_isUnicode;

  internal FormFieldType FormFieldType => (FormFieldType) ((int) this.m_params & 3);

  internal FieldType FieldType => this.m_fieldType;

  internal short Params
  {
    get => (short) this.m_params;
    set => this.m_params = (ushort) value;
  }

  internal int MaxLength
  {
    get => (int) this.m_maxLength;
    set => this.m_maxLength = (ushort) value;
  }

  internal int CheckBoxSize
  {
    get => (int) this.m_checkBoxSize;
    set => this.m_checkBoxSize = (ushort) value;
  }

  internal string Title
  {
    get => this.m_title;
    set => this.m_title = value;
  }

  internal bool Checked
  {
    get
    {
      switch (this.Value)
      {
        case 0:
          return false;
        case 1:
          return true;
        case 25:
          return this.m_defaultCheckBoxValue;
        default:
          throw new ArgumentException("Unsupported checkbox field value found.");
      }
    }
    set => this.Value = value ? 1 : 0;
  }

  internal bool DefaultCheckBoxValue
  {
    get => this.m_defaultCheckBoxValue;
    set => this.m_defaultCheckBoxValue = value;
  }

  internal int DefaultDropDownValue
  {
    get => (int) this.m_defaultDropDownValue;
    set => this.m_defaultDropDownValue = (ushort) value;
  }

  internal string DefaultTextInputValue
  {
    get => this.m_defaultTextInputValue;
    set => this.m_defaultTextInputValue = value;
  }

  internal string Format
  {
    get => this.m_textFormat;
    set => this.m_textFormat = value;
  }

  internal int Value
  {
    get => ((int) this.m_params & 124) >> 2;
    set => this.m_params = (ushort) ((int) this.m_params & -125 | value << 2);
  }

  internal string Help
  {
    get => this.m_help;
    set => this.m_help = value;
  }

  internal string Tooltip
  {
    get => this.m_tooltip;
    set => this.m_tooltip = value;
  }

  internal string MacroOnStart
  {
    get => this.m_macroOnStart;
    set => this.m_macroOnStart = value;
  }

  internal string MacroOnEnd
  {
    get => this.m_macroOnEnd;
    set => this.m_macroOnEnd = value;
  }

  internal int DropDownIndex
  {
    get => this.Value != 25 ? this.Value : (int) this.m_defaultDropDownValue;
    set => this.Value = value;
  }

  internal List<string> DropDownItems => this.m_dropDownItems;

  internal string DropDownValue
  {
    get => this.m_dropDownItems[this.DropDownIndex];
    set
    {
      for (int index = 0; index < this.m_dropDownItems.Count; ++index)
      {
        if (string.Compare(this.m_dropDownItems[index], value, true) == 0)
        {
          this.DropDownIndex = index;
          break;
        }
      }
    }
  }

  internal bool IsCalculateOnExit
  {
    get => ((int) this.m_params & 16384 /*0x4000*/) != 0;
    set => BaseWordRecord.SetBitsByMask((int) this.m_params, 16384 /*0x4000*/, 14, value ? 1 : 0);
  }

  internal bool IsCheckBoxExplicitSize
  {
    get => ((int) this.m_params & 1024 /*0x0400*/) != 0;
    set => BaseWordRecord.SetBitsByMask((int) this.m_params, 1024 /*0x0400*/, 11, value ? 1 : 0);
  }

  internal bool IsDisabled
  {
    get => ((int) this.m_params & 512 /*0x0200*/) != 0;
    set => BaseWordRecord.SetBitsByMask((int) this.m_params, 512 /*0x0200*/, 10, value ? 1 : 0);
  }

  internal TextFormFieldType TextFormFieldType
  {
    get => (TextFormFieldType) (((int) this.m_params & 14336) >> 11);
    set => this.m_params = (ushort) ((int) this.m_params & -14337 | (int) value << 11);
  }

  internal FormField(FieldType fieldType)
  {
    this.m_fieldType = fieldType;
    switch (fieldType)
    {
      case FieldType.FieldFormTextInput:
        this.m_params = (ushort) 0;
        break;
      case FieldType.FieldFormCheckBox:
        this.m_params = (ushort) 1;
        break;
      case FieldType.FieldFormDropDown:
        this.m_params = (ushort) 32770;
        this.m_dropDownItems = new List<string>();
        break;
      default:
        throw new Exception("Unknown field type.");
    }
    this.m_picf = new PICF();
    this.Value = 25;
  }

  internal FormField(FieldType fieldType, BinaryReader reader)
  {
    this.m_fieldType = fieldType;
    long position = reader.BaseStream.Position;
    this.m_picf = reader.BaseStream.Length <= reader.BaseStream.Position ? new PICF() : new PICF(reader);
    if (this.m_picf.lcb <= 68)
      return;
    byte num1 = reader.ReadByte();
    if (num1 == byte.MaxValue)
    {
      reader.BaseStream.Position += 3L;
      num1 = reader.ReadByte();
      this.m_isUnicode = true;
    }
    this.m_params = (ushort) reader.ReadByte();
    if (this.m_isUnicode)
      this.m_params = (ushort) (((uint) this.m_params << 8) + (uint) num1);
    else if (num1 != byte.MaxValue)
      this.m_params += (ushort) num1;
    this.m_maxLength = reader.ReadUInt16();
    this.m_checkBoxSize = reader.ReadUInt16();
    if (!this.m_isUnicode)
      reader.BaseStream.Position += 2L;
    this.m_title = this.ReadString(reader, true);
    switch (fieldType)
    {
      case FieldType.FieldLink:
        throw new NotImplementedException("Link fields are not yet supported");
      case FieldType.FieldFormTextInput:
        this.m_defaultTextInputValue = this.ReadString(reader, true);
        break;
      case FieldType.FieldFormCheckBox:
        this.m_defaultCheckBoxValue = reader.ReadUInt16() != (ushort) 0;
        break;
      case FieldType.FieldFormDropDown:
        this.m_defaultDropDownValue = reader.ReadUInt16();
        break;
    }
    this.m_textFormat = this.ReadString(reader, true);
    this.m_help = this.ReadString(reader, true);
    this.m_tooltip = this.ReadString(reader, true);
    this.m_macroOnStart = this.ReadString(reader, true);
    this.m_macroOnEnd = this.ReadString(reader, true);
    if (fieldType == FieldType.FieldFormDropDown)
    {
      int num2 = (int) reader.ReadUInt16();
      if (!this.m_isUnicode)
        reader.BaseStream.Position += 2L;
      int num3 = reader.ReadInt32();
      this.m_dropDownItems = new List<string>();
      if (!this.m_isUnicode)
        reader.BaseStream.Position += 6L;
      for (int index = 0; index < num3; ++index)
        this.m_dropDownItems.Add(this.ReadString(reader, false));
    }
    long num4 = reader.BaseStream.Position - position;
    if ((long) this.m_picf.lcb > num4)
      reader.BaseStream.Position = position + (long) this.m_picf.lcb;
    else if ((long) this.m_picf.lcb != num4)
      throw new ArgumentException("Unrecognized format of the form field.");
  }

  internal void Write(Stream stream)
  {
    BinaryWriter writer = new BinaryWriter(stream);
    long position1 = writer.BaseStream.Position;
    this.m_picf.Write(stream);
    writer.Write(uint.MaxValue);
    writer.Write(this.m_params);
    writer.Write(this.m_maxLength);
    writer.Write(this.m_checkBoxSize);
    FormField.WriteString(this.m_title, writer, true);
    switch (this.m_fieldType)
    {
      case FieldType.FieldFormTextInput:
        FormField.WriteString(this.m_defaultTextInputValue, writer, true);
        break;
      case FieldType.FieldFormCheckBox:
        writer.Write(this.m_defaultCheckBoxValue ? (ushort) 1 : (ushort) 0);
        break;
      case FieldType.FieldFormDropDown:
        writer.Write(this.m_defaultDropDownValue);
        break;
      default:
        throw new Exception("Unsupported form field type.");
    }
    FormField.WriteString(this.m_textFormat, writer, true);
    FormField.WriteString(this.m_help, writer, true);
    FormField.WriteString(this.m_tooltip, writer, true);
    FormField.WriteString(this.m_macroOnStart, writer, true);
    FormField.WriteString(this.m_macroOnEnd, writer, true);
    if (this.m_fieldType == FieldType.FieldFormDropDown)
    {
      writer.Write(ushort.MaxValue);
      writer.Write((uint) this.m_dropDownItems.Count);
      foreach (string dropDownItem in this.m_dropDownItems)
        FormField.WriteString(dropDownItem, writer, false);
    }
    long position2 = writer.BaseStream.Position;
    this.m_picf.lcb = (int) (position2 - position1);
    writer.BaseStream.Position = position1;
    this.m_picf.Write(stream);
    writer.BaseStream.Position = position2;
  }

  internal static string ReadUnicodeString(BinaryReader binaryReader, bool readZero)
  {
    int num1 = (int) binaryReader.ReadInt16();
    string str = Encoding.Unicode.GetString(binaryReader.ReadBytes(num1 * 2));
    if (readZero)
    {
      int num2 = (int) binaryReader.ReadInt16();
    }
    return str;
  }

  internal string ReadString(BinaryReader binaryReader, bool readZero)
  {
    int count = this.m_isUnicode ? (int) binaryReader.ReadInt16() * 2 : (int) binaryReader.ReadByte();
    byte[] bytes = binaryReader.ReadBytes(count);
    string empty = string.Empty;
    string str = !this.m_isUnicode ? DocIOEncoding.GetString(bytes) : Encoding.Unicode.GetString(bytes);
    if (readZero)
    {
      if (!this.m_isUnicode)
      {
        int num1 = (int) binaryReader.ReadByte();
      }
      else
      {
        int num2 = (int) binaryReader.ReadInt16();
      }
    }
    return str;
  }

  internal static void WriteString(string text, BinaryWriter writer, bool writeSeparator)
  {
    string s = text != null ? text : "";
    writer.Write((short) s.Length);
    writer.Write(Encoding.Unicode.GetBytes(s));
    if (!writeSeparator)
      return;
    writer.Write((short) 0);
  }
}
