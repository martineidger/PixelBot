using System;
using System.Windows;
using System.Drawing;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using System.Runtime.InteropServices;

namespace PixelBot
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const UInt32 MOUSEEVENT_LEFTDOWN = 0x0002;
        private const UInt32 MOUSEEVENT_LEFTUP = 0x0004;

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dX, uint dY,uint dwData, uint dwExtraInfo);
        [DllImport("user32.dll")]
        private static extern void SetCursorPos(int x, int y);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Click()
        {
            mouse_event(MOUSEEVENT_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENT_LEFTUP, 0, 0, 0, 0);
        }

        private void DoubleClickAtPosition(int posX, int posY)
        {
            SetCursorPos(posX, posY);
            Click();
            System.Threading.Thread.Sleep(250);
            Click();
        }
        private void OnButtonSeachPixelClick(object sender, RoutedEventArgs e)
        {
            string input = HexOfPixel.Text;
            GetPixelFromHex(input);
        }

        private bool GetPixelFromHex(string hexcode)
        {
            //Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Bitmap bitmap = new Bitmap(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
            Graphics graphics = Graphics.FromImage(bitmap as Image);
            graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);

            Color deziredColor = ColorTranslator.FromHtml(hexcode);

            for(int x=0; x < SystemInformation.VirtualScreen.Width; x++)
            {
                for(int y=0; y < SystemInformation.VirtualScreen.Height; y++)
                {
                    Color currentColor = bitmap.GetPixel(x, y); 
                    if(currentColor==deziredColor)
                    {
                        MessageBox.Show(String.Format($"Your color is at pixel {x}.{y}"));
                        DoubleClickAtPosition(x, y);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
