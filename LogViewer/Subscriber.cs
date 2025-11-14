using System;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client; 
using RabbitMQ.Client.Events;



public class Subscriber
{


private readonly string _hostname = "localhost"; //cambiar por la dirección que corresponda
private readonly string _exchangeName = "logs"; //reemplazar por el nombre de la cola
private readonly string _userName = "guest"; //utilizar las credenciales de un usuario de RabbitMQ
private readonly string _password = "guest";
private readonly int _port = 5672; //reemplazar por el puerto AMQP de RabbitMQ
private readonly IConnection _connection;
private readonly IModel _channel;
private readonly IBasicProperties _properties;

    public Subscriber()
    {
        var factory = new ConnectionFactory
        {
            HostName = _hostname,
            UserName = _userName,
            Password = _password,
            Port = _port
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(
            exchange: _exchangeName,
            type: ExchangeType.Fanout,
            durable: true,
            autoDelete: false,
            arguments: null
        );



        var tempQueue = _channel.QueueDeclare(queue: "",durable: false,exclusive: true,autoDelete: true);

        var queueName = tempQueue.QueueName;

        _channel.QueueBind(queue: queueName, exchange: _exchangeName, routingKey: "");

        Console.WriteLine($"Esperando logs en la cola efímera: {queueName}");


        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray(); //contenido del mensaje (array de bytes)
            var message = Encoding.UTF8.GetString(body); //se convierte de vuelta a string
            var log = JsonSerializer.Deserialize<dynamic>(message);
            Console.WriteLine($"Log recibido: {message}");
        }; 

        _channel.BasicConsume(queue: queueName,autoAck: true, consumer: consumer); 



    }

}