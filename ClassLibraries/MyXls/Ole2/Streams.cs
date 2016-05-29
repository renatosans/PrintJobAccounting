using System;
using System.Collections.Generic;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyOle2
{
    /// <summary>
    /// Represents and manages the collection of Streams for a given OLE2 Document.
    /// </summary>
	public class Streams
	{
		private readonly Ole2Document _doc;
		private readonly List<Stream> _streams = new List<Stream>();

        /// <summary>
        /// Initializes a new instance of the Streams class for the given Doc object.
        /// </summary>
        /// <param name="doc">The parent Doc object for this new Streams object.</param>
		public Streams(Ole2Document doc)
		{
			_doc = doc;
		}

        /// <summary>
        /// Adds a new Stream with the given name and containing the given bytes 
        /// to this Streams collection.  If a stream by the name exists, an exception
        /// will be thrown.
        /// </summary>
        /// <param name="bytes">The byte[] to be contained by the new Stream.</param>
        /// <param name="name">The byte[] of the new Stream's name.</param>
        /// <returns>The new Stream object in this Streams collection with the given
        /// name and bytes.</returns>
		public Stream AddNamed(byte[] bytes, byte[] name)
		{
			return AddNamed(new Bytes(bytes), name);
		}

        /// <summary>
        /// Adds a new Stream with the given name and containing the given bytes
        /// to this Streams collection.  If a stream by the name already exists,
        /// an exception will be thrown.
        /// </summary>
        /// <param name="bytes">A Bytes object containing the bytes for the new
        /// Stream.</param>
        /// <param name="name">A Bytes object containing the bytes for the name
        /// of the new Stream.</param>
        /// <returns>The new Stream object in this Streams collection with the given
        /// name and bytes.</returns>
		public Stream AddNamed(Bytes bytes, byte[] name)
        {
            return AddNamed(bytes, name, false);
        }

        /// <summary>
        /// Adds a new Stream with the given name and containing the given bytes
        /// to this Streams collection.  If a stream by the name already exists,
        /// an exception will be thrown or the stream's bytes will be overwritten,
        /// depending on the value in the overwrite parameter.
        /// </summary>
        /// <param name="bytes">A Bytes object containing the bytes for the new
        /// Stream.</param>
        /// <param name="name">A Bytes object containing the bytes for the name
        /// of the new Stream.</param>
        /// <param name="overwrite">Determines the behaviour (overwriting the bytes
        /// or throwing an exception) if a stream by the provided name already
        /// exists.</param>
        /// <returns>The new Stream object in this Streams collection with the given
        /// name and bytes.</returns>
        public Stream AddNamed(Bytes bytes, byte[] name, bool overwrite)
		{
            Stream stream;
            int streamIndex = GetIndex(name);
            if (streamIndex != -1 && !overwrite)
                throw new ArgumentException("value already exists", "name");
            else if (streamIndex != -1)
                stream = this[streamIndex];
            else
            {
                stream = new Stream(_doc);
                stream.Name = name;
                _streams.Add(stream);
            }

			stream.Bytes = bytes;

			return stream;
		}

        /// <summary>
        /// Gets a count of the number of streams included in or managed by this Streams
        /// collection (or the number of streams in this Doc).
        /// </summary>
		public int Count
		{
			get { return _streams.Count - 1; } //Don't count the Short Sector Storage Stream
		}

        /// <summary>
        /// Gets a count of the sectors required by all the streams in this Streams collection
        /// (or in this Doc).
        /// </summary>
		public int SectorCount
		{
			get
			{
				int sectorCount = 0;

				sectorCount += ShortSectorStorage.SectorCount;
				for (int i = 1; i <= Count; i++)
					sectorCount += this[i].SectorCount;

				return sectorCount;
			}
		}

        /// <summary>
        /// Gets the Short Sector Storage stream for this Doc.
        /// </summary>
		public Stream ShortSectorStorage
		{
			get { return _streams[0]; }
		}

        /// <summary>
        /// Gets the Stream with the given index.
        /// </summary>
        /// <param name="idx">The int index or byte[] name of the Stream to get.</param>
        /// <returns>The Stream object of the given int index or byte[] name.</returns>
		public Stream this[object idx]
		{
			get
			{
				if (idx is int)
				{
					return _streams[(int)idx];
				}
				else if (idx is byte[])
				{
					return _streams[GetIndex((byte[])idx)];
				}
				else if (idx is string) //TODO: Should probably exclude Root Storage
				{
					throw new NotImplementedException();
				}

				return null;
			}
		}

        /// <summary>
        /// Gets the int index of the Stream with the provided name.
        /// </summary>
        /// <param name="streamName">The byte[] name of the Stream to get the index of.</param>
        /// <returns>The int index of the Stream with the given byte[] name; else -1.</returns>
		public int GetIndex(byte[] streamName)
		{
			for (int i = 0; i < _streams.Count; i++)
			{
				if (Bytes.AreEqual(_streams[i].Name, streamName))
					return i;
			}

			return -1;
		}

        internal Bytes Bytes
		{
			get
			{
				Bytes bytes = new Bytes();

				int lastLen;
				Stream stream;

				for (int i = 1; i <= Count; i++)
				{
					stream = this[i];
					if (stream.IsShort)
					{
						bytes.Append(stream.Bytes);
						lastLen = (int)((decimal)stream.Bytes.Length % _doc.BytesPerShortSector);
						if (lastLen > 0)
							bytes.Append(new byte[_doc.BytesPerShortSector - lastLen]);
					}
				}
				lastLen = (int)((decimal)bytes.Length % _doc.BytesPerSector);
				if (lastLen > 0)
					bytes.Append(new byte[_doc.BytesPerSector - lastLen]);

				for (int i = 1; i <= Count; i++)
				{
					stream = this[i];
					if (!stream.IsShort)
					{
						bytes.Append(stream.Bytes);
						lastLen = (int)((decimal)stream.Bytes.Length % _doc.BytesPerSector);
						if (lastLen > 0)
							bytes.Append(new byte[_doc.BytesPerSector - lastLen]);
					}
				}

				return bytes;
			}
		}
	}
}
