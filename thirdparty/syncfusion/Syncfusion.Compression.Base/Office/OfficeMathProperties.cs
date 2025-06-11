// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathProperties
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMathProperties
{
  internal const short BreakOnBinaryOperatorsKey = 58;
  internal const short BreakOnBinarySubtractionKey = 59;
  internal const short DefaultJustificationKey = 60;
  internal const short DisplayMathDefaultsKey = 61;
  internal const short InterEquationSpacingKey = 62;
  internal const short IntegralLimitLocationsKey = 63 /*0x3F*/;
  internal const short IntraEquationSpacingKey = 64 /*0x40*/;
  internal const short LeftMarginKey = 65;
  internal const short MathFontKey = 66;
  internal const short NAryLimitLocationKey = 67;
  internal const short PostParagraphSpacingKey = 68;
  internal const short PreParagraphSpacingKey = 69;
  internal const short RightMarginKey = 70;
  internal const short SmallFractionKey = 71;
  internal const short WrapIndentKey = 72;
  internal const short WrapRightKey = 73;
  private Dictionary<int, object> m_propertiesHash;
  private byte m_bFlags;

  internal BreakOnBinaryOperatorsType BreakOnBinaryOperators
  {
    get => (BreakOnBinaryOperatorsType) this.GetPropertyValue(58);
    set => this.SetPropertyValue(58, (object) value);
  }

  internal BreakOnBinarySubtractionType BreakOnBinarySubtraction
  {
    get => (BreakOnBinarySubtractionType) this.GetPropertyValue(59);
    set => this.SetPropertyValue(59, (object) value);
  }

  internal MathJustification DefaultJustification
  {
    get => (MathJustification) this.GetPropertyValue(60);
    set => this.SetPropertyValue(60, (object) value);
  }

  internal bool DisplayMathDefaults
  {
    get => (bool) this.GetPropertyValue(61);
    set => this.SetPropertyValue(61, (object) value);
  }

  internal int InterEquationSpacing
  {
    get => (int) this.GetPropertyValue(62);
    set => this.SetPropertyValue(62, (object) value);
  }

  internal LimitLocationType IntegralLimitLocations
  {
    get => (LimitLocationType) this.GetPropertyValue(63 /*0x3F*/);
    set => this.SetPropertyValue(63 /*0x3F*/, (object) value);
  }

  internal int IntraEquationSpacing
  {
    get => (int) this.GetPropertyValue(64 /*0x40*/);
    set => this.SetPropertyValue(64 /*0x40*/, (object) value);
  }

  internal int LeftMargin
  {
    get => (int) this.GetPropertyValue(65);
    set => this.SetPropertyValue(65, (object) value);
  }

  internal string MathFont
  {
    get => (string) this.GetPropertyValue(66);
    set => this.SetPropertyValue(66, (object) value);
  }

  internal LimitLocationType NAryLimitLocation
  {
    get => (LimitLocationType) this.GetPropertyValue(67);
    set => this.SetPropertyValue(67, (object) value);
  }

  internal int PostParagraphSpacing
  {
    get => (int) this.GetPropertyValue(68);
    set => this.SetPropertyValue(68, (object) value);
  }

  internal int PreParagraphSpacing
  {
    get => (int) this.GetPropertyValue(69);
    set => this.SetPropertyValue(69, (object) value);
  }

  internal int RightMargin
  {
    get => (int) this.GetPropertyValue(70);
    set => this.SetPropertyValue(70, (object) value);
  }

  internal bool SmallFraction
  {
    get => (bool) this.GetPropertyValue(71);
    set => this.SetPropertyValue(71, (object) value);
  }

  internal int WrapIndent
  {
    get => (int) this.GetPropertyValue(72);
    set => this.SetPropertyValue(72, (object) value);
  }

  internal bool WrapRight
  {
    get => (bool) this.GetPropertyValue(73);
    set => this.SetPropertyValue(73, (object) value);
  }

  internal bool IsDefault
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal Dictionary<int, object> PropertiesHash
  {
    get
    {
      if (this.m_propertiesHash == null)
        this.m_propertiesHash = new Dictionary<int, object>();
      return this.m_propertiesHash;
    }
  }

  internal object this[int key]
  {
    get => !this.PropertiesHash.ContainsKey(key) ? this.GetDefValue(key) : this.PropertiesHash[key];
    set
    {
      this.PropertiesHash[key] = value;
      this.IsDefault = false;
    }
  }

  internal OfficeMathProperties() => this.IsDefault = true;

  private object GetPropertyValue(int propKey) => this[propKey];

  internal void SetPropertyValue(int propKey, object value) => this[propKey] = value;

  private object GetDefValue(int key)
  {
    switch (key)
    {
      case 58:
        return (object) BreakOnBinaryOperatorsType.Before;
      case 59:
        return (object) BreakOnBinarySubtractionType.MinusMinus;
      case 60:
        return (object) MathJustification.CenterGroup;
      case 61:
        return (object) true;
      case 62:
        return (object) 0;
      case 63 /*0x3F*/:
        return (object) LimitLocationType.SubSuperscript;
      case 64 /*0x40*/:
        return (object) 0;
      case 65:
        return (object) 0;
      case 66:
        return (object) "CambriMath";
      case 67:
        return (object) LimitLocationType.UnderOver;
      case 68:
        return (object) 0;
      case 69:
        return (object) 0;
      case 70:
        return (object) 0;
      case 71:
        return (object) false;
      case 72:
        return (object) 1440;
      case 73:
        return (object) false;
      default:
        return (object) new ArgumentException("key has invalid value");
    }
  }

  internal bool HasValue(int propertyKey) => this.HasKey(propertyKey);

  internal bool HasKey(int key)
  {
    return this.PropertiesHash != null && this.PropertiesHash.ContainsKey(key);
  }

  internal OfficeMathProperties Clone() => (OfficeMathProperties) this.MemberwiseClone();
}
