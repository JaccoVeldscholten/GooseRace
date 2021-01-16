using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GooseGUI {
    public static class Images {

        private static Dictionary<string, Bitmap> bitmapCache;

        public static void Init() {
            bitmapCache = new Dictionary<string, Bitmap>();
        }
        public static Bitmap GetBitmap(string imageName) {
            if (!bitmapCache.ContainsKey(imageName)) {
                Bitmap tempBitmap = new Bitmap(imageName);
                bitmapCache.Add(imageName, tempBitmap);
            }
            return bitmapCache[imageName];
        }

        public static void ClearCache() {
            bitmapCache.Clear();
        }

        public static Bitmap EmptyBitmap(int width, int height) {
            string emptyKey = "empty";
            if (!bitmapCache.ContainsKey(emptyKey)) {
                bitmapCache.Add(emptyKey, new Bitmap(width, height));
                Graphics g = Graphics.FromImage(bitmapCache[emptyKey]);
                System.Drawing.Color GooseBlue = System.Drawing.Color.FromArgb(68, 108, 206);
                g.Clear(GooseBlue);
            }
            return (Bitmap)bitmapCache[emptyKey].Clone();
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
            } finally {
                bitmap.UnlockBits(bitmapData);
            }
        }
    }
}
