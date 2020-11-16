using System;

namespace AdapterPattern
{

    // Abstract adapter

    public interface IRadioAdapter
    {
        void Send(byte channel, string content);
    }

    // Concrete adapter
    public class MotorolaRadioAdapter : IRadioAdapter
    {
        // Adaptee
        private readonly MotorolaRadio radio;

        private readonly string pincode;

        public MotorolaRadioAdapter(string pincode)
        {
            radio = new MotorolaRadio();

            this.pincode = pincode;
        }

        public void Send(byte channel, string content)
        {
            radio.PowerOn(pincode);
            radio.SelectChannel(channel);
            radio.Send(content);
            radio.PowerOff();
        }
    }

    

    public class MotorolaRadio
    {
        private bool enabled;

        private byte? selectedChannel;

        public MotorolaRadio()
        {
            enabled = false;
        }

        public void PowerOn(string pincode)
        {
            if (pincode == "1234")
            {
                enabled = true;
            }
        }

        public void SelectChannel(byte channel)
        {
            this.selectedChannel = channel;
        }

        public void Send(string message)
        {
            if (enabled && selectedChannel!=null)
            {
                Console.WriteLine($"<Xml><Send Channel={selectedChannel}><Message>{message}</Message></xml>");
            }
        }

        public void PowerOff()
        {
            enabled = false;
        }



    }
}
