﻿#region Dependencies

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MusicBeePlugin.AndroidRemote.Persistence;
using MusicBeePlugin.Tools;
using ServiceStack.Text;

#endregion

namespace MusicBeePlugin.AndroidRemote.Networking
{
	/// <summary>
	///     UDP Multicast Server class responsible used to provide easy connectivity
	///     info to the clients in the local network. Used for the automatic detection
	///     of the server.
	/// </summary>
	internal class ServiceDiscovery
	{
		/// <summary>
		///     The port of for the UDP multicast. The client has to connect to the
		///     specified port.
		/// </summary>
		private const int Port = 45345;

		/// <summary>
		///     The IP address for the UDP multicast. The client has to connect to the
		///     specified IP address.
		/// </summary>
		private const string MulticastIp = "239.1.5.10";

		/// <summary>
		///     Persistence controller instance responsible for the application settings.
		/// </summary>
		private readonly PersistenceController _controller;

		/// <summary>
		///     UDP connection listener
		/// </summary>
		private UdpClient _mListener;

		/// <summary>
		///     Creates a new ServiceDiscover with the provided <see cref="_controller" />.
		/// </summary>
		/// <param name="controller">
		///     <see cref="_controller" />
		/// </param>
		public ServiceDiscovery(PersistenceController controller)
		{
			_controller = controller;
		}

		/// <summary>
		///     Starts listening for incoming UDP multicast connections.
		/// </summary>
		public void Start()
		{
			_mListener = new UdpClient(Port, AddressFamily.InterNetwork) {EnableBroadcast = true};
			_mListener.JoinMulticastGroup(MulticastAddress);
			_mListener.BeginReceive(OnDataReceived, null);
		}

		/// <summary>
		///     Handles asynchronously any data send through the multicast channel.
		/// </summary>
		/// <param name="ar"></param>
		private void OnDataReceived(IAsyncResult ar)
		{
			var mEndPoint = new IPEndPoint(IPAddress.Any, Port);
			var request = _mListener.EndReceive(ar, ref mEndPoint);
			var mRequest = Encoding.UTF8.GetString(request);
			var incoming = JsonObject.Parse(mRequest);

			if (incoming.Get("context").Contains("discovery"))
			{
				var addresses = NetworkTools.GetPrivateAddressList();
				var clientAddress = IPAddress.Parse(incoming.Get("address"));
				var interfaceAddress = String.Empty;
				foreach (var ifAddress in from address in addresses
					let ifAddress = IPAddress.Parse(address)
					let subnetMask = NetworkTools.GetSubnetMask(address)
					let firstNetwork = NetworkTools.GetNetworkAddress(ifAddress, subnetMask)
					let secondNetwork = NetworkTools.GetNetworkAddress(clientAddress, subnetMask)
					where firstNetwork.Equals(secondNetwork)
					select ifAddress)
				{
					interfaceAddress = ifAddress.ToString();
					break;
				}

				var notify = new Dictionary<string, object>
				{
					{"context", "notify"},
					{"address", interfaceAddress},
					{"name", Environment.GetEnvironmentVariable("COMPUTERNAME")},
					{"port", _controller.Settings.WebSocketPort},
					{"http", _controller.Settings.HttpPort}
				};
				var response = Encoding.UTF8.GetBytes(JsonSerializer.SerializeToString(notify));
				_mListener.Send(response, response.Length, mEndPoint);
			}
			_mListener.BeginReceive(OnDataReceived, null);
		}

		/// <summary>
		///     The multicast <see cref="IPAddress" />.
		/// </summary>
		private static readonly IPAddress MulticastAddress = IPAddress.Parse(MulticastIp);
	}
}