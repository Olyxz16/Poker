using System.Net.Sockets;
using System.Text;

namespace Poker.Players.Net;

public abstract class TCPCommunicator
{

    private const int PACKET_SIZE_INDICATOR_LEN = 10;
    
    protected static Protocol Receive(TcpClient client) {
        var stream = client.GetStream();
        while(!stream.DataAvailable || client.Available<PACKET_SIZE_INDICATOR_LEN);
        var data = new byte[PACKET_SIZE_INDICATOR_LEN];
        stream.Read(data, 0, data.Length);

        var sizeData = Encoding.UTF8.GetString(data);
        uint size = uint.Parse(sizeData);

        while(client.Available != size);
        data = new byte[size];
        stream.Read(data, 0, data.Length);

        Protocol result;
        try {
            result = Protocol.Parse(Encoding.UTF8.GetString(data));
        } catch (Exception) {
            result = new Protocol();
        }
        return result;
    }

    protected static void Send(TcpClient client, Protocol value) {
        var stream = client.GetStream();
        var data = Encoding.UTF8.GetBytes(value.Serialize());
        var dataSize = GetDataSizeString(data);
        stream.Write(dataSize);
        stream.Write(data, 0, data.Length);
    }
    private static byte[] GetDataSizeString(byte[] data) {
        var len = data.Length;
        var result = len.ToString().PadLeft(10, '0');
        return Encoding.UTF8.GetBytes(result);
    }


}
