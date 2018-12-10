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
using System.Diagnostics;

namespace VisualCrypro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Bitmap work = null;
        private Random random = new Random();
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
            try
            {
                System.Diagnostics.Process.Start(@"..\..\about.html");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Orginal message:" + ex.Message, "Błąd");
            }
        }

        private void hideTwoPixel()
        {
            List<bool[]> white = new List<bool[]>();
            white.Add(new bool[] { false, true, false, true });
            white.Add(new bool[] { true, false, true, false });

            List<bool[]> black = new List<bool[]>();
            black.Add(new bool[] { true, false, false, true });
            black.Add(new bool[] { false, true, true, false });

            Bitmap part_1 = new Bitmap(work.Width * 2, work.Height);
            Bitmap part_2 = new Bitmap(work.Width * 2, work.Height);

            for (int i = 0; i < work.Height; ++i)
            {
                for (int j = 0; j < work.Width; ++j)
                {
                    var p = work.GetPixel(j, i);
                    bool[] v = new bool[4];
                    if (p.B == 255)
                        v = white.ElementAt(random.Next(2));
                    else
                        v = black.ElementAt(random.Next(2));

                    int[] value = new int[8];
                    for (int z = 0; z < 4; z++)
                        value[z] = v[z] == true ? 255 : 0;

                    part_1.SetPixel(2 * j, i, System.Drawing.Color.FromArgb(value[0], value[0], value[0], value[0]));
                    part_1.SetPixel((2 * j) + 1, i, System.Drawing.Color.FromArgb(value[1], value[1], value[1], value[1]));

                    part_2.SetPixel(2 * j, i, System.Drawing.Color.FromArgb(value[2], value[2], value[2], value[2]));
                    part_2.SetPixel((2 * j) + 1, i, System.Drawing.Color.FromArgb(value[3], value[3], value[3], value[3]));
                }
            }
            System.IO.Directory.CreateDirectory(@".\Parts\");
            string dirName = DateTime.Now.ToString("yyyy_dd_M HH_mm_ss");
            System.IO.Directory.CreateDirectory(@".\Parts\" + dirName);

            part_1.Save(@".\Parts\" + dirName + @"\part_1.bmp");
            part_2.Save(@".\Parts\" + dirName + @"\part_2.bmp");
        }

        private void hideFourPixel()
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

            Bitmap part_1 = new Bitmap(work.Width * 2, work.Height * 2);
            Bitmap part_2 = new Bitmap(work.Width * 2, work.Height * 2);

            for (int i = 0; i < work.Height; ++i)
            {
                for (int j = 0; j < work.Width; ++j)
                {
                    var p = work.GetPixel(j, i);
                    bool[] v = new bool[8];
                    if (p.B == 255)
                        v = white.ElementAt(random.Next(6));
                    else
                        v = black.ElementAt(random.Next(6));

                    int[] value = new int[8];
                    for (int z = 0; z < 6; z++)
                        value[z] = v[z] == true ? 255 : 0;

                    part_1.SetPixel(2 * j, 2 * i, System.Drawing.Color.FromArgb(value[0], value[0], value[0], value[0]));
                    part_1.SetPixel(2 * j, (2 * i) + 1, System.Drawing.Color.FromArgb(value[1], value[1], value[1], value[1]));

                    part_1.SetPixel((2 * j) + 1, 2 * i, System.Drawing.Color.FromArgb(value[2], value[2], value[2], value[2]));
                    part_1.SetPixel((2 * j) + 1, (2 * i) + 1, System.Drawing.Color.FromArgb(value[3], value[3], value[3], value[3]));

                    part_2.SetPixel(2 * j, 2 * i, System.Drawing.Color.FromArgb(value[4], value[4], value[4], value[4]));
                    part_2.SetPixel(2 * j, (2 * i) + 1, System.Drawing.Color.FromArgb(value[5], value[5], value[5], value[5]));

                    part_2.SetPixel((2 * j) + 1, 2 * i, System.Drawing.Color.FromArgb(value[6], value[6], value[6], value[6]));
                    part_2.SetPixel((2 * j) + 1, (2 * i) + 1, System.Drawing.Color.FromArgb(value[7], value[7], value[7], value[7]));
                }
            }
            System.IO.Directory.CreateDirectory(@".\Parts\");
            string dirName = DateTime.Now.ToString("yyyy_dd_M HH_mm_ss");
            System.IO.Directory.CreateDirectory(@".\Parts\" + dirName);

            part_1.Save(@".\Parts\" + dirName + @"\part_1.bmp");
            part_2.Save(@".\Parts\" + dirName + @"\part_2.bmp");
        }

        private void hide(object sender, RoutedEventArgs e)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                threshold(ref work);
                img.Source = BitmapToImageSource(work);
                if (two.IsChecked == true)
                    hideTwoPixel();
                else
                    hideTwoPixel();
                stopwatch.Stop();
                MessageBox.Show("Operacje zajęły: " + stopwatch.Elapsed, "Czas");
            }catch(Exception ex)
            {
                MessageBox.Show("Orginal message:" + ex.Message, "Błąd");
            }
        }

        private Bitmap mergeFourPixel(Bitmap part_1, Bitmap part_2)
        {
            Bitmap result = new Bitmap(part_1.Width / 2, part_1.Height / 2);

            List<bool[]> white = new List<bool[]>();
            white.Add(new bool[] { false, false, true, true, false, false, true, true });
            white.Add(new bool[] { true, true, false, false, true, true, false, false });
            white.Add(new bool[] { false, true, false, true, false, true, false, true });
            white.Add(new bool[] { true, false, true, false, true, false, true, false });
            white.Add(new bool[] { false, true, true, false, false, true, true, false });
            white.Add(new bool[] { true, false, false, true, true, false, false, true });

            for (int i = 0; i < result.Height; ++i)
            {
                for (int j = 0; j < result.Width; ++j)
                {
                    bool[] tmp = new bool[8];

                    tmp[0] = part_1.GetPixel(2 * j, 2 * i).B == 255 ? true : false;
                    tmp[1] = part_1.GetPixel(2 * j, (2 * i) + 1).B == 255 ? true : false;
                    tmp[2] = part_1.GetPixel((2 * j) + 1, 2 * i).B == 255 ? true : false;
                    tmp[3] = part_1.GetPixel((2 * j) + 1, (2 * i) + 1).B == 255 ? true : false;

                    tmp[4] = part_2.GetPixel(2 * j, 2 * i).B == 255 ? true : false;
                    tmp[5] = part_2.GetPixel(2 * j, (2 * i) + 1).B == 255 ? true : false;
                    tmp[6] = part_2.GetPixel((2 * j) + 1, 2 * i).B == 255 ? true : false;
                    tmp[7] = part_2.GetPixel((2 * j) + 1, (2 * i) + 1).B == 255 ? true : false;

                    if (white.ElementAt(0).SequenceEqual(tmp) ||
                        white.ElementAt(1).SequenceEqual(tmp) ||
                        white.ElementAt(2).SequenceEqual(tmp) ||
                        white.ElementAt(3).SequenceEqual(tmp) ||
                        white.ElementAt(4).SequenceEqual(tmp) ||
                        white.ElementAt(5).SequenceEqual(tmp) 
                        )
                    {
                        result.SetPixel(j, i, System.Drawing.Color.FromArgb(0,0,0,0));
                    }
                    else
                    {
                        result.SetPixel(j, i, System.Drawing.Color.FromArgb(255,255,255,255));
                    }
                }
            }

            return result;
        }

        private Bitmap mergeTwoPixel(Bitmap part_1, Bitmap part_2)
        {
            Bitmap result = new Bitmap(part_1.Width / 2, part_1.Height);

            List<bool[]> white = new List<bool[]>();
            white.Add(new bool[] { false, true, false, true });
            white.Add(new bool[] { true, false, true, false });

            for (int i = 0; i < result.Height; ++i)
            {
                for (int j = 0; j < result.Width; ++j)
                {
                    bool[] tmp = new bool[4];

                    tmp[0] = part_1.GetPixel(2 * j, i).B == 255 ? true : false;
                    tmp[1] = part_1.GetPixel((2 * j) + 1, i).B == 255 ? true : false;
                    
                    tmp[2] = part_2.GetPixel(2 * j, i).B == 255 ? true : false;
                    tmp[3] = part_2.GetPixel((2 * j) + 1, i).B == 255 ? true : false;

                    if (white.ElementAt(0).SequenceEqual(tmp) ||
                        white.ElementAt(1).SequenceEqual(tmp)                        
                        )
                    {
                        result.SetPixel(j, i, System.Drawing.Color.FromArgb(0, 0, 0, 0));
                    }
                    else
                    {
                        result.SetPixel(j, i, System.Drawing.Color.FromArgb(255, 255, 255, 255));
                    }
                }
            }

            return result;
        }

        private void merge(object sender, RoutedEventArgs e)
        {
            Bitmap part_1 = null;
            Bitmap part_2 = null;
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Title = "Select a picture";
                op.Filter = "Portable Network Graphic (*.png)|*.png|" + "Bmp files (*.bmp)|*.bmp";
                if (op.ShowDialog() == true)

                    part_1 = new Bitmap(op.FileName);

                if (op.ShowDialog() == true)
                    part_2 = new Bitmap(op.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Orginal message:" + ex.Message, "Błąd");
            }
            try
            {
                work = two.IsChecked == true ? mergeTwoPixel(part_1, part_2) : mergeFourPixel(part_1, part_2);
                img.Source = BitmapToImageSource(work);
            } catch(Exception ex)
            {
                MessageBox.Show("Orginal message:" + ex.Message, "Błąd");
            }

        }
    }
}
