using System;
using System.Collections.Generic;
using System.Text;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyOle2.Metadata
{
    public partial class MetadataStream
    {
        /// <summary>
        /// Represents a Section of a SummaryInformation stream.
        /// </summary>
        public abstract class Section
        {
            private byte[] _formatId = new byte[16];
            private PropertyList _properties = new PropertyList();

            internal Bytes Bytes
            {
                get
                {
                    Bytes propertyListBytes = Properties.Bytes;
                    uint length = (uint)(8 + propertyListBytes.Length);

                    Bytes bytes = new Bytes();
                    bytes.Append(BitConverter.GetBytes(length));
                    bytes.Append(BitConverter.GetBytes(Properties.Count));
                    bytes.Append(propertyListBytes);
                    return bytes;
                }
            }

            /// <summary>
            /// Gets or sets the Format ID of this MetadataStream Section.
            /// </summary>
            public byte[] FormatId
            {
                get { return _formatId; }
                set
                {
                    if (value == null || value.Length != 16)
                        throw new ArgumentException("Section FormatId must be 16 bytes in length and cannot be null");

                    _formatId = value;
                }
            }

            /// <summary>
            /// Gets the PropertyList of this Section.
            /// </summary>
            public PropertyList Properties
            {
                get { return _properties; }
            }

            /// <summary>
            /// Sets the Property of the specified id and type to the supplied value.
            /// </summary>
            /// <param name="id">The id of the Property whose value is to be set.</param>
            /// <param name="type">The type of the Property whose value is to be set.</param>
            /// <param name="value">The value to which the specified Property is to be set.</param>
            protected void SetProperty(uint id, Property.Types type, object value)
            {
                if (value == null)
                {
                    if (Properties.ContainsKey(id))
                        Properties.Remove(id);
                    return;
                }

                Properties.Add(new Property(id, type, value), true);
            }

            /// <summary>
            /// Gets the value of the Property with the specified id.
            /// </summary>
            /// <param name="id">The id of the Property whose value is to be returned.</param>
            /// <returns>The value of the specified Property.</returns>
            protected object GetProperty(uint id)
            {
                if (!Properties.ContainsKey(id))
                    return null;

                return Properties[id].Value;
            }
        }
    }
}
