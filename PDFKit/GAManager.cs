// Decompiled with JetBrains decompiler
// Type: PDFKit.GAManager
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;

#nullable disable
namespace PDFKit;

internal static class GAManager
{
  internal static void SendEvent(
    string strCategory,
    string strAction,
    string strLabel,
    long value)
  {
    Action<string, string, string, long> sendEventAction = Common.sendEventAction;
    if (sendEventAction == null)
      return;
    sendEventAction(strCategory, strAction, strLabel, value);
  }
}
