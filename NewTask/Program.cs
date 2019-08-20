using RabbitMQ.Client;
using System;
using System.Text;

namespace NewTask
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //Console.WriteLine("Hello World!");
            var factrory = new ConnectionFactory() { HostName = "LocalHost" };
            using (var connection = factrory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "work_queue",
                                      durable: true,
                                      exclusive: false,
                                      autoDelete: false,
                                      arguments: null);

                var message = GetMessage(args);
                var body = Encoding.UTF8.GetBytes(message.ToString());
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "",
                                     routingKey: "work_queue",
                                     basicProperties: properties,
                                     body: body);

            }
            
        }
        private static object GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
        }
    }
}
