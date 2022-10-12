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
// using System.Threading;



class TlsSocket
  {
  private SslStream tlsStream = null;
  private TcpClient tcpClient = null;
  private StringBuilder StatusBld = null;
  private IAsyncResult AsyncResult = null;



  private static Hashtable certificateErrors =
                                    new Hashtable();



  internal TlsSocket()
    {
    StatusBld = new StringBuilder();
    }


  internal string GetStatus()
    {
    string lines = StatusBld.ToString();
    StatusBld.Clear();
    return lines;
    }



  public static bool ValidateServerCertificate(
              object sender,
              X509Certificate certificate,
              X509Chain chain,
              SslPolicyErrors sslPolicyErrors )
    {
    // What is in the certificate?
    // Since this is a static function I need
    // to save it to a file.
    string certS = certificate.ToString();

    if( sslPolicyErrors == SslPolicyErrors.None )
      return true;

    // ( "Certificate error: " );
    // , sslPolicyErrors);

    return false;
    }



  internal bool Connect( string machineName,
                         string serverName,
                         int PortNumber )
    {
    if( tcpClient != null )
      {
      if( tcpClient.Connected )
        {
        StatusBld.Append(
                 "It is already connected.\r\n" );
        return true;
        }

      FreeEverything();
      }

    // The constructor will make it try
    // to connect.
    tcpClient = new TcpClient( machineName,
                               PortNumber );
    // Port is 443 for https.

    if( !tcpClient.Connected )
      {
      StatusBld.Append(
                 "Could not connect.\r\n" );
      return false;
      }

    tcpClient.ReceiveBufferSize = 1024 * 16;

    StatusBld.Append(
                 "It is connected.\r\n" );

    tcpClient.ReceiveTimeout = 5 * 1000;
    tcpClient.SendTimeout = 5 * 1000;

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
      StatusBld.Append( "Exception: " +
                     e.Message + "\r\n" );
      if( e.InnerException != null )
        {
        StatusBld.Append( "Inner Exception: " +
                     e.InnerException.Message +
                     "\r\n" );

        }

      FreeEverything();
      return false;
      }

    StatusBld.Append( "TLS is connected.\r\n" );

    return true; // It is connected now.
    }



  internal bool SendAsciiByteBuf( ByteBuf byteBuf )

    {
    if( tlsStream == null )
      return false;

    // byte[] messsage = Encoding.UTF8.GetBytes(
     //                        "Hello...");
    byte[] ToSend = byteBuf.GetByteArray();

    tlsStream.Write( ToSend );
    tlsStream.Flush();

    return true;
    }



  // What is my most recent UTF8 code?




  internal bool IsProcessingInBackground()
    {
    if( tcpClient == null )
      return false;

    // AsyncResult doesn't get created unless
    // an Async transfer gets started.
    if( AsyncResult == null )
      return false;

    // This is checked whenever it checks to see
    // if it is shut down, which is very often.
    if( AsyncResult.IsCompleted )
      {
      AsyncResult = null;
      // Then give it time before it gets shut down.
      // LastTransactTime.SetToNow();
      return false;
      }

    return true;
    }



  internal bool IsShutDown()
    {
    if( tcpClient == null )
      return true;

    if( IsProcessingInBackground())
      return false;

    // This only knows if it's connected as of
    // the last socket operation.
    if( !tcpClient.Connected )
      {
      FreeEverything();
      return true;
      }

    return false;
    }




  private int GetAvailable()
    {
    if( IsShutDown())
      return 0;

    try
    {
    return tcpClient.Available;
    }
    catch( Exception )
      {
      FreeEverything();
      return 0;
      }
    }




/*
  internal bool WriteBytesAsync( byte[] Bytes )
    {
    if( IsShutDown())
      return false;

    // This can only be called once.
    if( AsyncResult != null )
      {
      MForm.ShowListenerFormStatus( "AsyncResult != null in WriteBytesAsync." );
      return false;
      }

    if( NetStream == null )
      return false;

    // This just means it's a writeable stream.  It's not a test
    // for the write buffer being ready to write.
    // if( NetStream.CanWrite )

    try
    {
    AsyncResult = NetStream.BeginWrite( Bytes,
                  0,
                  Bytes.Length,
                  new AsyncCallback( BasicTCPClient.ProcessAsynchCallback ),
                  NetStream );

    LastTransactTime.SetToNow();
    return true;

    }
    catch( Exception Except )
      {
      MForm.ShowListenerFormStatus( "Exception in WriteBytesAsync." );
      MForm.ShowListenerFormStatus( Except.Message );
      FreeEverything();
      return false;
      }
    }




  static void ProcessAsynchCallback( IAsyncResult Result )
    {
    // The Async send is done only once, then it closes the socket.
    // But Result.AsyncState is the NetworkStream passed to BeginWrite.

    try
    {
    NetworkStream TheStream = (NetworkStream)(Result.AsyncState);
    // The EndWrite method blocks (in another thread) until the bytes
    // are sent.
    // Did it send all the bytes?
    TheStream.EndWrite( Result );
    }
    catch( Exception )
      {
      // This is static.
      // ErrorString += "The socket got closed, or it couldn't write to it, etc.\r\n";
      }
    }
*/



  internal string ReadAsciiString()
    {
    if( tlsStream == null )
      return "";

    if( GetAvailable() < 1 )
      return "";

    // This might be reading only one outer
    // block at a time.  Or something.
    byte [] buffer = new byte[1024 * 16];
    StringBuilder SBuilder = new StringBuilder();

    // This will block if there is nothing
    // to read.
    int bytes = tlsStream.Read( buffer, 0,
                              buffer.Length );

/*
=====
    public override IAsyncResult BeginRead(
byte[] buffer, 0, ReadBuffer.Length,
 AsyncCallback? asyncCallback, object? asyncState );
*/


    StatusBld.Append( "Bytes: " + bytes );

    if( bytes < 1 )
      return "";

    // Decoder decoder = Encoding.UTF8.GetDecoder();
    //  char[] chars = new char[decoder.
    //           GetCharCount(buffer,0,bytes)];
    //  decoder.GetChars(buffer, 0, bytes, chars,0);

    int max = bytes;
    // if( max > 2000 )
      // max = 2000;

    for( int Count = 0; Count < max; Count++ )
      {
      char letter = (char)buffer[Count];
      if( letter < 10 )
        continue;

      if( letter > 126 )
        continue;

      SBuilder.Append( (char)buffer[Count] );
      }

    return SBuilder.ToString();
    }




  internal void FreeEverything()
    {
    if( tlsStream != null )
      {
      tlsStream.Close();
      tlsStream.Dispose();
      tlsStream = null;
      }

    if( tcpClient != null )
      {
      tcpClient.Close();
      tcpClient.Dispose();
      tcpClient = null;
      }
    }



/*

namespace Examples.System.Net
{
    public sealed class SslTcpServer
    {
        static X509Certificate serverCertificate =
 null;
        // The certificate parameter specifies
 the name of the file
        // containing the machine certificate.
        public static void RunServer(string
 certificate)
        {
            serverCertificate =
X509Certificate.CreateFromCertFile(certificate);
            // Create a TCP/IP (IPv4) socket and
 listen for incoming connections.
            TcpListener listener = new
 TcpListener(IPAddress.Any, 5000);
            listener.Start();
            while (true)
            {
                Console.WriteLine("Waiting for
a client to connect...");
                // Application blocks while
 waiting for an incoming connection.
                // Type CNTL-C to terminate the
 server.
                TcpClient client =
 listener.AcceptTcpClient();
                ProcessClient(client);
            }
        }

        static void ProcessClient (TcpClient client)
        {
            // A client has connected. Create the
            // SslStream using the client's
 network stream.
            SslStream sslStream = new SslStream(
                client.GetStream(), false);
            // Authenticate the server but don't
 require the client to authenticate.
            try
            {
                sslStream.AuthenticateAsServer(
serverCertificate, clientCertificateRequired:
 false, checkCertificateRevocation: true);

        // Display the properties and settings
 for the authenticated stream.
                DisplaySecurityLevel(sslStream);
                DisplaySecurityServices(sslStream);
                DisplayCertificateInformation(
sslStream);
                DisplayStreamProperties(sslStream);

                // Set timeouts for the read and
 write to 5 seconds.
                sslStream.ReadTimeout = 5000;
                sslStream.WriteTimeout = 5000;
                // Read a message from the client.
                Console.WriteLine("Waiting for
 client message...");
                string messageData = ReadMessage(
sslStream);
                Console.WriteLine("Received: {0}",
 messageData);

                // Write a message to the client.
                byte[] message = Encoding.UTF8.
GetBytes("Hello from the server.<EOF>");
                Console.WriteLine("Sending hello
 message.");
                sslStream.Write(message);
            }
            catch (AuthenticationException e)
            {
                Console.WriteLine("Exception: {0}",
 e.Message);
                if (e.InnerException != null)
                {
                    Console.WriteLine(
"Inner exception: {0}", e.InnerException.Message);
                }
                Console.WriteLine (
"Authentication failed - closing the connection.");
                sslStream.Close();
                client.Close();
                return;
            }
            finally
            {
                // The client stream will be
 closed with the sslStream
                // because we specified this
 behavior when creating
                // the sslStream.
                sslStream.Close();
                client.Close();
            }
        }

        static string ReadMessage(
SslStream sslStream)
        {
            // Read the  message sent by the client.
            // The client signals the end of
 the message using the
            // "<EOF>" marker.
            byte [] buffer = new byte[2048];
            StringBuilder messageData = new
 StringBuilder();
            int bytes = -1;
            do
            {
                // Read the client's test message.
                bytes = sslStream.Read(buffer, 0,
 buffer.Length);

                // Use Decoder class to convert
 from bytes to UTF8
                // in case a character spans
 two buffers.
                Decoder decoder = Encoding.UTF8.
GetDecoder();
                char[] chars = new char[
decoder.GetCharCount(buffer,0,bytes)];
                decoder.GetChars(buffer, 0, bytes,
 chars,0);
                messageData.Append (chars);
                // Check for EOF or an
 empty message.
                if (messageData.ToString().
IndexOf("<EOF>") != -1)
                {
                    break;
                }
            } while (bytes !=0);

            return messageData.ToString();
        }

         static void DisplaySecurityLevel(
SslStream stream)
         {
            Console.WriteLine("Cipher: {0}
 strength {1}", stream.CipherAlgorithm,
 stream.CipherStrength);
            Console.WriteLine("Hash: {0} strength
 {1}", stream.HashAlgorithm, stream.HashStrength);
            Console.WriteLine("Key exchange:
 {0} strength {1}", stream.KeyExchangeAlgorithm,
 stream.KeyExchangeStrength);
            Console.WriteLine("Protocol: {0}",
 stream.SslProtocol);
         }
         static void DisplaySecurityServices(
SslStream stream)
         {
            Console.WriteLine("Is authenticated:
 {0} as server? {1}", stream.IsAuthenticated,
 stream.IsServer);
            Console.WriteLine("IsSigned: {0}",
 stream.IsSigned);
            Console.WriteLine("Is Encrypted: {0}",
 stream.IsEncrypted);
         }

         static void DisplayStreamProperties(
SslStream stream)
         {
            Console.WriteLine("Can read: {0},
 write {1}", stream.CanRead, stream.CanWrite);
            Console.WriteLine("Can timeout:
 {0}", stream.CanTimeout);
         }

        static void DisplayCertificateInformation(
SslStream stream)
        {
            Console.WriteLine("Certificate
revocation list checked: {0}", stream.
CheckCertRevocationStatus);

            X509Certificate localCertificate =
 stream.LocalCertificate;
            if (stream.LocalCertificate != null)
            {
                Console.WriteLine("Local cert
 was issued to {0} and is valid from {1}
 until {2}.",
                    localCertificate.Subject,
         localCertificate.GetEffectiveDateString(),
         localCertificate.GetExpirationDateString());
         } else
            {
                Console.WriteLine(
"Local certificate is null.");
            }
            // Display the properties of the
 client's certificate.
            X509Certificate remoteCertificate =
 stream.RemoteCertificate;
            if (stream.RemoteCertificate != null)
            {
            Console.WriteLine(
"Remote cert was issued to {0} and is valid
 from {1} until {2}.",
                remoteCertificate.Subject,
                remoteCertificate.
GetEffectiveDateString(),
                remoteCertificate.
GetExpirationDateString());
            } else
            {
                Console.WriteLine(
"Remote certificate is null.");
            }
        }

        private static void DisplayUsage()
        {
            Console.WriteLine("To start the
 server specify:");
            Console.WriteLine("serverSync
certificateFile.cer");
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
