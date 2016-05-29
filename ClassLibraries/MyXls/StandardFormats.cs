namespace org.in2bits.MyXls
{
    /// <summary>
    /// The standard (built-in) Excel Format strings.
    /// NOTE: Currency_1 thru Currency_8 differ from the excelfileformat.pdf doc 
    /// by the added \'s (backslashes)
    /// </summary>
    public static class StandardFormats
    {
        /// <summary>General: General</summary>
        public static readonly string General = "General";

        /// <summary>Decimal 1: 0</summary>
        public static readonly string Decimal_1 = "0";

        /// <summary>Decimal 2: 0.00</summary>
        public static readonly string Decimal_2 = "0.00";

        /// <summary>Decimal 3: #,##0</summary>
        public static readonly string Decimal_3 = "#,##0";

        /// <summary>Decimal 4: #,##0.00</summary>
        public static readonly string Decimal_4 = "#,##0.00";

        /// <summary>Currency 1: "$"#,##0_);("$"#,##0)</summary>
        public static readonly string Currency_1 = "\"$\"#,##0_);\\(\"$\"#,##0\\)";

        /// <summary>Currency 2: "$"#,##0_);[Red]("$"#,##0)</summary>
        public static readonly string Currency_2 = "\"$\"#,##0_);[Red]\\(\"$\"#,##0\\)";

        /// <summary>Currency 3: "$"#,##0.00_);("$"#,##0.00)</summary>
        public static readonly string Currency_3 = "\"$\"#,##0.00_);\\(\"$\"#,##0.00\\)";

        /// <summary>Currency 4: "$"#,##0.00_);[Red]("$"#,##0.00)</summary>
        public static readonly string Currency_4 = "\"$\"#,##0.00_);[Red]\\(\"$\"#,##0.00\\)";

        /// <summary>Percent 1: 0%</summary>
        public static readonly string Percent_1 = "0%";

        /// <summary>Percent 2: 0.00%</summary>
        public static readonly string Percent_2 = "0.00%";

        /// <summary>Scientific 1: 0.00E+00</summary>
        public static readonly string Scientific_1 = "0.00E+00";

        /// <summary>Fraction 1: # ?/?</summary>
        public static readonly string Fraction_1 = "# ?/?";

        /// <summary>Fraction 2: # ??/??</summary>
        public static readonly string Fraction_2 = "# ??/??";

        /// <summary>Date 1: M/D/YY</summary>
        public static readonly string Date_1 = "M/D/YY";

        /// <summary>Date 2: D-MMM-YY</summary>
        public static readonly string Date_2 = "D-MMM-YY";

        /// <summary>Date 3: D-MMM</summary>
        public static readonly string Date_3 = "D-MMM";

        /// <summary>Date 4: MMM-YY</summary>
        public static readonly string Date_4 = "MMM-YY";

        /// <summary>Time 1: h:mm AM/PM</summary>
        public static readonly string Time_1 = "h:mm AM/PM";

        /// <summary>Time 2: h:mm:ss AM/PM</summary>
        public static readonly string Time_2 = "h:mm:ss AM/PM";

        /// <summary>Time 3: h:mm</summary>
        public static readonly string Time_3 = "h:mm";

        /// <summary>Time 4: h:mm:ss</summary>
        public static readonly string Time_4 = "h:mm:ss";

        /// <summary>Date/Time: M/D/YY h:mm</summary>
        public static readonly string Date_Time = "M/D/YY h:mm";

        /// <summary>Accounting 1: _(#,##0_);(#,##0)</summary>
        public static readonly string Accounting_1 = "_(#,##0_);(#,##0)";

        /// <summary>Accounting 2: _(#,##0_);[Red](#,##0)</summary>
        public static readonly string Accounting_2 = "_(#,##0_);[Red](#,##0)";

        /// <summary>Accounting 3: _(#,##0.00_);(#,##0.00)</summary>
        public static readonly string Accounting_3 = "_(#,##0.00_);(#,##0.00)";

        /// <summary>Accounting 4: _(#,##0.00_);[Red](#,##0.00)</summary>
        public static readonly string Accounting_4 = "_(#,##0.00_);[Red](#,##0.00)";

        /// <summary>Currency 5: _("$"* #,##0_);_("$"* (#,##0);_("$"* "-"_);_(@_)</summary>
        public static readonly string Currency_5 = "_(\"$\"* #,##0_);_(\"$\"* \\(#,##0\\);_(\"$\"* \"-\"_);_(@_)";

        /// <summary>Currency 6: _(* #,##0_);_(* (#,##0);_(* "-"_);_(@_)</summary>
        public static readonly string Currency_6 = "_(* #,##0_);_(* \\(#,##0\\);_(* \"-\"_);_(@_)";

        /// <summary>Currency 7: _("$"* #,##0.00_);_("$"* (#,##0.00);_("$"* "-"??_);_(@_)</summary>
        public static readonly string Currency_7 = "_(\"$\"* #,##0.00_);_(\"$\"* \\(#,##0.00\\);_(\"$\"* \"-\"??_);_(@_)";

        /// <summary>Currency 8: _(* #,##0.00_);_(* (#,##0.00);_(* "-"??_);_(@_)</summary>
        public static readonly string Currency_8 = "_(* #,##0.00_);_(* \\(#,##0.00\\);_(* \"-\"??_);_(@_)";

        /// <summary>Time 5: mm:ss</summary>
        public static readonly string Time_5 = "mm:ss";

        /// <summary>Time 6: [h]:mm:ss</summary>
        public static readonly string Time_6 = "[h]:mm:ss";

        /// <summary>Time 7: mm:ss.0</summary>
        public static readonly string Time_7 = "mm:ss.0";

        /// <summary>Scientific 2: ##0.0E+0</summary>
        public static readonly string Scientific_2 = "##0.0E+0";

        /// <summary>Text: @</summary>
        public static readonly string Text = "@";
    }
}
