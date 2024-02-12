using MacroModules.Model.Modules.Responses;
using MacroModules.Model.Values;
using MacroModules.Model.Variables;

namespace MacroModules.Model.Modules.Concrete
{
    /// <summary>
    /// Represents a <see cref="Module"/> that branches execution flow given a <see cref="Value"/>
    /// and a set of condition statements.
    /// </summary>
    public class BranchModule : Module
    {
        /// <summary>
        /// Gets the maximum amount of contition statements allowed per <see cref="BranchModule"/>.
        /// This value is 10.
        /// </summary>
        public static int MaxConditions { get; } = 10;

        /// <summary>
        /// Indicates the source used to obtain a <see cref="Value"/>. This is either a
        /// <see cref="Variable"/> or an inner <see cref="ValuedModule"/>.
        /// </summary>
        /// <remarks>
        /// Any attempts to set the property to an object that is not a <see cref="Variable"/> or
        /// <see cref="ValuedModule"/> will be ignored.
        /// </remarks>
        public object? ValueSource
        {
            get { return valueSource; }
            set
            {
                if (value is Variable || value is ValuedModule)
                {
                    valueSource = value;
                }
            }
        }

        /// <summary>
        /// Gets a copy of the current conditions list. 
        /// </summary>
        /// <remarks>
        /// <para>
        ///     The conditions list is returned as a copy to prevent any modifications to the list
        ///     itself that may cause invalid state. The <see cref="BranchCondition"/> instances
        ///     themselves are copied by reference so any modifications to the contents of the
        ///     copied list will affect the <see cref="BranchModule"/>.
        /// </para>
        /// </remarks>
        public List<BranchCondition> Conditions
        {
            get { return new(conditions); }
        }

        public override ModuleType Type { get; } = ModuleType.Branch;

        public override bool IsConnectable { get; } = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchModule"/> class that has a single
        /// default exit port.
        /// </summary>
        public BranchModule()
        {
            ExitPorts[0] = new ExitPort(
                portName: "Default",
                description: "Activates if no specified conditions are met.");
        }

        /// <summary>
        /// Add an empty <see cref="BranchCondition"/> to the end of the conditions list.
        /// </summary>
        /// <returns>
        /// The <see cref="BranchCondition"/> that was added to the conditions list on success.
        /// <c>null</c> will be returned if <see cref="Conditions"/> already has a Count equal to
        /// <see cref="MaxConditions"/>.
        /// </returns>
        public BranchCondition? AddCondition()
        {
            if (conditions.Count >= MaxConditions)
            {
                return null;
            }

            BranchCondition condition = new();
            conditions.Add(condition);

            // Add ExitPort to end and swap with second to last index
            // This ensures that the default port is always at the bottom
            ExitPorts.Add(ExitPorts[^1]);
            ExitPorts[^2] = new();

            return condition;
        }

        /// <summary>
        /// Remove a condition from <see cref="Conditions"/> given its specific index.
        /// </summary>
        /// <param name="conditionIndex">The index of the condition to remove.</param>
        public void RemoveCondition(int conditionIndex)
        {
            if (conditionIndex < 0 || conditionIndex >= conditions.Count)
            {
                return;
            }

            conditions.RemoveAt(conditionIndex);
            ExitPorts.RemoveAt(conditionIndex);
        }

        /// <summary>
        /// Remove all condtions from <see cref="Conditions"/>.
        /// </summary>
        public void ClearConditions()
        {
            conditions.Clear();
            ExitPorts = [ExitPorts[^1]];
        }

        public override void Initialize(out object? processData)
        {
            processData = new BranchModuleData(Conditions);
        }

        public override IResponse Execute(ref object? processData)
        {
            var data = (BranchModuleData)processData!;

            switch (data.ExecuteState)
            {
                case ExecutionState.Startup:
                    if (valueSource == null)
                    {
                        break;
                    }
                    if (valueSource is Variable)
                    {
                        data.ExecuteState = ExecutionState.GettingVariableValue;
                        goto case ExecutionState.GettingVariableValue;
                    }
                    else
                    {
                        data.ExecuteState = ExecutionState.InitializeModuleValue;
                        goto case ExecutionState.InitializeModuleValue;
                    }

                case ExecutionState.GettingVariableValue:
                    data.MainValue = ((Variable)valueSource!).RuntimeValue;
                    data.ExecuteState = ExecutionState.BeginResultsProcessing;
                    goto case ExecutionState.BeginResultsProcessing;

                case ExecutionState.InitializeModuleValue:
                {
                    var innerModule = (ValuedModule)valueSource!;
                    innerModule.Initialize(out data.InnerModuleData);
                    data.ExecuteState = ExecutionState.GettingModuleValue;
                    goto case ExecutionState.GettingModuleValue;
                }

                case ExecutionState.GettingModuleValue:
                {
                    var innerModule = (ValuedModule)valueSource!;
                    IResponse response = innerModule.Execute(ref data.InnerModuleData);
                    ContinueResponse? contResponse = response as ContinueResponse;
                    if (contResponse == null)
                    {
                        return response;
                    }
                    if (contResponse.ReturnValue == null)
                    {
                        break;
                    }
                    data.InnerModuleData = null;
                    data.MainValue = contResponse.ReturnValue;
                    data.ExecuteState = ExecutionState.BeginResultsProcessing;
                    goto case ExecutionState.BeginResultsProcessing;
                }

                case ExecutionState.BeginResultsProcessing:
                    data.ProcessConditionResults();
                    data.ExecuteState = ExecutionState.WaitingForResult;
                    goto case ExecutionState.WaitingForResult;

                case ExecutionState.WaitingForResult:
                    for (int i = 0; i < data.ResultsTable.Count; ++i)
                    {
                        if (data.ResultsTable[i] == null)
                        {
                            return new WaitRepeatResponse(TimeSpan.FromMilliseconds(1));
                        }
                        if (data.ResultsTable[i] == true)
                        {
                            // First branch condition met, return index of ExitPort
                            return new ContinueResponse(i);
                        }
                    }
                    // If the for loop is exited, that means no conditions were met
                    break;
            }

            // Return default port
            return new ContinueResponse(ExitPorts.Count - 1);
        }

        public override void Restore(ref object? processData)
        {
            var data = (BranchModuleData)processData!;
            ValuedModule? innerModule = valueSource as ValuedModule;
            if (innerModule == null || data.InnerModuleData == null)
            {
                return;
            }

            innerModule.Restore(ref data.InnerModuleData);
        }

        private object? valueSource = null;
        private readonly List<BranchCondition> conditions = new();

        private enum ExecutionState
        {
            Startup,
            GettingVariableValue,
            InitializeModuleValue,
            GettingModuleValue,
            BeginResultsProcessing,
            WaitingForResult
        }

        private class BranchModuleData
        {
            /// <summary>
            /// Indicates the current execution state of the <see cref="BranchModule"/>.
            /// </summary>
            public ExecutionState ExecuteState { get; set; } = ExecutionState.Startup;

            /// <summary>
            /// Indicates the data of the contained <see cref="ValuedModule"/> of the
            /// <see cref="BranchModule"/> if present.
            /// </summary>
            /// <remarks>
            /// This property is only used if the <see cref="ValueSource"/> of the
            /// <see cref="BranchModule"/> is an inner <see cref="ValuedModule"/>.
            /// </remarks>
            public ref object? InnerModuleData
            {
                get { return ref innerModuleData; }
            }
            private object? innerModuleData = null;

            /// <summary>
            /// Indicates the main <see cref="Value"/> used when comparing from the list of
            /// conditions. 
            /// </summary>
            /// <remarks>
            /// This either comes from a <see cref="Variable"/> or as a result from an inner
            /// <see cref="ValuedModule"/>.
            /// </remarks>
            public Value? MainValue { get; set; } = null;

            /// <summary>
            /// Indicates the results of the comparisons using <see cref="MainValue"/>.
            /// </summary>
            /// <remarks>
            /// The index of a specific result corresponds to the index of its condition from the
            /// provided conditions list.
            /// </remarks>
            public List<bool?> ResultsTable { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="BranchModuleData"/> class that stores a
            /// copy of the <see cref="BranchModule"/>'s conditions list.
            /// </summary>
            /// <remarks>
            /// A copy of the conditions list is stored in case the list is modified while
            /// <see cref="ProcessConditionResults"/> is still running. This may happen if the user
            /// quickly exits the running state immediately after the module starts executing.
            /// </remarks>
            /// <param name="conditions">
            /// A list of <see cref="BranchCondition"/> instances that represent the ordered
            /// conditions of the <see cref="BranchModule"/>.
            /// </param>
            public BranchModuleData(List<BranchCondition> conditions)
            {
                ResultsTable = new(conditions.Count);
                conditionsCopy = new(conditions);

                for (int i = 0; i < conditions.Count; ++i)
                {
                    ResultsTable.Add(null);
                }
            }

            /// <summary>
            /// Begins the process of iterating over the conditions list and calculating the results
            /// into <see cref="ResultsTable"/>.
            /// </summary>
            /// <remarks>
            /// This method calculates the results asynchronously since some comparison operations
            /// take a significant amount of time (Snapshot equality). Once this method is called,
            /// the <see cref="ResultsTable"/> should be repeatedly polled for results. If an
            /// entry is <c>null</c>, the condition corresponding to that entry is still
            /// calcualting.
            /// </remarks>
            public void ProcessConditionResults()
            {
                if (MainValue == null)
                {
                    for (int i = 0; i < conditionsCopy.Count; ++i)
                    {
                        ResultsTable[i] = false;
                    }
                    return;
                }

                Parallel.Invoke(() =>
                {
                    for (int i = 0; i < conditionsCopy.Count; ++i)
                    {
                        ResultsTable[i] = conditionsCopy[i].GetResult(MainValue);
                    }
                    MainValue.Dispose();
                });
            }

            private List<BranchCondition> conditionsCopy;
        }
    }
}
