using System;
using System.Collections.Generic;
using System.Text;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyOle2.Metadata
{
    /// <summary>
    /// Represents a list of OLE2 Metadata Stream Properties.
    /// </summary>
    public class PropertyList
    {
        private Dictionary<uint, Property> _properties = new Dictionary<uint, Property>();

        internal PropertyList()
        {
            
        }

        /// <summary>
        /// Gets or sets the Property in this collection with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Property this[uint id]
        {
            get { return _properties[id]; }
            set { Add(value, true); }
        }

        /// <summary>
        /// Gets the number of Properties contained in this collection.
        /// </summary>
        public uint Count
        {
            get { return (uint) _properties.Count; }
        }

        /// <summary>
        /// Adds a Property to this Collection, optionally overwriting any previously
        /// existent Property with the same id.
        /// </summary>
        /// <param name="property">The Property to add to this collection.</param>
        /// <param name="overwrite">true to allow overwriting any existing Property with the
        /// same id, false otherwise.</param>
        public void Add(Property property, bool overwrite)
        {
            if (!overwrite && _properties.ContainsKey(property.Id))
                throw new ApplicationException(string.Format("Can't overwrite existing property with id {0}", property.Id));
            _properties[property.Id] = property;
        }

        internal Bytes Bytes
        {
            get
            {
                Bytes propertiesBytes = new Bytes();
                Bytes propertyListBytes = new Bytes();
                int offset = 8 + (8 * _properties.Count);
                foreach (uint id in _properties.Keys)
                {
                    Property property = _properties[id];
                    Bytes propertyBytes = property.Bytes;
                    propertyListBytes.Append(BitConverter.GetBytes(property.Id));
                    propertyListBytes.Append(BitConverter.GetBytes((uint)offset));
                    offset += propertyBytes.Length;
                    propertiesBytes.Append(propertyBytes);
                }

                Bytes bytes = new Bytes();

                bytes.Append(propertyListBytes);
                bytes.Append(propertiesBytes);

                return bytes;
            }
        }

        /// <summary>
        /// Determines whether the PropertyList contains the specified key.
        /// </summary>
        /// <param name="id">The id of the Property to check for.</param>
        /// <returns>true if this PropertyList contains the key id, false otherwise.</returns>
        public bool ContainsKey(uint id)
        {
            return _properties.ContainsKey(id);
        }

        /// <summary>
        /// Removes the specified key from the PropertyList.
        /// </summary>
        /// <param name="id">The id of the Propert to remove.</param>
        /// <returns>true if the Property with the specified id was removed, false otherwise.</returns>
        public bool Remove(uint id)
        {
            return _properties.Remove(id);
        }
    }
}
