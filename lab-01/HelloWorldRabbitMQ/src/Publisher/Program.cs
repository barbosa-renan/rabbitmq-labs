using RabbitMQ.Client;

var connectionFactory = new ConnectionFactory()
{
    Uri = new Uri(@"amqp://logUser:logPwd@127.0.0.1:5672/"),
    NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
    AutomaticRecoveryEnabled = true
};

using (var connection = connectionFactory.CreateConnection())
{
    using (var channel = connection.CreateModel())
    {
        channel.ExchangeDeclare(exchange: "amq.fanout", 
            type: ExchangeType.Fanout, 
            durable: true, 
            autoDelete: false);

        byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes("{{ name:\"Renan\", github:\"https://github.com/barbosa-renan\" }}");

        channel.BasicPublish(exchange: "amq.fanout",
                             routingKey: "",
                             basicProperties: null,
                             body: messageBodyBytes);

        Console.WriteLine("Publish!");
    }
}