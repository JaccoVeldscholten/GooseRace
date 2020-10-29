using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RaceGui {
    public static class ImageCache {
        //Dictionary to keep track of the bitmaps from the img url
        private static Dictionary<string, Bitmap> _cache = new Dictionary<string, Bitmap>();

        //Get a specific bitmap from dictionary based on URL
        public static Bitmap GetBitmap(string imageURL) {
            if (_cache.ContainsKey(imageURL)) {
                return _cache[imageURL];
            }
            //If url does not have bitmap create 1 and return
            else {
                _cache.Add(imageURL, new Bitmap(imageURL));
                return _cache[imageURL];
            }
        }

        public static Bitmap CreateEmptyBitmap(int width, int height) {
            if (!_cache.ContainsKey("empty")) {
                _cache.Add("empty", new Bitmap(width, height));
                Graphics g = Graphics.FromImage(_cache["empty"]);
                g.Clear(System.Drawing.Color.Red); 
            }
            return (Bitmap)_cache["empty"].Clone();
        }
        public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap) {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try {
                var size = (rect.Width * rect.Height) * 4;

                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally {
                bitmap.UnlockBits(bitmapData);
            }
        }

        public static void ClearCache() {
            _cache.Clear();
        }
    }
}
