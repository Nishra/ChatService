# ChatService README

### Docker Desktop
### RabbitMQ Configuration
# 1. Pull RabbitMQ Docker Image
In your terminal, pull the official RabbitMQ Docker image using the following command:

docker pull rabbitmq:management

This will download the RabbitMQ image with the management plugin, which provides a web-based UI for managing RabbitMQ.

# 2. Create a RabbitMQ Container
Run the following command to create a RabbitMQ container:

docker run -d --name rabbitmq-container -p 5672:5672 -p 15672:15672 rabbitmq:management

-d: Run the container in detached mode.
--name rabbitmq-container: Assign a name to your RabbitMQ container (you can choose a different name).
-p 5672:5672 -p 15672:15672: Map port 5672 (RabbitMQ) and port 15672 (RabbitMQ Management UI) from the container to your local machine.


# 3. Access RabbitMQ Management UI
You can access the RabbitMQ Management UI in your web browser at http://localhost:15672/.

Username: guest
Password: guest


# 4. Using RabbitMQ in Your Project
In your project code or configuration, use the following connection settings to connect to the RabbitMQ server:

Host: localhost
Port: 5672
Username: guest
Password: guest
