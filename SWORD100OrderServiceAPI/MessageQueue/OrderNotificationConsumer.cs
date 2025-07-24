
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;


public class OrderNotificationConsumer : BackgroundService
{
    private readonly ConnectionFactory _factory;
    private IConnection _connection;
    private IModel _channel;
    private readonly IConfiguration _iconfig;
    private readonly IServiceProvider _serviceProvider;


    public OrderNotificationConsumer(IConfiguration iconfig, IServiceProvider serviceProvider)
    {

        _iconfig = iconfig;
        _serviceProvider = serviceProvider;

        // this version must look in the actual "ConnectionStrings" section of .json
        //var s2 = _iconfig.GetConnectionString("DefaultConnection");

        _factory = new ConnectionFactory()
        {
            HostName = _iconfig["RabbitMQ:host"],
            Port = int.Parse(_iconfig["RabbitMQ:port"]),
            //UserName = "guest",
            //Password = "password",
            VirtualHost = "/",
        };

        _connection = _factory.CreateConnection();
        _channel = _connection.CreateModel();
        //_channel.QueueDeclare(queue: "notifyQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        _channel.QueueDeclare(queue: "notifyQueue", durable: false, exclusive: false, arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Shutdown += OnConsumerShutdown;
        consumer.Registered += OnConsumerRegistered;
        consumer.Unregistered += OnConsumerUnregistered;
        consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

        consumer.Received += (model, ea) =>
        {
            Console.WriteLine("OrderNotificationConsumer has received a new message");
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body.ToArray());
            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

            Console.WriteLine("Here is the message: " + message);
            OrderNotification orderNotification = JsonConvert.DeserializeObject<OrderNotification>(message);

            Console.WriteLine(orderNotification.Email);
            Console.WriteLine(orderNotification.Message);

            string subject = $"Your recent order with us: #{orderNotification.OrderGuid}";
            string bodytext = $"Account #: {orderNotification.UserGuid}\nOrder #: {orderNotification.OrderGuid}\n\nDear {orderNotification.Name},\n\nThank you for recent order with us!\n\n{orderNotification.Message}\n\nSincerely,\n\n THE MANAGEMENT";                      

            Console.WriteLine("Subject" + subject );
            Console.WriteLine("Body" + bodytext);
        };

        _channel.BasicConsume(queue: "notifyQueue", autoAck: false, consumer: consumer);

        return Task.CompletedTask;
    }

    private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e) { }
    private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }
    private void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }
    private void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }
    private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }
}