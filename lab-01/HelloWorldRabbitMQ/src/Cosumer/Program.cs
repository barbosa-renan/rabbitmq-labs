using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory connectionFactory = new ConnectionFactory()
{
    Uri = new Uri(@"amqp://logUser:logPwd@127.0.0.1:5672/platform"),
    NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
    AutomaticRecoveryEnabled = true
};

using (var connection = connectionFactory.CreateConnection())
{
    using (var channel = connection.CreateModel())
    {
        channel.QueueDeclare(queue: "queue_1",
                            durable: true,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body.ToArray());
            Console.WriteLine("[x] Message: {0}", message);
        };

        channel.BasicConsume(queue: "queue_1", autoAck: false, consumer: consumer);
    };
};

Console.WriteLine("Read!");