using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


    
RunServer();
    

static void ServerWorker()
{
    int serverPort = 11000;
    IPAddress ipAddress = IPAddress.Loopback;
    IPEndPoint endPoint = new IPEndPoint(ipAddress, serverPort);
    using Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
    listener.Bind(endPoint);
    listener.Listen(10);
    while (true)
    {
        Console.WriteLine($"Waiting for connections");
        using Socket handler = listener.Accept();
        Console.WriteLine($"Client connected");
        byte[] getBytes = new byte[1024];
        int bytesCount = handler.Receive(getBytes);
        string request = Encoding.UTF8.GetString(getBytes, 0, bytesCount);
        Console.WriteLine($"Receive: {request}");
        string reply;
        if (request.Contains("time"))
            reply = $"Current time: {DateTime.Now.ToString("HH:mm")}";
        else if (request.Contains("date"))
            reply = $"Current date: {DateTime.Now.ToString("yyyy-MM-dd")}";
        else if (request.Contains("ip"))
            reply = $"Server IP address: {ipAddress}";
        else if (request.Contains("info"))
            reply = "This is a similar client-server program which is made my student. This program work with sockets.";
        else if (request.Contains("funny fact"))
            reply = "The oldest cat was 38 years old. Her name is Creme Puff";
        else
            reply = $"Your request string has length: {request.Length}";
        byte[] sendingBytes = Encoding.UTF8.GetBytes(reply);
        handler.Send(sendingBytes);
        handler.Shutdown(SocketShutdown.Both);
        handler.Close();

        if (request.Contains("stop"))
        {
            break;
        }
    }

    Console.WriteLine($"Server stopped");
}

static void RunServer()
{
    try
    {
        ServerWorker();
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error: {e.Message}");
    }
}

