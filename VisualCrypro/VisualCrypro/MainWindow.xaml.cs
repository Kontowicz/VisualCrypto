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

namespace VisualCrypro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
                    img.SetPixel(j, i, System.Drawing.Color.FromArgb(avg, avg, avg, avg));
                }
            }
        }

        private void save(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void open(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void about(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }



    }
}
