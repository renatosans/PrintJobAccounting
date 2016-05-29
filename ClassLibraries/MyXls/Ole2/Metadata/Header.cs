using System;
using System.Collections.Generic;
using System.Text;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyOle2.Metadata
{
    public partial class MetadataStream
    {
        /// <summary>
        /// Represents and provides functionality for managing an OLE2 Metadata Stream's Header.
        /// </summary>
        public class Header
        {
            private OriginOperatingSystems _originOperatingSystem =
                OriginOperatingSystems.Default;

            private OriginOperatingSystemVersions _originOperatingSystemVersion =
                OriginOperatingSystemVersions.Default;

            private byte[] _classId = new byte[16];

            private MetadataStream _parent;

            internal Header(MetadataStream parent)
            {
                if (parent == null)
                    throw new ArgumentNullException("parent");

                _parent = parent;
            }

            /// <summary>
            /// Gets or sets the operating system on which this document was created.
            /// </summary>
            public OriginOperatingSystems OriginOperatingSystem
            {
                get { return _originOperatingSystem; }
                set { _originOperatingSystem = value; }
            }

            /// <summary>
            /// Gets or sets the version of the operating system on which this document was created.
            /// </summary>
            public OriginOperatingSystemVersions OriginOperatingSystemVersion
            {
                get { return _originOperatingSystemVersion; }
                set { _originOperatingSystemVersion = value; }
            }

            /// <summary>
            /// Gets or sets the ClassID related to this document.
            /// </summary>
            public byte[] ClassID
            {
                get { return _classId; }
                set { _classId = value; }
            }

            internal Bytes Bytes
            {
                get
                {
                    Bytes bytes = new Bytes();
                    bytes.Append(new byte[2]{0xFE, 0xFF});
                    bytes.Append(new byte[2]{0x00, 0x00}); //listed separately for ease of reference to the documentation
                    bytes.Append(Metadata.OriginOperatingSystemVersion.GetBytes(_originOperatingSystemVersion));
                    bytes.Append(Metadata.OriginOperatingSystem.GetBytes(_originOperatingSystem));
                    bytes.Append(_classId);
                    bytes.Append(BitConverter.GetBytes(_parent.Sections.Count));
                    return bytes;
                }
            }
        }
    }
}
