using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;
using System.Windows;

namespace iFindCap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const int ReadTimeoutMilliseconds = 1000;
        private readonly List<iFinDPacket> packetCache = new List<iFinDPacket>();
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly Timer cleanTimer = new Timer(1000 * 60 * 5);
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            cleanTimer.Elapsed += CleanTimerElapsed;
            Devices = CaptureDeviceList.Instance;
        }

        private CaptureDeviceList _devices;
        public CaptureDeviceList Devices
        {
            get => _devices;
            set => SetValue(ref _devices, value);
        }

        private ILiveDevice _selectedDevice;
        public ILiveDevice SelectedDevice
        {
            get => _selectedDevice;
            set => SetValue(ref _selectedDevice, value);
        }

        private bool _isStarted;
        public bool IsStarted
        {
            get => _isStarted;
            set => SetValue(ref _isStarted, value);
        }

        private void StartListenClick(object sender, RoutedEventArgs e)
        {
            if (SelectedDevice is ICaptureDevice captureDevice)
            {
                if (!IsStarted)
                {
                    captureDevice.OnPacketArrival += new PacketArrivalEventHandler(DevicePacketArrival);
                    captureDevice.Open(DeviceModes.Promiscuous, ReadTimeoutMilliseconds);
                    captureDevice.StartCapture();
                    cleanTimer.Start();
                    IsStarted = true;
                }
                else
                {
                    captureDevice.StopCapture();
                    captureDevice.OnPacketArrival -= new PacketArrivalEventHandler(DevicePacketArrival);
                    captureDevice.Close();
                    cleanTimer.Stop();
                    IsStarted = false;
                }
            }
        }

        private void SetValue<T>(ref T obj, T value, [CallerMemberName]string propertyName = null)
        {
            obj = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CleanTimerElapsed(object sender, ElapsedEventArgs e)
        {
            logger.Info($"Start to clean old packets, now packet cache size [{packetCache.Count}]");
            foreach (var packet in packetCache.Where(p => DateTime.Now - p.Timestamp > TimeSpan.FromMinutes(5)).ToArray())
            {
                packetCache.Remove(packet);
            }
            logger.Info($"Complete to clean old packets, now packet cache size [{packetCache.Count}]");
        }

        private void DevicePacketArrival(object sender, PacketCapture e)
        {
            try
            {
                RawCapture raw = e.GetPacket();
                Packet packet = Packet.ParsePacket(raw.LinkLayerType, raw.Data);
                if (packet != null && packet.Extract<TcpPacket>() is TcpPacket tcpPacket)
                {
                    if (tcpPacket != null && tcpPacket.SourcePort == iFinDData.iFundPort) // filter iFinD data packets
                    {
                        if (packet.Extract<IPv4Packet>() is IPv4Packet ipv4Packet)
                        {
                            byte[] data = ipv4Packet.PayloadPacket.PayloadData;
                            string dataString = Encoding.UTF8.GetString(data); // parse the tcp packet byte data to string

                            if (tcpPacket.Flags == 0x010 && data.Length > 4) // first packet of iFinD market data
                            {
                                int dataLength = data.Length - 4; // iFind data as fixed 4 bytes value in the head
                                int iFindDataLength = (data[2] << 8) | data[3] & 0x00FF; // get the length of iFind data
                                dataString = dataString.Substring(4); // get iFind json string
                                if (!dataString.StartsWith(iFinDData.Prefix))
                                {
                                    return;
                                }
                                packetCache.Add(new iFinDPacket(iFindDataLength, dataLength, dataString, tcpPacket.SequenceNumber));
                            }
                            else if (tcpPacket.Flags == 0x018) // remaining packets of iFind data, TCp packet has flag [ACK, PSH]
                            {
                                var iFindData = TryAppendData(p => p.RemainLength == data.Length && p.Seq < tcpPacket.SequenceNumber, dataString, data.Length);
                                if (iFindData != null)
                                {
                                    logger.Debug(iFindData.Json);
                                    logger.Debug($"{DateTimeOffset.FromUnixTimeMilliseconds(iFindData.timeSort).ToLocalTime()} -- {iFindData.time} -- {iFindData.sourceName}: {iFindData.thsCode} Bid [{iFindData.bidTime}]: {iFindData.bid}  Ofr [{iFindData.ofrTime}]: {iFindData.ofr}");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error on capture message");
            }
        }

        private iFinDData TryAppendData(Func<iFinDPacket, bool> fun, string dataString, int length)
        {
            foreach (var iFinDPacket in packetCache.Where(fun).ToArray())
            {
                if (iFinDPacket.TryAppend(dataString, length))
                {
                    if (iFinDPacket.IsCompleted)
                    {
                        packetCache.Remove(iFinDPacket);
                        return iFinDPacket.GetiFindDData();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return null;
        }
    }
}
