// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Enums
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal sealed class Enums
{
  private Enums()
  {
  }

  internal static Enum GetEnumValue(Type enumType, string s)
  {
    if (!enumType.IsEnum)
      throw new ArgumentException("Not an enumeration");
    s = s.Length > 0 && char.IsLetter(s[0]) && s.IndexOf(',') < 0 ? s.Replace('-', '_') : throw new ArgumentException();
    s = s.Replace('/', '_');
    return (Enum) Enum.Parse(enumType, s, false);
  }

  internal static Array GetEnumValues(Type enumType)
  {
    return enumType.IsEnum ? Enum.GetValues(enumType) : throw new ArgumentException("Not an enumeration type", nameof (enumType));
  }

  internal static Enum GetArbitraryValue(Type enumType)
  {
    Array enumValues = Enums.GetEnumValues(enumType);
    int index = (int) ((DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000L & (long) int.MaxValue) % enumValues.Length;
    return (Enum) enumValues.GetValue(index);
  }
}
