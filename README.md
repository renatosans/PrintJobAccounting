# PrintJobAccounting
Contabiliza trabalhos de impressão e cópias

This service looks for printer jobs when they are created in the printer spool.
This is done by monitoring the spool directory using .NET  FileSystemWatcher

You can use the web interface to generate reports based on the print logs.

Its included an alternative solution using PAPERCUT, where the print logs are generated by PAPERCUT and imported into SQL Server using C#
This tool logs are in the format:
        //     daily         Ex.:  papercut-print-log-2009-05-19.csv
        //     montlhy       Ex.:  papercut-print-log-2009-05.csv
        //     all-time      Ex.:  papercut-print-log-all-time.csv
