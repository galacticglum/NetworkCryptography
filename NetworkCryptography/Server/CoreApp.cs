﻿/*
 * Author: Shon Verch
 * File Name: CoreApp.cs
 * Project: NetworkCryptography
 * Creation Date: 9/27/2017
 * Modified Date: 9/27/2017
 * Description: The main application context; manages all logic.
 */

using System;
using System.Timers;
using NetworkCryptography.Core;
using NetworkCryptography.Core.Helpers;
using NetworkCryptography.Core.Logging;

namespace NetworkCryptography.Server
{
    /// <summary>
    /// The main application context; manages all logic.
    /// </summary>
    internal static class CoreApp
    {
        /// <summary>
        /// Indicates whether the application context is running.
        /// </summary>
        public static bool IsRunning => tickLoop.Running;

        /// <summary>
        /// Networked server peer.
        /// </summary>
        public static Server Server { get; private set; }

        /// <summary>
        /// The logic loop ticker.
        /// </summary>
        private static readonly TickLoop tickLoop;

        /// <summary>
        /// Properly formatted names of different cryptography methods.
        /// </summary>
        private static readonly string[] cryptographyTypeNames =
        {
            "Caesar Cipher",
            "Data Encryption Standard (DES)",
            "Rivest–Shamir–Adleman (RSA) Scheme"
        };

        static CoreApp()
        {
            tickLoop = new TickLoop(Tick);
        }

        /// <summary>
        /// Starts up the server on the specified port with the specified cryptography method.
        /// </summary>
        /// <param name="port"></param>
        /// <param name="cryptographyMethodType"></param>
        public static void Run(int port, CryptographyMethodType cryptographyMethodType)
        {
            Server = new Server(port);
            Server.Start();

            Initialize();

            Logger.Destination = LoggerDestination.All;
            Logger.Log($"Started server on <{NetworkHelper.GetLocalIpAddress()}:{port}>", LoggerVerbosity.Plain);
        }

        /// <summary>
        /// Run initialization logic for the application.
        /// </summary>
        private static void Initialize()
        {
            tickLoop.Start();
        }

        /// <summary>
        /// Run update logic for the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="elapsedEventArgs"></param>
        private static void Tick(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            Server?.Tick();
        }

        /// <summary>
        /// Process any server console commands.
        /// </summary>
        private static void ProcessConsoleCommands()
        {
            while (IsRunning)
            {
                Console.Write(">>> ");
                string line = Console.ReadLine();
                if (string.IsNullOrEmpty(line)) continue;

                switch (line)
                {
                    case "quit":
                        Exit();
                        break;
                    default:
                        Console.WriteLine($"'{line}' is not recognized as a command.{Environment.NewLine}");
                        break;
                }
            }
        }

        /// <summary>
        /// Close the application.
        /// </summary>
        private static void Exit()
        {
            tickLoop.Stop();
            Logger.FlushMessageBuffer();
        }

        /// <summary>
        /// Main entry point for the application.
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            int port = int.Parse(ConsoleDisplay.InputField("Please enter the port you would like to run the server on."));
            Console.Clear();

            CryptographyMethodType selectedMethod = EnumHelper.GetValue<CryptographyMethodType>(ConsoleDisplay.Menu(
                "What cryptography method would you like to use?", cryptographyTypeNames));

            Console.Clear();
            Run(port, selectedMethod);
            ProcessConsoleCommands();
        }
    }
}
