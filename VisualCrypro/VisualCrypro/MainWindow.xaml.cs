using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using Microsoft.Win32;
using System.IO;

namespace VisualCrypro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Bitmap work;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void threshold(ref Bitmap img)
        {
            for(int i = 0; i < img.Height; ++i)
            {
                for(int j = 0; j < img.Width; ++j)
                {
                    var p = img.GetPixel(j, i);
                    int[] tmp = new int[4];
                    tmp[0] = p.R;
                    tmp[1] = p.G;
                    tmp[2] = p.B;
                    tmp[3] = p.A;
                    int avg = (int)tmp.Average();
                    avg = avg > 128 ? 255 : 0;
                    img.SetPixel(j, i, System.Drawing.Color.FromArgb(avg, avg, avg, avg));
                }
            }
        }

        private void save(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Title = "Save a picture";
                dialog.Filter = "Portable Network Graphic (*.png)|*.png|" + "Bmp files (*.bmp)|*.bmp";

                if (dialog.ShowDialog() == true)
                    work.Save(dialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Orginal message:" + ex.Message, "Błąd");
            }
        }

        private void open(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Title = "Select a picture";
                op.Filter = "Portable Network Graphic (*.png)|*.png|" + "Bmp files (*.bmp)|*.bmp";
                if (op.ShowDialog() == true)
                {
                    img.Source = new BitmapImage(new Uri(op.FileName));
                    work = new Bitmap(op.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Orginal message:" + ex.Message, "Błąd");
            }
        }

        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
        private void about(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void thres(object sender, RoutedEventArgs e)
        {
            threshold(ref work);
            img.Source = BitmapToImageSource(work);
        }

        private void hide()
        {
            List<bool[]> white = new List<bool[]>();
            white.Add(new bool[] { false, false, true, true, false, false, true, true } );
            white.Add(new bool[] { true, true, false, false, true, true, false, false } );
            white.Add(new bool[] { false, true, false ,true, false, true, false, true } );
            white.Add(new bool[] { true, false, true, false, true, false, true, false } );
            white.Add(new bool[] { false, true, true, false, false, true, true, false } );
            white.Add(new bool[] { true, false, false, true, true, false, false, true } );

            List<bool[]> black = new List<bool[]>();
            black.Add(new bool[] { true, false, false, true, false, true, true, false } );
            black.Add(new bool[] { false, true, true, false, true, false, false, true } );
            black.Add(new bool[] { false, false, true, true, true, true, false, false } );
            black.Add(new bool[] { true, true, false, false, false, false, true, true } );
            black.Add(new bool[] { false, true, false, true, true, false, true, false } );
            black.Add(new bool[] { true, false, true, false, false, true, false, true } );
        }
    }
}
