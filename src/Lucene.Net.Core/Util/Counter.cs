﻿/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace Lucene.Net.Util
{
    using System.Threading;



    /// <summary>
    /// Simple counter class
    ///
    /// @lucene.internal
    /// @lucene.experimental
    /// </summary>
    public abstract class Counter
    {
        /// <summary>
        /// Adds the given delta to the counters current value
        /// </summary>
        /// <param name="delta">
        ///          the delta to add </param>
        /// <returns> the counters updated value </returns>
        public abstract long AddAndGet(long delta);

        /// <summary>
        /// Returns the counters current value
        /// </summary>
        /// <returns> the counters current value </returns>
        public abstract long Count { get; }

        /// <summary>
        /// Returns a new counter. The returned counter is not thread-safe.
        /// </summary>
        public static Counter NewCounter()
        {
            return NewCounter(false);
        }

        /// <summary>
        /// Returns a new counter.
        /// </summary>
        /// <param name="threadSafe">
        ///          <code>true</code> if the returned counter can be used by multiple
        ///          threads concurrently. </param>
        /// <returns> a new counter. </returns>
        public static Counter NewCounter(bool threadSafe)
        {
            return threadSafe ? new AtomicCounter() : (Counter)new SerialCounter();
        }

        private sealed class SerialCounter : Counter
        {
            private long count;

            public override long AddAndGet(long delta)
            {
                return this.count += delta;
            }

            public override long Count
            {
                get { return this.count; }
            }
        }

        private sealed class AtomicCounter : Counter
        {
            //internal readonly AtomicLong Count = new AtomicLong();
            private long count;

            public override long Count
            {
                get { return this.count; }
            }

            public override long AddAndGet(long delta)
            {
                return Interlocked.Add(ref this.count, delta);
            }


        }
    }

}
