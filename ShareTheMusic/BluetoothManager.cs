﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ShareTheMusic
{
    public interface BluetoothManager
    {
        string checkBluetooth(bool permition);

        string[] findBTdevices();

        void runServerSide();

        void runClientSide(string deviceName);

        void cancel();

        //List<byte[]> read();
        byte[] read();

        void write(byte[] bytes);
    }
}