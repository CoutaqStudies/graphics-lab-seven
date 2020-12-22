using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace graphics_lab_seven
{
    public partial class Form1 : Form
    {
        // Битовая картинка pictureBox
        Bitmap pictureBoxBitMap;
        // Битовая картинка динамического изображения
        Bitmap spriteBitMap;
        // Битовая картинка для временного хранения области экрана
        Bitmap cloneBitMap;
        // Графический контекст picturebox
        Graphics g_pictureBox;
        // Графический контекст спрайта
        Graphics g_sprite;
        int x, y; // Координаты автобуса
        int width = 451, height = 209; // Ширина и высота автобуса
        Timer timer;
        public Form1() { InitializeComponent(); }
        // Функция рисования спрайта (автобуса)
        void DrawSprite()
        {
            HatchBrush camo = new HatchBrush(HatchStyle.ZigZag, Color.Crimson, Color.Olive);
          
            g_sprite.FillRectangle(camo, new Rectangle(0, 50, 200, 50));
            g_sprite.FillRectangle(camo, new Rectangle(25, 10, 135, 40));
            g_sprite.FillRectangle(camo, new Rectangle(150, 25, 100, 10));
            g_sprite.FillEllipse(new SolidBrush(Color.Black), 0, 60, 50, 50);
            g_sprite.FillEllipse(new SolidBrush(Color.Black), 50, 70, 50, 50);
            g_sprite.FillEllipse(new SolidBrush(Color.Black), 100, 70, 50, 50);
            g_sprite.FillEllipse(new SolidBrush(Color.Black), 150, 60, 50, 50);
        }
        void SavePart(int xt, int yt)
        {
            Rectangle cloneRect = new Rectangle(xt, yt, width, height);
            System.Drawing.Imaging.PixelFormat format =

            pictureBoxBitMap.PixelFormat;
            // Клонируем изображение, заданное прямоугольной областью
            cloneBitMap = pictureBoxBitMap.Clone(cloneRect, format);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // Создаём Bitmap для pictureBox1 и графический контекст
            pictureBox1.Image = Image.FromFile(@"bg.png");
            pictureBoxBitMap = new Bitmap(pictureBox1.Image);
            g_pictureBox = Graphics.FromImage(pictureBox1.Image);
            // Создаём Bitmap для спрайта и графический контекст
            spriteBitMap = new Bitmap(width, height);
            g_sprite = Graphics.FromImage(spriteBitMap);
            // Рисуем линию движения автобуса
            g_pictureBox.DrawLine(new Pen(Color.Black, 2), 0, 410,
            pictureBox1.Width - 1, 410);
            // Рисуем автобус на графическом контексте g_sprite
            DrawSprite();
            // Создаём Bitmap для временного хранения части изображения
            cloneBitMap = new Bitmap(width, height);
            // Задаем начальные координаты вывода движущегося объекта
            x = 0; y = 200;
            // Сохраняем область экрана перед первым выводом объекта
            SavePart(x, y);
            // Выводим автобус на графический контекст g_pictureBox
            g_pictureBox.DrawImage(spriteBitMap, x, y);
            // Перерисовываем pictureBox1
            pictureBox1.Invalidate();
            // Создаём таймер с интервалом 100 миллисекунд
            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += new EventHandler(timer1_Tick);
        }

        // Обрабатываем событие от таймера
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Восстанавливаем затёртую область статического изображения
            g_pictureBox.DrawImage(cloneBitMap, x, y+7);
            // Изменяем координаты для следующего вывода автобуса
            x += 6;
            // Проверяем на выход изображения автобуса за правую границу
            if (x > pictureBox1.Width - 1) x = pictureBox1.Location.X;
            // Сохраняем область экрана перед первым выводом автобуса
            try
            {
                SavePart(x, y);
            }
            catch(Exception)
            {
                explode();
                timer.Enabled = false;
            }
            
            // Выводим автобус
            g_pictureBox.DrawImage(spriteBitMap, x, y);
            // Перерисовываем pictureBox1
            pictureBox1.Invalidate();
        }
        void explode()
        {
            int newX = x - width / 2;
            int l = 50;
            int s = 15;
            int newY = y - height / 2;
            Point[] explosion_s = {
                    new Point(newX - l, newY),
                    new Point(newX - s, newY - s),
                    new Point(newX, newY - l),
                    new Point(newX + s, newY - s),
                    new Point(newX + l, newY),
                    new Point(newX + s, newY + s),
                    new Point(newX, newY + l),
                    new Point(newX - s, newY + s)
                };
            g_sprite.FillPolygon(Brushes.OrangeRed, explosion_s);
            l = 100;
            s = 40;
            
        }
        // Включаем таймер по нажатию на кнопку
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }
    }
}