using System;
using System.Collections.Generic;
using System.Text;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyXls
{
    internal class UnicodeBytes
    {
        internal static void Write(string text, Bytes bytes)
        {
            ushort offset = 0;
            Write(text, bytes, ref offset);
        }

        internal static void Write(string text, Bytes bytes, ref ushort offset)
        {
            throw new NotImplementedException();
        }

        internal static string Read(Bytes bytes, int lengthBits)
        {
            Record record = new Record(RID.Empty, bytes);
            return Read(record, lengthBits, 0);
        }

        private static string Read(Record record, int lengthBits, ushort offset)
        {
            int continueIndex = -1; //throw away
            return Read(record, lengthBits, ref continueIndex, ref offset);
        }

        internal static string Read(Record record, int lengthBits, ref int continueIndex, ref ushort offset)
        {
            string text = string.Empty;

            ReadState state = new ReadState(record, lengthBits, continueIndex, offset);
            Read(state);
            continueIndex = state.ContinueIndex;
            offset = state.Offset;

            return new string(state.CharactersRead.ToArray());
        }

        private static void Read(ReadState state)
        {
            Bytes data = state.GetRecordData();
            
            bool compressed = (state.OptionsFlags & 0x01) == 0;

            ushort bytesAvailable = (ushort)(data.Length - state.Offset);

            ushort bytesToRead;
            if (state.CharactersRead.Count < state.TotalCharacters)
            {
                ushort charBytesRemaining = (ushort)(state.TotalCharacters - state.CharactersRead.Count);
                if (!compressed)
                    charBytesRemaining *= 2;

                if (bytesAvailable < charBytesRemaining)
                    bytesToRead = bytesAvailable;
                else
                    bytesToRead = charBytesRemaining;

                byte[] charBytes = data.Get(state.Offset, bytesToRead).ByteArray;

                if (compressed)
                {
                    //decompress
                    byte[] wideBytes = new byte[charBytes.Length * 2];
                    for (int i = 0; i < charBytes.Length; i++)
                        wideBytes[2 * i] = charBytes[i];
                    charBytes = wideBytes;
                }
                state.Offset += bytesToRead;
                bytesAvailable -= bytesToRead;

                state.CharactersRead.AddRange(Encoding.Unicode.GetChars(charBytes));
            }

            bool allCharsRead = state.CharactersRead.Count == state.TotalCharacters;

            if (state.HasRichTextSettings && bytesAvailable > 0 && allCharsRead &&
                state.FormattingRunBytes.Count < (state.FormattingRunCount * 4))
            {
                bytesToRead = Math.Min(bytesAvailable, (ushort) Math.Min(state.FormattingRunCount*4, ushort.MaxValue));
                state.FormattingRunBytes.AddRange(data.Get(state.Offset, bytesToRead).ByteArray);
                state.Offset += bytesToRead;
                bytesAvailable -= bytesToRead;
            }

            if (state.HasAsianPhonetics && bytesAvailable > 0 && allCharsRead &&
                state.PhoneticSettingsBytes.Count < state.PhoneticSettingsByteCount)
            {
                bytesToRead = Math.Min(bytesAvailable, (ushort)Math.Min(state.PhoneticSettingsByteCount, ushort.MaxValue));
                state.PhoneticSettingsBytes.AddRange(data.Get(state.Offset, bytesToRead).ByteArray);
                state.Offset += bytesToRead;
                bytesAvailable -= bytesToRead;
            }

            if (state.CharactersRead.Count < state.TotalCharacters ||
                state.FormattingRunBytes.Count < (state.FormattingRunCount * 4) ||
                state.PhoneticSettingsBytes.Count < state.PhoneticSettingsByteCount)
            {
                state.Continue(true);
                Read(state);
            }
			else if (bytesAvailable == 0 && (state.ContinueIndex + 1) < state.Record.Continues.Count)
			{
				state.Continue(false);
			}
        }

        private class ReadState
        {
            public Record Record;
            public int LengthBits;
            public ushort TotalCharacters = 0;
            public int ContinueIndex;
            public ushort Offset;
            public List<char> CharactersRead = new List<char>();
            public bool HasAsianPhonetics = false;
            public bool HasRichTextSettings = false;
            public ushort FormattingRunCount = 0;
            public List<byte> FormattingRunBytes = new List<byte>();
            public uint PhoneticSettingsByteCount = 0;
            public List<byte> PhoneticSettingsBytes = new List<byte>();
            public byte OptionsFlags = 0x00;

            public ReadState(Record record, int lengthBits, int continueIndex, ushort offset)
            {
                LengthBits = lengthBits;
                Record = record;
                ContinueIndex = continueIndex;
                Offset = offset;

                Bytes data = GetRecordData();

                if (LengthBits == 8)
                {
                    TotalCharacters = data.Get(offset, 1).ByteArray[0];
                    Offset++;
                }
                else
                {
                    TotalCharacters = data.Get(offset, 2).GetBits().ToUInt16();
                    Offset += 2;
                }

                ReadOptionsFlags();

                HasAsianPhonetics = (OptionsFlags & 0x04) == 0x04;
                HasRichTextSettings = (OptionsFlags & 0x08) == 0x08;

                if (HasRichTextSettings)
                {
                    FormattingRunCount = BitConverter.ToUInt16(data.Get(Offset, 2).ByteArray, 0);
                    Offset += 2;
                    //bytesRemaining += (ushort)(4 * formattingRuns);
                    //NOTE: When implementing Rich Text, remember to add total length of formating runs to bytesRead
                    //throw new NotSupportedException("Rich Text text values in cells are not yet supported");
                }

                if (HasAsianPhonetics)
                {
                    PhoneticSettingsByteCount = BitConverter.ToUInt32(data.Get(Offset, 4).ByteArray, 0);
                    Offset += 4;
                    //NOTE: When implementing Asian Text, remember to add total length of Asian Phonetic Settings Block to bytesRead
                    //throw new NotSupportedException("Asian Phonetic text values in cells are not yet supported");
                }
            }

            private void ReadOptionsFlags()
            {
                OptionsFlags = GetRecordData().Get(Offset++, 1).ByteArray[0];
            }

            public Bytes GetRecordData()
            {
                if (ContinueIndex == -1)
                    return Record.Data;
                else
                    return Record.Continues[ContinueIndex].Data;
            }

            public void Continue(bool readOptions)
            {
                ContinueIndex++;
                Offset = 0;
				if (readOptions)
				{
					ReadOptionsFlags();
				}
            }
        }
    }
}
