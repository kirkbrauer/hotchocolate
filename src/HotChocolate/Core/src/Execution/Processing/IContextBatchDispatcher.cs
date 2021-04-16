using System;
using HotChocolate.Fetching;

namespace HotChocolate.Execution.Processing
{
    internal interface IContextBatchDispatcher
    {
        IBatchDispatcher BatchDispatcher { get; }

        /// <summary>Marks a context as having started</summary>
        void Register(IExecutionContext context);

        /// <summary>Marks a context as having completed</summary>
        void Unregister(IExecutionContext context);

        /// <summary>Suspend the batch dispatching for all registered contexts</summary>
        void Suspend();

        /// <summary>Resume the batch dispatching for all registered contexts</summary>
        void Resume();
    }
}