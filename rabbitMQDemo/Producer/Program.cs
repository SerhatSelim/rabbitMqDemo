using Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Producer
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "QCoffeeShop",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Coffee coffee = JsonConvert.DeserializeObject<Coffee>(message);
                    Console.WriteLine($" Name: {coffee.Name} Size:{coffee.Size} For [{coffee.Message}]");
                };
                channel.BasicConsume(queue: "QCoffeeShop",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Coffee received :)");
                Console.ReadLine();
            }
        }
    }
}
