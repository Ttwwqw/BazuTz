using System;

namespace _Scripts.EditorUtils
{
    public class TrisAndPolygonsData
    {
        public string selectedObject = String.Empty;
        public int trisCount = 0;
        public int polyCount = 0;
        public string errorText = String.Empty;

        public string formatedTrisCount => FormatBigValues(trisCount);
        public string formatedPolyCount => FormatBigValues(polyCount);
        
        
        private static string FormatBigValues(int trisCount)
        {
            if (trisCount >= 1_000_000)
            {
                return $"{(float)(trisCount) / 1_000_000:0.00} M";
            }
            else if (trisCount >= 1_000)
            {
                return $"{(float)(trisCount) / 1_000:0.00} K";
            }

            return $"{trisCount}";
        }
    }
}