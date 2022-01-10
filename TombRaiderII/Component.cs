﻿using LiveSplit.Model;                    // LiveSplitState
using LiveSplit.UI.Components.AutoSplit;  // IAutoSplitter
using TRUtil;                             // ClassicAutosplitter, ClassicComponent

namespace TR2
{
    /// <summary>
    ///     Implementation of <see cref="AutoSplitComponent"/>.
    /// </summary>
    /// <remarks>
    ///     <see cref="AutoSplitComponent"/> is derived from <see cref="LogicComponent"/>,
    ///     which derives from <see cref="IComponent"/> and <see cref="IDisposable"/>.
    /// </remarks>
    internal sealed class Component : ClassicComponent
    {
        /// <summary>Initializes the component.</summary>
        /// <param name="autosplitter"><see cref="IAutoSplitter"/> implementation</param>
        /// <param name="state"><see cref="LiveSplitState"/> passed by LiveSplit</param>
        public Component(ClassicAutosplitter autosplitter, LiveSplitState state) : base(autosplitter, state)
        {
        }

        /// <inheritdoc/>
        public override string ComponentName => "Tomb Raider II";
    }
}
