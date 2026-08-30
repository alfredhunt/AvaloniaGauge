using Avalonia.Media;
using AvaloniaGauge.Controls;
using ReactiveUI;
using System;

namespace AvaloniaGauge.Demo.ViewModels;

public sealed class DialMarkerViewModel : ReactiveObject
{
    public Array Placements =>
        Enum.GetValues<DialMarkerPlacement>();

    private double _value;
    private string _text = string.Empty;
    private DialMarkerPlacement _placement = DialMarkerPlacement.Outside;
    private double _gap;
    private double _offset;
    private bool _showLine = true;
    private double _lineThickness = 2;
    private string _foreground = "#FFFFFF";
    private string _lineColor = "#FFFFFF";
    private double _fontSize = 14;

    public double Value
    {
        get => _value;
        set => this.RaiseAndSetIfChanged(ref _value, value);
    }

    public string Text
    {
        get => _text;
        set => this.RaiseAndSetIfChanged(ref _text, value);
    }

    public DialMarkerPlacement Placement
    {
        get => _placement;
        set => this.RaiseAndSetIfChanged(ref _placement, value);
    }

    public double Gap
    {
        get => _gap;
        set => this.RaiseAndSetIfChanged(ref _gap, value);
    }

    public double Offset
    {
        get => _offset;
        set => this.RaiseAndSetIfChanged(ref _offset, value);
    }

    public bool ShowLine
    {
        get => _showLine;
        set => this.RaiseAndSetIfChanged(ref _showLine, value);
    }

    public double LineThickness
    {
        get => _lineThickness;
        set => this.RaiseAndSetIfChanged(ref _lineThickness, value);
    }

    public string Foreground
    {
        get => _foreground;
        set => this.RaiseAndSetIfChanged(ref _foreground, value);
    }

    public string LineColor
    {
        get => _lineColor;
        set => this.RaiseAndSetIfChanged(ref _lineColor, value);
    }

    public double FontSize
    {
        get => _fontSize;
        set => this.RaiseAndSetIfChanged(ref _fontSize, value);
    }

    public IBrush ForegroundBrush => ParseBrush(Foreground);

    public IBrush LineBrush => ParseBrush(LineColor);

    private static IBrush ParseBrush(string value)
    {
        try
        {
            return Brush.Parse(value);
        }
        catch
        {
            return Brushes.Transparent;
        }
    }
}