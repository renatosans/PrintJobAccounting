using System;
using System.Collections.Generic;
using System.Text;

namespace org.in2bits.MyOle2.Metadata
{
    /// <summary>
    /// Represents and presents functionality for managing an OLE2 DocumentSummaryInformation
    /// Metadata Stream.
    /// </summary>
    public class DocumentSummaryInformationSection : MetadataStream.Section
    {
        private static readonly byte[] FORMAT_ID_SECTION_0 = new byte[] {
                0x02, 0xD5, 0xCD, 0xD5, 0x9C, 0x2E, 0x1B, 0x10, 0x93, 0x97, 0x08, 0x00, 0x2B, 0x2C, 0xF9, 0xAE };

        private const uint ID_DICTIONARY = 0;
        private const uint ID_CODEPAGE = 1;
        private const uint ID_CATEGORY = 2;
        private const uint ID_PRESENTATION_TARGET = 3;
        private const uint ID_BYTES = 4;
        private const uint ID_LINES = 5;
        private const uint ID_PARAGRAPHS = 6;
        private const uint ID_SLIDES = 7;
        private const uint ID_NOTES = 8;
        private const uint ID_HIDDEN_SLIDES = 9;
        private const uint ID_MM_CLIPS = 10;
        private const uint ID_SCALE_CROP = 11;
        private const uint ID_HEADING_PAIRS = 12;
        private const uint ID_TITLES_OF_PARTS = 13;
        private const uint ID_MANAGER = 14;
        private const uint ID_COMPANY = 15;
        private const uint ID_LINKS_UP_TO_DATE = 16;

        /// <summary>
        /// Initializes a new instance of the DocumentSummaryInformation class.
        /// </summary>
        public DocumentSummaryInformationSection()
        {
            FormatId = FORMAT_ID_SECTION_0;
            CodePage = 1252;
        }

        //TODO: Implement OLE2 Summary Stream Dictionary object
//        public object Dictionary
//        {
//            get { return _dictionary; }
//            set { _dictionary = value; }
//        }

        /// <summary>
        /// Gets or sets the CodePage of this DocumentSummaryInformation Section.  Setting to null removes the property.
        /// </summary>
        public short? CodePage
        {
            get { return (short?)GetProperty(ID_CODEPAGE); }
            set { SetProperty(ID_CODEPAGE, Property.Types.VT_I2, value); }
        }

        /// <summary>
        /// Gets or sets the Category of this DocumentSummaryInformation Section.  Setting to null removes the property.
        /// </summary>
        public string Category
        {
            get { return (string)GetProperty(ID_CATEGORY); }
            set { SetProperty(ID_CATEGORY, Property.Types.VT_LPSTR, value); }
        }

        /// <summary>
        /// Gets or sets the PresentationTarget of this DocumentSummaryInformation Section.  Setting to null removes the property.
        /// </summary>
        public string PresentationTarget
        {
            get { return (string)GetProperty(ID_PRESENTATION_TARGET); }
            set { SetProperty(ID_PRESENTATION_TARGET, Property.Types.VT_LPSTR, value); }
        }

        /// <summary>
        /// Gets or sets the Bytes Property of this DocumentSummaryInformation Section.  Setting to null removes the property.
        /// </summary>
        public int? BytesProperty
        {
            get { return (int?)GetProperty(ID_BYTES); }
            set { SetProperty(ID_BYTES, Property.Types.VT_I4, value); }
        }

        /// <summary>
        /// Gets or sets the Lines of this DocumentSummaryInformation Section.  Setting to null removes the property.
        /// </summary>
        public int? Lines
        {
            get { return (int?)GetProperty(ID_LINES); }
            set { SetProperty(ID_LINES, Property.Types.VT_I4, value); }
        }

        /// <summary>
        /// Gets or sets the Paragraphs of this DocumentSummaryInformation Section.  Setting to null removes the property.
        /// </summary>
        public int? Paragraphs
        {
            get { return (int?)GetProperty(ID_PARAGRAPHS); }
            set { SetProperty(ID_PARAGRAPHS, Property.Types.VT_I4, value); }
        }

        /// <summary>
        /// Gets or sets the Slides of ths DocumentSummaryInformation Section.  Setting to null removes the property.
        /// </summary>
        public int? Slides
        {
            get { return (int?)GetProperty(ID_SLIDES); }
            set { SetProperty(ID_SLIDES, Property.Types.VT_I4, value); }
        }

        /// <summary>
        /// Gets or sets the Notes of this DocumentSummaryInformation Section.  Setting to null removes the property.
        /// </summary>
        public int? Notes
        {
            get { return (int?)GetProperty(ID_NOTES); }
            set { SetProperty(ID_NOTES, Property.Types.VT_I4, value); }
        }

        /// <summary>
        /// Gets or sets the HiddenSlides of this DocumentSummaryInformation Section.  Setting to null removes property.
        /// </summary>
        public int? HiddenSlides
        {
            get { return (int?)GetProperty(ID_HIDDEN_SLIDES); }
            set { SetProperty(ID_HIDDEN_SLIDES, Property.Types.VT_I4, value); }
        }

        /// <summary>
        /// Gets or sets the MM Clips of this DocumentSummaryInformation Section.  Setting to null removes property.
        /// </summary>
        public int? MmClips
        {
            get { return (int?)GetProperty(ID_MM_CLIPS); }
            set { SetProperty(ID_MM_CLIPS, Property.Types.VT_I4, value); }
        }

//        public bool? ScaleCrop
//        {
//            get { return (bool?)GetProperty(ID_SCALE_CROP); }
//            set { SetProperty(ID_SCALE_CROP, Property.Types.VT_BOOL, value); }
//        }

//        public object HeadingPairs
//        {
//            get { return _headingPairs; }
//            set { _headingPairs = value; }
//        }

//        public object TitlesOfParts
//        {
//            get { return _titlesOfParts; }
//            set { _titlesOfParts = value; }
//        }

        /// <summary>
        /// Gets or sets the Manager of this DocumentSummaryInformation Section.  Setting to null removes the property.
        /// </summary>
        public string Manager
        {
            get { return (string)GetProperty(ID_MANAGER); }
            set { SetProperty(ID_MANAGER, Property.Types.VT_LPSTR, value); }
        }

        /// <summary>
        /// Gets or sets the Company of this DocumentSummaryInformation Section.  Setting to null removes the property.
        /// </summary>
        public string Company
        {
            get { return (string)GetProperty(ID_COMPANY); }
            set { SetProperty(ID_COMPANY, Property.Types.VT_LPSTR, value); }
        }

//        public bool? LinksUpToDate
//        {
//            get { return _linksUpToDate; }
//            set { _linksUpToDate = value; }
//        }
    }
}
