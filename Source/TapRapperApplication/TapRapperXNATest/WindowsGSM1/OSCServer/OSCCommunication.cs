using System;
using System.Threading;
using System.IO;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using Bespoke.Common.Osc;
using ProjectCommon;
using System.Text;
using System.Collections;
using WiimoteData;

namespace Server
{
	public class OSCCommunication
	{
		public void Initialize(Wiimotes wiimotesObj)
        {
            sSendMessages = true;

            if (TransportType == TransportType.Tcp)
            {
                sOscClient = new OscClient(Destination.Address, Destination.Port);
                sOscClient.Connect();
            }

            _sourceEndPoint = new IPEndPoint(IPAddress.Loopback, Port);

            wiimotesObj.RecordingStartedEvent += new Wiimotes.OnRecordingStartedEvent(OnRecordingStartedEvent);
            wiimotesObj.RecordingCompletedEvent += new Wiimotes.OnRecordingCompletedEvent(OnRecordingCompletedEvent);

            Thread serverThread = new Thread(RunWorker);
            serverThread.Start();
        }

        private OscMessage CreateOscMessage(SensorData sensorData)
        {
            OscMessage dataOscMessage = new OscMessage(_sourceEndPoint, "/taprapper/sensordata", sOscClient);
            dataOscMessage.Append(sensorData.AccDataObject.DataVector.X);
            dataOscMessage.Append(sensorData.AccDataObject.DataVector.Y);
            dataOscMessage.Append(sensorData.AccDataObject.DataVector.Z);
            dataOscMessage.Append(sensorData.GyroDataObject.Pitch);
            dataOscMessage.Append(sensorData.GyroDataObject.Roll);
            dataOscMessage.Append(sensorData.GyroDataObject.Yaw);
            dataOscMessage.Append(sensorData.QuaternionObject.X);
            dataOscMessage.Append(sensorData.QuaternionObject.Y);
            dataOscMessage.Append(sensorData.QuaternionObject.Z);
            dataOscMessage.Append(sensorData.QuaternionObject.W);

            return dataOscMessage;
        }

        public void SendMessage(SensorData sensor1Data, SensorData sensor2Data)
        {
            OscBundle sBundle = new OscBundle(_sourceEndPoint);

            OscMessage dataOscMessage = CreateOscMessage(sensor1Data);
            sBundle.Append(dataOscMessage);

            dataOscMessage = CreateOscMessage(sensor2Data);
            sBundle.Append(dataOscMessage);

            _oscMessageQueue.Enqueue(dataOscMessage);  

            //MemoryStream memoryStream = new MemoryStream();
            //Resources.Image.Save(memoryStream, ImageFormat.Jpeg);

            //oscMessage.Append(memoryStream.ToArray());

            //sSendMessages = false;
        }

        public void StopOscServer()
        {
            sSendMessages = false;
        }

		public void RunWorker()
		{
            try
            {
                while (sSendMessages)
                {
                    while (_oscMessageQueue.Count == 0)
                        Thread.Sleep(1000);

                    OscBundle bundle = (OscBundle)_oscMessageQueue.Dequeue();

                    switch (TransportType)
                    {
                        case TransportType.Udp:
                            bundle.Send(Destination);
                            break;

                        case TransportType.Tcp:
                            bundle.Send();
                            break;
                    }

                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (TransportType == TransportType.Tcp)
            {
                sOscClient.Close();
            }

		}

        public void OnRecordingStartedEvent(object sender, WiimoteRecordingEventArgs e)
        {

        }

        public void OnRecordingCompletedEvent(object sender, WiimoteRecordingEventArgs e)
        {

        }

        public void OnRecordingDataReceivedEvent(object sender, RecordingDataReceivedEventArgs args)
        {

        }


        private static readonly TransportType TransportType = TransportType.Udp;
        private static readonly IPAddress MulticastAddress = IPAddress.Parse("224.25.26.27");
		private static readonly int Port = 5103;
		private static readonly IPEndPoint Destination = new IPEndPoint(MulticastAddress, Port);
        private static readonly string AliveMethod = "/osctest/alive";

		private static volatile bool sSendMessages;
//        private static OscBundle sBundle;
//		private static OscMessage oscMessage;
        private static OscClient sOscClient;
        private Queue _oscMessageQueue;
        private IPEndPoint _sourceEndPoint;
	}
}
