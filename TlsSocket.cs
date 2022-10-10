// Copyright Eric Chauvin 2022.


// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html


using System;
using System.IO;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Authentication;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Collections;



class TlsSocket
  {
  private SslStream tlsStream;
  private TcpClient tcpClient;
  private StringBuilder StatusBld;

  private static Hashtable certificateErrors =
                                    new Hashtable();



  internal TlsSocket()
    {
    StatusBld = new StringBuilder();
    }



  // The following method is invoked by the
  // RemoteCertificateValidationDelegate.
  public static bool ValidateServerCertificate(
              object sender,
              X509Certificate certificate,
              X509Chain chain,
              SslPolicyErrors sslPolicyErrors )
    {
    // What is in the certificate?
    // StatusBld.Append( something );

    if( sslPolicyErrors == SslPolicyErrors.None )
      return true;

    // MForm.ShowStatus( "Certificate error: " );
    // , sslPolicyErrors);

    // Do not allow this client to communicate
    // with unauthenticated servers.
    return false;
    }



  internal bool Connect(
        string machineName, string serverName)
    {
    tcpClient = new TcpClient( machineName,
                               5000 );
    // Is it connected?
    // Console.WriteLine("Client connected.");

    tlsStream = new SslStream(
                tcpClient.GetStream(),
                false,
                new
                RemoteCertificateValidationCallback
                (ValidateServerCertificate),
                null
                );

    try
    {
    tlsStream.AuthenticateAsClient( serverName );
    }
    catch (AuthenticationException e)
      {
      // Console.WriteLine("Exception: {0}",
      // e.Message);
      if( e.InnerException != null )
        {
        // Console.WriteLine("Inner exception:
        //  {0}", e.InnerException.Message);
        }

      // Console.WriteLine ("Authentication
      //  failed - closing the connection.");
      tcpClient.Close();
      return false;
      }

    // Encode a test message into a byte array.
    // Signal the end of the message using the
    // "<EOF>".
    byte[] messsage = Encoding.UTF8.GetBytes(
           "Hello from the client.<EOF>");
    // Send hello message to the server.
    tlsStream.Write( messsage );
    tlsStream.Flush();

    // Read message from the server.
    string serverMessage = ReadMessage(
                                 tlsStream );
    // Console.WriteLine("Server says: {0}",
    //  serverMessage);
    // Close the client connection.
    tcpClient.Close();
    //   Console.WriteLine("Client closed.");

    return true;
    }



  string ReadMessage( SslStream sslStream )
    {
    // Read the  message sent by the server.
    // The end of the message is signaled using the
    // "<EOF>" marker.
    byte [] buffer = new byte[2048];
    StringBuilder messageData = new StringBuilder();
    int bytes = -1;
    do
      {
       bytes = sslStream.Read( buffer, 0,
                              buffer.Length );

       // Use Decoder class to convert from
       // bytes to UTF8
       // in case a character spans two buffers.
       Decoder decoder = Encoding.UTF8.GetDecoder();
       char[] chars = new char[decoder.
                  GetCharCount(buffer,0,bytes)];
       decoder.GetChars(buffer, 0, bytes, chars,0);
       messageData.Append (chars);
       // Check for EOF.
       if( messageData.ToString().IndexOf("<EOF>")
                       != -1)
         {
         break;
         }
       } while (bytes != 0);

     return messageData.ToString();
     }



  internal void FreeEverything()
    {
    tcpClient.Close();
    tlsStream.Close();
    }



/*
   private static void DisplayUsage()
        {
            Console.WriteLine("To start the client specify:");
            Console.WriteLine("clientSync machineName [serverName]");
            Environment.Exit(1);
        }



        public static int Main(string[] args)
        {
            string serverCertificateName = null;
            string machineName = null;
            if (args == null ||args.Length <1 )
            {
                DisplayUsage();
            }
            // User can specify the machine name and server name.
            // Server name must match the name on the server's certificate.
            machineName = args[0];
            if (args.Length <2 )
            {
                serverCertificateName = machineName;
            }
            else
            {
                serverCertificateName = args[1];
            }
            SslTcpClient.RunClient (machineName, serverCertificateName);
            return 0;
        }
    }
*/






/*

namespace Examples.System.Net
{
    public sealed class SslTcpServer
    {
        static X509Certificate serverCertificate = null;
        // The certificate parameter specifies the name of the file
        // containing the machine certificate.
        public static void RunServer(string certificate)
        {
            serverCertificate = X509Certificate.CreateFromCertFile(certificate);
            // Create a TCP/IP (IPv4) socket and listen for incoming connections.
            TcpListener listener = new TcpListener(IPAddress.Any, 5000);
            listener.Start();
            while (true)
            {
                Console.WriteLine("Waiting for a client to connect...");
                // Application blocks while waiting for an incoming connection.
                // Type CNTL-C to terminate the server.
                TcpClient client = listener.AcceptTcpClient();
                ProcessClient(client);
            }
        }

        static void ProcessClient (TcpClient client)
        {
            // A client has connected. Create the
            // SslStream using the client's network stream.
            SslStream sslStream = new SslStream(
                client.GetStream(), false);
            // Authenticate the server but don't require the client to authenticate.
            try
            {
                sslStream.AuthenticateAsServer(serverCertificate, clientCertificateRequired: false, checkCertificateRevocation: true);

                // Display the properties and settings for the authenticated stream.
                DisplaySecurityLevel(sslStream);
                DisplaySecurityServices(sslStream);
                DisplayCertificateInformation(sslStream);
                DisplayStreamProperties(sslStream);

                // Set timeouts for the read and write to 5 seconds.
                sslStream.ReadTimeout = 5000;
                sslStream.WriteTimeout = 5000;
                // Read a message from the client.
                Console.WriteLine("Waiting for client message...");
                string messageData = ReadMessage(sslStream);
                Console.WriteLine("Received: {0}", messageData);

                // Write a message to the client.
                byte[] message = Encoding.UTF8.GetBytes("Hello from the server.<EOF>");
                Console.WriteLine("Sending hello message.");
                sslStream.Write(message);
            }
            catch (AuthenticationException e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
                if (e.InnerException != null)
                {
                    Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
                }
                Console.WriteLine ("Authentication failed - closing the connection.");
                sslStream.Close();
                client.Close();
                return;
            }
            finally
            {
                // The client stream will be closed with the sslStream
                // because we specified this behavior when creating
                // the sslStream.
                sslStream.Close();
                client.Close();
            }
        }

        static string ReadMessage(SslStream sslStream)
        {
            // Read the  message sent by the client.
            // The client signals the end of the message using the
            // "<EOF>" marker.
            byte [] buffer = new byte[2048];
            StringBuilder messageData = new StringBuilder();
            int bytes = -1;
            do
            {
                // Read the client's test message.
                bytes = sslStream.Read(buffer, 0, buffer.Length);

                // Use Decoder class to convert from bytes to UTF8
                // in case a character spans two buffers.
                Decoder decoder = Encoding.UTF8.GetDecoder();
                char[] chars = new char[decoder.GetCharCount(buffer,0,bytes)];
                decoder.GetChars(buffer, 0, bytes, chars,0);
                messageData.Append (chars);
                // Check for EOF or an empty message.
                if (messageData.ToString().IndexOf("<EOF>") != -1)
                {
                    break;
                }
            } while (bytes !=0);

            return messageData.ToString();
        }

         static void DisplaySecurityLevel(SslStream stream)
         {
            Console.WriteLine("Cipher: {0} strength {1}", stream.CipherAlgorithm, stream.CipherStrength);
            Console.WriteLine("Hash: {0} strength {1}", stream.HashAlgorithm, stream.HashStrength);
            Console.WriteLine("Key exchange: {0} strength {1}", stream.KeyExchangeAlgorithm, stream.KeyExchangeStrength);
            Console.WriteLine("Protocol: {0}", stream.SslProtocol);
         }
         static void DisplaySecurityServices(SslStream stream)
         {
            Console.WriteLine("Is authenticated: {0} as server? {1}", stream.IsAuthenticated, stream.IsServer);
            Console.WriteLine("IsSigned: {0}", stream.IsSigned);
            Console.WriteLine("Is Encrypted: {0}", stream.IsEncrypted);
         }

         static void DisplayStreamProperties(SslStream stream)
         {
            Console.WriteLine("Can read: {0}, write {1}", stream.CanRead, stream.CanWrite);
            Console.WriteLine("Can timeout: {0}", stream.CanTimeout);
         }

        static void DisplayCertificateInformation(SslStream stream)
        {
            Console.WriteLine("Certificate revocation list checked: {0}", stream.CheckCertRevocationStatus);

            X509Certificate localCertificate = stream.LocalCertificate;
            if (stream.LocalCertificate != null)
            {
                Console.WriteLine("Local cert was issued to {0} and is valid from {1} until {2}.",
                    localCertificate.Subject,
                    localCertificate.GetEffectiveDateString(),
                    localCertificate.GetExpirationDateString());
             } else
            {
                Console.WriteLine("Local certificate is null.");
            }
            // Display the properties of the client's certificate.
            X509Certificate remoteCertificate = stream.RemoteCertificate;
            if (stream.RemoteCertificate != null)
            {
            Console.WriteLine("Remote cert was issued to {0} and is valid from {1} until {2}.",
                remoteCertificate.Subject,
                remoteCertificate.GetEffectiveDateString(),
                remoteCertificate.GetExpirationDateString());
            } else
            {
                Console.WriteLine("Remote certificate is null.");
            }
        }

        private static void DisplayUsage()
        {
            Console.WriteLine("To start the server specify:");
            Console.WriteLine("serverSync certificateFile.cer");
            Environment.Exit(1);
        }



        public static int Main(string[] args)
        {
            string certificate = null;
            if (args == null ||args.Length < 1 )
            {
                DisplayUsage();
            }
            certificate = args[0];
            SslTcpServer.RunServer (certificate);
            return 0;
        }
    }
}
*/


  }
