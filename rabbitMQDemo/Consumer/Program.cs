using Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Consumer
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
            Coffee coffee = new Coffee() { Name = "Latte", Size = CoffeeSize.Grande, Id = 1, Message = "Serhat!" };
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "QCoffeeShop",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = JsonConvert.SerializeObject(coffee);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "QCoffeeShop",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine($"Coffee: {coffee.Name}-{coffee.Size}-For-{coffee.Message}");
            }

            Console.WriteLine(" Coffee sent...");
            Console.ReadLine();
        }
    }
}
