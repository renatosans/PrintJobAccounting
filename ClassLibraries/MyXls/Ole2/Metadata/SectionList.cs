using System;
using System.Collections.Generic;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyOle2.Metadata
{
    public partial class MetadataStream
    {
        /// <summary>
        /// Manages the list of Sections for a SummaryInformation object.
        /// </summary>
        public class SectionList
        {
            private List<Section> _sections = new List<Section>();

            internal Bytes Bytes
            {
                get
                {
                    Bytes bytes = new Bytes();

                    List<Bytes> sectionBytesList = new List<Bytes>();

                    int offset = 28 + (20 * _sections.Count);
                    foreach (Section section in _sections)
                    {
                        bytes.Append(section.FormatId);
                        bytes.Append(BitConverter.GetBytes((uint)offset));
                        sectionBytesList.Add(section.Bytes);
                        offset += sectionBytesList[sectionBytesList.Count - 1].Length;
                    }

                    foreach (Bytes sectionBytesListItem in sectionBytesList)
                        bytes.Append(sectionBytesListItem);

                    return bytes;
                }
            }

            /// <summary>
            /// Gets the number of Section objects in this SectionList.
            /// </summary>
            public uint Count
            {
                get { return (uint)_sections.Count; }
            }

            /// <summary>
            /// Adds a Section to this MetadataStream's Sections collection.
            /// </summary>
            /// <param name="section"><see crefk="Section" /> to add.</param>
            public void Add(Section section)
            {
                _sections.Add(section);
            }
        }
    }
}
