// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.EmptyCollectionToObjectConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Collections;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class EmptyCollectionToObjectConverter : EmptyObjectToObjectConverter
{
  protected override bool CheckValueIsEmpty(object value)
  {
    bool flag = true;
    if (value is IEnumerable enumerable)
      flag = !enumerable.GetEnumerator().MoveNext();
    return flag;
  }
}
