// Decompiled with JetBrains decompiler
// Type: NLog.Conditions.ConditionLayoutExpression
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Internal;
using NLog.Layouts;
using System.Text;
using System.Threading;

#nullable disable
namespace NLog.Conditions;

internal sealed class ConditionLayoutExpression : ConditionExpression
{
  private readonly SimpleLayout _simpleLayout;
  private StringBuilder _fastObjectPool;

  public ConditionLayoutExpression(SimpleLayout layout) => this._simpleLayout = layout;

  public Layout Layout => (Layout) this._simpleLayout;

  public override string ToString() => this._simpleLayout.ToString();

  protected override object EvaluateNode(LogEventInfo context)
  {
    if (this._simpleLayout.IsSimpleStringText || !this._simpleLayout.ThreadAgnostic)
      return (object) this._simpleLayout.Render(context);
    StringBuilder stringBuilder = Interlocked.Exchange<StringBuilder>(ref this._fastObjectPool, (StringBuilder) null) ?? new StringBuilder();
    try
    {
      this._simpleLayout.RenderAppendBuilder(context, stringBuilder);
      return (object) stringBuilder.ToString();
    }
    finally
    {
      stringBuilder.ClearBuilder();
      Interlocked.Exchange<StringBuilder>(ref this._fastObjectPool, stringBuilder);
    }
  }
}
