using System.ComponentModel;
using System.Reflection;

namespace Painter.Data;

public record Binding(PropertyInfo BoundProperty, INotifyPropertyChanged BindingSource, PropertyInfo SourceProperty, BindingType Type);