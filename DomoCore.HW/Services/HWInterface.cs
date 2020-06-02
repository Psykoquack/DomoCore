using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Logging;
using System;
using System.Device.Gpio;
using System.Device.Spi;
using System.Security.Cryptography.X509Certificates;

namespace DomoCore.HW.MyServices
{
    public class HWInterface
    {
        // Properties
        public static bool IsInitialized { get; private set; }

        // Constants for CS pins
        private const int gpioInput1 = 4;
        private const int gpioInput2 = 17;
        private const int gpioOutput1 = 27;
        private const int gpioOutput2 = 5;
        private const int gpioOutput3 = 6;
        private const int gpioOutput4 = 13;

        // Constants for MPC23S17
        private const byte MPC23S17_GPIOA = 0x12;
        private const byte MPC23S17_GPIOB = 0x13;
        private const byte MPC23S17_IO_READ = 0x41;

        // Fields
        private static object ctrllock = new object();
        private static GpioController gpioController = new GpioController();
        private static SpiDevice spiDevice;

        public void Init()
        {
            lock (ctrllock)
            {
                // Open the CS pins as outputs
                gpioController.OpenPin(gpioInput1, PinMode.Output);
                gpioController.OpenPin(gpioInput2, PinMode.Output);
                gpioController.OpenPin(gpioOutput1, PinMode.Output);
                gpioController.OpenPin(gpioOutput2, PinMode.Output);
                gpioController.OpenPin(gpioOutput3, PinMode.Output);
                gpioController.OpenPin(gpioOutput4, PinMode.Output);

                // Set the CS pins to the high state
                gpioController.Write(gpioInput1, PinValue.High);
                gpioController.Write(gpioInput2, PinValue.High);
                gpioController.Write(gpioOutput1, PinValue.High);
                gpioController.Write(gpioOutput2, PinValue.High);
                gpioController.Write(gpioOutput3, PinValue.High);
                gpioController.Write(gpioOutput4, PinValue.High);


                // Set up the SPI connection
                SpiConnectionSettings spiConnectionSettings = new SpiConnectionSettings(0, 0);
                spiConnectionSettings.ClockFrequency = 500000;
                spiConnectionSettings.Mode = SpiMode.Mode1;
                spiDevice = SpiDevice.Create(spiConnectionSettings);
            }

            IsInitialized = true;

            return;
        }


        /// <summary>
        /// Read the inputs
        /// </summary>
        /// <returns>32 bit value containing the state of the buttons where '1' = pressed and '0' = not pressed</returns>
        public uint ReadInputs(ILogger<InputMonitorService> logger)
        {
            uint value = 0xFFFFFFFF;
            byte[] readBuffer = new byte[3];
            byte[] writeBuffer = new byte[3] { MPC23S17_IO_READ, MPC23S17_GPIOA, 0 };

            lock (ctrllock)
            {
                writeBuffer[1] = MPC23S17_GPIOA;
                gpioController.Write(gpioInput1, PinValue.Low);
                spiDevice.TransferFullDuplex(writeBuffer, readBuffer);
                gpioController.Write(gpioInput1, PinValue.High);
                value &= (uint)((readBuffer[2] << 0) | 0xFFFFFF00);

                writeBuffer[1] = MPC23S17_GPIOB;
                gpioController.Write(gpioInput1, PinValue.Low);
                spiDevice.TransferFullDuplex(writeBuffer, readBuffer);
                gpioController.Write(gpioInput1, PinValue.High);
                value &= (uint)((readBuffer[2] << 8) | 0xFFFF00FF);

                writeBuffer[1] = MPC23S17_GPIOA;
                gpioController.Write(gpioInput2, PinValue.Low);
                spiDevice.TransferFullDuplex(writeBuffer, readBuffer);
                gpioController.Write(gpioInput2, PinValue.High);
                value &= (uint)((readBuffer[2] << 16) | 0xFF00FFFF);

                writeBuffer[1] = MPC23S17_GPIOB;
                gpioController.Write(gpioInput2, PinValue.Low);
                spiDevice.TransferFullDuplex(writeBuffer, readBuffer);
                gpioController.Write(gpioInput2, PinValue.High);
                value &= (uint)((readBuffer[2] << 24) | 0x00FFFFFF);
            }

            value = ~value;

            return value;
        }



        /// <summary>
        /// Sets the outputs of all relay drivers
        /// </summary>
        /// <param name="value">Bitwise output value where '1' = on and '0' = off</param>
        public void SetOutputs(uint value)
        {
            byte value0;
            
            value0 = (byte)(value & 0x000000FF);
            SetOutput(gpioOutput1, value0);
            value0 = (byte)((value & 0x0000FF00) >> 8);
            SetOutput(gpioOutput2, value0);
            value0 = (byte)((value & 0x00FF0000) >> 16);
            SetOutput(gpioOutput3, value0);
            value0 = (byte)((value & 0xFF000000) >> 24);
            SetOutput(gpioOutput4, value0);

        }


        /// <summary>
        /// Sets the outputs of one relay driver
        /// </summary>
        /// <param name="chipSelect">Chip select of the relay driver</param>
        /// <param name="value">Bitwise output value where '1' = on and '0' = off</param>
        private void SetOutput(int chipSelect, byte value)
        {
            byte[] readBuffer = new byte[2];
            byte[] writeBuffer = new byte[2];
            uint writeValue = 0xFFFFFFFF;
            for (int j = 0; j < 8; j++)
            {
                if ((value & (0x01 << j)) == (0x01 << j))
                {
                    writeValue &= (uint)~(0x00000001 << 2 * j);
                }
            }
            writeBuffer[0] = (byte)((writeValue >> 8) & (0x00FF));
            writeBuffer[1] = (byte)(writeValue & (0x00FF));
            
            lock (ctrllock)
            {
                gpioController.Write(chipSelect, PinValue.Low);
                spiDevice.TransferFullDuplex(writeBuffer, readBuffer);
                gpioController.Write(chipSelect, PinValue.High);
            }
        }

    }
}
