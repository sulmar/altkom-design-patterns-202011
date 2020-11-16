using System;

namespace AdapterPattern
{
    // Concrete adapter
    public class HyteraRadioAdapter : IRadioAdapter
    {
        // Adaptee
        private readonly HyteraRadio radio = new HyteraRadio();

        public void Send(byte channel, string content)
        {
            radio.Init();
            radio.SendMessage(channel, content);
            radio.Release();
        }
    }

    public class HyteraRadio
    {

        private RadioStatus status;

        public void Init()
        {
            status = RadioStatus.On;
        }

        public void SendMessage(byte channel, string content)
        {
            if (status == RadioStatus.On)
            {
                Console.WriteLine($"CHANNEL {channel}, MESSAGE {content}");
            }
        }

        public void Release()
        {
            status = RadioStatus.Off;
        }

        public enum RadioStatus
        {
            On,
            Off
        }

    }
}
