// Decompiled with JetBrains decompiler
// Type: Syncfusion.Licensing.LicenseError
// Assembly: Syncfusion.Licensing, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=632609b4d040f6b4
// MVID: 46253E3A-52AF-49F3-BF00-D811A33B7BC6
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Licensing.dll

using System.ComponentModel;

#nullable disable
namespace Syncfusion.Licensing;

[EditorBrowsable(EditorBrowsableState.Never)]
public enum LicenseError
{
  None,
  DesigntimeNoKey,
  DesigntimeKeyExpired,
  DesigntimeNotLicensedControl,
  RuntimeKeyExpired,
  RuntimeNotLicensedControl,
  RuntimeLicensedComponentEntryMissing,
  RuntimeNoLicx,
  Unknown,
}
