using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public static class IOSafe
{
    public static class File
    {
        public static void Copy(string src, string dest, bool overwrite)
        {
            if (System.IO.File.Exists(dest))
            {
                System.IO.File.SetAttributes(dest, FileAttributes.Normal);
            }

            System.IO.File.Copy(src, dest, overwrite);
            System.IO.File.SetAttributes(dest, FileAttributes.Normal);
        }

        public static void Delete(string src)
        {
            if (System.IO.File.Exists(src))
            {
                System.IO.File.SetAttributes(src, FileAttributes.Normal);
                System.IO.File.Delete(src);
            }
        }

        public static void Move(string src, string dest, bool overwrite)
        {
            if (src.Equals(dest))
                return;

            if (!System.IO.File.Exists(dest))
            {
                System.IO.File.SetAttributes(src, FileAttributes.Normal);
                System.IO.File.Move(src, dest);
            }
            else
            {
                if (overwrite)
                {
                    Delete(dest);
                    System.IO.File.SetAttributes(src, FileAttributes.Normal);
                    System.IO.File.Move(src, dest);
                }
                else
                {
                    throw new IOException("Cannot create a file when that file already exists. FixedFileMove cannot override files with overwrite disabled.");
                }
            }
        }
    }
}
