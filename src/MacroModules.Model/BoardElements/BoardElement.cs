using MacroModules.Model.Modules.Concrete;
using System.Text.Json.Serialization;

namespace MacroModules.Model.BoardElements
{
    /// <summary>
    /// Represents and element on a board that has a coordinate position relative to the board.
    /// </summary>
    [JsonDerivedType(typeof(Label), typeDiscriminator: "label")]
    [JsonDerivedType(typeof(BranchModule), typeDiscriminator: "branchModule")]
    [JsonDerivedType(typeof(CloseProgramModule), typeDiscriminator: "closeProgramModule")]
    [JsonDerivedType(typeof(FocusWindowModule), typeDiscriminator: "focusWindowModule")]
    [JsonDerivedType(typeof(GetCursorPositionModule), typeDiscriminator: "getCursorPositionModule")]
    [JsonDerivedType(typeof(GetInputStateModule), typeDiscriminator: "getInputStateModule")]
    [JsonDerivedType(typeof(GetPixelColorModule), typeDiscriminator: "getPixelColorModule")]
    [JsonDerivedType(typeof(GetSnapshotModule), typeDiscriminator: "getSnapshotModule")]
    [JsonDerivedType(typeof(MoveCursorModule), typeDiscriminator: "moveCursorModule")]
    [JsonDerivedType(typeof(OpenProgramModule), typeDiscriminator: "openProgramModule")]
    [JsonDerivedType(typeof(PlaySoundModule), typeDiscriminator: "playSoundModule")]
    [JsonDerivedType(typeof(ScrollModule), typeDiscriminator: "scrollModule")]
    [JsonDerivedType(typeof(SendInputModule), typeDiscriminator: "sendInputModule")]
    [JsonDerivedType(typeof(StartupEntryModule), typeDiscriminator: "startupEntryModule")]
    [JsonDerivedType(typeof(TriggerEntryModule), typeDiscriminator: "triggerEntryModule")]
    [JsonDerivedType(typeof(WaitModule), typeDiscriminator: "waitModule")]
    public abstract class BoardElement
    {
        /// <summary>
        /// Indicates the horizontal position of the element.
        /// </summary>
        /// <remarks>
        /// A value of 0 represents a <see cref="BoardElement"/> that's positioned to the left edge
        /// of the board. Positive values indicate a position to the right while negative values
        /// indicate a position to the left.
        /// </remarks>
        public double PositionX { get; set; }

        /// <summary>
        /// Indicates the vertical position of the element.
        /// </summary>
        /// <remarks>
        /// A value of 0 represents a <see cref="BoardElement"/> that's positioned to the top edge
        /// of the board. Positive values indicate a position downwards while negative values
        /// indicate a position upwards.
        /// </remarks>
        public double PositionY { get; set; }
    }
}
