using Lucene.Net.Util;

namespace Lucene.Net.Analysis.TokenAttributes
{
    public class TermFrequencyAttribute : Attribute, ITermFrequencyAttribute
    {
        public TermFrequencyAttribute()
        {
            TermFrequency = 1;
        }

        public TermFrequencyAttribute(int termFrequency)
        {
            TermFrequency = termFrequency;
        }

        public override void Clear()
        {
            TermFrequency = 1;
        }

        public override void CopyTo(IAttribute target)
        {
            var other = (TermFrequencyAttribute) target;
            other.TermFrequency = TermFrequency;
        }

        public int TermFrequency { get; set; }

        public override bool Equals(object other)
        {
            if (other == this) return true;
            var otherAsTermFreq = other as TermFrequencyAttribute;
            return otherAsTermFreq != null && otherAsTermFreq.TermFrequency == TermFrequency;
        }
    }

    public interface ITermFrequencyAttribute : IAttribute
    {
        int TermFrequency { get; set; }
    }
}