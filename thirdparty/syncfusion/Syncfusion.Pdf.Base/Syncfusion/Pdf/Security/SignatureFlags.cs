// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.SignatureFlags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

[Flags]
internal enum SignatureFlags
{
  None = 0,
  SignaturesExists = 1,
  AppendOnly = 2,
}
