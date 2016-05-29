using System;
using System.Collections;
using System.Collections.Generic;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyXls
{
    /// <summary>
    /// Manages the collection of Worksheets for a Workbook.
    /// </summary>
	public class Worksheets : IEnumerable<Worksheet>
	{
        private List<Worksheet> _worksheets = new List<Worksheet>();

		private readonly XlsDocument _doc;

		private int _streamOffset;

        internal Worksheets(XlsDocument doc) : base()
		{
			_doc = doc;
		}

        internal void Add(Record boundSheetRecord, List<Record> sheetRecords)
	    {
            Worksheet sheet = new Worksheet(_doc, boundSheetRecord, sheetRecords);
            _worksheets.Add(sheet);
	    }

        /// <summary>
        /// Adds a Worksheet with the given name to this collection.
        /// </summary>
        /// <param name="name">The name of the new worksheet.</param>
        /// <returns>The new Worksheet with the given name in this collection.</returns>
        public Worksheet Add(string name)
        {
            Worksheet sheet = new Worksheet(_doc);
            sheet.Name = name;
            _worksheets.Add(sheet);
            return sheet;
        }

        /// <summary>
        /// Gets the count of Worksheet objects in this collection.
        /// </summary>
        public int Count
        {
            get
            {
                return _worksheets.Count;
            }
        }

        /// <summary>
        /// OBSOLETE - Use Add(string) instead.  Adds a Worksheet with the given 
        /// name to this collection.
        /// </summary>
        /// <param name="name">The name of the new worksheet.</param>
        /// <returns>The new Worksheet with the given name in this collection.</returns>
        [Obsolete]
		public Worksheet AddNamed(string name)
		{
            return Add(name);
		}

        /// <summary>
        /// Gets the Worksheet from this collection with the given index.
        /// </summary>
        /// <param name="index">The index of the Worksheet in this collection to get.</param>
        /// <returns>The Worksheet from this collection with the given index.</returns>
		public Worksheet this[int index]
		{
			get
			{
    			return _worksheets[index];
			}
		}

        /// <summary>
        /// Gets the Worksheet from this collection with the given name.
        /// </summary>
        /// <param name="name">The name of the Worksheet in this collection to get.</param>
        /// <returns>The Worksheet from this collection with the given name.</returns>
        public Worksheet this[string name]
        {
            get
            {
                return this[GetIndex(name)];
            }
        }

        /// <summary>
        /// Gets the index of the Workseet in this collection by the given name.
        /// </summary>
        /// <param name="sheetName">The name of the Worksheet for which to return the index.</param>
        /// <returns>The index of the Worksheet by the given name.</returns>
		public int GetIndex(string sheetName)
		{
            int i = 0;
			foreach (Worksheet sheet in this)
			{
				if (string.Compare(sheet.Name, sheetName, false) == 0)
					return i;
			    i++;
			}

			throw new IndexOutOfRangeException(sheetName);
		}

        internal int StreamOffset
		{
			get { return _streamOffset; }
			set { _streamOffset = value; }
		}

        /// <summary>
        /// Returns an enumerator that iterates through the collection of Worksheets.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection of worksheets.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<Worksheet> GetEnumerator()
        {
            return new WorksheetEnumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection of worksheets.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        ///<summary>
        /// Enumerator for the Workseets collection.
        ///</summary>
        public class WorksheetEnumerator : IEnumerator<Worksheet>
        {
            private Worksheets _worksheets = null;
            private int _index = -1;

            ///<summary>
            /// Creates and initializes a new instance of the WorksheetEnumerator class for the given Worksheets collection instance.
            ///</summary>
            ///<param name="worksheets">The Worksheets object for which to initialize this WorksheetEnumerator object.</param>
            ///<exception cref="ArgumentNullException"></exception>
            public WorksheetEnumerator(Worksheets worksheets)
            {
                if (worksheets == null)
                    throw new ArgumentNullException("worksheets");

                _worksheets = worksheets;
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            /// <filterpriority>2</filterpriority>
            public void Dispose()
            {
                //no-op (no resources to release)
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>
            /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception><filterpriority>2</filterpriority>
            public bool MoveNext()
            {
                if (_worksheets.Count == 0)
                    return false;

                if (_index == (_worksheets.Count - 1))
                    return false;

                _index++;
                return true;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception><filterpriority>2</filterpriority>
            public void Reset()
            {
                _index = -1;
            }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <returns>
            /// The element in the collection at the current position of the enumerator.
            /// </returns>
            public Worksheet Current
            {
                get { return _worksheets[_index]; }
            }

            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            /// <returns>
            /// The current element in the collection.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.-or- The collection was modified after the enumerator was created.</exception><filterpriority>2</filterpriority>
            object IEnumerator.Current
            {
                get { return Current; }
            }
        }
	}
}
