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
    private Color _foreground = Colors.White;
    private Color _lineColor = Colors.White;
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

    public Color Foreground
    {
        get => _foreground;
        set 
        {
            if (_foreground == value)
                return;

            this.RaiseAndSetIfChanged(ref _foreground, value);
            this.RaisePropertyChanged(nameof(ForegroundHexColor));
        }
    }

    public string ForegroundHexColor => $"#{Foreground.R:X2}{Foreground.G:X2}{Foreground.B:X2}";

    public Color LineColor
    {
        get => _lineColor;
        set
        {
            if (_lineColor == value)
                return;

            this.RaiseAndSetIfChanged(ref _lineColor, value);
            this.RaisePropertyChanged(nameof(LineColorHexColor));
        }
    }

    public string LineColorHexColor => $"#{LineColor.R:X2}{LineColor.G:X2}{LineColor.B:X2}";

    public double FontSize
    {
        get => _fontSize;
        set => this.RaiseAndSetIfChanged(ref _fontSize, value);
    }

    public IBrush ForegroundBrush => new SolidColorBrush(Foreground);

    public IBrush LineBrush => new SolidColorBrush(LineColor);

}