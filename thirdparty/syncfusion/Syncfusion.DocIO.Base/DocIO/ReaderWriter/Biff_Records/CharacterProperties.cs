// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.CharacterProperties
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class CharacterProperties
{
  private CharacterPropertyException m_chpx;
  private WordStyleSheet m_styleSheet;
  private byte m_bFlags = 1;

  internal CharacterProperties(WordStyleSheet styleSheet)
  {
    this.m_chpx = new CharacterPropertyException();
    this.m_styleSheet = styleSheet;
  }

  internal CharacterProperties(CharacterPropertyException chpx, WordStyleSheet styleSheet)
  {
    this.m_chpx = chpx;
    this.m_styleSheet = styleSheet;
  }

  internal SinglePropertyModifierArray Sprms
  {
    get => this.m_chpx == null ? (SinglePropertyModifierArray) null : this.m_chpx.PropertyModifiers;
  }

  internal CharacterPropertyException CharacterPropertyException => this.m_chpx;

  internal bool ComplexScript
  {
    get => this.Sprms.GetBoolean(2178, false);
    set => this.Sprms.SetBoolValue(2178, value);
  }

  internal bool Bold
  {
    get => this.Sprms.GetBoolean(2101, false);
    set => this.Sprms.SetBoolValue(2101, value);
  }

  internal byte BoldComplex
  {
    get => this.Sprms.GetByte(2101, (byte) 0);
    set
    {
      if (value == byte.MaxValue)
        return;
      this.Sprms.SetByteValue(2101, value);
    }
  }

  internal bool Italic
  {
    get => this.Sprms.GetBoolean(2102, false);
    set => this.Sprms.SetBoolValue(2102, value);
  }

  internal byte ItalicComplex
  {
    get => this.Sprms.GetByte(2102, (byte) 0);
    set
    {
      if (value == byte.MaxValue)
        return;
      this.Sprms.SetByteValue(2102, value);
    }
  }

  internal bool Strike
  {
    get => this.Sprms.GetBoolean(2103, false);
    set => this.Sprms.SetBoolValue(2103, value);
  }

  internal byte ShadowComplex
  {
    get => this.Sprms.GetByte(2105, (byte) 0);
    set
    {
      if (value == byte.MaxValue)
        return;
      this.Sprms.SetByteValue(2105, value);
    }
  }

  internal byte StrikeComplex
  {
    get => this.Sprms.GetByte(2103, (byte) 0);
    set
    {
      if (value == byte.MaxValue)
        return;
      this.Sprms.SetByteValue(2103, value);
    }
  }

  internal bool DoubleStrike
  {
    get => this.Sprms.GetBoolean(10835, false);
    set
    {
      if (value)
        this.Strike = false;
      this.Sprms.SetBoolValue(10835, value);
    }
  }

  internal byte UnderlineCode
  {
    get => this.Sprms.GetByte(10814, (byte) 0);
    set => this.Sprms.SetByteValue(10814, value);
  }

  internal string FontName
  {
    get
    {
      return this.m_styleSheet.FontNamesList.Count == 0 ? string.Empty : this.m_styleSheet.FontNamesList[(int) this.FontAscii];
    }
    set
    {
      int index = this.m_styleSheet.FontNameToIndex(value);
      if (index >= 0)
      {
        this.FontAscii = this.FontFarEast = this.FontNonFarEast = (ushort) index;
      }
      else
      {
        this.FontAscii = this.FontFarEast = this.FontNonFarEast = (ushort) this.m_styleSheet.FontNamesList.Count;
        this.m_styleSheet.UpdateFontName(value);
      }
    }
  }

  internal string FontNameAscii
  {
    get
    {
      return this.m_styleSheet.FontNamesList.Count == 0 ? string.Empty : this.m_styleSheet.FontNamesList[(int) this.FontAscii];
    }
    set
    {
      int index = this.m_styleSheet.FontNameToIndex(value);
      if (index >= 0)
      {
        this.FontAscii = (ushort) index;
      }
      else
      {
        this.FontAscii = (ushort) this.m_styleSheet.FontNamesList.Count;
        this.m_styleSheet.UpdateFontName(value);
      }
    }
  }

  internal string FontNameFarEast
  {
    get
    {
      return this.m_styleSheet.FontNamesList.Count == 0 ? string.Empty : this.m_styleSheet.FontNamesList[(int) this.FontFarEast];
    }
    set
    {
      int index = this.m_styleSheet.FontNameToIndex(value);
      if (index >= 0)
      {
        this.FontFarEast = (ushort) index;
      }
      else
      {
        this.FontFarEast = (ushort) this.m_styleSheet.FontNamesList.Count;
        this.m_styleSheet.UpdateFontName(value);
      }
    }
  }

  internal string FontNameNonFarEast
  {
    get
    {
      return this.m_styleSheet.FontNamesList.Count == 0 ? string.Empty : this.m_styleSheet.FontNamesList[(int) this.FontNonFarEast];
    }
    set
    {
      int index = this.m_styleSheet.FontNameToIndex(value);
      if (index >= 0)
      {
        this.FontNonFarEast = (ushort) index;
      }
      else
      {
        this.FontNonFarEast = (ushort) this.m_styleSheet.FontNamesList.Count;
        this.m_styleSheet.UpdateFontName(value);
      }
    }
  }

  internal string FontNameBi
  {
    get
    {
      return this.m_styleSheet.FontNamesList.Count == 0 ? string.Empty : this.m_styleSheet.FontNamesList[(int) this.FontBi];
    }
    set
    {
      int index = this.m_styleSheet.FontNameToIndex(value);
      if (index >= 0)
      {
        this.FontBi = (ushort) index;
      }
      else
      {
        this.FontBi = (ushort) this.m_styleSheet.FontNamesList.Count;
        this.m_styleSheet.UpdateFontName(value);
      }
    }
  }

  internal ushort FontAscii
  {
    get => this.Sprms.GetUShort(19023, (ushort) 0);
    set => this.Sprms.SetUShortValue(19023, value);
  }

  internal ushort FontFarEast
  {
    get => this.Sprms.GetUShort(19024, (ushort) 0);
    set => this.Sprms.SetUShortValue(19024, value);
  }

  internal ushort FontNonFarEast
  {
    get => this.Sprms.GetUShort(19025, (ushort) 0);
    set => this.Sprms.SetUShortValue(19025, value);
  }

  internal ushort FontBi
  {
    get => this.Sprms.GetUShort(19038, (ushort) 0);
    set => this.Sprms.SetUShortValue(19038, value);
  }

  internal float FontSize
  {
    get => (float) this.FontSizeHP / 2f;
    set => this.FontSizeHP = (ushort) ((double) value * 2.0);
  }

  internal ushort FontSizeHP
  {
    get => this.Sprms.GetUShort(19011, (ushort) 20);
    set => this.Sprms.SetUShortValue(19011, value);
  }

  internal byte FontColor
  {
    get => this.Sprms.GetByte(10818, (byte) 0);
    set => this.Sprms.SetByteValue(10818, value);
  }

  internal Color FontColorExt
  {
    get
    {
      uint rgb = this.Sprms.GetUInt(26736, uint.MaxValue);
      return rgb == uint.MaxValue ? WordColor.ConvertIdToColor((int) this.FontColor) : WordColor.ConvertRGBToColor(rgb);
    }
    set => this.Sprms.SetUIntValue(26736, WordColor.ConvertColorToRGB(value));
  }

  internal uint FontColorRGB
  {
    get
    {
      uint num = this.Sprms.GetUInt(26736, uint.MaxValue);
      return num == uint.MaxValue ? WordColor.ConvertIdToRGB((int) this.FontColor) : num;
    }
    set => this.Sprms.SetUIntValue(26736, value);
  }

  internal byte HighlightColor
  {
    get => this.Sprms.GetByte(10764, (byte) 0);
    set => this.Sprms.SetByteValue(10764, value);
  }

  internal byte SubSuperScript
  {
    get => this.Sprms.GetByte(10824, (byte) 0);
    set
    {
      byte num = value;
      switch (num)
      {
        case 0:
        case 1:
        case 2:
          this.Sprms.SetByteValue(10824, num);
          break;
      }
    }
  }

  internal byte Clear
  {
    get => this.Sprms.GetByte(10361, (byte) 0);
    set
    {
      byte num = value;
      switch (num)
      {
        case 0:
        case 1:
        case 2:
        case 3:
          this.Sprms.SetByteValue(10361, num);
          break;
      }
    }
  }

  internal int PicLocation
  {
    get
    {
      int picLocation = 0;
      if (this.Sprms[27139] != null)
        picLocation = this.Sprms.GetInt(27139, 0);
      return picLocation;
    }
    set => this.Sprms.SetIntValue(27139, value);
  }

  internal bool Outline
  {
    get => this.Sprms.GetBoolean(2104, false);
    set => this.Sprms.SetBoolValue(2104, value);
  }

  internal bool Shadow
  {
    get => this.Sprms.GetBoolean(2105, false);
    set => this.Sprms.SetBoolValue(2105, value);
  }

  internal bool Emboss
  {
    get => this.Sprms.GetBoolean(2136, false);
    set => this.Sprms.SetBoolValue(2136, value);
  }

  internal byte EmbossComplex
  {
    get => this.Sprms.GetByte(2136, byte.MaxValue);
    set
    {
      if (value == byte.MaxValue)
        return;
      this.Sprms.SetByteValue(2136, value);
    }
  }

  internal bool Engrave
  {
    get => this.Sprms.GetBoolean(2132, false);
    set => this.Sprms.SetBoolValue(2132, value);
  }

  internal byte EngraveComplex
  {
    get => this.Sprms.GetByte(2132, byte.MaxValue);
    set
    {
      if (value == byte.MaxValue)
        return;
      this.Sprms.SetByteValue(2132, value);
    }
  }

  internal bool Hidden
  {
    get => this.Sprms.GetBoolean(2108, false);
    set => this.Sprms.SetBoolValue(2108, value);
  }

  internal bool SpecVanish
  {
    get => this.Sprms.GetBoolean(2072, false);
    set => this.Sprms.SetBoolValue(2072, value);
  }

  internal bool SmallCaps
  {
    get => this.Sprms.GetBoolean(2106, false);
    set => this.Sprms.SetBoolValue(2106, value);
  }

  internal bool AllCaps
  {
    get => this.Sprms.GetBoolean(2107, false);
    set => this.Sprms.SetBoolValue(2107, value);
  }

  internal byte AllCapsComplex
  {
    get => this.Sprms.GetByte(2107, (byte) 0);
    set
    {
      if (value == byte.MaxValue)
        return;
      this.Sprms.SetByteValue(2107, value);
    }
  }

  internal short Position
  {
    get => this.Sprms.GetShort(18501, (short) 0);
    set => this.Sprms.SetShortValue(18501, value);
  }

  internal short LineSpacing
  {
    get => this.Sprms.GetShort(34880, (short) 0);
    set => this.Sprms.SetShortValue(34880, value);
  }

  internal ushort Scaling
  {
    get => this.Sprms.GetUShort(18514, (ushort) 0);
    set => this.Sprms.SetUShortValue(18514, value);
  }

  internal ushort Kern
  {
    get => this.Sprms.GetUShort(18507, (ushort) 0);
    set => this.Sprms.SetUShortValue(18507, value);
  }

  internal ShadingDescriptor Shading
  {
    get => new ShadingDescriptor(this.Sprms.GetShort(18534, (short) 0));
    set => this.Sprms.SetShortValue(18534, value.Save());
  }

  internal ShadingDescriptor ShadingNew
  {
    get
    {
      byte[] byteArray = this.Sprms.GetByteArray(51825);
      ShadingDescriptor shadingNew = new ShadingDescriptor();
      shadingNew.ReadNewShd(byteArray, 0);
      return shadingNew;
    }
    set => this.Sprms.SetByteArrayValue(51825, value.SaveNewShd());
  }

  internal BorderCode Border
  {
    get => new BorderCode(this.Sprms.GetByteArray(26725), 0);
    set
    {
      byte[] arr = new byte[4];
      value.SaveBytes(arr, 0);
      this.Sprms.SetByteArrayValue(26725, arr);
    }
  }

  internal bool StickProperties
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool Special
  {
    get => this.Sprms.GetBoolean(2133, false);
    set
    {
      if (value)
        this.Sprms.SetBoolValue(2133, value);
      else
        this.Sprms.RemoveValue(2133);
    }
  }

  internal SymbolDescriptor Symbol
  {
    get
    {
      byte[] byteArray = this.Sprms.GetByteArray(27145);
      SymbolDescriptor symbol = new SymbolDescriptor();
      if (byteArray != null)
        symbol.Parse(byteArray);
      return symbol;
    }
    set => this.Sprms.SetByteArrayValue(27145, value.Save());
  }

  internal byte HiddenComplex
  {
    get => this.Sprms.GetByte(2108, (byte) 0);
    set
    {
      if (value == byte.MaxValue)
        return;
      this.Sprms.SetByteValue(2108, value);
    }
  }

  internal byte DoubleStrikeComplex
  {
    get => this.Sprms.GetByte(10835, (byte) 0);
    set
    {
      if (value == byte.MaxValue)
        return;
      this.Sprms.SetByteValue(10835, value);
    }
  }

  internal byte SmallCapsComplex
  {
    get => this.Sprms.GetByte(2106, (byte) 0);
    set
    {
      if (value == byte.MaxValue)
        return;
      this.Sprms.SetByteValue(2106, value);
    }
  }

  internal bool FldVanish
  {
    get => this.Sprms.GetBoolean(2050, false);
    set => this.Sprms.SetBoolValue(2050, value);
  }

  internal byte FldVanishComplex
  {
    get => this.Sprms.GetByte(2050, (byte) 0);
    set
    {
      if (value == byte.MaxValue)
        return;
      this.Sprms.SetByteValue(2050, value);
    }
  }

  internal bool NoProof
  {
    get => this.Sprms.GetBoolean(2165, false);
    set => this.Sprms.SetBoolValue(2165, value);
  }

  internal byte IdctHint
  {
    get => this.Sprms.GetByte(10351, (byte) 0);
    set => this.Sprms.SetByteValue(10351, value);
  }

  internal bool IsInsertRevision
  {
    get => this.Sprms.GetBoolean(2049, false);
    set => this.Sprms.SetBoolValue(2049, value);
  }

  internal bool IsDeleteRevision
  {
    get => this.Sprms.GetBoolean(2048 /*0x0800*/, false);
    set => this.Sprms.SetBoolValue(2048 /*0x0800*/, value);
  }

  internal bool IsChangedFormat
  {
    get
    {
      if (this.Sprms.GetByteArray(51799) == null)
      {
        byte[] byteArray = this.Sprms.GetByteArray(51849);
        if (byteArray != null)
          return byteArray[0] == (byte) 1;
      }
      return false;
    }
    set
    {
      byte[] numArray = new byte[7];
      numArray[0] = (byte) 1;
      this.Sprms.SetByteArrayValue(51849, numArray);
    }
  }

  internal int ListPictureIndex
  {
    get
    {
      bool listHasImage = this.ListHasImage;
      int maxValue = int.MaxValue;
      if (listHasImage)
        maxValue = this.Sprms.GetInt(26759, int.MaxValue);
      return maxValue;
    }
    set
    {
      if (value == int.MaxValue)
        return;
      this.Sprms.SetIntValue(26759, value);
    }
  }

  internal bool ListHasImage
  {
    get => this.Sprms.GetBoolean(18568, false);
    set => this.Sprms.SetBoolValue(18568, value);
  }

  internal bool BoldBi
  {
    get => this.Sprms.GetBoolean(2140, false);
    set => this.Sprms.SetBoolValue(2140, value);
  }

  internal bool ItalicBi
  {
    get => this.Sprms.GetBoolean(2141, false);
    set => this.Sprms.SetBoolValue(2141, value);
  }

  internal bool Bidi
  {
    get => this.Sprms.GetBoolean(2138, false);
    set => this.Sprms.SetBoolValue(2138, value);
  }

  internal ushort FontSizeBi
  {
    get => (ushort) ((uint) this.Sprms.GetUShort(19041, (ushort) 1) / 2U);
    set => this.Sprms.SetUShortValue(19041, (ushort) ((uint) value * 2U));
  }

  internal WordStyleSheet StyleSheet
  {
    get => this.m_styleSheet;
    set => this.m_styleSheet = value;
  }

  internal bool IsOle2
  {
    get
    {
      SinglePropertyModifierRecord sprm = this.Sprms[2058];
      return sprm != null && sprm.BoolValue;
    }
    set => this.Sprms.SetBoolValue(2058, value);
  }

  internal bool IsData
  {
    get => this.Sprms.GetBoolean(2054, false);
    set => this.Sprms.SetBoolValue(2054, value);
  }

  internal ushort CharacterStyleId
  {
    get => this.Sprms.GetUShort(18992, (ushort) 0);
    set
    {
      if (value == (ushort) 0)
        return;
      this.Sprms.SetUShortValue(18992, value);
    }
  }

  internal short LocaleIdASCII
  {
    get => this.Sprms.GetShort(18541, (short) 1033);
    set
    {
      if (value == short.MaxValue)
        return;
      this.Sprms.SetShortValue(18541, value);
    }
  }

  internal short LocaleIdASCII1
  {
    get => this.Sprms.GetShort(18547, (short) 0);
    set
    {
      if (value == short.MaxValue)
        return;
      this.Sprms.SetShortValue(18547, value);
    }
  }

  internal short LocaleIdFarEast
  {
    get => this.Sprms.GetShort(18542, (short) 1033);
    set
    {
      if (value == short.MaxValue)
        return;
      this.Sprms.SetShortValue(18542, value);
    }
  }

  internal short LocaleIdFarEast1
  {
    get => this.Sprms.GetShort(18548, (short) 0);
    set
    {
      if (value == short.MaxValue)
        return;
      this.Sprms.SetShortValue(18548, value);
    }
  }

  internal short LidBi
  {
    get => this.Sprms.GetShort(18527, (short) 0);
    set
    {
      if (value == short.MaxValue)
        return;
      this.Sprms.SetShortValue(18527, value);
    }
  }

  internal bool HasOptions(int option) => this.Sprms[option] != null;

  internal SinglePropertyModifierArray GetCopiableSprm()
  {
    SinglePropertyModifierArray copiableSprm = new SinglePropertyModifierArray();
    int count = this.Sprms.Modifiers.Count;
    for (int sprmIndex = 0; sprmIndex < count; ++sprmIndex)
    {
      SinglePropertyModifierRecord sprmByIndex = this.Sprms.GetSprmByIndex(sprmIndex);
      switch (sprmByIndex.TypedOptions)
      {
        case 0:
        case 2101:
        case 2102:
        case 2103:
        case 2104:
        case 2105:
        case 2106:
        case 2107:
        case 2108:
        case 2132:
        case 2133:
        case 2136:
        case 2138:
        case 2140:
        case 2141:
        case 2165:
        case 2560 /*0x0A00*/:
        case 10351:
        case 10752:
        case 10764:
        case 10814:
        case 10818:
        case 10824:
        case 10835:
        case 18501:
        case 18527:
        case 18534:
        case 18541:
        case 18542:
        case 18547:
        case 18548:
        case 18944:
        case 18992:
        case 19011:
        case 19023:
        case 19024:
        case 19025:
        case 19038:
        case 19041:
        case 26624:
        case 26645:
        case 26646:
        case 26725:
        case 26736:
        case 26880:
        case 27136:
        case 27139:
        case 27145:
        case 34880:
        case 43264:
        case 43520:
        case 43776:
        case 51200:
        case 51825:
          continue;
        default:
          copiableSprm.Modifiers.Add(sprmByIndex);
          continue;
      }
    }
    return copiableSprm;
  }

  internal bool GetBoolean(SinglePropertyModifierRecord record) => record.BoolValue;

  internal string GetFontName(SinglePropertyModifierRecord record)
  {
    return (int) record.UshortValue >= this.m_styleSheet.FontNamesList.Count ? "Times New Roman" : this.m_styleSheet.FontNamesList[(int) record.UshortValue];
  }

  internal BorderCode GetBorder(SinglePropertyModifierRecord record)
  {
    return new BorderCode(record.ByteArray, 0);
  }

  internal ShadingDescriptor GetShading(SinglePropertyModifierRecord record)
  {
    byte[] byteArray = record.ByteArray;
    ShadingDescriptor shading;
    if (byteArray.Length == 2)
    {
      shading = new ShadingDescriptor(record.ShortValue);
    }
    else
    {
      shading = new ShadingDescriptor();
      shading.ReadNewShd(byteArray, 0);
    }
    return shading;
  }

  internal SymbolDescriptor GetSymbol(SinglePropertyModifierRecord record)
  {
    SymbolDescriptor symbol = new SymbolDescriptor();
    byte[] byteArray = record.ByteArray;
    if (byteArray != null)
      symbol.Parse(byteArray);
    return symbol;
  }

  internal Color GetColor(SinglePropertyModifierRecord record)
  {
    uint uintValue = record.UIntValue;
    return uintValue == uint.MaxValue ? WordColor.ConvertIdToColor((int) this.FontColor) : WordColor.ConvertRGBToColor(uintValue);
  }

  internal void SetAllFontNames(string fontName)
  {
    int index = this.m_styleSheet.FontNameToIndex(fontName);
    if (index >= 0)
    {
      this.Sprms.Add(new SinglePropertyModifierRecord(19023)
      {
        UshortValue = (ushort) index
      });
      this.Sprms.Add(new SinglePropertyModifierRecord(19024)
      {
        UshortValue = (ushort) index
      });
      this.Sprms.Add(new SinglePropertyModifierRecord(19025)
      {
        UshortValue = (ushort) index
      });
    }
    else
    {
      int count = (int) (ushort) this.m_styleSheet.FontNamesList.Count;
      SinglePropertyModifierRecord modifier1 = new SinglePropertyModifierRecord(19023);
      modifier1.UshortValue = (ushort) count;
      this.Sprms.Add(modifier1);
      SinglePropertyModifierRecord modifier2 = modifier1.Clone();
      modifier2.TypedOptions = 19024;
      this.Sprms.Add(modifier2);
      SinglePropertyModifierRecord modifier3 = modifier1.Clone();
      modifier3.TypedOptions = 19025;
      this.Sprms.Add(modifier3);
      this.m_styleSheet.UpdateFontName(fontName);
    }
  }

  internal void SetFontName(string fontName, int option)
  {
    int index = this.m_styleSheet.FontNameToIndex(fontName);
    if (index >= 0)
    {
      this.Sprms.Add(new SinglePropertyModifierRecord(option)
      {
        UshortValue = (ushort) index
      });
    }
    else
    {
      int count = (int) (ushort) this.m_styleSheet.FontNamesList.Count;
      this.Sprms.Add(new SinglePropertyModifierRecord(option)
      {
        UshortValue = (ushort) count
      });
      this.m_styleSheet.UpdateFontName(fontName);
    }
  }

  internal void AddSprmWithBoolValue(int option, bool value)
  {
    this.Sprms.Add(new SinglePropertyModifierRecord(option)
    {
      BoolValue = value
    });
  }

  internal void AddSprmWithByteValue(int option, byte value)
  {
    this.Sprms.Add(new SinglePropertyModifierRecord(option)
    {
      ByteValue = value
    });
  }

  internal void AddSprmWithUShortValue(int option, ushort value)
  {
    this.Sprms.Add(new SinglePropertyModifierRecord(option)
    {
      UshortValue = value
    });
  }

  internal void AddSprmWithShortValue(int option, short value)
  {
    this.Sprms.Add(new SinglePropertyModifierRecord(option)
    {
      ShortValue = value
    });
  }

  internal void AddSprmWithIntValue(int option, int value)
  {
    this.Sprms.Add(new SinglePropertyModifierRecord(option)
    {
      IntValue = value
    });
  }

  internal CharacterPropertyException CloneChpx()
  {
    CharacterPropertyException chpx = this.m_chpx;
    this.m_chpx = new CharacterPropertyException();
    if (this.StickProperties && chpx != null)
    {
      int sprmIndex = 0;
      for (int modifiersCount = chpx.ModifiersCount; sprmIndex < modifiersCount; ++sprmIndex)
      {
        if (chpx.PropertyModifiers.GetSprmByIndex(sprmIndex).Operand != null)
        {
          SinglePropertyModifierRecord modifier = chpx.PropertyModifiers.GetSprmByIndex(sprmIndex).Clone();
          if (modifier != null)
            this.m_chpx.PropertyModifiers.Add(modifier);
        }
      }
    }
    return chpx;
  }

  internal void RemoveSprm(int option)
  {
    List<SinglePropertyModifierRecord> modifiers = this.Sprms.Modifiers;
    int index = 0;
    for (int count = modifiers.Count; index < count; ++index)
    {
      if (modifiers[index].TypedOptions == option)
      {
        modifiers.RemoveAt(index);
        break;
      }
    }
  }

  internal bool HasSprms() => this.m_chpx != null && this.m_chpx.HasSprms();

  internal SinglePropertyModifierRecord GetNewSprm(int option)
  {
    SinglePropertyModifierRecord newSprm = (SinglePropertyModifierRecord) null;
    int newPropsStartIndex = this.GetNewPropsStartIndex();
    if (newPropsStartIndex == -1)
      return newSprm;
    int sprmIndex = newPropsStartIndex;
    for (int modifiersCount = this.m_chpx.ModifiersCount; sprmIndex < modifiersCount; ++sprmIndex)
    {
      SinglePropertyModifierRecord sprmByIndex = this.m_chpx.PropertyModifiers.GetSprmByIndex(sprmIndex);
      if (sprmByIndex.OptionType == (WordSprmOptionType) option)
        return sprmByIndex;
    }
    return (SinglePropertyModifierRecord) null;
  }

  private int GetNewPropsStartIndex()
  {
    SinglePropertyModifierRecord propertyModifier = this.m_chpx.PropertyModifiers[10883];
    return propertyModifier != null ? this.m_chpx.PropertyModifiers.Modifiers.IndexOf(propertyModifier) + 1 : -1;
  }

  private int ConvertColor(int brg)
  {
    byte[] bytes = BitConverter.GetBytes(brg);
    byte num = bytes[0];
    bytes[0] = bytes[2];
    bytes[2] = num;
    return BitConverter.ToInt32(bytes, 0);
  }

  private static bool GetComplexBoolean(byte sprmValue, bool styleSheetValue)
  {
    if (sprmValue < (byte) 128 /*0x80*/)
      return sprmValue == (byte) 1;
    if (sprmValue == (byte) 128 /*0x80*/)
      return styleSheetValue;
    if (sprmValue == (byte) 129)
      return !styleSheetValue;
    throw new Exception("Complex boolean value is expected.");
  }

  public override string ToString() => base.ToString();

  internal void Close()
  {
    if (this.m_chpx != null)
    {
      if (this.m_chpx.PropertyModifiers != null)
      {
        this.m_chpx.PropertyModifiers.Close();
        this.m_chpx.PropertyModifiers = (SinglePropertyModifierArray) null;
      }
      this.m_chpx = (CharacterPropertyException) null;
    }
    this.m_styleSheet = (WordStyleSheet) null;
  }
}
