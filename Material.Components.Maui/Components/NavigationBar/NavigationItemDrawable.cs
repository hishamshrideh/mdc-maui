﻿namespace Material.Components.Maui.Core.NavigationBar;

internal class NavigationItemDrawable
{
    private readonly NavigationItem view;
    internal NavigationItemDrawable(NavigationItem view)
    {
        this.view = view;
    }

    internal void Draw(SKCanvas canvas, SKRect bounds)
    {
        // this.view.ForegroundColor = MaterialColors.OnSurfaceVariant;
        canvas.Clear(this.view.BackgroundColour.ToSKColor());
        this.DrawOverlayLayer(canvas, bounds);
        this.DrawStateLayer(canvas, bounds);
        this.DrawActiveIndicator(canvas, bounds);
        this.DrawPathIcon(canvas, bounds);
        this.DrawImageIcon(canvas, bounds);
        this.DrawText(canvas, bounds);
        this.DrawRippleEffect(canvas, bounds);
    }

    private void DrawBackground(SKCanvas canvas, SKRect bounds)
    {
        var color = this.view.BackgroundColour.MultiplyAlpha(this.view.BackgroundOpacity);
        var radii = new CornerRadius(0).GetRadii();
        canvas.DrawBackground(bounds, color, radii);
    }

    private void DrawActiveIndicator(SKCanvas canvas, SKRect bounds)
    {
        if (!this.view.IsActived) return;
        canvas.Save();

        var percent = this.view.RipplePercent == 0f ? 1f : this.view.RipplePercent;
        var _bounds = new SKRect
        {
            Left = (bounds.Width / 2) - 16 - (16 * percent),
            Top = this.view.HasLabel ? 12 : bounds.MidY - 16,
            Right = (bounds.Width / 2) + 16 + (16 * percent),
            Bottom = this.view.HasLabel ? 44 : bounds.MidY + 16,
        };
        canvas.DrawBackground(_bounds, this.view.ActiveIndicatorColor, 16);

        canvas.Restore();
    }

    private void DrawOverlayLayer(SKCanvas canvas, SKRect bounds)
    {
        var radii = new CornerRadius(0).GetRadii();
        var _bounds = new SKRect(bounds.Left - 1, bounds.Top - 1, bounds.Right + 1, bounds.Bottom + 1);
        canvas.DrawOverlayLayer(_bounds, Elevation.Level2, radii);
    }

    private void DrawStateLayer(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();

        var _bounds = new SKRect
        {
            Left = (bounds.Width / 2) - 32,
            Top = this.view.HasLabel ? 12 : bounds.MidY - 16,
            Right = (bounds.Width / 2) + 32,
            Bottom = this.view.HasLabel ? 44 : bounds.MidY + 16,
        };
        var color = this.view.StateLayerColor.MultiplyAlpha(this.view.StateLayerOpacity);
        canvas.DrawBackground(_bounds, color, 16);

        canvas.Restore();
    }

    private void DrawPathIcon(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.Image is not null || this.view.Icon == IconKind.None) return;
        canvas.Save();

        var paint = new SKPaint
        {
            Color = this.view.ForegroundColor.MultiplyAlpha(this.view.ForegroundOpacity).ToSKColor(),
            IsAntialias = true,
        };
        if (!this.view.IsActived)
        {
            paint.IsStroke = true;
            paint.StrokeWidth = 2f;
        }
        var path = SKPath.ParseSvgPathData(this.view.Icon.GetData());
        var x = (bounds.Width / 2) - 12;
        var y = this.view.HasLabel ? 16 : bounds.MidY - 12;
        path.Offset(x, y);
        canvas.DrawPath(path, paint);

        canvas.Restore();
    }

    private void DrawImageIcon(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.Image is null || this.view.Image is null) return;
        canvas.Save();

        var paint = new SKPaint
        {
            IsAntialias = true,
            ColorFilter = SKColorFilter.CreateBlendMode(
               this.view.ForegroundColor.MultiplyAlpha(this.view.ForegroundOpacity).ToSKColor(),
                SKBlendMode.SrcIn)
        };
        if (!this.view.IsActived)
        {
            paint.IsStroke = true;
            paint.StrokeWidth = 2f;
        }
        var scale = 24 / this.view.Image.CullRect.Width;
        var x = (bounds.Width / 2) - 12;
        var y = this.view.HasLabel ? 16 : bounds.MidY - 12;
        var matrix = new SKMatrix
        {
            ScaleX = scale,
            ScaleY = scale,
            TransX = x,
            TransY = y,
            Persp2 = 1f
        };
        canvas.DrawPicture(this.view.Image, ref matrix, paint);

        canvas.Restore();
    }

    private void DrawText(SKCanvas canvas, SKRect bounds)
    {
        if (!this.view.HasLabel) return;
        canvas.Save();

        this.view.TextStyle.TextColor = this.view.ForegroundColor.MultiplyAlpha(this.view.ForegroundOpacity).ToSKColor();
        var x = bounds.MidX - (this.view.TextBlock.MeasuredWidth / 2);
        var y = 48 + ((16 - this.view.TextBlock.MeasuredHeight) / 2);
        this.view.TextBlock.Paint(canvas, new SKPoint(x, y));

        canvas.Restore();
    }

    private void DrawRippleEffect(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.RipplePercent < 0) return;
        var color = this.view.RippleColor;
        var _bounds = new SKRect
        {
            Left = (bounds.Width / 2) - 32,
            Top = this.view.HasLabel ? 12 : bounds.MidY - 16,
            Right = (bounds.Width / 2) + 32,
            Bottom = this.view.HasLabel ? 44 : bounds.MidY + 16,
        };
        var point = new SKPoint(_bounds.MidX, _bounds.MidY);
        canvas.DrawRippleEffect(_bounds, 16, 32, point, color, this.view.RipplePercent);
    }
}
