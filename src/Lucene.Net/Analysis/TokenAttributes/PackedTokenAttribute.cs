using System;
using Lucene.Net.Util;

namespace Lucene.Net.Analysis.TokenAttributes
{
    public class PackedTokenAttribute : CharTermAttribute, ITypeAttribute, IPositionIncrementAttribute, IPositionLengthAttribute, IOffsetAttribute
    {
        private int positionIncrement = 1;
        private int positionLength = 1;
        private int termFrequency = 1;


        public void CopyTo(IAttribute target)
        {
            
        }

        public string Type { get; set; }
        public int PositionIncrement { get; set; }
        public int PositionLength { get; set; }

        public void SetOffset(int startOffset, int endOffset)
        {
            if (startOffset < 0 || endOffset < startOffset)
            {
                throw new ArgumentException("startOffset must be non-negative, and endOffset must be >= " +
                                            $"startOffset; got startOffset={startOffset}, endOffset={endOffset}");
            }
            StartOffset = startOffset;
            EndOffset = endOffset;
        }

        public int StartOffset { get; private set; }
        public int EndOffset { get; private set; }
        public int TermFrequency { get; set; }

        public override void Clear()
        {
            
        }
    }
}