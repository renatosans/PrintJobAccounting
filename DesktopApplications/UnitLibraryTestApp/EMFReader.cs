using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AccountingLib.Spool.EMF;


namespace UnitLibraryTestApp
{
    public class EMFReader
    {
        private Boolean fileProcessed;

        private FileStream fileStream;

        private MemoryStream memStream;

        private BinaryReader fileReader;

        private EMFPageHeader header;

        private List<EMFRecord> records;


        public EMFReader(String filename)
        {
            fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            fileReader = new BinaryReader(fileStream, Encoding.Unicode);

            // Marca a posição inicial de leitura do fileStream
            Int64 startPos = fileReader.BaseStream.Position;

            // Faz a leitura do header e retorna para a posição inicial
            header = new EMFPageHeader(fileReader);
            fileReader.BaseStream.Seek(startPos, SeekOrigin.Begin);

            Byte[] buffer = fileReader.ReadBytes(header.FileSize);

            this.fileProcessed = false;
            this.memStream = new MemoryStream();
            this.memStream.Write(buffer, 0, buffer.Length);
            this.memStream.Seek(0, SeekOrigin.Begin);
        }

        [DllImport("gdi32", EntryPoint = "CopyEnhMetaFile")]
        public static extern int CopyEnhMetaFileW(IntPtr hemfSrc, String lpszFile);

        private void ProcessFile()
        {
            records = new List<EMFRecord>();
            for (int record = 1; record <= header.RecordCount; record++) // O primeiro registro é o header
            {
                EMFRecord emfRecord = new EMFRecord(memStream);
                records.Add(emfRecord);
                memStream.Seek(emfRecord.RecSeek + emfRecord.RecSize, SeekOrigin.Begin);
            }
            fileProcessed = true;
        }

        public static void Save(Metafile metafile, String filename)
        {
            IntPtr metafilePtr = metafile.GetHenhmetafile();
            CopyEnhMetaFileW(metafilePtr, filename);
        }

        public Image RenderEMFRecords()
        {
            Image renderingSurface = null;

            if (!fileProcessed) ProcessFile();
            long eofPos = 0;
            foreach (EMFRecord record in records)
            {
                if (record.RecType == EmfPlusRecordType.EmfEof)
                    eofPos = record.RecSeek;
            }
            return renderingSurface;
        }

        private Boolean IsEMF()
        {
            if (fileStream.Length < 48)
                return false;

            return true;
        }
        /*
        static MagickBooleanType IsEMF(const unsigned char *magick,const size_t length)
        {
          if (length < 48)
            return(MagickFalse);
          if (memcmp(magick+40,"\040\105\115\106\000\000\001\000",8) == 0)
            return(MagickTrue);
          return(MagickFalse);
        }
        */
        
        private Boolean IsWMF()
        {
            if (fileStream.Length < 4)
                return false;
            
            return true;
        }
        /*
        static MagickBooleanType IsWMF(const unsigned char *magick,const size_t length)
        {
          if (length < 4)
            return(MagickFalse);
          if (memcmp(magick,"\327\315\306\232",4) == 0)
            return(MagickTrue);
          if (memcmp(magick,"\001\000\011\000",4) == 0)
            return(MagickTrue);
          return(MagickFalse);
        }
        */
    }

}

/*

#include "magick/blob.h"
#include "magick/blob-private.h"
#include "magick/cache.h"
#include "magick/exception.h"
#include "magick/exception-private.h"
#include "magick/geometry.h"
#include "magick/image.h"
#include "magick/image-private.h"
#include "magick/list.h"
#include "magick/magick.h"
#include "magick/memory_.h"
#include "magick/quantum-private.h"
#include "magick/static.h"
#include "magick/string_.h"
#include "magick/module.h"
%
%


#if defined(MAGICKCORE_HAVE__WFOPEN)
static size_t UTF8ToUTF16(const unsigned char *utf8,wchar_t *utf16)
{
  register const unsigned char
    *p;

  if (utf16 != (wchar_t *) NULL)
    {
      register wchar_t
        *q;

      wchar_t
        c;

      
      //  Convert UTF-8 to UTF-16.
      
      q=utf16;
      for (p=utf8; *p != '\0'; p++)
      {
        if ((*p & 0x80) == 0)
          *q=(*p);
        else
          if ((*p & 0xE0) == 0xC0)
            {
              c=(*p);
              *q=(c & 0x1F) << 6;
              p++;
              if ((*p & 0xC0) != 0x80)
                return(0);
              *q|=(*p & 0x3F);
            }
          else
            if ((*p & 0xF0) == 0xE0)
              {
                c=(*p);
                *q=c << 12;
                p++;
                if ((*p & 0xC0) != 0x80)
                  return(0);
                c=(*p);
                *q|=(c & 0x3F) << 6;
                p++;
                if ((*p & 0xC0) != 0x80)
                  return(0);
                *q|=(*p & 0x3F);
              }
            else
              return(0);
        q++;
      }
      *q++='\0';
      return(q-utf16);
    }
  
    // Compute UTF-16 string length.
  
  for (p=utf8; *p != '\0'; p++)
  {
    if ((*p & 0x80) == 0)
      ;
    else
      if ((*p & 0xE0) == 0xC0)
        {
          p++;
          if ((*p & 0xC0) != 0x80)
            return(0);
        }
      else
        if ((*p & 0xF0) == 0xE0)
          {
            p++;
            if ((*p & 0xC0) != 0x80)
              return(0);
            p++;
            if ((*p & 0xC0) != 0x80)
              return(0);
         }
       else
         return(0);
  }
  return(p-utf8);
}

static wchar_t *ConvertUTF8ToUTF16(const unsigned char *source)
{
  size_t
    length;

  wchar_t
    *utf16;

  length=UTF8ToUTF16(source,(wchar_t *) NULL);
  if (length == 0)
    {
      register ssize_t
        i;

      
      //   Not UTF-8, just copy.
 
      length=strlen((char *) source);
      utf16=(wchar_t *) AcquireQuantumMemory(length+1,sizeof(*utf16));
      if (utf16 == (wchar_t *) NULL)
        return((wchar_t *) NULL);
      for (i=0; i <= (ssize_t) length; i++)
        utf16[i]=source[i];
      return(utf16);
    }
  utf16=(wchar_t *) AcquireQuantumMemory(length+1,sizeof(*utf16));
  if (utf16 == (wchar_t *) NULL)
    return((wchar_t *) NULL);
  length=UTF8ToUTF16(source,utf16);
  return(utf16);
}
#endif


 // This method reads either an enhanced metafile, a regular 16bit Windows
 // metafile, or an Aldus Placeable metafile and converts it into an enhanced
 // metafile.  Width and height are returned in .01mm units.

#if defined(MAGICKCORE_WINGDI32_DELEGATE)
static HENHMETAFILE ReadEnhMetaFile(const char *path,ssize_t *width,
  ssize_t *height)
{
#pragma pack( push, 2 )
  typedef struct
  {
    DWORD dwKey;
    WORD hmf;
    SMALL_RECT bbox;
    WORD wInch;
    DWORD dwReserved;
    WORD wCheckSum;
  } APMHEADER, *PAPMHEADER;
#pragma pack( pop )

  DWORD
    dwSize;

  ENHMETAHEADER
    emfh;

  HANDLE
    hFile;

  HDC
    hDC;

  HENHMETAFILE
    hTemp;

  LPBYTE
    pBits;

  METAFILEPICT
    mp;

  HMETAFILE
    hOld;

  *width=512;
  *height=512;
  hTemp=GetEnhMetaFile(path);
#if defined(MAGICKCORE_HAVE__WFOPEN)
  if (hTemp == (HENHMETAFILE) NULL)
    {
      wchar_t
        *unicode_path;

      unicode_path=ConvertUTF8ToUTF16((const unsigned char *) path);
      if (unicode_path != (wchar_t *) NULL)
        {
          hTemp=GetEnhMetaFileW(unicode_path);
          unicode_path=(wchar_t *) RelinquishMagickMemory(unicode_path);
        }
    }
#endif
  if (hTemp != (HENHMETAFILE) NULL)
    {
      
      //  Enhanced metafile.
      
      GetEnhMetaFileHeader(hTemp,sizeof(ENHMETAHEADER),&emfh);
      *width=emfh.rclFrame.right-emfh.rclFrame.left;
      *height=emfh.rclFrame.bottom-emfh.rclFrame.top;
      return(hTemp);
    }
  hOld=GetMetaFile(path);
  if (hOld != (HMETAFILE) NULL)
    {
      
      //  16bit windows metafile.
      
      dwSize=GetMetaFileBitsEx(hOld,0,NULL);
      if (dwSize == 0)
        {
          DeleteMetaFile(hOld);
          return((HENHMETAFILE) NULL);
        }
      pBits=(LPBYTE) AcquireQuantumMemory(dwSize,sizeof(*pBits));
      if (pBits == (LPBYTE) NULL)
        {
          DeleteMetaFile(hOld);
          return((HENHMETAFILE) NULL);
        }
      if (GetMetaFileBitsEx(hOld,dwSize,pBits) == 0)
        {
          pBits=(BYTE *) DestroyString((char *) pBits);
          DeleteMetaFile(hOld);
          return((HENHMETAFILE) NULL);
        }
      
      //  Make an enhanced metafile from the windows metafile.
      
      mp.mm=MM_ANISOTROPIC;
      mp.xExt=1000;
      mp.yExt=1000;
      mp.hMF=NULL;
      hDC=GetDC(NULL);
      hTemp=SetWinMetaFileBits(dwSize,pBits,hDC,&mp);
      ReleaseDC(NULL,hDC);
      DeleteMetaFile(hOld);
      pBits=(BYTE *) DestroyString((char *) pBits);
      GetEnhMetaFileHeader(hTemp,sizeof(ENHMETAHEADER),&emfh);
      *width=emfh.rclFrame.right-emfh.rclFrame.left;
      *height=emfh.rclFrame.bottom-emfh.rclFrame.top;
      return(hTemp);
    }
 
    // Aldus Placeable metafile.
  
  hFile=CreateFile(path,GENERIC_READ,0,NULL,OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL,
    NULL);
  if (hFile == INVALID_HANDLE_VALUE)
    return(NULL);
  dwSize=GetFileSize(hFile,NULL);
  pBits=(LPBYTE) AcquireQuantumMemory(dwSize,sizeof(*pBits));
  ReadFile(hFile,pBits,dwSize,&dwSize,NULL);
  CloseHandle(hFile);
  if (((PAPMHEADER) pBits)->dwKey != 0x9ac6cdd7l)
    {
      pBits=(BYTE *) DestroyString((char *) pBits);
      return((HENHMETAFILE) NULL);
    }
  
  //  Make an enhanced metafile from the placable metafile.
  
  mp.mm=MM_ANISOTROPIC;
  mp.xExt=((PAPMHEADER) pBits)->bbox.Right-((PAPMHEADER) pBits)->bbox.Left;
  *width=mp.xExt;
  mp.xExt=(mp.xExt*2540l)/(DWORD) (((PAPMHEADER) pBits)->wInch);
  mp.yExt=((PAPMHEADER)pBits)->bbox.Bottom-((PAPMHEADER) pBits)->bbox.Top;
  *height=mp.yExt;
  mp.yExt=(mp.yExt*2540l)/(DWORD) (((PAPMHEADER) pBits)->wInch);
  mp.hMF=NULL;
  hDC=GetDC(NULL);
  hTemp=SetWinMetaFileBits(dwSize,&(pBits[sizeof(APMHEADER)]),hDC,&mp);
  ReleaseDC(NULL,hDC);
  pBits=(BYTE *) DestroyString((char *) pBits);
  return(hTemp);
}

#define CENTIMETERS_INCH 2.54

static Image *ReadEMFImage(const ImageInfo *image_info,
  ExceptionInfo *exception)
{
  BITMAPINFO
    DIBinfo;

  HBITMAP
    hBitmap,
    hOldBitmap;

  HDC
    hDC;

  HENHMETAFILE
    hemf;

  Image
    *image;

  ssize_t
    height,
    width,
    y;

  RECT
    rect;

  register ssize_t
    x;

  register PixelPacket
    *q;

  RGBQUAD
    *pBits,
    *ppBits;

  image=AcquireImage(image_info);
  hemf=ReadEnhMetaFile(image_info->filename,&width,&height);
  if (hemf == (HENHMETAFILE) NULL)
    ThrowReaderException(CorruptImageError,"ImproperImageHeader");
  if ((image->columns == 0) || (image->rows == 0))
    {
      double
        y_resolution,
        x_resolution;

      y_resolution=DefaultResolution;
      x_resolution=DefaultResolution;
      if (image->y_resolution > 0)
        {
          y_resolution=image->y_resolution;
          if (image->units == PixelsPerCentimeterResolution)
            y_resolution*=CENTIMETERS_INCH;
        }
      if (image->x_resolution > 0)
        {
          x_resolution=image->x_resolution;
          if (image->units == PixelsPerCentimeterResolution)
            x_resolution*=CENTIMETERS_INCH;
        }
      image->rows=(size_t) ((height/1000.0/CENTIMETERS_INCH)*
        y_resolution+0.5);
      image->columns=(size_t) ((width/1000.0/CENTIMETERS_INCH)*
        x_resolution+0.5);
    }
  if (image_info->size != (char *) NULL)
    {
      ssize_t
        x;

      image->columns=width;
      image->rows=height;
      x=0;
      y=0;
      (void) GetGeometry(image_info->size,&x,&y,&image->columns,&image->rows);
    }
  if (image_info->page != (char *) NULL)
    {
      char
        *geometry;

      ssize_t
        sans;

      register char
        *p;

      MagickStatusType
        flags;

      geometry=GetPageGeometry(image_info->page);
      p=strchr(geometry,'>');
      if (p == (char *) NULL)
        {
          flags=ParseMetaGeometry(geometry,&sans,&sans,&image->columns,
            &image->rows);
          if (image->x_resolution != 0.0)
            image->columns=(size_t) floor((image->columns*
              image->x_resolution)+0.5);
          if (image->y_resolution != 0.0)
            image->rows=(size_t) floor((image->rows*image->y_resolution)+
              0.5);
        }
      else
        {
          *p='\0';
          flags=ParseMetaGeometry(geometry,&sans,&sans,&image->columns,
            &image->rows);
          if (image->x_resolution != 0.0)
            image->columns=(size_t) floor(((image->columns*
              image->x_resolution)/DefaultResolution)+0.5);
          if (image->y_resolution != 0.0)
            image->rows=(size_t) floor(((image->rows*
              image->y_resolution)/DefaultResolution)+0.5);
        }
      geometry=DestroyString(geometry);
    }
  hDC=GetDC(NULL);
  if (hDC == (HDC) NULL)
    {
      DeleteEnhMetaFile(hemf);
      ThrowReaderException(ResourceLimitError,"UnableToCreateADC");
    }
  
   //  Initialize the bitmap header info.
  
  (void) ResetMagickMemory(&DIBinfo,0,sizeof(BITMAPINFO));
  DIBinfo.bmiHeader.biSize=sizeof(BITMAPINFOHEADER);
  DIBinfo.bmiHeader.biWidth=(LONG) image->columns;
  DIBinfo.bmiHeader.biHeight=(-1)*(LONG) image->rows;
  DIBinfo.bmiHeader.biPlanes=1;
  DIBinfo.bmiHeader.biBitCount=32;
  DIBinfo.bmiHeader.biCompression=BI_RGB;
  hBitmap=CreateDIBSection(hDC,&DIBinfo,DIB_RGB_COLORS,(void **) &ppBits,
    NULL,0);
  ReleaseDC(NULL,hDC);
  if (hBitmap == (HBITMAP) NULL)
    {
      DeleteEnhMetaFile(hemf);
      ThrowReaderException(ResourceLimitError,"UnableToCreateBitmap");
    }
  hDC=CreateCompatibleDC(NULL);
  if (hDC == (HDC) NULL)
    {
      DeleteEnhMetaFile(hemf);
      DeleteObject(hBitmap);
      ThrowReaderException(ResourceLimitError,"UnableToCreateADC");
    }
  hOldBitmap=(HBITMAP) SelectObject(hDC,hBitmap);
  if (hOldBitmap == (HBITMAP) NULL)
    {
      DeleteEnhMetaFile(hemf);
      DeleteDC(hDC);
      DeleteObject(hBitmap);
      ThrowReaderException(ResourceLimitError,"UnableToCreateBitmap");
    }
  
  //  Initialize the bitmap to the image background color.
  
  pBits=ppBits;
  for (y=0; y < (ssize_t) image->rows; y++)
  {
    for (x=0; x < (ssize_t) image->columns; x++)
    {
      pBits->rgbRed=ScaleQuantumToChar(image->background_color.red);
      pBits->rgbGreen=ScaleQuantumToChar(image->background_color.green);
      pBits->rgbBlue=ScaleQuantumToChar(image->background_color.blue);
      pBits++;
    }
  }
  rect.top=0;
  rect.left=0;
  rect.right=(LONG) image->columns;
  rect.bottom=(LONG) image->rows;
  
   // Convert metafile pixels.
  
  PlayEnhMetaFile(hDC,hemf,&rect);
  pBits=ppBits;
  for (y=0; y < (ssize_t) image->rows; y++)
  {
    q=QueueAuthenticPixels(image,0,y,image->columns,1,exception);
    if (q == (PixelPacket *) NULL)
      break;
    for (x=0; x < (ssize_t) image->columns; x++)
    {
      q->red=ScaleCharToQuantum(pBits->rgbRed);
      q->green=ScaleCharToQuantum(pBits->rgbGreen);
      q->blue=ScaleCharToQuantum(pBits->rgbBlue);
      SetOpacityPixelComponent(q,OpaqueOpacity);
      pBits++;
      q++;
    }
    if (SyncAuthenticPixels(image,exception) == MagickFalse)
      break;
  }
  DeleteEnhMetaFile(hemf);
  SelectObject(hDC,hOldBitmap);
  DeleteDC(hDC);
  DeleteObject(hBitmap);
  return(GetFirstImageInList(image));
}
#endif // MAGICKCORE_WINGDI32_DELEGATE
%
%  RegisterEMFImage() adds attributes for the EMF image format to
%  the list of supported formats.  The attributes include the image format
%  tag, a method to read and/or write the format, whether the format
%  supports the saving of more than one frame to the same file or blob,
%  whether the format supports native in-memory I/O, and a brief
%  description of the format.
%
%  The format of the RegisterEMFImage method is:
%
%      size_t RegisterEMFImage(void)
%

ModuleExport size_t RegisterEMFImage(void)
{
  MagickInfo
    *entry;

  entry=SetMagickInfo("EMF");
#if defined(MAGICKCORE_WINGDI32_DELEGATE)
  entry->decoder=ReadEMFImage;
#endif
  entry->description=ConstantString(
    "Windows WIN32 API rendered Enhanced Meta File");
  entry->magick=(IsImageFormatHandler *) IsEMF;
  entry->blob_support=MagickFalse;
  entry->module=ConstantString("WMF");
  (void) RegisterMagickInfo(entry);
  entry=SetMagickInfo("WMFWIN32");
#if defined(MAGICKCORE_WINGDI32_DELEGATE)
  entry->decoder=ReadEMFImage;
#endif
  entry->description=ConstantString("Windows WIN32 API rendered Meta File");
  entry->magick=(IsImageFormatHandler *) IsWMF;
  entry->blob_support=MagickFalse;
  entry->module=ConstantString("WMFWIN32");
  (void) RegisterMagickInfo(entry);
  return(MagickImageCoderSignature);
}

%
%  UnregisterEMFImage() removes format registrations made by the
%  EMF module from the list of supported formats.
%
%  The format of the UnregisterEMFImage method is:
%
%      UnregisterEMFImage(void)
%

ModuleExport void UnregisterEMFImage(void)
{
  (void) UnregisterMagickInfo("EMF");
  (void) UnregisterMagickInfo("WMFWIN32");
}

*/
