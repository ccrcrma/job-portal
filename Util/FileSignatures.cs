using System.Collections.Generic;

namespace job_portal.Util
{
    public static class FileSignatures
    {
        public static readonly Dictionary<string, List<byte[]>> signatures = new Dictionary<string, List<byte[]>>
        {
            {".png", new List<byte[]>{
                new byte[] {0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A}
            }},
            { ".jpeg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                }
            },
            {".jpg", new List<byte[]>
                {
                    new byte[]{0xFF, 0xD8, 0xFF, 0xE0},
                    new byte[]{0xFF, 0xD8, 0xFF, 0xE1},
                    new byte[]{0xFF, 0xD8, 0xFF, 0xE8},
                }
            }
        };
    }
}