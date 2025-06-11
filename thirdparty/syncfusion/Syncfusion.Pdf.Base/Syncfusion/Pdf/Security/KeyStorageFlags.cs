// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.KeyStorageFlags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

public enum KeyStorageFlags
{
  DefaultKeySet = 0,
  UserKeySet = 1,
  MachineKeySet = 2,
  Exportable = 4,
  UserProtected = 8,
  PersistKeySet = 16, // 0x00000010
}
