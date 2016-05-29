using System;
using System.Collections.Generic;
using System.Text;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyOle2.Metadata
{
    /// <summary>
    /// Provides base functionality to aid in representint an OLE2 Metadata Stream, 
    /// such as the SummaryInformation or DocumentSummaryInformation streams.  Implementation
    /// requirements were obtained from http://www.rainer-klute.de/~klute/Software/poibrowser/doc/HPSF-Description.html.
    /// </summary>
    public partial class MetadataStream
    {
        private Ole2Document _parentDocument;
        private Header _header;
        private SectionList _sectionList = new SectionList();

        /// <summary>
        /// Initializes a new instance of the MetadataStream class for the given parent
        /// Ole2Document.
        /// </summary>
        /// <param name="parentDocument">The parent Ole2Document for which to initialize
        /// this MetadataStream.</param>
        public MetadataStream(Ole2Document parentDocument)
        {
            _parentDocument = parentDocument;
            _header = new Header(this);
        }

        /// <summary>
        /// Gets the SectionList of this Metadata object.
        /// </summary>
        public SectionList Sections
        {
            get { return _sectionList; }
        }

        /// <summary>
        /// Gets a Bytes object containing all the bytes in this Metadata stream.
        /// </summary>
        public Bytes Bytes
        {
            get
            {
                Bytes bytes = new Bytes();

                bytes.Append(_header.Bytes);
                bytes.Append(_sectionList.Bytes);
                
                return bytes;
            }
        }
    }
}
