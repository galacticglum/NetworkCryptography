﻿/*
 * Author: Shon Verch
 * File Name: CoreClientApp.cs
 * Project: NetworkCryptography
 * Creation Date: 9/27/2017
 * Modified Date: 10/14/2017
 * Description: The main application context; manages all logic.
 */

using System.Timers;
using NetworkCryptography.Core;

namespace NetworkCryptography.Client
{
    /// <summary>
    /// Main application context; manages all logic.
    /// </summary>
    public static class CoreClientApp
    {
        /// <summary>
        /// Is the application still running.
        /// </summary>
        public static bool IsRunning => tickLoop.Running;

        /// <summary>
        /// Networked client peer.
        /// </summary>
        public static Client Client { get; private set; }

        /// <summary>
        /// The logic loop ticker.
        /// </summary>
        private static readonly TickLoop tickLoop;

        static CoreClientApp()
        {
            tickLoop = new TickLoop(Tick);
        }

        /// <summary>
        /// Starts up the client and connects to a server.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public static void Run(string username, string ip, int port)
        {
            Client = new Client();
            Client.Connect(username, ip, port);
        }

        public static void Quit()
        {
            Client.Disconnect();
        }

        /// <summary>
        /// Runs continuous logic for the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="elapsedEventArgs"></param>
        private static void Tick(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            Client?.Tick();
        }
    }
}
