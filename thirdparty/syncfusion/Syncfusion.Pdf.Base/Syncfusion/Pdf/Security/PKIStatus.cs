// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.PKIStatus
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

[Flags]
internal enum PKIStatus
{
  Granted = 0,
  GrantedWithMods = 1,
  Rejection = 2,
  Waiting = Rejection | GrantedWithMods, // 0x00000003
  RevocationWarning = 4,
  RevocationNotification = RevocationWarning | GrantedWithMods, // 0x00000005
}
