using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class MultiThreadedServer             //SERVER
{

	private static void ProcessClientRequests(object argument)
	{
		TcpClient client = (TcpClient)argument;
		try
		{
			StreamReader reader = new StreamReader(client.GetStream()); //USING CLIENT.GETSTREAM METHOD TO CREATE NEW STREAM READER AND WRITER
			StreamWriter writer = new StreamWriter(client.GetStream());
			                                                           //USED SYSTEM.IO LIBRARY FOR STREAM READER AND WRITER
			string s = String.Empty;   //EMPTY STRING TO FILL IN AFTERWARDS
			while (!(s = reader.ReadLine()).Equals("Exit") || (s == null))
			{
				Console.WriteLine("Message From client -> " + s); //GET THE STRING AND WRITE IT TO THE LOCAL CONSOLE
				writer.WriteLine("Reply From server -> " + s);  //WRITE IT BACK TO THE CLIENT USING WRITER
				writer.Flush();  //THEN FLUSH THE WRITEN
			}
			reader.Close();  //CLOSE THE READER
			writer.Close();  //CLOSE THE WRITER
			client.Close();    ///CLOSE CLIENT
			Console.WriteLine("Closing client connection!");
		}
		catch (IOException)
		{
			Console.WriteLine("Problem with client communication. Exiting thread."); //IF THERE IS AN ISSUE THIS MSG
		}
		finally         //FINAL THING WE DO IS TO MAKE SURE THAT CLIENT CONNECTED HAS BEEN CLOSED
		{
			if (client != null)
			{
				client.Close();
			}
		}
	}

	public static void Main()
	{
		TcpListener listener = null;   //FIRST WE CREATE THE TCP LISTENER AND SETTING VALUE TO NULL
		try  //USING TRY CATCH FOR EXCEPTIONS
		{
			listener = new TcpListener(IPAddress.Parse("192.168.72.159"), 8080); //CREATING TCP CLIENT OBJECT
			listener.Start();  //STARTING LISTENER AND PRINTING MSG BELOW
			Console.WriteLine("MultiThreadedServer started...");
			while (true)   //USING WHILE TRUE SO THAT IT REPEATS ITSELF BEING OPENED BY ITS CONNECTION
				          
				//NOW WE ENTER WHILE LOOP
			{
				Console.WriteLine("Waiting for incoming client connections...");
				TcpClient client = listener.AcceptTcpClient(); //CREATED TCP CLIENT AND LISTNER.TCP CLIENT METHOD IS CALLED AND WIATS FOR INCOMING CONNECTIONS
				Console.WriteLine("Accepted new client connection..."); //SOON AS IT RECEIVES THE CONNECTION THE CLIENT OBJECT IS CALLED AND PRINTS MSG SAYING ACCEPTED NEW CLIENT
				Thread t = new Thread(ProcessClientRequests); //CREATED THREADS HERE TO PROCESS CLIENT REQUEST
				t.Start(client); //NOW WE START THREAD AND PASS CLIENT OBJECT
				                 //CAN ALSO BE CALLED PARAMETERIZED THREAD START
				               
                //ITERATION GOES ON AND CONTROL ENTERS AGAIN IN THE WHILE LOOP AND LISTNER.ACCPET(TCP CLIENT) BLOCK TILL IT RECEIVES A NEW TCP CONNECTION AND THE NEW CONNECTION IS DETECTED
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e);        //PRINT EXCEPTION IF ANY
		}
		finally
		{
			if (listener != null)
			{
				listener.Stop();    //IF THE LISTNER IS STILL OPEN WE WILL STOP IT HERE
			}
		}
	} // end Main()
} // end class definition