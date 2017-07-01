using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

namespace StockImageProducer.RabbitMqPublish
{
    public class Send
    {
        byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        public void start(string key, string url)
        {
            var factory = new ConnectionFactory() { HostName = "172.16.0.11" };
            try
            {
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "TRAINING-USEDIMAGE-Queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                        NameValueCollection msg = new NameValueCollection();
                        msg.Add(key, url);
                        byte[] mbody = ObjectToByteArray(msg);
                        channel.BasicPublish(exchange: "", routingKey: "TRAINING-USEDIMAGE-Queue", basicProperties: null, body: mbody);
                        //Console.WriteLine(" [x] Sent {0}", msg.Get(0));

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            // Console.WriteLine(" Press [enter] to exit.");
            // Console.ReadLine();
        }
    }
}