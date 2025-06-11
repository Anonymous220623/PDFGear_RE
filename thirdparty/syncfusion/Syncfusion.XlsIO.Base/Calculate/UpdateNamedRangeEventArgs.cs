// Decompiled with JetBrains decompiler
// Type: Syncfusion.Calculate.UpdateNamedRangeEventArgs
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.Calculate;

internal class UpdateNamedRangeEventArgs : EventArgs
{
  internal string Name;
  internal string Address = string.Empty;
  internal bool IsFormulaUpdated;

  internal UpdateNamedRangeEventArgs(string Name) => this.Name = Name;
}
