using MySql.Data.MySqlClient;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
namespace StockImageConsumer
{
    public class Receive
    {
        public object byteToObject(byte[] data)
        {
            if (data == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data))
            {
                object obj = bf.Deserialize(ms);
                return obj;
            }
        }
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public void start()
        {
            var factory = new ConnectionFactory() { HostName = "172.16.0.11" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "TRAINING-USEDIMAGE-Queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                string imgSourceUrl = null;
                string key = null;
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    NameValueCollection nvc = (NameValueCollection)byteToObject(body);
                    //var message = Encoding.UTF8.GetString(body);
                    key = nvc.GetKey(0);
                    imgSourceUrl = nvc.Get(0);
                    Console.WriteLine(" [x] Received id {0}  value{1}", nvc.GetKey(0), nvc.Get(0));
                    processUrl(key, imgSourceUrl);

                };
                channel.BasicConsume(queue: "TRAINING-USEDIMAGE-Queue", noAck: true, consumer: consumer);

                Console.WriteLine(" Press [enter] to exit." + imgSourceUrl);
                Console.ReadLine();
            }
        }
        void processUrl(string key, string imageUrl)
        {
            String saveLocation = imageDownload(key, imageUrl);
            if (saveLocation.Equals(""))
                return;
            resizeDownloaded(key, saveLocation);
            //saveImageLocation(int.Parse(key));
        }

        public string imageDownload(string key, string imageUrl)
        {
            //string imageUrl = @"http://hd.wallpaperswide.com/thumbs/hummer-t2.jpg
            WebResponse imageResponse = null;
            Stream responseStream = null;
            bool isImageSaved = false;
            string saveLocation = @"C:\Users\nishaant.sharma\Desktop\StockManagement\StockManagement\UsedStockManagement\Images\normal-" + key + ".jpg";
            string ret = "";
            try
            {


                byte[] imageBytes;
                HttpWebRequest imageRequest = (HttpWebRequest)WebRequest.Create(imageUrl);
                imageResponse = imageRequest.GetResponse();

                responseStream = imageResponse.GetResponseStream();

                using (BinaryReader br = new BinaryReader(responseStream))
                {
                    imageBytes = br.ReadBytes(500000);
                    br.Close();
                }
                responseStream.Close();
                imageResponse.Close();

                FileStream fs = new FileStream(saveLocation, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);
                try
                {
                    bw.Write(imageBytes);
                    isImageSaved = true;
                }
                finally
                {
                    fs.Close();
                    bw.Close();
                }
                ret = isImageSaved ? saveLocation : "";
                return ret;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return ret;
                //throw;
            }
            finally
            {
                //responseStream.Close();
                try
                {
                    imageResponse.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    //throw;
                }
            }
        }

        public void resizeDownloaded(string key, string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                return; //image Can not be resized.. because its not present in local storage
            }
            try
            {
                Image pic = Image.FromFile(imagePath);
                Bitmap pic1 = ResizeImage(pic, 310, 174);
                Bitmap pic2 = ResizeImage(pic, 640, 348);
                pic.Dispose();
                string pic1Path = @"C:\Users\nishaant.sharma\Desktop\StockManagement\StockManagement\UsedStockManagement\Images\small-" + key + ".jpg";
                string pic2Path = @"C:\Users\nishaant.sharma\Desktop\StockManagement\StockManagement\UsedStockManagement\Images\medium-" + key + ".jpg";
                Directory.CreateDirectory(Path.GetDirectoryName(pic1Path));
                Directory.CreateDirectory(Path.GetDirectoryName(pic2Path));
                pic1.Save(pic1Path, System.Drawing.Imaging.ImageFormat.Jpeg);
                pic1.Dispose();
                pic2.Save(pic2Path, System.Drawing.Imaging.ImageFormat.Jpeg);
                pic2.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //throw;
            }

        }
        public static void Main()
        {
            Receive receive = new Receive();
            receive.start();
        }

    }
}
