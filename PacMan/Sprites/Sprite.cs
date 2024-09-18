using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PacMan.Sprites
{
    public class Sprite
    {
        private BitmapSource _bitmap;
        private double _rotation;
        public Sprite(BitmapImage spriteSheet, Int32Rect sourceRect, bool isLetter = false)
        {
            var croppedBitman = new CroppedBitmap(spriteSheet, sourceRect);
            int scale = isLetter ? 4 : 2;
            var scaleTransform = new ScaleTransform(scale, scale);
            _bitmap = new TransformedBitmap(croppedBitman, scaleTransform);
        }

        public void Draw(DrawingContext dc, Point position)
        {
            dc.DrawImage(_bitmap, new Rect(position, new Size(_bitmap.Width, _bitmap.Height)));
        }

        public void Rotate(double angle)
        {
            RotateTransform rotateTransform = new(-_rotation);
            _bitmap = new TransformedBitmap(_bitmap, rotateTransform);
            _rotation = angle;
            rotateTransform = new RotateTransform(angle);
            _bitmap = new TransformedBitmap(_bitmap, rotateTransform);
        }
    }
}