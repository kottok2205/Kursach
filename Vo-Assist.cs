using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;
using System.Security;
using NAudio.Wave;
using NAudio.FileFormats;
using NAudio.CoreAudioApi;
using NAudio;
using System.Speech.Synthesis;

namespace Vo_Assist
{

    public partial class Vo_Assist : Form
    {
        // WaveIn - поток для записи
        WaveIn waveIn;
        //Класс для записи в файл
        WaveFileWriter writer;
        //Имя файла для записи
        string outputFilename = "recording.wav";
        bool ON = false;
        

        public Vo_Assist()
        {
            InitializeComponent();
        }

        //Получение данных из входного буфера 
        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            //Записываем данные из буфера в файл
            writer.WriteData(e.Buffer, 0, e.BytesRecorded);
        }

        //Окончание записи
        private void waveIn_RecordingStopped(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler(waveIn_RecordingStopped), sender, e);
            }
            else
            {
                waveIn.Dispose();
                waveIn = null;
                writer.Close();
                writer = null;
            }
        }

        private void Vo_Assist_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            if(ON == false)
            {
                waveIn = new WaveIn();
                //Дефолтное устройство для записи
                //встроенный микрофон ноутбука 0
                waveIn.DeviceNumber = 1;
                //Прикрепляем к событию DataAvailable обработчик, возникающий при наличии записываемых данных
                waveIn.DataAvailable += waveIn_DataAvailable;
                //Прикрепляем обработчик завершения записи
                waveIn.RecordingStopped += new EventHandler<NAudio.Wave.StoppedEventArgs>(waveIn_RecordingStopped);
                //Формат wav-файла - принимает параметры - частоту дискретизации и количество каналов(здесь mono)
                waveIn.WaveFormat = new WaveFormat(16000, 1);
                //Инициализируем объект WaveFileWriter
                writer = new WaveFileWriter(outputFilename, waveIn.WaveFormat);
                label1.Text = "Йде запис...";
                button1.Text = "Стоп";
                //Начало записи
                waveIn.StartRecording();
                ON = true;
            }
            else
            {
                waveIn.StopRecording();
                label1.Text = "";
                ON = false;
                button1.Text = "Записати";
                //button2_Click(this, EventArgs.Empty);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            

            Random random = new Random();

            //List<string> hello = new List<string>() {"Привіт", "Доброго дня", "Як ся маєш", "Добрий ранок", "Добрий вечір", "Hello", "Assist"};
            //List<string> hello_1 = new List<string>() { "привіт", "доброго дня", "як ся маєш", "добрий ранок", "добрий вечір", "Hello", "Assist" };

            List<string> anekdot = new List<string>() { };
            anekdot.Add("Директор новому водителю:" +
                "\n- Как ваша фамилия ? Я к водителям только по фамилии обращаюсь!" +
                "\n- Андрей!" +
                "\n- Че фамилия такая ?" +
                "\n-Нет имя." +
                "\n- Вы меня не поняли, мне нужно знать вашу фамилию!" +
                "\n- Вы меня не будете звать по фамилии, зовите Андрей!" +
                "\n- Слышь, боец ты че тупой, я еще раз спрашиваю как твоя фамилия ?" +
                "\n-Ну, Любимый моя фамилия!" +
                "\n- Поехали, Андрей...");
            anekdot.Add("Садятся в такси трое пьяных парней. " +
                "Водитель заметил, что они пьяны и решил их обмануть. " +
                "Он завел двигатель и через несколько секунд выключил его." +
                "Поворачивается к парням и говорит:" +
                "\n-Все мы приехали." +
                "Один парень поблагодарил его, " +
                "другой протянул ему деньги, " +
                "а третий влепил водиле оплеуху." +
                "Подумав, что третий догадался о проделке, водитель спросил:" +
                "\n-За что ??Парень отвечает:" +
                "\n-Чтобы в следующий раз не превышал скорости." +
                "Ты нас чуть не убил! Шумахер гребаный!");
            anekdot.Add("- Вы первый кто проехал на этом перекрёстке без нарушений, вот вам 1000 рублей." +
                "\n- О! Права куплю!" +
                "\n- Вы без прав?" +
                "\nЖена :" +
                "\n- Не слушайте его, чего по пьяни не скажешь ?" +
                "\n- Так Вы и пьян?" +
                "\nТёща:" +
                "\n- Я же говорила, что на ворованной машине далеко не уедешь!" +
                "\nГолос из багажника:" +
                "\n- Чё границу уже переехали? ");
            anekdot.Add("Приходит мужик в зоомагазин: " +
                "\n- Есть у вас что - нибудь чтоб умело разговаривать?" +
                "\n- Есть! Говорящая сороконожка." +
                "\nПриходит домой, накормил ее и говорит:" +
                "\n- Гулять идем? " +
                "\n- Та молчит." +
                "\n- Гулять идем или нет? " +
                "\n- снова молчит." +
                "\nМужик в бешенстве:" +
                "\n- Обманули! Какая же ты говорящая сороконожка?!" +
                "\n- Тихо, блин.Я обуваюсь...");
            anekdot.Add("Девочки с Вовочкиного класса договорились," +
                " что если Вовочка скажет какую-нибуть пошлость, " +
                "то они встанут и выйдут с класса. " +
                "\nНачался урок и учитель спрашивает:" +
                "\n- Дети, а что нового строиться в нашем городе ?" +
                "\n- Школа." +
                "\n- Библиотека!" +
                "\n- Музей!" +
                "\nВовочка:" +
                "\n- Публичный дом." +
                "\nТе ж девушки встают и выходят с класса, " +
                "а Вовочка им кричит:" +
                "\n- Куда же вы ? Там только фундамент залили!");

            List<string> fakt = new List<string>() { };
            fakt.Add("Гра «камінь, ножиці, папір» була винайдена в 206 році до н.е. в Китаї.");
            fakt.Add("У волоському горісі вітаміну С в десять разів більше, ніж в чорній " +
                 "Смородині і цілих в сорок разів більше, ніж у лимоні.");
            fakt.Add("Медузи жили ще до динозаврів.");
            fakt.Add("Геймерам краще вдається керувати своїми снами, ніж іншим людям.");
            fakt.Add("Стінки мильної бульбашки - найтонша матерія, яку людина може побачити неозброєним поглядом.");
            fakt.Add("ДНК людини - На 30% збігається з ДНК салату.");
            fakt.Add("Доведено, що прийом сніданку зміцнює пам'ять і підвищує успішність у навчанні.");
            fakt.Add("Психологи стверджують, що, змушуючи себе посміхатися, людина може підняти собі настрій.");
            fakt.Add("Ідеальна температура для глибокого сну - 15 градусів за Цельсієм.");

            WebRequest request = WebRequest.Create("https://www.google.com/speech-api/v2/recognize?output=json&lang=uk-UA&key=AIzaSyBOti4mM-6x9WDnZIjIeyEU21OpBXqWBgw");
            //
            request.Method = "POST";
            byte[] byteArray = File.ReadAllBytes(outputFilename);
            request.ContentType = "audio/l16; rate=16000"; //"16000";
            request.ContentLength = byteArray.Length;
            request.GetRequestStream().Write(byteArray, 0, byteArray.Length);


            // Отримайте відповідь.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // Відкрийте потік за допомогою StreamReader для зручного доступу.
            StreamReader reader = new StreamReader(response.GetResponseStream());
            // Прочитайте зміст.


            string strtrs = reader.ReadToEnd();
            var rg = new Regex(@"transcript" + '"' + ":" + '"' + "([A-Z, А-Я, і, ї, ', ґ, Ґ, є, Є, a-z, а-я, , 0-9]*)");
            var result = rg.Match(strtrs).Groups[1].Value; //распознанный текст
            label1.Text = result;

            //label1.Text = strtrs;
            if(result.Contains("Привіт") || result.Contains("привіт"))
            {
                label1.Text = "Доброго дня, Вас вітає голосовий асистент \"Vo-Assist\" або просто \"Assist\"";
            }
            else if(result.Contains("Знайди ") || result.Contains("знайди"))
            {
                label1.Text = "Шукаю";
                string scan = result;
                string scanN = scan.Replace("Знайди", "");
                scanN = scan.Replace("знайди", "");
                Process.Start("https://www.google.com.ua/search?q=" + scanN);
            }
            else if(result.Contains("Відкрий браузер") || result.Contains("відкрий браузер"))
            {
                label1.Text = "Відкриваю";
                Process.Start("chrome.exe");
            }
            else if(result.Contains("Відкрий записник") || result.Contains("відкрий записник"))
            {
                label1.Text = "Відкриваю";
                Process.Start("notepad.exe");
            }
            else if(result.Contains("Відкрий калькулятор") || result.Contains("відкрий калькулятор"))
            {
                label1.Text = "Відкриваю";
                Process.Start("calc.exe");
            }
            else if(result.Contains("Відкрий консоль") || result.Contains("відкрий консоль"))
            {
                label1.Text = "Відкриваю";
                Process.Start("cmd.exe");
            }
            else if(result.Contains("Розкажи цікавий факт") || result.Contains("розкажи цікавий факт") || result.Contains("Розкажіть цікавий факт") || result.Contains("розкажіть цікавий факт"))
            {
                label1.Text = fakt[random.Next(9)];
            }
            else if(result.Contains("Розкажи анекдот") || result.Contains("розкажи анекдот"))
            {
                label1.Text = anekdot[random.Next(5)];
                //label1.Text = anekdot[1];
            }
            else if(result.Contains("Назви випадкове число") || result.Contains("назви випадкове число"))
            {
                /*string rand = result;
                string randN = rand.Replace("Назви випадкове число від", "");
                randN = rand.Replace("назви випадкове число від", "");*/
                label1.Text = Convert.ToString(random.Next(0, 100));
            }
            else if(result.Contains("Відкрий YouTude") || result.Contains("відкрий YouTube"))
            {
                label1.Text = "Відкриваю";
                Process.Start("https://www.youtube.com/");
            }
            else
            {
                label1.Text = "Невідома команда";
            }
            // Очистка потоков
            reader.Close();
            response.Close();
        }
        static string getResponse(string urlAddress)
        {
            string data = "";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;
                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }
                data = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
            }
            return data;
        }
        private void Comands_Click(object sender, EventArgs e)
        {

        }
    }
}
