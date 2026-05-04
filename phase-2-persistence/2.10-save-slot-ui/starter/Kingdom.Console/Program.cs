using Kingdom.Console;
using Kingdom.Engine.Infrastructure;
using Kingdom.Persistence.EfCore;

var saveFolder = Path.Combine(AppContext.BaseDirectory, "saves");
Directory.CreateDirectory(saveFolder);
var dbPath = Path.Combine(saveFolder, "kingdoms-ef.db");

IRandom rng = new SystemRandom();
IClock clock = new SystemClock();

var store = new KingdomEfStore(dbPath);
SaveSlotUI.Run(store, rng, clock);
