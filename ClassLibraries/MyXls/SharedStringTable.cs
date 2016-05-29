using System.Collections.Generic;
using org.in2bits.MyXls.ByteUtil;
using System;

namespace org.in2bits.MyXls
{
    /// <summary>
    /// NOTE: This seems kludgy and wrong, but it was the easiest way
    /// I could think to allow for > int.MaxValue unique shared string 
    /// values.  (The index into the SST is a uint, so the max possible 
    /// unique BIFF8 allows for is uint.MaxValue.)
    /// </summary>
    internal class SharedStringTable
    {
        private readonly List<string> _stringsA = new List<string>();
        private readonly List<string> _stringsB = new List<string>();
        private readonly List<uint> _countsA = new List<uint>();
        private readonly List<uint> _countsB = new List<uint>();
        private bool _listAIsFull = false;
        private uint _countUnique = 0;
        private uint _countAll = 0;

        internal uint CountUnique
        {
            get { return _countUnique; }
        }

        internal uint Add(string sharedString)
        {
            _countAll++;
            int index = _stringsA.IndexOf(sharedString);
            if (index != -1)
            {
                _countsA[index]++;
                return (uint)index;
            }
            
            if (!_listAIsFull && _stringsA.Count == int.MaxValue)
                _listAIsFull = true;
            
            if (_listAIsFull)
            {
                index = _stringsB.IndexOf(sharedString);
                if (index != -1)
                {
                    _countsB[index]++;
                    return int.MaxValue + (uint)index;
                }
                else
                {
                    _stringsB.Add(sharedString);
                    _countsB.Add(1);
                    _countUnique++;
                    return int.MaxValue + (uint) (_stringsB.Count - 1);
                }
            }
            else
            {
                _stringsA.Add(sharedString);
                _countsA.Add(1);
                _countUnique++;
                return (uint)(_stringsA.Count - 1);
            }
        }

        internal string GetString(uint atIndex)
        {
            if (atIndex <= int.MaxValue)
                return _stringsA[(int)atIndex];
            else
            {
                atIndex -= int.MaxValue;
                return _stringsB[(int)atIndex];
            }
        }

        internal uint GetCount(uint atIndex)
        {
            if (atIndex <= int.MaxValue)
                return _countsA[(int) atIndex];
            else
            {
                atIndex -= int.MaxValue;
                return _countsB[(int) atIndex];
            }
        }

        internal Bytes Bytes
        {
            get
            {
                Bytes sst = new Bytes();

                bool isFirstContinue = true;

                Bytes bytes = new Bytes();
                bytes.Append(BitConverter.GetBytes(_countUnique));
                bytes.Append(BitConverter.GetBytes(_countAll));

                int remainingRecordBytes = BIFF8.MaxDataBytesPerRecord - bytes.Length;

                AddStrings(_stringsA, ref remainingRecordBytes, ref bytes, sst, ref isFirstContinue);
                AddStrings(_stringsB, ref remainingRecordBytes, ref bytes, sst, ref isFirstContinue);

                //close out -- don't need to keep the new bytes ref
                Continue(sst, bytes, out remainingRecordBytes, ref isFirstContinue);

                return sst;
            }
        }

        //TODO: Implement Rich Text Value support
        //TODO: Implement Asian Text Value support
        internal void ReadBytes(Record sstRecord)
        {
            uint totalStrings = sstRecord.Data.Get(0, 4).GetBits().ToUInt32();
			uint uniqueStrings = sstRecord.Data.Get(4, 4).GetBits().ToUInt32();
            int stringIndex = 0;
            ushort offset = 8;
            int continueIndex = -1;
			while (stringIndex < uniqueStrings)
            {
                string sharedString = UnicodeBytes.Read(sstRecord, 16, ref continueIndex, ref offset);
                Add(sharedString);
                stringIndex++;
            }
        	_countAll = totalStrings;
        }

        private void AddStrings(List<string> stringList, ref int remainingRecordBytes, ref Bytes bytes, Bytes sst, ref bool isFirstContinue)
        {
            foreach (string sharedString in stringList)
            {
                Bytes stringBytes = XlsDocument.GetUnicodeString(sharedString, 16);

                //per excelfileformat.pdf sec. 5.22, can't split a
                //Unicode string to another CONTINUE record before 
                //the first character's byte/s are written, and must 
                //repeat string option flags byte if it is split
                //OPTIM: For smaller filesize, handle the possibility of compressing continued portion of uncompressed strings (low ROI!)
                byte stringOptionFlag = 0xFF;
                bool charsAre16Bit = false;
                int minimumToAdd = int.MaxValue;

                if (stringBytes.Length > remainingRecordBytes)
                {
                    stringOptionFlag = stringBytes.Get(2, 1).ByteArray[0];
                    charsAre16Bit = (stringOptionFlag & 0x01) == 0x01;
                    minimumToAdd = charsAre16Bit ? 5 : 4;
                }

                while (stringBytes != null)
                {
                    if (stringBytes.Length > remainingRecordBytes) //add what we can and continue
                    {
                        bool stringWasSplit = false;
                        if (remainingRecordBytes > minimumToAdd)
                        {
                            int overLength = (stringBytes.Length - remainingRecordBytes);
                            bytes.Append(stringBytes.Get(0, remainingRecordBytes));
                            stringBytes = stringBytes.Get(remainingRecordBytes, overLength);
                            remainingRecordBytes -= remainingRecordBytes;
                            stringWasSplit = true;
                        }

                        bytes = Continue(sst, bytes, out remainingRecordBytes, ref isFirstContinue);

                        if (stringWasSplit)
                        {
                            bytes.Append(stringOptionFlag);
                            remainingRecordBytes--;
                        }
                    }
                    else //add what's left
                    {
                        bytes.Append(stringBytes);
                        remainingRecordBytes -= stringBytes.Length;
                        stringBytes = null; //exit loop to continue to next sharedString
                    }
                }
            }
        }

        //NOTE: Don't want to pass recordBytes by ref or when we set it to a new Bytes
        //instance, it will wipe out what was appended to bytes
        private Bytes Continue(Bytes sst, Bytes bytes, out int remainingRecordBytes, ref bool isFirstContinue)
        {
            sst.Append(Record.GetBytes(isFirstContinue ? RID.SST : RID.CONTINUE, bytes));

            remainingRecordBytes = BIFF8.MaxDataBytesPerRecord;
            isFirstContinue = false;
            return new Bytes();
        }
    }
}
