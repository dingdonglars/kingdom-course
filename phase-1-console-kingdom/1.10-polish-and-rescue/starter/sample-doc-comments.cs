// Sample XML doc-comments for the four "public surface" types.
// Copy each /// block above the corresponding type in your engine. Don't ship this file.

namespace Kingdom.Engine;

/// <summary>
/// The aggregate root of the kingdom. Owns buildings, citizens, resources, and the event log.
/// Advanced one tick at a time via <see cref="AdvanceDay"/>. Construct with an
/// <see cref="Infrastructure.IRandom"/> and <see cref="Infrastructure.IClock"/>; the no-arg
/// constructor uses production defaults.
/// </summary>
public class Kingdom_DocCommentSample { }

namespace Kingdom.Engine.Infrastructure;

/// <summary>
/// Random-number source for the engine. Production: <see cref="SystemRandom"/>.
/// Tests: a FakeItEasy fake (<c>A.Fake&lt;IRandom&gt;()</c>).
/// </summary>
/// <remarks>
/// Designed to be the *minimum* surface the engine needs. Add a method here only when
/// the engine actually calls it; never expose more than necessary.
/// </remarks>
public interface IRandom_DocCommentSample { }

/// <summary>Wall-clock time abstraction. Production: <see cref="SystemClock"/>; tests: a fake.</summary>
public interface IClock_DocCommentSample { }

namespace Kingdom.Engine.Events;

/// <summary>
/// Picks a random event each tick (or none). Pure function of (kingdom state, IRandom rolls).
/// </summary>
public class EventEngine_DocCommentSample { }
