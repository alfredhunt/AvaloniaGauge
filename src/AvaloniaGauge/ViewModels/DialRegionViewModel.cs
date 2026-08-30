using Avalonia.Media;
using ReactiveUI;

namespace AvaloniaGauge.Demo.ViewModels;

public sealed class DialRegionViewModel : ReactiveObject
{
    private double _start;
    private double _end;
    private Color _color;
    private double _thickness;

    public DialRegionViewModel(
        double start = 0,
        double end = 100,
        Color color = default,
        double thickness = 0)
    {
        _start = start;
        _end = end;
        _color = color;
        _thickness = thickness;
    }
        public double Start
    {
        get => _start;
        set => this.RaiseAndSetIfChanged(ref _start, value);
    }

    public double End
    {
        get => _end;
        set => this.RaiseAndSetIfChanged(ref _end, value);
    }

    public Color Color
    {
        get => _color;
        set
        {
            if (_color == value)
                return;

            this.RaiseAndSetIfChanged(ref _color, value);
            this.RaisePropertyChanged(nameof(HexColor));
        }
    }

    public string HexColor => $"#{Color.R:X2}{Color.G:X2}{Color.B:X2}";

    public double Thickness
    {
        get => _thickness;
        set => this.RaiseAndSetIfChanged(ref _thickness, value);
    }
}