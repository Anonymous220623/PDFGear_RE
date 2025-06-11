// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.CharacterPropertyException
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class CharacterPropertyException : BaseWordRecord
{
  private byte m_btLength;
  protected SinglePropertyModifierArray m_arrSprms;

  internal SinglePropertyModifierArray PropertyModifiers
  {
    get
    {
      if (this.m_arrSprms == null)
        this.m_arrSprms = new SinglePropertyModifierArray();
      return this.m_arrSprms;
    }
    set => this.m_arrSprms = value;
  }

  internal int ModifiersCount => this.PropertyModifiers.Count;

  internal override int Length => 1 + this.PropertyModifiers.Length;

  internal ushort FontAscii
  {
    get => this.PropertyModifiers.GetUShort(19023, ushort.MaxValue);
    set => this.PropertyModifiers.SetUShortValue(19023, value);
  }

  internal ushort FontFarEast
  {
    get => this.PropertyModifiers.GetUShort(19024, (ushort) 0);
    set => this.PropertyModifiers.SetUShortValue(19024, value);
  }

  internal ushort FontNonFarEast
  {
    get => this.PropertyModifiers.GetUShort(19025, (ushort) 0);
    set => this.PropertyModifiers.SetUShortValue(19025, value);
  }

  internal CharacterPropertyException()
  {
  }

  internal CharacterPropertyException(byte[] arrData, int iOffset)
  {
    this.Parse(arrData, iOffset, arrData.Length);
  }

  internal CharacterPropertyException(UniversalPropertyException property)
  {
    this.PropertyModifiers.Parse(property.Data, 0, property.Data.Length);
  }

  internal CharacterPropertyException(byte[] arrData)
  {
    this.PropertyModifiers.Parse(arrData, 0, arrData.Length);
  }

  internal override void Parse(byte[] arrData, int iOffset, int iCount)
  {
    this.m_btLength = arrData[iOffset];
    ++iOffset;
    this.PropertyModifiers.Parse(arrData, iOffset, (int) this.m_btLength);
  }

  internal override int Save(byte[] arrData, int iOffset)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    byte length = (byte) this.m_arrSprms.Length;
    if (iOffset < 0 || iOffset + (int) length + 1 > arrData.Length)
      throw new ArgumentOutOfRangeException(nameof (iOffset));
    arrData[iOffset++] = length;
    if (this.m_arrSprms != null)
      this.m_arrSprms.Save(arrData, iOffset);
    return (int) length + 1;
  }

  internal int Save(BinaryWriter writer, Stream stream, int length)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (stream));
    byte length1 = (byte) (length - 1);
    writer.Write(length1);
    if (this.m_arrSprms != null)
      this.m_arrSprms.Save(writer, stream, (int) length1);
    return (int) length1 + 1;
  }

  internal bool HasSprms() => this.m_arrSprms != null && this.m_arrSprms.Count > 0;

  internal override void Close()
  {
    base.Close();
    if (this.m_arrSprms == null)
      return;
    this.m_arrSprms = (SinglePropertyModifierArray) null;
  }

  public bool Equals(CharacterPropertyException chpx)
  {
    bool flag = false;
    foreach (SinglePropertyModifierRecord propertyModifier1 in this.PropertyModifiers)
    {
      ushort options = propertyModifier1.Options;
      if (options <= (ushort) 18501)
      {
        if (options <= (ushort) 2178)
        {
          if (options <= (ushort) 2108)
          {
            switch ((int) options - 2048 /*0x0800*/)
            {
              case 0:
                SinglePropertyModifierRecord propertyModifier2 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                flag = propertyModifier2 != null && this.IsSprmEqual(propertyModifier1, propertyModifier2, SprmCompareType.ByteValue);
                goto label_88;
              case 1:
                SinglePropertyModifierRecord propertyModifier3 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                flag = propertyModifier3 != null && this.IsSprmEqual(propertyModifier1, propertyModifier3, SprmCompareType.ByteValue);
                goto label_88;
              case 2:
                SinglePropertyModifierRecord propertyModifier4 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                flag = propertyModifier4 != null && this.IsSprmEqual(propertyModifier1, propertyModifier4, SprmCompareType.ByteValue);
                goto label_88;
              case 3:
              case 4:
              case 5:
                break;
              case 6:
                SinglePropertyModifierRecord propertyModifier5 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                flag = propertyModifier5 != null && this.IsSprmEqual(propertyModifier1, propertyModifier5, SprmCompareType.ByteValue);
                goto label_88;
              default:
                if (options != (ushort) 2058)
                {
                  switch ((int) options - 2101)
                  {
                    case 0:
                      SinglePropertyModifierRecord propertyModifier6 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                      flag = propertyModifier6 != null && this.IsSprmEqual(propertyModifier1, propertyModifier6, SprmCompareType.ByteValue);
                      goto label_88;
                    case 1:
                      SinglePropertyModifierRecord propertyModifier7 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                      flag = propertyModifier7 != null && this.IsSprmEqual(propertyModifier1, propertyModifier7, SprmCompareType.ByteValue);
                      goto label_88;
                    case 2:
                      SinglePropertyModifierRecord propertyModifier8 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                      flag = propertyModifier8 != null && this.IsSprmEqual(propertyModifier1, propertyModifier8, SprmCompareType.ByteValue);
                      goto label_88;
                    case 3:
                      SinglePropertyModifierRecord propertyModifier9 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                      flag = propertyModifier9 != null && this.IsSprmEqual(propertyModifier1, propertyModifier9, SprmCompareType.ByteValue);
                      goto label_88;
                    case 4:
                      SinglePropertyModifierRecord propertyModifier10 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                      flag = propertyModifier10 != null && this.IsSprmEqual(propertyModifier1, propertyModifier10, SprmCompareType.ByteValue);
                      goto label_88;
                    case 5:
                      SinglePropertyModifierRecord propertyModifier11 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                      flag = propertyModifier11 != null && this.IsSprmEqual(propertyModifier1, propertyModifier11, SprmCompareType.ByteValue);
                      goto label_88;
                    case 6:
                      SinglePropertyModifierRecord propertyModifier12 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                      flag = propertyModifier12 != null && this.IsSprmEqual(propertyModifier1, propertyModifier12, SprmCompareType.ByteValue);
                      goto label_88;
                    case 7:
                      SinglePropertyModifierRecord propertyModifier13 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                      flag = propertyModifier13 != null && this.IsSprmEqual(propertyModifier1, propertyModifier13, SprmCompareType.ByteValue);
                      goto label_88;
                  }
                }
                else
                {
                  SinglePropertyModifierRecord propertyModifier14 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                  flag = propertyModifier14 != null && this.IsSprmEqual(propertyModifier1, propertyModifier14, SprmCompareType.ByteValue);
                  goto label_88;
                }
                break;
            }
          }
          else
          {
            switch ((int) options - 2132)
            {
              case 0:
                SinglePropertyModifierRecord propertyModifier15 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                flag = propertyModifier15 != null && this.IsSprmEqual(propertyModifier1, propertyModifier15, SprmCompareType.ByteValue);
                goto label_88;
              case 1:
                SinglePropertyModifierRecord propertyModifier16 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                flag = propertyModifier16 != null && this.IsSprmEqual(propertyModifier1, propertyModifier16, SprmCompareType.ByteValue);
                goto label_88;
              case 2:
                SinglePropertyModifierRecord propertyModifier17 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                flag = propertyModifier17 != null && this.IsSprmEqual(propertyModifier1, propertyModifier17, SprmCompareType.ByteValue);
                goto label_88;
              case 3:
              case 5:
              case 7:
                break;
              case 4:
                SinglePropertyModifierRecord propertyModifier18 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                flag = propertyModifier18 != null && this.IsSprmEqual(propertyModifier1, propertyModifier18, SprmCompareType.ByteValue);
                goto label_88;
              case 6:
                SinglePropertyModifierRecord propertyModifier19 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                flag = propertyModifier19 != null && this.IsSprmEqual(propertyModifier1, propertyModifier19, SprmCompareType.ByteValue);
                goto label_88;
              case 8:
                SinglePropertyModifierRecord propertyModifier20 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                flag = propertyModifier20 != null && this.IsSprmEqual(propertyModifier1, propertyModifier20, SprmCompareType.ByteValue);
                goto label_88;
              case 9:
                SinglePropertyModifierRecord propertyModifier21 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                flag = propertyModifier21 != null && this.IsSprmEqual(propertyModifier1, propertyModifier21, SprmCompareType.ByteValue);
                goto label_88;
              default:
                if (options != (ushort) 2165)
                {
                  if (options == (ushort) 2178)
                  {
                    SinglePropertyModifierRecord propertyModifier22 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                    flag = propertyModifier22 != null && this.IsSprmEqual(propertyModifier1, propertyModifier22, SprmCompareType.ByteValue);
                    goto label_88;
                  }
                  break;
                }
                SinglePropertyModifierRecord propertyModifier23 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                flag = propertyModifier23 != null && this.IsSprmEqual(propertyModifier1, propertyModifier23, SprmCompareType.ByteValue);
                goto label_88;
            }
          }
        }
        else if (options <= (ushort) 10814)
        {
          if (options != (ushort) 10351)
          {
            if (options != (ushort) 10764)
            {
              if (options == (ushort) 10814)
              {
                SinglePropertyModifierRecord propertyModifier24 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                flag = propertyModifier24 != null && this.IsSprmEqual(propertyModifier1, propertyModifier24, SprmCompareType.ByteValue);
                goto label_88;
              }
            }
            else
            {
              SinglePropertyModifierRecord propertyModifier25 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
              flag = propertyModifier25 != null && this.IsSprmEqual(propertyModifier1, propertyModifier25, SprmCompareType.ByteValue);
              goto label_88;
            }
          }
          else
          {
            SinglePropertyModifierRecord propertyModifier26 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
            flag = propertyModifier26 != null && this.IsSprmEqual(propertyModifier1, propertyModifier26, SprmCompareType.ByteValue);
            goto label_88;
          }
        }
        else if (options <= (ushort) 10824)
        {
          switch ((int) options - 10818)
          {
            case 0:
              SinglePropertyModifierRecord propertyModifier27 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
              flag = propertyModifier27 != null && this.IsSprmEqual(propertyModifier1, propertyModifier27, SprmCompareType.ByteValue);
              goto label_88;
            case 1:
              break;
            case 2:
              SinglePropertyModifierRecord propertyModifier28 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
              flag = propertyModifier28 != null && this.IsSprmEqual(propertyModifier1, propertyModifier28, SprmCompareType.ByteArray);
              goto label_88;
            default:
              if (options == (ushort) 10824)
              {
                SinglePropertyModifierRecord propertyModifier29 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                flag = propertyModifier29 != null && this.IsSprmEqual(propertyModifier1, propertyModifier29, SprmCompareType.ByteValue);
                goto label_88;
              }
              break;
          }
        }
        else if (options != (ushort) 10835)
        {
          if (options == (ushort) 18501)
          {
            SinglePropertyModifierRecord propertyModifier30 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
            flag = propertyModifier30 != null && this.IsSprmEqual(propertyModifier1, propertyModifier30, SprmCompareType.UShortValue);
            goto label_88;
          }
        }
        else
        {
          SinglePropertyModifierRecord propertyModifier31 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
          flag = propertyModifier31 != null && this.IsSprmEqual(propertyModifier1, propertyModifier31, SprmCompareType.ByteValue);
          goto label_88;
        }
      }
      else if (options <= (ushort) 19011)
      {
        if (options <= (ushort) 18534)
        {
          if (options != (ushort) 18514)
          {
            if (options != (ushort) 18527)
            {
              if (options == (ushort) 18534)
              {
                SinglePropertyModifierRecord propertyModifier32 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                flag = propertyModifier32 != null && this.IsSprmEqual(propertyModifier1, propertyModifier32, SprmCompareType.ShortValue);
                goto label_88;
              }
            }
            else
            {
              SinglePropertyModifierRecord propertyModifier33 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
              flag = propertyModifier33 != null && this.IsSprmEqual(propertyModifier1, propertyModifier33, SprmCompareType.ShortValue);
              goto label_88;
            }
          }
          else
          {
            SinglePropertyModifierRecord propertyModifier34 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
            flag = propertyModifier34 != null && this.IsSprmEqual(propertyModifier1, propertyModifier34, SprmCompareType.UShortValue);
            goto label_88;
          }
        }
        else if (options <= (ushort) 18548)
        {
          switch ((int) options - 18541)
          {
            case 0:
              SinglePropertyModifierRecord propertyModifier35 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
              flag = propertyModifier35 != null && this.IsSprmEqual(propertyModifier1, propertyModifier35, SprmCompareType.ShortValue);
              goto label_88;
            case 1:
              SinglePropertyModifierRecord propertyModifier36 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
              flag = propertyModifier36 != null && this.IsSprmEqual(propertyModifier1, propertyModifier36, SprmCompareType.ShortValue);
              goto label_88;
            default:
              switch ((int) options - 18547)
              {
                case 0:
                  SinglePropertyModifierRecord propertyModifier37 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                  flag = propertyModifier37 != null && this.IsSprmEqual(propertyModifier1, propertyModifier37, SprmCompareType.ShortValue);
                  goto label_88;
                case 1:
                  SinglePropertyModifierRecord propertyModifier38 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                  flag = propertyModifier38 != null && this.IsSprmEqual(propertyModifier1, propertyModifier38, SprmCompareType.ShortValue);
                  goto label_88;
              }
              break;
          }
        }
        else if (options != (ushort) 18992)
        {
          if (options == (ushort) 19011)
          {
            SinglePropertyModifierRecord propertyModifier39 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
            flag = propertyModifier39 != null && this.IsSprmEqual(propertyModifier1, propertyModifier39, SprmCompareType.UShortValue);
            goto label_88;
          }
        }
        else
        {
          SinglePropertyModifierRecord propertyModifier40 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
          flag = propertyModifier40 != null && this.IsSprmEqual(propertyModifier1, propertyModifier40, SprmCompareType.UShortValue);
          goto label_88;
        }
      }
      else if (options <= (ushort) 19041)
      {
        switch ((int) options - 19023)
        {
          case 0:
            SinglePropertyModifierRecord propertyModifier41 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
            flag = propertyModifier41 != null && this.IsSprmEqual(propertyModifier1, propertyModifier41, SprmCompareType.UShortValue);
            goto label_88;
          case 1:
            SinglePropertyModifierRecord propertyModifier42 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
            flag = propertyModifier42 != null && this.IsSprmEqual(propertyModifier1, propertyModifier42, SprmCompareType.UShortValue);
            goto label_88;
          case 2:
            SinglePropertyModifierRecord propertyModifier43 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
            flag = propertyModifier43 != null && this.IsSprmEqual(propertyModifier1, propertyModifier43, SprmCompareType.UShortValue);
            goto label_88;
          default:
            if (options != (ushort) 19038)
            {
              if (options == (ushort) 19041)
              {
                SinglePropertyModifierRecord propertyModifier44 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
                flag = propertyModifier44 != null && this.IsSprmEqual(propertyModifier1, propertyModifier44, SprmCompareType.UShortValue);
                goto label_88;
              }
              break;
            }
            SinglePropertyModifierRecord propertyModifier45 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
            flag = propertyModifier45 != null && this.IsSprmEqual(propertyModifier1, propertyModifier45, SprmCompareType.UShortValue);
            goto label_88;
        }
      }
      else if (options <= (ushort) 26759)
      {
        if (options != (ushort) 26736)
        {
          if (options == (ushort) 26759)
          {
            SinglePropertyModifierRecord propertyModifier46 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
            flag = propertyModifier46 != null && this.IsSprmEqual(propertyModifier1, propertyModifier46, SprmCompareType.UIntValue);
            goto label_88;
          }
        }
        else
        {
          SinglePropertyModifierRecord propertyModifier47 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
          flag = propertyModifier47 != null && this.IsSprmEqual(propertyModifier1, propertyModifier47, SprmCompareType.UIntValue);
          goto label_88;
        }
      }
      else if (options != (ushort) 27139)
      {
        if (options == (ushort) 34880)
        {
          SinglePropertyModifierRecord propertyModifier48 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
          flag = propertyModifier48 != null && this.IsSprmEqual(propertyModifier1, propertyModifier48, SprmCompareType.ShortValue);
          goto label_88;
        }
      }
      else
      {
        SinglePropertyModifierRecord propertyModifier49 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
        flag = propertyModifier49 != null && this.IsSprmEqual(propertyModifier1, propertyModifier49, SprmCompareType.IntValue);
        goto label_88;
      }
      SinglePropertyModifierRecord propertyModifier50 = chpx.PropertyModifiers[(int) propertyModifier1.Options];
      flag = propertyModifier50 != null && this.IsSprmEqual(propertyModifier1, propertyModifier50, SprmCompareType.ByteArray);
label_88:
      if (!flag)
        return flag;
    }
    return flag;
  }

  private bool IsSprmEqual(
    SinglePropertyModifierRecord sprm,
    SinglePropertyModifierRecord prevSprm,
    SprmCompareType sprmCompareType)
  {
    switch (sprmCompareType)
    {
      case SprmCompareType.Boolean:
        return sprm.BoolValue == prevSprm.BoolValue;
      case SprmCompareType.ByteArray:
        for (int index = 0; index < sprm.ByteArray.Length; ++index)
        {
          if ((int) sprm.ByteArray[index] != (int) prevSprm.ByteArray[index])
            return false;
        }
        return true;
      case SprmCompareType.ByteValue:
        return (int) sprm.ByteValue == (int) prevSprm.ByteValue;
      case SprmCompareType.IntValue:
        return sprm.IntValue == prevSprm.IntValue;
      case SprmCompareType.ShortValue:
        return (int) sprm.ShortValue == (int) prevSprm.ShortValue;
      case SprmCompareType.UIntValue:
        return (int) sprm.UIntValue == (int) prevSprm.UIntValue;
      case SprmCompareType.UShortValue:
        return (int) sprm.UshortValue == (int) prevSprm.UshortValue;
      default:
        return false;
    }
  }
}
