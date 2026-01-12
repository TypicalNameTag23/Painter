using System.ComponentModel;
using Painter.Data;

namespace Painter.Drawables;

public partial class Drawable
{
    private Dictionary<string, Binding> _boundProperties = [];

    public object? DataContext
    {
        get => field ?? Parent?.DataContext;
        set
        {
            var oldValue = field;
            if (!SetAndRedrawIfChanged(ref field, value))
            {
                return;
            }

            if (oldValue is INotifyPropertyChanged oldNotifier)
            {
                oldNotifier.PropertyChanged -= OnPropertyChanged;
            }

            if (value is INotifyPropertyChanged newNotifier)
            {
                newNotifier.PropertyChanged += OnPropertyChanged;
            }
        }
    }
    
    public Drawable Bind(string boundPropertyName, string sourcePropertyName, BindingType bindingType)
    {
        if (DataContext is null)
        {
            throw new ArgumentException("Trying to bind to null DataContext.", sourcePropertyName);
        }
        
        if (_boundProperties.ContainsKey(boundPropertyName))
        {
            throw new ArgumentException($"Property {boundPropertyName} has already been bound.", boundPropertyName);
        }
        
        var boundProperty = this
            .GetType()
            .GetProperty(boundPropertyName);
        
        if (boundProperty is null)
        {
            throw new ArgumentException($"Could not find the bound property {boundProperty} on the type {GetType().Name}", boundPropertyName);
        }
        
        var sourceProperty = DataContext
            .GetType()
            .GetProperty(sourcePropertyName);
        
        if (sourceProperty is null)
        {
            throw new ArgumentException($"Could not find the source property {sourceProperty} on the type {DataContext.GetType().Name}", sourcePropertyName);
        }

        // todo: allow traversing the drawable tree to get datacontext
        if (DataContext is not INotifyPropertyChanged notifier)
        {
            return this;
        }
        
        _boundProperties.Add(sourcePropertyName, new Binding(boundProperty, notifier, sourceProperty, bindingType));
        return this;
    }
    
    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName is null)
        {
            return;
        }

        if (!_boundProperties.TryGetValue(args.PropertyName, out var binding))
        {
            return;
        }

        var value = binding.SourceProperty.GetValue(binding.BindingSource);
        binding.BoundProperty.SetValue(this, 
            binding.BoundProperty.PropertyType == typeof(string) 
                ? value?.ToString()
                : value);
    }
}