﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChat.Net.IO
{
    class PacketBuilder
    {
        MemoryStream _ms;
        public PacketBuilder()
        {
            _ms = new MemoryStream();
        }
        public void WriteOpCode(byte opcode)
        {
            _ms.WriteByte(opcode);
        }

        public void WriteString(string msg)
        {
            var msgLengh = msg.Length;
            _ms.Write(BitConverter.GetBytes(msgLengh));
            _ms.Write(Encoding.ASCII.GetBytes(msg));
        }

        public byte[] GetPacketBytes()
        {
            return _ms.ToArray();
        }
    }
}
