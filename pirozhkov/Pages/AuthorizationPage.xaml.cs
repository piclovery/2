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
using System.IO;
using System.Timers;
using System.Windows.Threading;

namespace pirozhkov
.Pages
{

    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    /// 

    public partial class AuthorizationPage : Page
    {

      

        private readonly Random _rnd = new Random();
        private int _count = 0;
        private string txt = "";
        private DispatcherTimer _timer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(10)
        };
        public AuthorizationPage()
        {
            InitializeComponent();
            _timer.Tick += _timer_Tick;
        }
        private void _timer_Tick(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            _timer.Stop();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
           
            if (TBoxLogin.Text != "" && TBoxPassword.Text != "")
            {
                if (App.Context.User.FirstOrDefault(p => p.UserLogin == TBoxLogin.Text && p.UserPassword == TBoxPassword.Text) is User user)
                {
                    App.CurrentUser = user;
                    TBoxLogin.Text = "";
                    TBoxPassword.Text = "";
                    NavigationService.Navigate(new PageContent());

                }
                else
                {                   
                    MessageBox.Show("Неверные данные!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    _count++;
                }
            }
            else
            {
                MessageBox.Show("Все поля должны быть заполнены!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (_count > 0)
            {
                txt = GenerateTextCapcha();
                
            }
            if (_count>1)
            {
                this.IsEnabled = false;
                _timer.Start();
            }
        }
        private char RandomLetter()
        {
            return (char)_rnd.Next(97, 123);
        }
        private char RandomDigit()
        {
            return (char)_rnd.Next(48, 58);
        }
        private string GenerateTextCapcha()
        {
            int count = 2;
            string str = "";
            for (int i = 0; i < count; i++)
            {
                switch (_rnd.Next(2))
                {
                    case 0:
                        str += RandomLetter();
                        break;
                    default: 
                        str += RandomDigit();
                        break;
                }
            }
            str = str.Insert(_rnd.Next(count), RandomLetter().ToString());
            str = str.Insert(_rnd.Next(count +1), RandomDigit().ToString());
            return str;
                
        }
        private Bitmap GenerateCapcha(int width, int height, string text)
        {
            
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                graphics.FillRectangle(System.Drawing.Brushes.LightBlue, 0, 0, bmp.Width, bmp.Height);
                for (int i = 0; i < text.Length; i++)
                {
                    Font font = new Font("Comic Sans MS", _rnd.Next(50, 75));
                    graphics.DrawString(text[i].ToString(), font, System.Drawing.Brushes.DarkGreen, _rnd.Next(25, 35) * i, _rnd.Next(height / 2));
                    font.Dispose();
                }
                
                graphics.Flush();
                graphics.Dispose();
                    
            }
            int x, y;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    int q = _rnd.Next(100);
                    if (q<=40)
                    {
                        bmp.SetPixel(x, y, System.Drawing.Color.Gray);
                    }
                }
            }
            return bmp;
        }
        private BitmapImage BitmapToImageSource (Bitmap bitmap)
        {
            using(MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
       

        private void MElementData_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            txt = GenerateTextCapcha();
        }

        private void BtnGest_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentUser = null;
            NavigationService.Navigate(new PageContent());
            
        }
    }
}
