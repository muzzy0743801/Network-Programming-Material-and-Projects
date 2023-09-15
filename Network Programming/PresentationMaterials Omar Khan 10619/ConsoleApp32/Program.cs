using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class Client
{
    public static void Main()          //CLIENT
    {
        try   //USING TRY CATCH FOR EXCEPTIONS SO THAT IF THERE IS NO SERVER LISTENING THENIT WILL JUMP TO CATCH BLOCK
        {
            TcpClient client = new TcpClient("192.168.72.159", 8080);    //CREATING TCP CLIENT OBJECT WITH IP AND PORT NUMBER
            StreamReader reader = new StreamReader(client.GetStream());   //CREATING STREAM READER
            StreamWriter writer = new StreamWriter(client.GetStream());    //CREATING STREAM WRITER
            String s = String.Empty;    //CREATED EMPTY STRING
            while (!s.Equals("Exit"))   //CREATED WHILE LOOP TILL IT DOES NOT EXITS WOULD BE CONNECTED TO THE SERVER
            {
                Console.Write("Enter a string to send to the server: ");
                s = Console.ReadLine();     //SENDING STRING TO THE SERVER AND EXPECTING IT TO BACK BACK FROM THE SERVER AND WRITE THAT TO THE LOCAL CONSOLE
                Console.WriteLine();

                //THE SERVER IS MULTI THREADED AND WE CAN CONNECT AS MANY CLIENTS TO IT AS WE WANT
                writer.WriteLine(s);
                writer.Flush();
                String server_string = reader.ReadLine();
                Console.WriteLine(server_string);
            }
            reader.Close();    //READER,WRITER AND CLIENT WILL BE THEN CLOSED HERE
            writer.Close();
            client.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        Console.ReadKey();
    }
}