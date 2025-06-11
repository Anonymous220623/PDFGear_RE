// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.PropertySupport
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Syncfusion.Windows.Shared;

public static class PropertySupport
{
  public static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
  {
    if (propertyExpression == null)
      throw new ArgumentNullException(nameof (propertyExpression));
    if (!(propertyExpression.Body is MemberExpression body))
      throw new ArgumentException(SharedLocalizationResourceAccessor.Instance.GetString("PropertySupport_NotMemberAccessExpression_Exception"), nameof (propertyExpression));
    PropertyInfo member = body.Member as PropertyInfo;
    if (member == (PropertyInfo) null)
      throw new ArgumentException(SharedLocalizationResourceAccessor.Instance.GetString("PropertySupport_ExpressionNotProperty_Exception"), nameof (propertyExpression));
    if (member.GetGetMethod(true).IsStatic)
      throw new ArgumentException(SharedLocalizationResourceAccessor.Instance.GetString("PropertySupport_StaticExpression_Exception"), nameof (propertyExpression));
    return body.Member.Name;
  }
}
