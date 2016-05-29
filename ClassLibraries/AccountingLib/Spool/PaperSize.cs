using System;
using DocMageFramework.CustomAttributes;


namespace AccountingLib.Spool
{
    public enum PaperSize
    {
        [AssociatedText("Letter 8 1/2 x 11 in")]
        DMPAPER_LETTER = 1,
        [AssociatedText("Letter Small 8 1/2 x 11 in")]
        DMPAPER_LETTERSMALL = 2,
        [AssociatedText("Tabloid 11 x 17 in")]
        DMPAPER_TABLOID = 3,
        [AssociatedText("Ledger 17 x 11 in")]
        DMPAPER_LEDGER = 4,
        [AssociatedText("Legal 8 1/2 x 14 in")]
        DMPAPER_LEGAL = 5,
        [AssociatedText("Statement 5 1/2 x 8 1/2 in")]
        DMPAPER_STATEMENT = 6,
        [AssociatedText("Executive 7 1/4 x 10 1/2 in")]
        DMPAPER_EXECUTIVE = 7,
        [AssociatedText("A3 297 x 420 mm")]
        DMPAPER_A3 = 8,
        [AssociatedText("A4 210 x 297 mm")]
        DMPAPER_A4 = 9,
        [AssociatedText("A4 Small 210 x 297 mm")]
        DMPAPER_A4SMALL = 10,
        [AssociatedText("A5 148 x 210 mm")]
        DMPAPER_A5 = 11,
        [AssociatedText("B4 250 x 354")]
        DMPAPER_B4 = 12,
        [AssociatedText("B5 182 x 257 mm")]
        DMPAPER_B5 = 13,
        [AssociatedText("Folio 8 1/2 x 13 in")]
        DMPAPER_FOLIO = 14,
        [AssociatedText("Quarto 215 x 275 mm")]
        DMPAPER_QUARTO = 15,
        [AssociatedText("10x14 in")]
        DMPAPER_10X14 = 16,
        [AssociatedText("11x17 in")]
        DMPAPER_11X17 = 17,
        [AssociatedText("Note 8 1/2 x 11 in")]
        DMPAPER_NOTE = 18,
        [AssociatedText("Envelope #9 3 7/8 x 8 7/8")]
        DMPAPER_ENV_9 = 19,
        [AssociatedText("Envelope #10 4 1/8 x 9 1/2")]
        DMPAPER_ENV_10 = 20,
        [AssociatedText("Envelope #11 4 1/2 x 10 3/8")]
        DMPAPER_ENV_11 = 21,
        [AssociatedText("Envelope #12 4 276 x 11")]
        DMPAPER_ENV_12 = 22,
        [AssociatedText("Envelope #14 5 x 11 1/2")]
        DMPAPER_ENV_14 = 23,
        [AssociatedText("C size sheet")]
        DMPAPER_CSHEET = 24,
        [AssociatedText("D size sheet")]
        DMPAPER_DSHEET = 25,
        [AssociatedText("E size sheet")]
        DMPAPER_ESHEET = 26,
        [AssociatedText("Envelope DL 110 x 220mm")]
        DMPAPER_ENV_DL = 27,
        [AssociatedText("Envelope C5 162 x 229 mm")]
        DMPAPER_ENV_C5 = 28,
        [AssociatedText("Envelope C3  324 x 458 mm")]
        DMPAPER_ENV_C3 = 29,
        [AssociatedText("Envelope C4  229 x 324 mm")]
        DMPAPER_ENV_C4 = 30,
        [AssociatedText("Envelope C6  114 x 162 mm")]
        DMPAPER_ENV_C6 = 31,
        [AssociatedText("Envelope C65 114 x 229 mm")]
        DMPAPER_ENV_C65 = 32,
        [AssociatedText("Envelope B4  250 x 353 mm")]
        DMPAPER_ENV_B4 = 33,
        [AssociatedText("Envelope B5  176 x 250 mm")]
        DMPAPER_ENV_B5 = 34,
        [AssociatedText("Envelope B6  176 x 125 mm")]
        DMPAPER_ENV_B6 = 35,
        [AssociatedText("Envelope 110 x 230 mm")]
        DMPAPER_ENV_ITALY = 36,
        [AssociatedText("Envelope Monarch 3.875 x 7.5 in")]
        DMPAPER_ENV_MONARCH = 37,
        [AssociatedText("6 3/4 Envelope 3 5/8 x 6 1/2 in")]
        DMPAPER_ENV_PERSONAL = 38,
        [AssociatedText("US Std Fanfold 14 7/8 x 11 in")]
        DMPAPER_FANFOLD_US = 39,
        [AssociatedText("German Std Fanfold 8 1/2 x 12 in")]
        DMPAPER_FANFOLD_STD_GERMAN = 40,
        [AssociatedText("German Legal Fanfold 8 1/2 x 13 in")]
        DMPAPER_FANFOLD_LGL_GERMAN = 41,
        [AssociatedText("B4 (ISO) 250 x 353 mm")]
        DMPAPER_ISO_B4 = 42,
        [AssociatedText("Japanese Postcard 100 x 148 mm")]
        DMPAPER_JAPANESE_POSTCARD = 43,
        [AssociatedText("9 x 11 in")]
        DMPAPER_9X11 = 44,
        [AssociatedText("10 x 11 in")]
        DMPAPER_10X11 = 45,
        [AssociatedText("15 x 11 in")]
        DMPAPER_15X11 = 46,
        [AssociatedText("Envelope Invite 220 x 220 mm")]
        DMPAPER_ENV_INVITE = 47,
        [AssociatedText("RESERVED-DO NOT USE")]
        DMPAPER_RESERVED_48 = 48,
        [AssociatedText("RESERVED-DO NOT USE")]
        DMPAPER_RESERVED_49 = 49,
        [AssociatedText("Letter Extra 9 275 x 12 in")]
        DMPAPER_LETTER_EXTRA = 50,
        [AssociatedText("Legal Extra 9 275 x 15 in")]
        DMPAPER_LEGAL_EXTRA = 51,
        [AssociatedText("Tabloid Extra 11.69 x 18 in")]
        DMPAPER_TABLOID_EXTRA = 52,
        [AssociatedText("A4 Extra 9.27 x 12.69 in")]
        DMPAPER_A4_EXTRA = 53,
        [AssociatedText("Letter Transverse 8 275 x 11 in")]
        DMPAPER_LETTER_TRANSVERSE  = 54,
        [AssociatedText("A4 Transverse 210 x 297 mm")]
        DMPAPER_A4_TRANSVERSE = 55,
        [AssociatedText("Letter Extra Transverse 9 275 x 12 in")]
        DMPAPER_LETTER_EXTRA_TRANSVERSE = 56,
        [AssociatedText("SuperASuperAA4 227 x 356 mm")]
        DMPAPER_A_PLUS = 57,
        [AssociatedText("SuperBSuperBA3 305 x 487 mm")]
        DMPAPER_B_PLUS = 58,
        [AssociatedText("Letter Plus 8.5 x 12.69 in")]
        DMPAPER_LETTER_PLUS = 59,
        [AssociatedText("A4 Plus 210 x 330 mm")]
        DMPAPER_A4_PLUS = 60,
        [AssociatedText("A5 Transverse 148 x 210 mm")]
        DMPAPER_A5_TRANSVERSE = 61,
        [AssociatedText("B5 (JIS) Transverse 182 x 257 mm")]
        DMPAPER_B5_TRANSVERSE = 62,
        [AssociatedText("A3 Extra 322 x 445 mm")]
        DMPAPER_A3_EXTRA = 63,
        [AssociatedText("A5 Extra 174 x 235 mm")]
        DMPAPER_A5_EXTRA = 64,
        [AssociatedText("B5 (ISO) Extra 201 x 276 mm")]
        DMPAPER_B5_EXTRA = 65,
        [AssociatedText("A2 420 x 594 mm")]
        DMPAPER_A2 = 66,
        [AssociatedText("A3 Transverse 297 x 420 mm")]
        DMPAPER_A3_TRANSVERSE = 67,
        [AssociatedText("A3 Extra Transverse 322 x 445 mm")]
        DMPAPER_A3_EXTRA_TRANSVERSE = 68,
        [AssociatedText("Japanese Double Postcard 200 x 148 mm")]
        DMPAPER_DBL_JAPANESE_POSTCARD = 69,
        [AssociatedText("A6 105 x 148 mm")]
        DMPAPER_A6 = 70,
        [AssociatedText("Japanese Envelope Kaku #2")]
        DMPAPER_JENV_KAKU2 = 71,
        [AssociatedText("Japanese Envelope Kaku #3")]
        DMPAPER_JENV_KAKU3 = 72,
        [AssociatedText("Japanese Envelope Chou #3")]
        DMPAPER_JENV_CHOU3 = 73,
        [AssociatedText("Japanese Envelope Chou #4")]
        DMPAPER_JENV_CHOU4 = 74,
        [AssociatedText("Letter Rotated 11 x 8 1/2 in")]
        DMPAPER_LETTER_ROTATED = 75,
        [AssociatedText("A3 Rotated 420 x 297 mm")]
        DMPAPER_A3_ROTATED = 76,
        [AssociatedText("A4 Rotated 297 x 210 mm")]
        DMPAPER_A4_ROTATED = 77,
        [AssociatedText("A5 Rotated 210 x 148 mm")]
        DMPAPER_A5_ROTATED = 78,
        [AssociatedText("B4 (JIS) Rotated 364 x 257 mm")]
        DMPAPER_B4_JIS_ROTATED = 79,
        [AssociatedText("B5 (JIS) Rotated 257 x 182 mm")]
        DMPAPER_B5_JIS_ROTATED = 80,
        [AssociatedText("Japanese Postcard Rotated 148 x 100 mm")]
        DMPAPER_JAPANESE_POSTCARD_ROTATED = 81,
        [AssociatedText("Double Japanese Postcard Rotated 148 x 200 mm")]
        DMPAPER_DBL_JAPANESE_POSTCARD_ROTATED = 82,
        [AssociatedText("A6 Rotated 148 x 105 mm")]
        DMPAPER_A6_ROTATED = 83,
        [AssociatedText("Japanese Envelope Kaku #2 Rotated")]
        DMPAPER_JENV_KAKU2_ROTATED = 84,
        [AssociatedText("Japanese Envelope Kaku #3 Rotated")]
        DMPAPER_JENV_KAKU3_ROTATED = 85,
        [AssociatedText("Japanese Envelope Chou #3 Rotated")]
        DMPAPER_JENV_CHOU3_ROTATED = 86,
        [AssociatedText("Japanese Envelope Chou #4 Rotated")]
        DMPAPER_JENV_CHOU4_ROTATED = 87,
        [AssociatedText("B6 (JIS) 128 x 182 mm")]
        DMPAPER_B6_JIS = 88,
        [AssociatedText("B6 (JIS) Rotated 182 x 128 mm")]
        DMPAPER_B6_JIS_ROTATED = 89,
        [AssociatedText("12 x 11 in")]
        DMPAPER_12X11 = 90,
        [AssociatedText("Japanese Envelope You #4")]
        DMPAPER_JENV_YOU4 = 91,
        [AssociatedText("Japanese Envelope You #4 Rotated")]
        DMPAPER_JENV_YOU4_ROTATED = 92,
        [AssociatedText("PRC 16K 146 x 215 mm")]
        DMPAPER_P16K = 93,
        [AssociatedText("PRC 32K 97 x 151 mm")]
        DMPAPER_P32K = 94,
        [AssociatedText("PRC 32K(Big) 97 x 151 mm")]
        DMPAPER_P32KBIG = 95,
        [AssociatedText("PRC Envelope #1 102 x 165 mm")]
        DMPAPER_PENV_1 = 96,
        [AssociatedText("PRC Envelope #2 102 x 176 mm")]
        DMPAPER_PENV_2 = 97,
        [AssociatedText("PRC Envelope #3 125 x 176 mm")]
        DMPAPER_PENV_3 = 98,
        [AssociatedText("PRC Envelope #4 110 x 208 mm")]
        DMPAPER_PENV_4 = 99,
        [AssociatedText("PRC Envelope #5 110 x 220 mm")]
        DMPAPER_PENV_5 = 100,
        [AssociatedText("PRC Envelope #6 120 x 230 mm")]
        DMPAPER_PENV_6 = 101,
        [AssociatedText("PRC Envelope #7 160 x 230 mm")]
        DMPAPER_PENV_7 = 102,
        [AssociatedText("PRC Envelope #8 120 x 309 mm")]
        DMPAPER_PENV_8 = 103,
        [AssociatedText("PRC Envelope #9 229 x 324 mm")]
        DMPAPER_PENV_9 = 104,
        [AssociatedText("PRC Envelope #10 324 x 458 mm")]
        DMPAPER_PENV_10 = 105,
        [AssociatedText("PRC 16K Rotated")]
        DMPAPER_P16K_ROTATED = 106,
        [AssociatedText("PRC 32K Rotated")]
        DMPAPER_P32K_ROTATED = 107,
        [AssociatedText("PRC 32K(Big) Rotated")]
        DMPAPER_P32KBIG_ROTATED = 108,
        [AssociatedText("PRC Envelope #1 Rotated 165 x 102 mm")]
        DMPAPER_PENV_1_ROTATED = 109,
        [AssociatedText("PRC Envelope #2 Rotated 176 x 102 mm")]
        DMPAPER_PENV_2_ROTATED = 110,
        [AssociatedText("PRC Envelope #3 Rotated 176 x 125 mm")]
        DMPAPER_PENV_3_ROTATED = 111,
        [AssociatedText("PRC Envelope #4 Rotated 208 x 110 mm")]
        DMPAPER_PENV_4_ROTATED = 112,
        [AssociatedText("PRC Envelope #5 Rotated 220 x 110 mm")]
        DMPAPER_PENV_5_ROTATED = 113,
        [AssociatedText("PRC Envelope #6 Rotated 230 x 120 mm")]
        DMPAPER_PENV_6_ROTATED = 114,
        [AssociatedText("PRC Envelope #7 Rotated 230 x 160 mm")]
        DMPAPER_PENV_7_ROTATED = 115,
        [AssociatedText("PRC Envelope #8 Rotated 309 x 120 mm")]
        DMPAPER_PENV_8_ROTATED = 116,
        [AssociatedText("PRC Envelope #9 Rotated 324 x 229 mm")]
        DMPAPER_PENV_9_ROTATED = 117,
        [AssociatedText("PRC Envelope #10 Rotated 458 x 324 mm")]
        DMPAPER_PENV_10_ROTATED = 118,
        [AssociatedText("User Defined Paper Size")]
        DMPAPER_USER_DEFINED = 256
    }

}
