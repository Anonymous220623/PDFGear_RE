// Decompiled with JetBrains decompiler
// Type: Syncfusion.Licensing.LicenseType
// Assembly: Syncfusion.Licensing, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=632609b4d040f6b4
// MVID: 46253E3A-52AF-49F3-BF00-D811A33B7BC6
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Licensing.dll

using System.ComponentModel;

#nullable disable
namespace Syncfusion.Licensing;

[EditorBrowsable(EditorBrowsableState.Never)]
public enum LicenseType
{
  [Description("")] Licensed,
  [Description("This application was built using a trial version of Syncfusion Essential Studio. Please include a valid license to permanently remove this license validation message. You can also obtain a free 30 day evaluation license to temporarily remove this message during the evaluation period. Please refer to this help topic(https://help.syncfusion.com/common/essential-studio/licensing/licensing-errors#license-key-not-registered) for more information.")] Unlicensed,
  [Description("")] Trial,
  [Description("Your Syncfusion trial license has expired. Please refer to this help topic(https://help.syncfusion.com/common/essential-studio/licensing/licensing-errors#trial-expired) for more information.")] TrialExpired,
  [Description("The included Syncfusion license (v##LicenseVersion) is invalid for version ##Requireversion. Please refer to this help topic(https://help.syncfusion.com/common/essential-studio/licensing/licensing-errors#version-mismatch) for more information.")] VersionMismatch,
  [Description("The included Syncfusion license is invalid (Platform mismatch). Please refer to this help topic(https://help.syncfusion.com/common/essential-studio/licensing/licensing-errors#platform-mismatch) for more information.")] PlatformMismatch,
  [Description("The included Syncfusion license is invalid. Please refer to this help topic(https://help.syncfusion.com/common/essential-studio/licensing/licensing-errors#invalid-key) for more information.")] WrongKey,
  [Description("This application was built using a trial version of Syncfusion Essential Studio. Please include a valid license to permanently remove this license validation message. You can also obtain a free 30 day evaluation license to temporarily remove this message during the evaluation period. Please refer to this help topic(https://help.syncfusion.com/common/essential-studio/licensing/licensing-errors#license-key-not-registered) for more information.")] NoLicense,
}
