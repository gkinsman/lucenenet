using System;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.TokenAttributes;
using Lucene.Net.Documents;
using Lucene.Net.Util;
using NUnit.Framework;

using static Lucene.Net.Search.DocIdSetIterator;

namespace Lucene.Net.Index
{
    public class TestCustomTermFreq : LuceneTestCase
    {
        private sealed class CannedTermFreqs : TokenStream
        {
            private readonly int[] termFreqs;
            private readonly string[] terms;

            private ICharTermAttribute termAtt;
            private ITermFrequencyAttribute termFreqAtt;
            private int Upto = 0;

            public CannedTermFreqs(string[] terms, int[] termFreqs)
            {
                this.terms = terms;
                this.termFreqs = termFreqs;
                AreEqual(terms.Length, termFreqs.Length);

                termAtt = AddAttribute<ICharTermAttribute>();
                termFreqAtt = AddAttribute<ITermFrequencyAttribute>();
            }

            public override bool IncrementToken()
            {
                if (Upto == terms.Length) return false;

                ClearAttributes();

                termAtt.Append(terms[Upto]);
                termFreqAtt.TermFrequency = termFreqs[Upto];

                Upto++;
                return true;
            }

            public override void Reset()
            {
                Upto = 0;
            }
        }

        [Test]
        public void TestSingletonTermsOneDoc()
        {
            var dir = NewDirectory();
            var analyser = new MockAnalyzer(new Random());
            var writer = new IndexWriter(dir, new IndexWriterConfig(TEST_VERSION_CURRENT, analyser));

            var doc = new Document();
            var field = new TextField("field", new CannedTermFreqs(new[] {"foo", "bar"}, new[] {42, 128}));
            doc.Add(field);
            writer.AddDocument(doc);

            doc = new Document();
            field = new TextField("field", new CannedTermFreqs(new[] {"foo", "bar"}, new[] {50, 50}));
            doc.Add(field);
            writer.AddDocument(doc);

            var reader = DirectoryReader.Open(writer, true);
            var postings = MultiFields.GetTermDocsEnum(reader, null, "field", new BytesRef("bar"));
            NotNull(postings);
            Equals(0, postings.NextDoc());
            Equals(128, postings.Freq);
            Equals(1, postings.NextDoc());
            Equals(50, postings.Freq);
            Equals(NO_MORE_DOCS, postings.NextDoc());

            reader.Dispose();
            writer.Dispose();
            dir.Dispose();
        }
    }
}