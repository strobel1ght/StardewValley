using System.Linq;
using StardewModdingAPI;

namespace ConvenientChests.CategorizeChests.Framework.Persistence {
    /// <summary>
    /// The class responsible for saving and loading the mod state.
    /// </summary>
    class SaveManager : ISaveManager {
        private readonly CategorizeChestsModule _module;
        private readonly ISemanticVersion       _version;

        public SaveManager(ISemanticVersion version, CategorizeChestsModule module) {
            _version = version;
            _module  = module;
        }

        /// <summary>
        /// Generate save data and write it to the given file path.
        /// </summary>
        /// <param name="relativePath">The path of the save file relative to the mod folder.</param>
        public void Save(string relativePath) {
            var saver = new Saver(_version, _module.ChestDataManager);
            _module.ModEntry.Helper.Data.WriteJsonFile(relativePath, saver.GetSerializableData());
        }

        /// <summary>
        /// Load save data from the given file path.
        /// </summary>
        /// <param name="relativePath">The path of the save file relative to the mod folder.</param>
        public void Load(string relativePath) {
            var model = _module.ModEntry.Helper.Data.ReadJsonFile<SaveData>(relativePath) ?? new SaveData();

            foreach (var entry in model.ChestEntries) {
                try {
                    var chest     = _module.ChestFinder.GetChestByAddress(entry.Address);
                    var chestData = _module.ChestDataManager.GetChestData(chest);

                    chestData.AcceptedItemKinds = entry.GetItemSet();
                    foreach (var key in chestData.AcceptedItemKinds.Where(k => !_module.ItemDataManager.Prototypes.ContainsKey(k)))
                        _module.ItemDataManager.Prototypes.Add(key, key.GetOne());
                }
                catch (InvalidSaveDataException e) {
                    _module.Monitor.Log(e.Message, LogLevel.Warn);
                }
            }
        }
    }
}